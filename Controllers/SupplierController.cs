using PagedList;
using QLNS.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class SupplierController : Controller
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
        // GET: Supplier
        public ActionResult Index()
        {
            if (IsLogin() != null) return IsLogin();
            return View(db.NHA_CUNG_CAP.Where(x => x.DELETED_AT == null).ToList());
        }

        public ActionResult Create()
        {
            if (IsLogin() != null) return IsLogin();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NHA_CUNG_CAP ncc)
        {
            if(IsLogin() != null) return IsLogin();
            var dup = db.NHA_CUNG_CAP.Find(ncc.MA_NCC);
            if (dup == null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.NHA_CUNG_CAP.Add(ncc);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }
            ViewBag.error = "Nhà cung cấp đã tồn tại";
            return View(dup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (IsLogin() != null) return IsLogin();
            NHA_CUNG_CAP ncc = db.NHA_CUNG_CAP.Find(id);
            ncc.DELETED_AT = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (IsLogin() != null) return IsLogin();
            NHA_CUNG_CAP ncc = db.NHA_CUNG_CAP.Find(id);
            return View(ncc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NHA_CUNG_CAP ncc)
        {
            if (IsLogin() != null) return IsLogin();
            if (ModelState.IsValid)
            {
                db.Entry(ncc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ncc);
        }
    }
}