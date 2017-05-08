using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

[RoutePrefix("rts-account")]
public class RtsAccountController : Controller
{
    [Route("logout")]
    public async Task<ActionResult> Logoff()
    {
        HttpContext.GetOwinContext().Authentication.SignOut();
        try
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            await logger.LogAsync(new LogEntry { Log = EventLog.Security, Message = "Logoff" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return Redirect("/");
    }

    [AllowAnonymous]
    [Route("login")]
    public ActionResult Login(bool success = true, string status = "OK")
    {
        ViewBag.success = success;
        ViewBag.status = status;

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(RtsLoginModel model, string returnUrl = "/")
    {
        if (string.IsNullOrEmpty(model.UserName))
            return RedirectToAction("login", "rts-account", new { success = false, status = "Username cannot be set to null or empty." });
        if (string.IsNullOrEmpty(model.Password))
            return RedirectToAction("login", "rts-account", new { success = false, status = "Password cannot be set to null or empty." });

        var logger = ObjectBuilder.GetObject<ILogger>();

        var directory = ObjectBuilder.GetObject<IDirectoryService>();
        if (await directory.AuthenticateAsync(model.UserName, model.Password))
        {
            var identity = new ClaimsIdentity(ConfigurationManager.ApplicationName + "Cookie");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));
            var roles = Roles.GetRolesForUser(model.UserName).Select(x => new Claim(ClaimTypes.Role, x));
            identity.AddClaims(roles);


            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == model.UserName);
            await logger.LogAsync(new LogEntry { Log = EventLog.Security });
            if (null != profile)
            {
                // user email address verification pending
                //if (!profile.HasChangedDefaultPassword)
                //    return RedirectToAction("login", "rts-account", new { success = false, status = "Email verification pending. Please check your inbox for a verification email. You will be allowed to sign in after verification is complete." });

                var claims = profile.GetClaims();
                identity.AddClaims(claims);

                var designation = context.LoadOneFromSources<Designation>(x => x.Name == profile.Designation);
                if (null != designation && designation.EnforceStartModule)
                    profile.StartModule = designation.StartModule;

                HttpContext.GetOwinContext().Authentication.SignIn(identity);

                if (string.IsNullOrEmpty(profile.Designation) ||
                    !profile.Designation.Equals("Rts admin"))
                    return Redirect("/sph");

                if (returnUrl == "/" ||
                    returnUrl.Equals("/rts", StringComparison.InvariantCultureIgnoreCase) ||
                    returnUrl.Equals("/rts#", StringComparison.InvariantCultureIgnoreCase) ||
                    returnUrl.Equals("/rts/", StringComparison.InvariantCultureIgnoreCase) ||
                    returnUrl.Equals("/rts/#", StringComparison.InvariantCultureIgnoreCase) ||
                    string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect("/rts#" + profile.StartModule);
            }
            HttpContext.GetOwinContext().Authentication.SignIn(identity);
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return Redirect("/");
        }
        var user = await directory.GetUserAsync(model.UserName);
        await logger.LogAsync(new LogEntry { Log = EventLog.Security, Message = "Login Failed" });
        if (null != user && user.IsLockedOut)
            return RedirectToAction("login", "rts-account", new { success = false, status = "Your acount has beeen locked, Please contact your administrator." });
        else
            return RedirectToAction("login", "rts-account", new { success = false, status = "The user name or password provided is incorrect." });
    }

    public class RtsLoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}