using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;
using System.IO;
using System.Web.UI;
using System.Web.WebPages;
using Image = System.Web.UI.WebControls.Image;
using System.Web.Script.Serialization;
using System.Drawing.Imaging;
using System.Text;
using Newtonsoft.Json;

namespace Group3_MVC4.Controllers
{
    public class HomeController : Controller
    {
        private const string wallClock = "Treo Tường";
        private const string wristWatch = "Đeo Tay";
        private const string timepiece = "Quả Lắc";

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
            using (var ctx = new WatchShopEntities())
            {
                var items = ctx.Watches.Where(s => s.Name.Contains("Loc")).ToList();
                var json = new JavaScriptSerializer().Serialize(items);
                return View();
            }
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

        //#ducnt
        public ActionResult Search()
        {
            return View();
        }
        //#ducnt
        [HttpPost]
        public ActionResult Search(string search)
        {
            var db = new WatchShopEntities();
            if (!string.IsNullOrEmpty(search))
            {
                // tìm watch với tên và transactiontype = 0, status = 1 hoặc 2
                var searchResult = db.Watches.Where(s => (s.Name.ToUpper().Contains(search.ToUpper()) && s.TransactionType == 0 && s.Status == 1) || (s.Name.ToUpper().Contains(search.ToUpper()) && s.TransactionType == 0 && s.Status == 2)).ToList();
                // ViewBag.SearchResult = searchResult;
                if (searchResult.Count != 0)
                {
                    return PartialView("_Search", searchResult);
                }
                return null;

                //var searchResult = db.Watches.Include("Name").Where(s => s.Name.Contains(search)).ToList();
                //var json = new JavaScriptSerializer().Serialize(searchResult);
                //return Json(json, JsonRequestBehavior.AllowGet);                   
            }
            return null;
        }

		//Minh
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
		
        public ActionResult BuyWatch()
        {
            BuyWatchModel model = new BuyWatchModel();
            return View(model);
        }


        //Minh
        [HttpPost]
        public ActionResult BuyWatch(BuyWatchModel model, FormCollection collection)
        {
            bool isHuman = true;
            //list of files user upload
            HttpFileCollectionBase list = Request.Files;
            bool isValidated = true;

            //Check captcha 
            if (Session["Captcha"] == null || Session["Captcha"].ToString() != collection["captcha"])
            {
                isHuman = false;
            }
            if (isHuman)
            {
                //check some input here

                if (isValidated)
                {

                    string phoneNumber = model.phoneNumber;
                    string watchType = model.ThreeType;
                    string brandName = model.Brand;
                    string modelName = model.ModelName;
                    int modelIdInput = 0;
                    double priceInput = model.InTransactionPrice;
                    Model mo = new Model();
                    Brand br = new Brand();
                    if (brandName.IsEmpty())
                    {
                        if (!modelName.IsEmpty())
                        {
                            using (var ctx = new WatchShopEntities())
                            {
                                if (ctx.Set<Model>().Where(m => m.Name == modelName).Count() < 1)
                                {
                                    mo.Name = modelName;
                                    mo.Price = priceInput;
                                    //Model ko ton tai, ko co Brand -> tao moi model thuoc Brand "Khac"
                                    br = ctx.Set<Brand>().Where(b => b.Name == "Khác").First();
                                    mo.Brand = br;
                                    ctx.Models.Add(mo);
                                    ctx.SaveChanges();
                                }
                                else
                                {
                                    mo = ctx.Set<Model>().Where(m => m.Name == modelName).First();
                                    
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var ctx = new WatchShopEntities())
                        {
                            if (ctx.Set<Brand>().Where(b => b.Name == brandName).Count() < 1)
                            {
                                br = new Brand();
                                br.Name = brandName;
                                ctx.Brands.Add(br);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                br = ctx.Set<Brand>().Where(b => b.Name == brandName).First();
                                
                            }
                            if (!model.ModelName.IsEmpty())
                            {
                                if (ctx.Set<Model>().Where(m => m.Name == model.ModelName).Count() < 1)
                                {
                                    mo = new Model();
                                    mo.Price = model.InTransactionPrice;
                                    mo.Brand = br;
                                }
                                else
                                {
                                    mo = ctx.Set<Model>().Where(m => m.Name == model.ModelName).First();
                                }
                                mo.Brand = br;
                            }
                            else
                            {
                                mo = new Model();
                                mo.Brand = br;
                                mo.Price = 0;
                                ctx.Models.Add(mo);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    if (watchType.Equals(wristWatch))
                    {
                        WristWatch w = new WristWatch();
                        w.Watch = new Watch();
                        w.Watch.Name = model.Name;
                        w.Watch.CaseMeterial = model.CaseMeterial;
                        //w.Watch.Model = mo;
                        //w.Gender = collection["gender"];
                        w.Gender = true;
                        //watchType cua Deo tay ???nam nu ?
                        w.WatchType = true;
                        //isSmartWatch ko dc null ???
                        w.IsSmartWatch = false;
                        w.WaterProof = model.WaterProof;
                        w.Watch.InTransactionPrice = model.InTransactionPrice;
                        w.Watch.TransactionType = 5;
                        w.Watch.Status = 0;
                        w.Size = model.SizeWrist;
                        w.Watch.GlassType = model.GlassType;
                        w.Watch.MainColor = model.MainColor;

                        Session["BuyWatchRequest"] = w;
                    }
                    else if (watchType.Equals(timepiece))
                    {
                        Timepeice t = new Timepeice();
                        t.Watch = new Watch();
                        //t.Watch.Model = mo;

                        t.Watch.Name = model.Name;
                        t.Watch.CaseMeterial = model.CaseMeterial;
                        t.Size = model.SizeWrist;
                        t.Watch.InTransactionPrice = model.InTransactionPrice;
                        t.Watch.TransactionType = 5;
                        t.Watch.Status = 0;
                        t.Watch.GlassType = model.GlassType;
                        t.Watch.MainColor = model.MainColor;
                        Session["BuyWatchRequest"] = t;
                    }
                    else if (watchType.Equals(wallClock))
                    {
                        WallLock wa = new WallLock();
                        wa.Watch = new Watch();
                        ///wa.Watch.Model = mo;
                        wa.Watch.Name = model.Name;
                        wa.Watch.CaseMeterial = model.CaseMeterial;
                        wa.Size = model.SizeWrist;
                        wa.Watch.InTransactionPrice = model.InTransactionPrice;
                        wa.Watch.TransactionType = 5;
                        wa.Watch.Status = 0;
                        wa.Shape = collection["glassShape"];
                        wa.Watch.GlassType = model.GlassType;
                        wa.Watch.MainColor = model.MainColor;
                        Session["BuyWatchRequest"] = wa;
                    }
                    Random rd = new Random();
                    int confirmNumber = rd.Next(1000, 9999);
                    Session["ConfirmNumber"] = confirmNumber.ToString();
                    //Send ma xac nhan...
                    Session["TypeWatch"] = watchType;
                    return RedirectToAction("ConfirmNumber", "Home");
                }
                return View(model);
            }
            return View(model);
        }

        //Minh
        public ActionResult ShowMessage(String message)
        {
            ViewBag.Message = message;
            return View();
        }

        //Minh
        public ActionResult GetCaptcha()
        {
            var rd = new Random();
            int number = rd.Next(1000, 9999);
            Session["Captcha"] = number;
            FileContentResult imgResult = null;

            using (var memory = new MemoryStream())
            using (var bitmap = new Bitmap(130, 30))
            using (var grap = Graphics.FromImage(bitmap))
            {
                grap.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                grap.SmoothingMode = SmoothingMode.AntiAlias;
                grap.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                grap.DrawString(number.ToString(), new Font("Tahoma", 14), Brushes.Red, 2, 3);
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                imgResult = this.File(memory.GetBuffer(), "/Images");
            }
            //return FileContextResult
            return imgResult;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateSellingRequest()
        {
            using (var ctx = new WatchShopEntities())
            {                
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
            Session["SellingRequest"] = userWatch;
            String enteredCaptcha = Request.Params.Get("Captcha");
            if (!enteredCaptcha.Equals(Session["Captcha"].ToString()))
            {
                return RedirectToAction("CreateSellingRequest");
            }
            Session["Captcha"] = null;            
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    Session["UploadFile"] = file;
                    
                }
            }
            return RedirectToAction("ConfirmSMSCode", "Home");
            
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

        //code by HoangTDH
        public ActionResult GetCapchar()
        {
            //Generate captcha string
            Random random = new Random();
            string sampleCharacter = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                captcha.Append(sampleCharacter[random.Next(sampleCharacter.Length)]);
            }
            Session["Captcha"] = captcha;

            //get captcha image path
            string folderPath = Server.MapPath("~/Images/Captcha/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }        
            string captchaPath = Path.Combine(folderPath,"captcha.jpg");

            //Generate captcha image
            int height = 40;
            int width = 120;
            Bitmap bmp = new Bitmap(width, height);
            RectangleF rectf = new RectangleF(10, 5, 0, 0);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(captcha.ToString(), new Font("Thaoma", 18, FontStyle.Italic | FontStyle.Strikeout), Brushes.Green, rectf);
            g.DrawRectangle(new Pen(Color.White), 1, 1, width - 2, height - 2);
            g.Flush();
            bmp.Save(captchaPath,ImageFormat.Jpeg);
            g.Dispose();
            bmp.Dispose();
            return base.File(captchaPath, "image/jpg");
        }

        //code by HoangTDH
        public ActionResult ConfirmSMSCode(FormCollection collection)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmSMSCode(String smsCode)
        {

            int count = Session["CountSmsCode"] == null ? 0 : (int)Session["CountSmsCode"];//kiem tra so lan nhap ma sms sai            
            //check sms code
            bool check = true;
            if (check)
            {
                //check spam
                bool checkSpam = true;
                if (checkSpam)
                {
                    Watch userWatch = (Watch)Session["SellingRequest"];
                    if (userWatch != null)
                    {
                        userWatch.TransactionType = 0;
                        userWatch.Status = 0;
                        HttpPostedFileBase file = (HttpPostedFileBase)Session["UploadFile"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = new TimeSpan(DateTime.Now.Date.Ticks).TotalMilliseconds.ToString() + "." + file.FileName.Substring(file.FileName.IndexOf(".") + 1);
                            //check folder is exists or not
                            string folderPath = Server.MapPath("~/Images/Upload/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            var path = Path.Combine(folderPath, fileName);

                            userWatch.Images = "/Images/Upload/" + fileName;
                            file.SaveAs(path);
                        }
                        using (var ctx = new WatchShopEntities())
                        {
                            var member = ctx.Members.SingleOrDefault(s => s.Id == userWatch.MemberId);
                            
                            if (member != null)//user exists
                            {
                                member.Watches.Add(userWatch);
                                ctx.SaveChanges();
                                //clear session
                                Session["SellingRequest"] = null;

                                return Json("Confirm", JsonRequestBehavior.AllowGet);
                            }
                            else//create new user
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
                    
                }
            }
            else
            {
                count++;
                if (count > 3)//nhap sai qua 3 lan thi clear request
                {
                    Session["SellingRequest"] = null;
                    Session["CountSmsCode"] = null;
                }
                else
                {
                    Session["CountSmsCode"] = count;
                    return View();
                }
            }
            return View();
        }
		
		//Minh
        public ActionResult ConfirmNumber()
        {
            return View();
        }


        // Shopping cart
        public ActionResult ViewCart()
        {
            return View();
        }
		
		//Minh
        [HttpPost]
        public ActionResult ConfirmNumber(FormCollection collection)
        {
            if (collection["confirmNumber"].Equals(Session["ConfirmNumber"]))
            {
                using (var ctx = new WatchShopEntities())
                {
                    string typeWatch = Session["TypeWatch"].ToString();
                    if (typeWatch.Equals(wristWatch))
                    {
                        WristWatch wrist = (WristWatch)Session["BuyWatchRequest"];
                        ctx.Watches.Add(wrist.Watch);
                        ctx.WristWatches.Add(wrist);
                    }
                    else if (typeWatch.Equals(wallClock))
                    {
                        WallLock wall = (WallLock)Session["BuyWatchRequest"];
                        ctx.Watches.Add(wall.Watch);
                        ctx.WallLocks.Add(wall);
                    }
                    else if (typeWatch.Equals(timepiece))
                    {
                        Timepeice time = (Timepeice)Session["BuyWatchRequest"];
                        ctx.Watches.Add(time.Watch);
                        ctx.Timepeices.Add(time);
                    }

                    ctx.SaveChanges();

                    return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thành Công" });

                    //return RedirectToAction("ShowMessage", "Home", new { message = "Đặt Mua Thất Bại" });
                }
            }
            return View();
        }
		
		public JsonResult GetListModel()
        {
            using (var ctx = new WatchShopEntities())
            {
                var models = ctx.Models.ToList();
                string json = "[";
                var lastModel = models.Last();
                foreach (var model in models)
                {
                    if (model.Equals(lastModel))
                    {
                        json += '{' + "\"Id\":\"" + model.Id.ToString() + "\",\"Name\" : \"" + model.Name.ToString() + "\" }]";
                    }
                    else
                    {
                        json += "{" + "\"Id\":\"" + model.Id.ToString() + "\",\"Name\" : \"" + model.Name.ToString() + "\" },";
                    }
                    
                }
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetListStore()
        {
            using (var ctx = new WatchShopEntities())
            {
                var stores = ctx.Stores.ToList();
                string json = "[";
                var lastStore = stores.Last();
                foreach (var store in stores)
                {
                    if (store.Equals(lastStore))
                    {
                        json += '{' + "\"Id\":\"" + store.Id.ToString() + "\",\"Name\" : \"" + store.Name.ToString() + "\" }]";
                    }
                    else
                    {
                        json += "{" + "\"Id\":\"" + store.Id.ToString() + "\",\"Name\" : \"" + store.Name.ToString() + "\" },";
                    }

                }
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
