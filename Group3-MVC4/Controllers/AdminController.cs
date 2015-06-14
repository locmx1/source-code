using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;
using Group3_MVC4.net.webservicex.www;
using HtmlAgilityPack;

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

        //#ducnt
        public ActionResult CollectingData()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CollectingData(string link)
        {
            using (var db = new WatchShopEntities())
            {             
   
            var getHtmlWeb = new HtmlWeb();
            var document = getHtmlWeb.Load(link);
            net.webservicex.www.CurrencyConvertor convert = new CurrencyConvertor();

            //find a div contain 
            var ulNodes = document.DocumentNode.SelectNodes("//ul[@class='product-block']");
            int counter = 1;
            List<String> links = new List<String>();
            List<String> list = new List<String>();
            if (ulNodes != null)
            {
                foreach (HtmlNode node in ulNodes)
                {
                    HtmlNodeCollection childs = node.ChildNodes;
                    foreach (var child in childs)
                    {
                        if (child.Name.Equals("li"))
                        {
                            IEnumerable<HtmlNode> aLinks = child.Descendants("a");
                            links.Add(aLinks.ElementAt(0).GetAttributeValue("href", "Link null."));
                        }
                    }
                }
            }

            if (links != null)
            {
                foreach (var href in links)
                {
                    var getHtmlLink = new HtmlWeb();
                    var nodeHref = getHtmlLink.Load(href);
                    var divNodeDetails = nodeHref.DocumentNode.SelectNodes("//div[contains(@class,'descript-holder')]");
                    if (divNodeDetails != null)
                    {
                        foreach (var htmlTag in divNodeDetails)
                        {
                            try
                            {
                                var brandWatch = new Brand();
                                var modelWatch = new Model();
                                var price = htmlTag.SelectSingleNode("//dd[@rel='price']").InnerText.Split('$')[1];
                                var brand = htmlTag.SelectSingleNode("//h1[contains(@class,'brand')]").InnerText;
                                var model = htmlTag.SelectSingleNode("//span[contains(@class,'descript-title')]").InnerText;
                                var priceExchange = double.Parse(price) * convert.ConversionRate(Currency.USD, Currency.VND);
                                var priceFormat = string.Format("{0:0}", priceExchange);

                                if (CheckBrand(brand) == true)
                                {
                                    brandWatch.Name = brand;
                                    db.Brands.Add(brandWatch);
                                    db.SaveChanges();
                                    modelWatch.Name = model;
                                    modelWatch.Price = float.Parse( priceFormat);
                                    modelWatch.BrandId = brandWatch.Id;
                                    db.Models.Add(modelWatch);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    if (CheckModel(model)==true)
                                    {
                                        var brandId = db.Brands.SingleOrDefault(s => s.Name.Equals(brand));
                                        modelWatch.Name = model;
                                        modelWatch.Price = float.Parse(priceFormat);
                                        modelWatch.BrandId = brandId.Id;
                                        db.Models.Add(modelWatch);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        var modelId = db.Models.FirstOrDefault(a => a.Name.Equals(model));
                                        modelId.Price = float.Parse(priceFormat);
                                        db.SaveChanges();
                                    }
                                }

                                var res = counter + " - " + "Hiệu: " + brand + " - " + "Giá: " + priceFormat + " - " + "Model: " + model;
                                counter++;
                                list.Add(res);
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            Response.ContentType = "text/html";
            ViewBag.List = list;
            return PartialView("_CollectingData");
            }
        }
        //#ducnt
        public Boolean CheckBrand(string name)
        {
            using (var db = new WatchShopEntities())
            {
                var brand = db.Brands.SingleOrDefault(b => b.Name.Equals(name));
                if (brand != null)
                {
                    return false;
                }
                return true;
            }
            
        }
        //#ducnt
        public Boolean CheckModel(string name)
        {
            using (var db = new WatchShopEntities())
            {
                var model = db.Models.SingleOrDefault(b => b.Name.Equals(name));
                if (model != null)
                {
                    return false;
                }
                return true;
            }
        }
        //#ducnt
        public Boolean CheckPriceSpam(string nameWatch, string price)
        {
            // hàm này cần phải hỏi lại ......
            using (var db = new WatchShopEntities())
            {
                //hỏi ngay chỗ này, chỉ nhập tên ko có model thì làm sao check giá trong model
                var watch = db.Watches.FirstOrDefault(s => s.Name.Contains(nameWatch));
                if (watch != null)
                {
                    var model = db.Models.Where(s => s.Id == watch.Id).FirstOrDefault();
                    if (model != null)
                    {
                        var maxPrice = model.Price;
                        if (double.Parse(price) < maxPrice)
                        {
                            return true; // ko spam
                        }
                        return false; //false là spam
                    }                                        
                }
                return true;
                // true, chưa tồn tại watch với model trong sys => ko spam
                //lưu model + giá làm maxprice
            }
        }
        //#ducnt
        public Boolean CheckSpam(string nameWatch, string phoneMember, string priceWatch)
        {
            using (var db = new WatchShopEntities())
            {
                var member = db.Members.FirstOrDefault(s => s.Phone == phoneMember);
                if (member != null)
                {
                    //Đã có Member trong Sys
                    //Check thông tin đưa vào (có thể thêm yếu tố khác ngoài Watch Name)
                    var watch = db.Watches.FirstOrDefault(s => s.MemberId == member.Id);
                    if (watch != null)
                    {
                        if (watch.Name.Equals(nameWatch) == true )
                        {
                            return false; // cùng 1 member, cùng 1 thông tin trong request => spam
                        }
                        else
                        {
                            if (CheckPriceSpam(nameWatch, priceWatch) == true)
                            {
                                return true; // ko spam
                            }                            
                            return false; // spam price
                        }
                    }
                    else
                    {
                        if (CheckPriceSpam(nameWatch, priceWatch) == true)
                        {
                            return true; //Chưa có Watch trong Sys + Price OK => Ko Spam
                        }
                        return false; // Chưa có Watch trong Sys, nhưng Price spam => Spam                     
                    }
                }
                else
                {
                    return true; // Chưa có Member trong Sys => Ko Spam
                }
            }         
        }
    }
}
