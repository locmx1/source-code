using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group3_MVC4.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string account, string password)
        {
            return View();
        }

        public ActionResult MyTransaction()
        {
            //if (Session["Account"] == null)
            //{
            //    return RedirectToAction("LogIn", "Account");
            //}
            return View();
        }

        [HttpPost]
        public ActionResult BuyWatch()
        {
            Session["Cart"] = ""; // add shopping cart to session here.
            return RedirectToAction("ViewShoppingCart");
        }

    }
}
