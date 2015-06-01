using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group3_MVC4.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateNewStaff()
        {
            return View();
        }
        public ActionResult ModifyStaff()
        {
            return View();
        }
        public ActionResult ModifyStore()
        {
            return View();
        }
        public ActionResult CreateNewStore()
        {
            return View();
        }
        public ActionResult Mail()
        {
            return View();
        }
    }
}
