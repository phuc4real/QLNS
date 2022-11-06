using PagedList;
using QLNS.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult LoginPermission()
        {
            ActionResult res;
            if (!SessionHelper.IsLogin())
            {
                TempData["error"] = "Vui lòng đăng nhập vào hệ thống";
                res = RedirectToAction("Index", "Auth");
            }
            else if (SessionHelper.GetPermission() == "user") res = Redirect("/Sale");
                else res = Redirect("/Book");
            return res;
        }
        public ActionResult Index()
        {
            return LoginPermission();
        }
    }
}