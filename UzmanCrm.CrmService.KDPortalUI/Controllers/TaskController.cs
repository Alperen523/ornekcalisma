using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UzmanCrm.CrmService.KDPortalUI.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult GetList()
        {
            return View();
        }
    }
}