﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}
