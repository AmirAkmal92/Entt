using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

[RoutePrefix("api/device-tokens")]
public class ScannerTokenServiceController : BaseApiController
{
    [Route("")]
    [HttpPost]
    public async Task<IHttpActionResult> CreateToken([FromBody]GetTokenModel model)
    {
        if (model.grant_type == "password" && !Membership.ValidateUser(model.username, model.password))
            return Json(new { success = false, status = 403, message = "Cannot validate your username or password" });

        if (model.grant_type == "admin" && !User.IsInRole("administrators"))
            return Json(new { success = false, status = 403, message = "You are not in administrator role" });

        var ip = this.GetClientIp();

        if (model.grant_type == "local_network" && !ip.StartsWith("1"))
            return Json(new { success = false, status = 403, message = "local_network request must be done within specified IP address range " + ip });

        model.expiry = DateTime.Today.AddMonths(1);// give it 1 month validity
        var tokenService = ObjectBuilder.GetObject<ITokenService>();

        var context = new SphDataContext();
        var user = await context.LoadOneAsync<UserProfile>(x => x.UserName == model.username);
        if (null == user)
        {
            user = new UserProfile
            {
                UserName = model.username,
                Designation = "Device",
                HasChangedDefaultPassword = true,
                Email = $"{model.username}@pos.com.my",
                Department = "PPL",
                IsLockedOut = false
            };
            var profile = new Profile
            {
                UserName = model.username,
                Password = "Pa$$w0rd!",
                ConfirmPassword = "Pa$$w0rd!",
                Roles = new[] { "devices" },
                Designation = "Device",
                Email = $"{model.username}@pos.com.my"
            };
            var ok = await AddUserAsync(profile).ConfigureAwait(false);
            if (!ok)
                return Invalid("Cannot create device with id " + model.username);
        }

        // get existing token that still have at least 14 days validity
        var repos = ObjectBuilder.GetObject<ITokenRepository>();
        var lo = await repos.LoadAsync(model.username, DateTime.Today.AddDays(14));
        var existing = lo.ItemCollection.LastOrDefault();
        if (null != existing)
        {
            var token1 = existing.GenerateToken();
            return Json(existing.ToJson().Replace("\"WebId\"", $"\"token\":\"{token1}\",\r\n\"WebId\""));
        }

        var roles = Roles.GetRolesForUser(model.username);
        var claim = await tokenService.CreateTokenAsync(user, roles, model.expiry);
        var token = claim.GenerateToken();
        var json = claim.ToJson()
            .Replace("\"WebId\"", $"\"token\":\"{token}\",\r\n\"WebId\"");

        return Json(json);
    }

    [Route("check-validity")]
    [HttpGet]
    public IHttpActionResult CheckValidity()
    {
        return Ok(new { message = "Token is valid" });
    }

    private string GetClientIp(HttpRequestMessage request = null)
    {
        request = request ?? Request;

        if (request.Properties.ContainsKey("MS_HttpContext"))
        {
            return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        }
        if (!request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            return HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;
        var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
        return prop.Address;
    }


    private static async Task<bool> AddUserAsync(Profile profile)
    {
        var context = new SphDataContext();
        var userName = profile.UserName;
        if (string.IsNullOrWhiteSpace(profile.Designation)) throw new ArgumentNullException("Designation for  " + userName + " cannot be set to null or empty");
        var designation = await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation);
        if (null == designation) throw new InvalidOperationException("Cannot find designation " + profile.Designation);
        var roles = designation.RoleCollection.ToArray();

        var em = Membership.GetUser(userName);

        if (null != em)
        {
            profile.Roles = roles;
            em.Email = profile.Email;

            var originalRoles = Roles.GetRolesForUser(userName);
            if (originalRoles.Length > 0)
                Roles.RemoveUserFromRoles(userName, originalRoles);

            Roles.AddUserToRoles(userName, profile.Roles);
            Membership.UpdateUser(em);
            await CreateProfile(profile, designation);
            return true;

        }

        try
        {
            Membership.CreateUser(userName, profile.Password, profile.Email);
        }
        catch (MembershipCreateUserException ex)
        {
            ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(ex));
            return false;
        }

        Roles.AddUserToRoles(userName, roles);
        profile.Roles = roles;

        await CreateProfile(profile, designation);

        return true;
    }


    private static async Task<UserProfile> CreateProfile(Profile profile, Designation designation)
    {
        if (null == profile) throw new ArgumentNullException(nameof(profile));
        if (null == designation) throw new ArgumentNullException(nameof(designation));
        if (string.IsNullOrWhiteSpace(designation.Name)) throw new ArgumentNullException(nameof(designation), "Designation Name cannot be null, empty or whitespace");
        if (string.IsNullOrWhiteSpace(profile.UserName)) throw new ArgumentNullException(nameof(profile), "Profile UserName cannot be null, empty or whitespace");

        var context = new SphDataContext();
        var usp = await context.LoadOneAsync<UserProfile>(p => p.UserName == profile.UserName) ?? new UserProfile();
        usp.UserName = profile.UserName;
        usp.FullName = profile.FullName;
        usp.Designation = profile.Designation;
        usp.Department = profile.Department;
        usp.Mobile = profile.Mobile;
        usp.Telephone = profile.Telephone;
        usp.Email = profile.Email;
        usp.RoleTypes = string.Join(",", profile.Roles);
        usp.StartModule = designation.StartModule;
        if (usp.IsNewItem) usp.Id = profile.UserName.ToIdFormat();

        using (var session = context.OpenSession())
        {
            session.Attach(usp);
            await session.SubmitChanges();
        }

        return usp;
    }


}