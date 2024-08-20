using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UzmanCrm.CrmService.KDPortalUI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult SignIn()
        {
            return View();
        }

        // GET: Login
        public ActionResult SignOut()
        {
            return View();
        }
    }
}