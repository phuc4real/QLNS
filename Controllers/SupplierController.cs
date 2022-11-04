using PagedList;
using QLNS.Code;
using System;
using System.Collections.Generic;
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
        public IEnumerable<NHA_CUNG_CAP> BooksPaged(int? page)
        {
            int PageSize = 4;
            int PageNumber = (page ?? 1);
            return db.NHA_CUNG_CAP.OrderBy(x => x.MA_NCC).ToPagedList(PageNumber, PageSize);
        }
        // GET: Staff
        public ActionResult Index(int? page)
        {
            if (IsLogin() != null) return IsLogin();
            if (page == null) page = 1;
            var list = BooksPaged(page);
            return View(list);
        }
    }
}