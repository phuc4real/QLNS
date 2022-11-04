using PagedList;
using QLNS.Code;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    }
}