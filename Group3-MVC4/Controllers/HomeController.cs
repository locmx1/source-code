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
        private Dictionary<int, string> watchTransactionTypes = new Dictionary<int, string>()
        {
	        {0, "Watche for sale."},
            {1, "Watche for buying."},
            {2, "Watche sell to store."},
            {3, "Watche for consigning."},
            {4, "Watche bough by user."},
            {5, "Watch bough by user (Not available at the moment)"},
            {6, "Owned watch."},
            {7, "Wish watch."}
	    };

        private Dictionary<int, string> watchStatuses = new Dictionary<int, string>()
        {
	        {0, "Initalization."},
            {1, "Confirmed by SMS."},
            {2, "Confirmed by staff."},
            {3, "Done."}
	    };

        // Code by LocMX
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewWatches()
        {
            using (var ctx = new WatchShopEntities())
            {
                // Get brand
                var brands = ctx.Brands.ToList();
                // Get watches
                var colors = new List<string>();
                colors.Add("Đen nhạt");
                colors.Add("Xám xanh");
                colors.Add("Trắng");
                colors.Add("Kem");
                colors.Add("Da bò");
                colors.Add("Nâu");
                colors.Add("Xanh rêu");
                colors.Add("Đen");
                colors.Add("Bạch kim");
                // Get tag
                var tags = ctx.Tags.ToList();
                ViewBag.Brands = brands;
                ViewBag.Tags = tags;
                ViewBag.Colors = colors;
                return View();
            }
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
            //WatchSystem.SendNotification("01663855778");
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
		
		public ActionResult GetModelByBrand(string brandName)
        {
            //BuyWatchModel model = new BuyWatchModel();
            var ctx = new WatchShopEntities();
            List<Model> mlist = ctx.Set<Model>().Where(m => m.Brand.Name.Contains(brandName)).ToList();
            List<ModelViewModel> list = new List<ModelViewModel>();
            foreach (Model m in mlist)
            {
                list.Add(new ModelViewModel()
                {
                    Name = m.Name
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BuyWatch(BuyWatchModel model, FormCollection collection)
        {

            var uploadDir = "~/Upload/Images/";
            var images = "";
            HttpFileCollectionBase list = Request.Files;
            bool isValidated = true;
            if (list.Count >= 5)
            {
                isValidated = false;
            }
            else if (list.Count < 5 && list.Count >= 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    HttpPostedFileBase file = list[i];
                    if (file.ContentLength > 4 * 1024 * 1024)
                    {
                        isValidated = false;
                    }
                }
            }
            if (isValidated)
            {
                //Upload 4 file anh
                for (int i = 0; i < list.Count; i++)
                {
                    HttpPostedFileBase file = list[i];
                    if (file.FileName.Length > 0)
                    {
                        Int32 unixTimestamp =
                            (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + i;
                        int index = file.FileName.IndexOf('.');
                        string ext = file.FileName.Substring(index, file.FileName.Length - index);
                        //rename - TH user upload 2 file giong ten
                        var fileName = unixTimestamp.ToString() + ext;
                        var serverPath = Path.Combine(Server.MapPath(uploadDir), fileName);
                        var webPath = uploadDir + fileName;
                        images = images + ";" + webPath;
                        file.SaveAs(serverPath);
                    }
                }
                string watchType = collection["watchType"];
                if (watchType.Equals("Đeo Tay"))
                {
                    WristWatch w = new WristWatch();
                    w.Watch = new Watch();
                    w.Watch.Name = model.Name;
                    w.Watch.CaseMeterial = model.CaseMeterial;
                    //w.Gender = collection["gender"];
                    w.Gender = true;
                    //watchType cua Deo tay ???
                    w.WatchType = true;
                    //isSmartWatch ko dc null ???
                    w.IsSmartWatch = false;
                    w.WaterProof = Int32.Parse(collection["atmDegree"]);
                    w.Watch.Images = images;
                    w.Watch.InTransactionPrice = model.InTransactionPrice;
                    w.Watch.TransactionType = 5;
                    w.Watch.Status = 0;
                    w.Size = model.SizeWrist;
                    w.Watch.GlassType = model.GlassType;
                    w.Watch.MainColor = model.MainColor;
                    //Check watch nao hop dk
                    //Do check ?

                    WatchShopEntities ctx = new WatchShopEntities();
                    ctx.WristWatches.Add(w);
                    ctx.Watches.Add(w.Watch);
                    if (ctx.SaveChanges() == 2)
                    {
                        return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thành Công" });
                    }
                }
                else if (watchType.Equals("Quả Lắc"))
                {
                    Timepeice t = new Timepeice();
                    t.Watch = new Watch();
                    t.Watch.Name = model.Name;
                    t.Watch.CaseMeterial = model.CaseMeterial;
                    t.Watch.Images = images;
                    t.Size = model.SizeWrist;
                    t.Watch.InTransactionPrice = model.InTransactionPrice;
                    t.Watch.TransactionType = 5;
                    t.Watch.Status = 0;
                    t.Watch.GlassType = model.GlassType;
                    t.Watch.MainColor = model.MainColor;

                    //Check watch nao hop dk
                    //Do check ?

                    WatchShopEntities ctx = new WatchShopEntities();
                    ctx.Timepeices.Add(t);
                    ctx.Watches.Add(t.Watch);
                    if (ctx.SaveChanges() == 2)
                    {
                        return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thành Công" });
                    }
                }
                else if (watchType.Equals("Treo Tường"))
                {
                    WallLock wa = new WallLock();
                    wa.Watch = new Watch();
                    wa.Watch.Name = model.Name;
                    wa.Watch.CaseMeterial = model.CaseMeterial;
                    wa.Watch.Images = images;
                    wa.Size = model.SizeWrist;
                    wa.Watch.InTransactionPrice = model.InTransactionPrice;
                    wa.Watch.TransactionType = 5;
                    wa.Watch.Status = 0;
                    wa.Shape = collection["glassShape"];
                    wa.Watch.GlassType = model.GlassType;
                    wa.Watch.MainColor = model.MainColor;

                    //Check watch nao hop dk
                    //Do check ?

                    WatchShopEntities ctx = new WatchShopEntities();
                    ctx.WallLocks.Add(wa);
                    ctx.Watches.Add(wa.Watch);
                    if (ctx.SaveChanges() == 2)
                    {
                        return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thành Công" });
                    }
                }
            }

            //return ShowMessage("Dat Mua that bai!");
            return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thất Bại" });
        }

        public ActionResult ShowMessage(String message)
        {
            ViewBag.Message = message;
            return View();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateSellingRequest()
        {
            using (var ctx = new WatchShopEntities())
            {
                //var models = from s in model.Models select s;
                //var stores = from s in model.Stores select s;
                //ViewBag.models = models.ToList();
                //ViewBag.stores = stores.ToList();

                var models = ctx.Models.ToList();
                var stores = ctx.Stores.ToList();
                ViewBag.Models = models;
                ViewBag.Stores = stores;
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
                    var fileName = new TimeSpan(DateTime.Now.Date.Ticks).TotalMilliseconds.ToString() + "." + file.FileName.Substring(file.FileName.IndexOf(".") + 1);
                    var path = Path.Combine(Server.MapPath("~/Images/Upload/"), fileName);
                    userWatch.Images = "/Images/Upload/" + fileName;
                    file.SaveAs(path);
                }
            }

            using (var ctx = new WatchShopEntities())
            {
                userWatch.TransactionType = watchTransactionTypes.ElementAt(2).Key;
                userWatch.Status = watchStatuses.ElementAt(0).Key;
                var member = ctx.Members.SingleOrDefault(s => s.Id == userWatch.MemberId);
                if (member != null)
                {
                    member.Watches.Add(userWatch);
                    ctx.SaveChanges();
                    return Json("Confirm", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    member = new Member();
                    member.Id = userWatch.MemberId;
                    member.IsActive = true;
                    member.Phone = userWatch.MemberId;
                    member.Password = WatchSystem.GenerateCode();
                    member.RoleId = 1;
                    member.ConfirmCode = member.Password; // Use the same password to confirm.
                    ctx.Members.Add(member);
                    member.Watches.Add(userWatch);
                    ctx.SaveChanges();
                    return Json("Confirm", JsonRequestBehavior.AllowGet);
                }
            }

        }
		
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.Url != null && Request.ApplicationPath != null)
                //ViewBag.baseUrl = Request.Url.Scheme + "://";
                ViewBag.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            base.OnActionExecuting(filterContext);
        }

        // Advanced filter
        public ActionResult Filter(FormCollection collection)
        {
            return View();
        }
    }
}
