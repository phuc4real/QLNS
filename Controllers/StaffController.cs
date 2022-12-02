using PagedList;
using QLNS.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class StaffController : Controller
    {

        private readonly CSDLEntities db = new CSDLEntities();

        public ActionResult LoginPermission()
        {
            ActionResult res = null;
            if (!SessionHelper.IsLogin()) {
                TempData["error"] = "Vui lòng đăng nhập vào hệ thống";
                res =  RedirectToAction("Index", "Auth");
            }
            else if (SessionHelper.GetPermission() != "admin") res = Redirect("/");
            return res;
        }
        public IEnumerable<NHAN_VIEN> StaffsPaged(int? page)
        {
            int PageSize = 4;
            int PageNumber = (page ?? 1);
            return db.NHAN_VIEN.Where(x => x.DELETED_AT == null).OrderBy(x => x.MA_NV).ToPagedList(PageNumber, PageSize);
        }
        // GET: Staff
        public ActionResult Index(int? page)
        {
            if (LoginPermission() != null) return LoginPermission();
            if (page == null) page = 1;
            var list = StaffsPaged(page);
            return View(list);
        }
        //Xoa nhan vien
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (LoginPermission() != null) return LoginPermission();
            //Xoa tai khoan
            TAI_KHOAN tk = db.TAI_KHOAN.Where(s => s.MA_NV == id).SingleOrDefault();
            if (tk != null) tk.DELETED_AT = DateTime.Now; //db.TAI_KHOAN.Remove(tk);
            //Xoa nhan vien
            NHAN_VIEN nv = db.NHAN_VIEN.Find(id);
            nv.DELETED_AT = DateTime.Now;
            //db.NHAN_VIEN.Remove(nv);

            db.SaveChanges();

            if (tk != null) if (tk.USERNAME == SessionHelper.GetSession().Get()) return RedirectToAction("Logout", "Auth");
            return RedirectToAction("Index");
        }
        //GET Them nhan vien
        public ActionResult Create()
        {
            if (LoginPermission() != null) return LoginPermission();
            return View();
        }
        //POST Them nhan vien
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NHAN_VIEN nv)
        {
            if (LoginPermission() != null) return LoginPermission();
            var dup = db.NHAN_VIEN.Where(s => s.MA_NV == nv.MA_NV).FirstOrDefault();
            if (dup == null)
            {
                nv.NGAY_GIA_NHAP = DateTime.Now;
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.NHAN_VIEN.Add(nv);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }
            ViewBag.error = "Mã nhân viên đã tồn tại";
            return View(nv);
        }
        public ActionResult Detail(string id)
        {
            if (LoginPermission() != null) return LoginPermission();
            NHAN_VIEN nv = db.NHAN_VIEN.Where(s => s.MA_NV == id).FirstOrDefault();

            var listJob = new List<CO_CV>();
            listJob = nv.CO_CV.ToList();
            ViewBag.listJob = listJob;
            return View(nv);
        }

        //POST Sua nhan vien
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NHAN_VIEN nv)
        {
            if (LoginPermission() != null) return LoginPermission();
            if (ModelState.IsValid)
            {
                db.Entry(nv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nv);
        }
    }
}