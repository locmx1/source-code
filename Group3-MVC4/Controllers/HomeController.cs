using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;
using System.IO;

namespace Group3_MVC4.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewWatches()
        {
            return View();
        }

        public ActionResult ViewWatchDetail()
        {
            return View();
        }
        
        public ActionResult Tag(string id, string title)
        {
            return View();
        }

        public ActionResult SendMessage()
        {
            WatchSystem.SendNotification("01663855778");
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult BuyWatch()
        {
            BuyWatchModel model = new BuyWatchModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult BuyWatch(BuyWatchModel model)
        {

            Watch watch = new Watch();
            watch.Name = "123";
            watch.GlassType = model.GlassType;
            watch.CaseMeterial = model.CaseMeterial;
            watch.MainColor = model.MainColor;
            watch.Images = model.Images;
            watch.InTransactionPrice = model.InTransactionPrice;
            watch.TransactionType = 0;
            watch.Status = 0;
            watch.Description = "";
            watch.MemberId = "1";
            watch.ModelId = model.ModelId;
            watch.AvailableAt = 1;
            var dbCtx = new WatchShopEntities();
            dbCtx.Watches.Add(watch);
            var result = dbCtx.SaveChanges();
            if (result == 1)
            {
                return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thành Công" });
            }
            //return ShowMessage("Dat Mua that bai!");
            return RedirectToAction("ShowMessage", "Home", new {message = "Đặt Mua Thất Bại"});
        }

        public ActionResult ShowMessage(String message)
        {
            ViewBag.Message = message;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateSellingRequest()
        {
            using (var model = new WatchShopEntities())
            {
                var models = from s in model.Models select s;
                var stores = from s in model.Stores select s;
                ViewBag.models = models.ToList();
                ViewBag.stores = stores.ToList();
                return View();
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSellingRequest(Watch userWatch)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Upload"), fileName);
                    userWatch.Images = path;
                    file.SaveAs(path);
                }
            }

            using (var model = new WatchShopEntities())
            {
                userWatch.TransactionType = 1;
                userWatch.Status = 1;
                var member = model.Members.Single(s => s.Id == userWatch.MemberId);
                member.Watches.Add(userWatch);
                model.SaveChanges();
                return RedirectToAction("index");
            }
            
        }
    }
}
