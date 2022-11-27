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
        // GET: Supplier
        public ActionResult Index()
        {
            return View(db.NHA_CUNG_CAP.Where(x => x.DELETED_AT == null).ToList());
        }
    }
}