using System;
using System.Web.Mvc;

namespace web.sph.App_Code
{
    [RoutePrefix("sample")]
    public class SampleController : Controller
    {
        [Route("test")]
        public ActionResult Index()
        {
            return Content("test data");
        }

        [Route("menu")]
        public ActionResult Menu()
        {
            var model = new Vehicle{
                Name = "Van",
                Cc = 2000
            };
            return View(model);
        }
    }

    public class Vehicle
    {
        public string Name{get;set;}
        public int Cc {get;set;}
    }
}