using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UzmanCrm.CrmService.KDPortalUI.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserProfile()
        {
            return View();
        }
    }
}