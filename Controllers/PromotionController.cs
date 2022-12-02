using QLNS.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class PromotionController : Controller
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
        // GET: Promotion
        public ActionResult Index()
        {
            if (IsLogin() != null) return IsLogin();
            return View(db.KHUYEN_MAI.Where(x => x.DELETED_AT == null).ToList());
        }
        public ActionResult Create()
        {
            if (IsLogin() != null) return IsLogin();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KHUYEN_MAI km)
        {
            if (IsLogin() != null) return IsLogin();
            var dup = db.KHUYEN_MAI.Find(km.MA_KM);
            if (dup == null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.KHUYEN_MAI.Add(km);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }
            ViewBag.error = "Khuyến mãi đã tồn tại";
            return View(dup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (IsLogin() != null) return IsLogin();
            KHUYEN_MAI km = db.KHUYEN_MAI.Find(id);
            km.DELETED_AT = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            if (IsLogin() != null) return IsLogin();
            KHUYEN_MAI km = db.KHUYEN_MAI.Find(id);
            return View(km);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KHUYEN_MAI km)
        {
            if (IsLogin() != null) return IsLogin();
            if (ModelState.IsValid)
            {
                db.Entry(km).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(km);
        }
    }
}