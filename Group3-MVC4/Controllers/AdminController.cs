using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;

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

        public ActionResult DeleteSellRequest(string id)
        {
            return RedirectToAction("ViewSellRequestDetail");
        }

        public ActionResult AddNewSellRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewSellRequest(FormCollection infoFormCollection)
        {
            return RedirectToAction("ManageSellRequest");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ManageSellRequest()
        {
            using (var model = new WatchShopEntities())
            {
                IEnumerable<Watch> watches = (IEnumerable<Watch>)from s in model.Watches where s.TransactionType == 1 select s;
                return View(watches.ToList());
            }

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewSellRequestDetail(string id)
        {
            int watchId = Int32.Parse(id);
            //int watchId = 8;
            using (var model = new WatchShopEntities())
            {
                Watch watch = model.Watches.Single(s => s.Id == watchId);
                var models = from s in model.Models select s;
                var stores = from s in model.Stores select s;
                ViewBag.models = models.ToList();
                ViewBag.stores = stores.ToList();
                return View(watch);
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ViewSellRequestDetail(FormCollection collection)
        {
            int watchId = Int32.Parse(collection.Get("id"));
            String watchName = collection.Get("name");
            String watchGlassType = collection.Get("glassType");
            String watchCaseMaterial = collection.Get("caseMeterial");
            String watchMainColor = collection.Get("mainColor");
            String description = collection.Get("description");
            using (var model = new WatchShopEntities())
            {
                Watch watch = model.Watches.Single(s => s.Id == watchId);
                watch.Name = watchName;
                watch.GlassType = watchGlassType;
                watch.CaseMeterial = watchCaseMaterial;
                watch.MainColor = watchMainColor;
                watch.Description = description;
                model.SaveChanges();
                return RedirectToAction("ManageSellRequest");
            }
        }

    }
}
