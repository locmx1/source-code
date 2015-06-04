using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;

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

        public ActionResult CreateSellingRequest()
        {
            return View();
        }

        public ActionResult Tag(string id, string title)
        {
            return View();
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

    }
}
