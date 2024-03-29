﻿
using QLNS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly CSDLEntities db = new CSDLEntities();
        public ActionResult IsLogin()
        {
            ActionResult res = null;
            if (!SessionHelper.IsLogin())
            {
                TempData["error"] = "Vui lòng đăng nhập vào hệ thống";
                res = RedirectToAction("Index", "Auth");
            }
            return res;
        }
        // GET: Order
        public ActionResult Index()
        {
            if (IsLogin() != null) return IsLogin();
            return View(db.HOA_DON.Where(x => x.DELETED_AT == null).ToList());
        }

        public ActionResult Details(int id)
        {
            if (IsLogin() != null) return IsLogin();
            var list = db.CHI_TIET_HOA_DON.Where(s => s.MA_HD == id).OrderBy(x => x.MA_HD);

            return View(list);
        }
    }
}