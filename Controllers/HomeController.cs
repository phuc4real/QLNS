using PagedList;
using QLNS.Code;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class HomeController : Controller
    {
        private readonly csdlEntities db = new csdlEntities();
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
    }
}