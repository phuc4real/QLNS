using QLNS.Code;
using QLNS.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class AuthController : Controller
    {
        private readonly CSDLEntities db = new CSDLEntities();
        //Ma hoa SHA256
        static string Sha256(string str)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        //GET Trang dang nhap
        public ActionResult Index()
        {
            if (SessionHelper.IsLogin()) return Redirect("/");

            if (TempData["error"] != null) ViewBag.error = TempData["error"];
            if (TempData["message"] != null) ViewBag.message = TempData["message"];

            return View();
        }
        //POST Trang dang nhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            model.Password = Sha256(model.Password);
            var result = new AccountModel().Login(model.Username, model.Password);
            if (result && ModelState.IsValid)
            {
                SessionHelper.SetSession(new UserSession(model.Username));
                SessionHelper.SetPermission(new AccountModel().GetPermission(model.Username));
                return Redirect("/");
            }
            else
            { ViewBag.error = "Tài khoản hoặc mật khẩu không chính xác"; }
            return View(model);
        }
        //Dang xuat
        public ActionResult Logout()
        {
            SessionHelper.RemoveSession();
            return Redirect("/Auth");
        }
        //GET Doi mat khau
        public ActionResult ChangePassword()
        {
            if (!SessionHelper.IsLogin()) return RedirectToAction("IsLogin", "Home");
            var user = SessionHelper.GetSession().Get();
            TAI_KHOAN tk = db.TAI_KHOAN.Where(s => s.USERNAME == user).SingleOrDefault();
            tk.PASSWORD = "";
            return View(tk);
        }
        //POST Doi mat khau
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string USERNAME, string PASSWORD, string REPASSWORD)
        {
            if (!SessionHelper.IsLogin()) return RedirectToAction("IsLogin", "Home");
            TAI_KHOAN tk = db.TAI_KHOAN.Find(USERNAME);
            if (PASSWORD == REPASSWORD)
            {
                string hash = Sha256(PASSWORD);
                if (tk.PASSWORD != hash)
                {
                    tk.PASSWORD = hash;
                    if (ModelState.IsValid)
                    {
                        db.Entry(tk).State = EntityState.Modified;
                        db.SaveChanges();
                        SessionHelper.RemoveSession();
                        TempData["message"] = "Đổi mật khẩu thành công! Vui lòng đăng nhập lại";
                        return RedirectToAction("Index");
                    }
                    else
                    { ViewBag.error = "Lỗi xảy ra!"; }

                }
                else
                { ViewBag.error = "Không được dùng lại mật khẩu cũ!"; }
            }
            else
            { ViewBag.error = "Mật khẩu nhập lại không đúng!"; }
            return View(tk);
        }
        //GET Tao tai khoan
        public ActionResult Create(string id)
        {
            if (!SessionHelper.IsLogin()) return RedirectToAction("IsLogin", "Home");
            var res = db.TAI_KHOAN.Where(s => s.MA_NV == id).FirstOrDefault();
            if (res != null) { return RedirectToAction("Index", "Staff");}
            ViewBag.manv = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string MA_NV, string USERNAME, string PASSWORD, string REPASSWORD,string QUYEN)
        {
            if (!SessionHelper.IsLogin()) return RedirectToAction("IsLogin", "Home");

            TAI_KHOAN tk = new TAI_KHOAN
            {
                USERNAME = USERNAME,
                PASSWORD = Sha256(PASSWORD),
                QUYEN = QUYEN,
                MA_NV = MA_NV
            };

            var res = db.TAI_KHOAN.Where(s => s.USERNAME == USERNAME).FirstOrDefault();
            if (res == null) {
                if (PASSWORD == REPASSWORD)
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            db.TAI_KHOAN.Add(tk);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Staff");
                        }
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Error Save Data");
                    }
                }
                else ViewBag.error = "Mật khẩu nhập lại không đúng!";
            }
            else ViewBag.error = "Tên tài khoản đã được sử dụng!";
            return View();
        }
    }
}