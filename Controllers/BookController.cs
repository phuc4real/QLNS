using PagedList;
using QLNS.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class BookController : Controller
    {
        private readonly CSDLEntities db = new CSDLEntities();
        //Kiem tra trang thai login
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
        public IEnumerable<SACH> BooksPaged(int? page)
        {
            int PageSize = 8;
            int PageNumber = (page ?? 1);
            return db.SACHes.Where(x=> x.DELETED_AT == null).OrderBy(x => x.MA_SACH).ToPagedList(PageNumber, PageSize);
        }
        //Danh sach san pham sach
        public ActionResult Index(int? page)
        {
            if (IsLogin() != null) return IsLogin();
            if (page == null) page = 1;
            var list = BooksPaged(page);
            return View(list);
        }

        //Lay ds the loai
        private List<SelectListItem> GetGenres()
        {
            var listGenre = new List<SelectListItem>();
            listGenre = db.THE_LOAI.Where(x => x.DELETED_AT == null).Select(t => new SelectListItem()
            {
                Value = t.MA_TL.ToString(),
                Text = t.TEN_TL
            }).ToList();

            return listGenre;
        }
        //Lay ds dang sach
        private List<SelectListItem> GetTypes()
        {
            var listType = new List<SelectListItem>();
            listType = db.DANG_SACH.Where(x => x.DELETED_AT == null).Select(t => new SelectListItem()
            {
                Value = t.MA_DS.ToString(),
                Text = t.TEN_DS
            }).ToList();

            return listType;
        }
        //Lay ds do duoi
        private List<SelectListItem> GetAges()
        {
            var listAge = new List<SelectListItem>();
            listAge = db.DO_TUOI.Where(x => x.DELETED_AT == null).Select(t => new SelectListItem()
            {
                Value = t.MA_DT.ToString(),
                Text = t.TEN_DT
            }).ToList();

            return listAge;
        }

        //Thong tin chi tiet
        public ActionResult Details(string id)
        {
            if (IsLogin() != null) return IsLogin();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            SACH sach = db.SACHes.SingleOrDefault(s => s.MA_SACH == id && s.DELETED_AT == null);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.Genres = GetGenres();
            ViewBag.Types = GetTypes();
            ViewBag.Ages = GetAges();

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
            ViewBag.Ages = GetAges();

            return View();
        }
        //POST Them sach
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SACH sach, String SO_TIEN, HttpPostedFileBase fileUpload)
        {
            if (IsLogin() != null) return IsLogin();
            var dup = db.SACHes.SingleOrDefault(s => s.MA_SACH == sach.MA_SACH);
            if (dup == null)
                try
                {
                    if (ModelState.IsValid)
                    {
                        sach.ANH_BIA = UploadFile(fileUpload, sach.MA_SACH);
                        DON_GIA dg = new DON_GIA
                        {
                            SO_TIEN = int.Parse(SO_TIEN),
                            NGAY_AP_DUNG = DateTime.Now,
                            SACH = sach
                        };
                        db.SACHes.Add(sach);
                        db.DON_GIA.Add(dg);
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
        public ActionResult Edit(string MA_SACH, String SO_TIEN, HttpPostedFileBase fileUpload)
        {
            var sachSua = db.SACHes.Find(MA_SACH);
            DON_GIA dg = new DON_GIA
            {
                SO_TIEN = int.Parse(SO_TIEN),
                NGAY_AP_DUNG = DateTime.Now,
                SACH = sachSua
            };
            db.DON_GIA.Add(dg);

            sachSua.ANH_BIA = UploadFile(fileUpload, MA_SACH);
            string path = db.SACHes.Where(s => s.MA_SACH == MA_SACH).Select(s => s.ANH_BIA).Single();
            if (sachSua.ANH_BIA != null)
            {
                DeleteFile(path);
            }
            else sachSua.ANH_BIA = path;
            if (TryUpdateModel(sachSua, "", new string[] { "TEN_SACH", "ANH_BIA", "MA_DS", "MA_TL", "MA_DT", "TAC_GIA", "NHA_XB", "NAM_XB", "SL_TON", "SL_BAN",  "MO_TA_SACH" }))
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
            sach.DELETED_AT = DateTime.Now;
            //db.SACHes.Remove(sach);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SearchBook(string term, int? page)
        {
            if (IsLogin() != null) return IsLogin();
            if (term == null) return Redirect("/");
            if (page == null) page = 1;
            var s = term.ToLower();
            var sachs = db.SACHes.Where(x => x.DELETED_AT == null);
            var result = sachs.Where(b => b.MA_SACH.ToLower().Contains(s) ||
                                    b.TEN_SACH.ToLower().Contains(s) ||
                                    b.TAC_GIA.ToLower().Contains(s) ||
                                    b.NHA_XB.ToLower().Contains(s)
                                  )
                            .OrderBy(x => x.MA_SACH)
                            .ToList();
            int PageSize = 8;
            int PageNumber = (page ?? 1);
            return View(result.ToPagedList(PageNumber, PageSize));
        }
    }
}