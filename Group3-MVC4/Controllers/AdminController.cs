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
            using (var model = new WatchShopEntities())
            {
                IEnumerable<Member> members = (IEnumerable<Member>)from s in model.Members where s.RoleId==2 select s;
                return View(members.ToList());
            }
        }

        public ActionResult ViewStores()
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
        public ActionResult ViewSellRequestDetail(Watch editWatch)
        {            
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = new TimeSpan(DateTime.Now.Date.Ticks).TotalMilliseconds.ToString() + "." + file.FileName.Substring(file.FileName.IndexOf(".") + 1);
                    var path = Path.Combine(Server.MapPath("~/Images/Upload/"), fileName);
                    editWatch.Images = "/Images/Upload/" + fileName;
                    file.SaveAs(path);
                }
            }
            using (var model = new WatchShopEntities())
            {
                Watch watch = model.Watches.Single(s => s.Id == editWatch.Id);
                watch.Name = editWatch.Name;
                watch.GlassType = editWatch.GlassType;
                watch.CaseMeterial = editWatch.CaseMeterial;
                watch.MainColor = editWatch.MainColor;
                watch.Description = editWatch.Description;
                watch.ModelId = editWatch.ModelId;
                watch.AvailableAt = editWatch.AvailableAt;
                watch.Images = editWatch.Images;
                model.SaveChanges();
                return RedirectToAction("ManageSellRequest");
            }
        }

        public ActionResult ManageStaff()
        {
            using (var model = new WatchShopEntities())
            {
                IEnumerable<Member> members = (IEnumerable<Member>)from s in model.Members where s.RoleId == 2 select s;
                return View(members.ToList());
            }
        }


    }
}
