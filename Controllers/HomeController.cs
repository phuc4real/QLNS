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
        private readonly CSDLEntities db = new CSDLEntities();
        public ActionResult IsLogin()
        {
            ActionResult res = null;
            if (!SessionHelper.IsLogin()) {
                TempData["error"] = "Vui lòng đăng nhập vào hệ thống";
                res = RedirectToAction("Index", "Auth");
            }
            return res;
        }
        public IEnumerable<SACH> BooksPaged(int? page)
        {
            int PageSize = 8;
            int PageNumber = (page ?? 1);
            return db.SACHes.OrderBy(x => x.MA_SACH).ToPagedList(PageNumber, PageSize);
        }
        //Danh sach sach
        public ActionResult Index(int? page)
        {
            if (IsLogin()!=null) return IsLogin();
            if (page == null) page = 1;
            var list = BooksPaged(page);
            return View(list);
        }

        //Danh sach the loai
        private List<SelectListItem> GetGenres()
        {
            var listGenre = new List<SelectListItem>();
            listGenre = db.THE_LOAI.Select(t => new SelectListItem()
            {
                Value = t.MA_TL.ToString(),
                Text = t.TEN_TL
            }).ToList();

            return listGenre;
        }
        //Danh sach dang sach
        private List<SelectListItem> GetTypes()
        {
            var listType = new List<SelectListItem>();
            listType = db.DANG_SACH.Select(t => new SelectListItem()
            {
                Value = t.MA_DS.ToString(),
                Text = t.TEN_DS
            }).ToList();

            return listType;
        }
        //Thong tin chi tiet
        public ActionResult Details(string id)
        {
            if (IsLogin() != null) return IsLogin();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            SACH sach = db.SACHes.SingleOrDefault(s => s.MA_SACH == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.Genres = GetGenres();
            ViewBag.Types = GetTypes();

            return View(sach);
        }
        //Them anh bia
        private string UploadFile(HttpPostedFileBase fileUpload, string maSach)
        {
            if (fileUpload == null) return null;
            var extension = Path.GetExtension(fileUpload.FileName);
            var fileName = Guid.NewGuid() + maSach + extension;
            var path = Path.Combine(Server.MapPath("~/Assets/img/book/"), fileName);
            if (!Directory.Exists(Server.MapPath("~/Assets/img/book/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Assets/img/book/"));
            }
            fileUpload.SaveAs(path);
            return fileName;
        }
        //Xoa anh bia
        private void DeleteFile(string img)
        {
            if (img == null) return;
            var path = Path.Combine(Server.MapPath("~/Assets/img/book/"), img).ToString();
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        //Them sach
        public ActionResult Create()
        {
            if (IsLogin() != null) return IsLogin();
            ViewBag.Genres = GetGenres();
            ViewBag.Types = GetTypes();

            return View();
        }
        //POST Them sach
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SACH sach, HttpPostedFileBase fileUpload)
        {
            if (IsLogin() != null) return IsLogin();
            var dup = db.SACHes.SingleOrDefault(s => s.MA_SACH == sach.MA_SACH);
            if (dup == null)
                try
                {
                if (ModelState.IsValid)
                {
                    sach.ANH_BIA = UploadFile(fileUpload, sach.MA_SACH);
                    db.SACHes.Add(sach);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Genres = GetGenres();
                    ViewBag.Types = GetTypes();
                    return View(sach);
                }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            else
            {
                ViewBag.error = "Sách đã tồn tại trong hệ thống";
                ViewBag.Genres = GetGenres();
                ViewBag.Types = GetTypes();
                return View(sach);
            }
            return Redirect("/");
        }
        //POST Sua thong tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string MA_SACH, HttpPostedFileBase fileUpload)
        {
            var sachSua = db.SACHes.Find(MA_SACH);
            sachSua.ANH_BIA = UploadFile(fileUpload, MA_SACH);
            string path = db.SACHes.Where(s => s.MA_SACH == MA_SACH).Select(s => s.ANH_BIA).Single();
            if (sachSua.ANH_BIA != null)
            {
                DeleteFile(path);
            }
            else sachSua.ANH_BIA = path;
            if (TryUpdateModel(sachSua, "", new string[] { "TEN_SACH", "ANH_BIA", "MA_DS", "MA_TL", "GIA_BAN", "TAC_GIA", "NHA_XB", "NAM_XB", "SOLUONG", "MO_TA_SACH" }))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Entry(sachSua).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Error Save Data");
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(sachSua);
        }
        //Xoa sach
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (IsLogin() != null) return IsLogin();
            //Xoa sach
            SACH sach = db.SACHes.SingleOrDefault(s => s.MA_SACH == id);
            db.SACHes.Remove(sach);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}