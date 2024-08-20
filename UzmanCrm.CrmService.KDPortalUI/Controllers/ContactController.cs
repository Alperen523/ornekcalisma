using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UzmanCrm.CrmService.KDPortalUI.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult GetList()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CustomerList() {
            return View();
        }


    }
}