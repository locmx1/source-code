using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;

namespace Group3_MVC4.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        [HttpPost]
        public ActionResult ConfirmAccount(string code)
        {
            using (var ctx = new WatchShopEntities())
            {
                var item = ctx.Members.SingleOrDefault(m => m.ConfirmCode.Equals(code));
                if (item != null)
                {
                    return Json("Done", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
        }

    }
}
