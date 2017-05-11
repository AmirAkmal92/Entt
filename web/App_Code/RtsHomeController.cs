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

[RoutePrefix("rts")]
public class RtsHomeController : Controller
{
    [Route("")]
    public ActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login", "RtsAccount");
        return View("Default");
    }
}