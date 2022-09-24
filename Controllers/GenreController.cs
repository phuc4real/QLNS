using QLNS.Code;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class GenreController : Controller
    {
        private readonly csdlEntities db = new csdlEntities();

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
        //GET index
        public ActionResult Index()
        {
            if (IsLogin()!=null) return IsLogin();
            return View(db.THE_LOAI.ToList());
        }
        //GET Them the loai
        public ActionResult Create()
        {
            if (IsLogin()!=null) return IsLogin();
            return View();
        }
        //POST Them the loai
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(THE_LOAI tl)
        {
            if (IsLogin()!=null) return IsLogin();
            var dup = db.THE_LOAI.Where(s => s.MA_TL == tl.MA_TL).FirstOrDefault();
            if (dup == null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.THE_LOAI.Add(tl);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }
            return View(tl);
        }
        //GET Sua the loai
        public ActionResult Edit(string id)
        {
            if (IsLogin()!=null) return IsLogin();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THE_LOAI tl = db.THE_LOAI.Find(id);
            if (tl == null)
            {
                return HttpNotFound();
            }
            return View(tl);
        }
        //POST Sua the loai
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(THE_LOAI tl)
        {
            if (IsLogin()!=null) return IsLogin();
            if (ModelState.IsValid)
            {
                db.Entry(tl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tl);
        }
        //Xoa the loai
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (IsLogin()!=null) return IsLogin();
            THE_LOAI tl = db.THE_LOAI.Find(id);
            db.THE_LOAI.Remove(tl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
