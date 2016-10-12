using System;
using System.Web.Mvc;

namespace web.sph.App_Code
{
    [RoutePrefix("custom-appointment")]
    public class CustomAppointmentController : Controller
    {
     

        [Route("form")]
        public ActionResult Form()
        {
         
            return View();
        }
    }

   
}