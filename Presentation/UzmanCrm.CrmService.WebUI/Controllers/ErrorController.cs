using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UzmanCrm.CrmService.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return Redirect("http://viptest.vakko.com.tr/Home/Search");
        }

        public ActionResult Error403()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}