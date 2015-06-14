using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;

namespace Group3_MVC4.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Typeahead()
        {
            return View();
        }

        public ActionResult DataTable()
        {
            return View();
        }

        public ActionResult FormWizard()
        {
            return View();
        }

        public ActionResult PopupModel()
        {
            return Json("Confirm", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInfo()
        {
            using (var ctx = new WatchShopEntities())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                var models = ctx.Models.ToList();
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Combobox()
        {
            using (var ctx = new WatchShopEntities())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                var models = ctx.Models.ToList();
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
