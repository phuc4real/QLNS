using PagedList;
using QLNS.Code;
using QLNS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class SaleController : Controller
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
            return db.SACHes.Where(x => x.DELETED_AT == null).OrderBy(x => x.MA_SACH).ToPagedList(PageNumber, PageSize);
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
            SACH sach = db.SACHes.SingleOrDefault(s => s.MA_SACH == id);
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
        //Tim kiem sach
        public ActionResult SearchBook(string term, int? page)
        {
            if (IsLogin() != null) return IsLogin();
            if (term == null) return RedirectToAction("Index");
            if (page == null) page = 1;
            var s = term.ToLower();
            var sachs = db.SACHes.Where(x => x.DELETED_AT == null);
            var result = sachs.Where(b => b.MA_SACH.ToLower().Contains(s) ||
                                   b.TEN_SACH.ToLower().Contains(s) ||
                                   b.TAC_GIA.ToLower().Contains(s) ||
                                   b.NHA_XB.ToLower().Contains(s))
                            .OrderBy(x => x.MA_SACH)
                            .ToList();
            int PageSize = 8;
            int PageNumber = (page ?? 1);
            return View(result.ToPagedList(PageNumber, PageSize));
        }
        //Cap nhat gio hang
        [HttpPost]
        public JsonResult UpdateCart(string id, string quality)
        {
            List<CartItem> listCartItem = (List<CartItem>)Session["ShoppingCart"];
            double sum = 0, total = 0;
            string err = "";
            int cartcount = 0;
            if (!int.TryParse(quality, out int a) || int.Parse(quality) < 0)
            {
                err = "Số lượng không hợp lệ";
                foreach (CartItem item in listCartItem)
                {
                    if (item.SachOrder.MA_SACH == id)
                    {
                        a = item.Quantity;
                        sum = item.SumPrice();
                        break;
                    }
                }
            }
            else
            {
                if (Session["ShoppingCart"] != null)
                {
                    foreach (CartItem item in listCartItem)
                    {
                        if (item.SachOrder.MA_SACH == id)
                        {
                            item.Quantity = a;
                            sum = item.SumPrice();
                            break;
                        }
                    }
                    Session["ShoppingCart"] = listCartItem;
                }
            }
            foreach (CartItem i in listCartItem)
            {
                total += i.SumPrice();
                cartcount += i.Quantity;
            }

            return Json(new { ItemAmount = a, SumPrice = sum, Total = total, Error = err, cartcount });
        }
        //Them vao gio hang
        [HttpPost]
        public JsonResult AddToCart(string id)
        {
            //Process Add To Cart
            List<CartItem> listCardItem;
            if (Session["ShoppingCart"] == null)
            {
                //Create New Shopping Cart Session 
                listCardItem = new List<CartItem>
                {
                    new CartItem { Quantity = 1, SachOrder = db.SACHes.Find(id) }
                };
                Session["ShoppingCart"] = listCardItem;
            }
            else
            {
                bool flag = false;
                listCardItem = (List<CartItem>)Session["ShoppingCart"];
                foreach (CartItem item in listCardItem)
                {
                    if (item.SachOrder.MA_SACH == id)
                    {
                        item.Quantity++; flag = true;
                        break;
                    }
                }

                if (!flag)
                    listCardItem.Add(new CartItem { Quantity = 1, SachOrder = db.SACHes.Find(id) });

                Session["ShoppingCard"] = listCardItem;
            }
            //Count item in shopping cart 
            int cartcount = 0;
            List<CartItem> ls = (List<CartItem>)Session["ShoppingCart"];
            foreach (CartItem item in ls)
            {
                cartcount += item.Quantity;
            }
            return Json(new { ItemAmount = cartcount });
        }

        [HttpPost]
        public RedirectToRouteResult RemoveCart(string id)
        {
            List<CartItem> cart = (List<CartItem>)Session["ShoppingCart"];
            CartItem remove = cart.SingleOrDefault(m => m.SachOrder.MA_SACH == id);
            if (remove != null)
            {
                cart.Remove(remove);
            }

            return RedirectToAction("ShoppingCart");
        }
        public ActionResult ShoppingCart()
        {
            if (IsLogin() != null) return IsLogin();
            return View();
        }

        public ActionResult Payment(double Total)
        {
            List<CartItem> cart = (List<CartItem>)Session["ShoppingCart"];
            var user = SessionHelper.GetSession().Get();
            var nv = db.TAI_KHOAN.Where(x => x.USERNAME == user).SingleOrDefault().NHAN_VIEN;
            //Them hoa don
            HOA_DON hd = new HOA_DON
            {
                NHAN_VIEN = nv,
                NGAY_LAP = DateTime.Now,
                THANH_TIEN = Total
            };
            db.HOA_DON.Add(hd);
            db.SaveChanges();

            //Them chi tiet hoa don
            foreach (CartItem item in cart)
            {
                CHI_TIET_HOA_DON cthd = new CHI_TIET_HOA_DON()
                {
                    MA_HD = hd.MA_HD,
                    MA_SACH = item.SachOrder.MA_SACH,
                    SL_SACH = item.Quantity
                };
                db.CHI_TIET_HOA_DON.Add(cthd);
                SACH sach = db.SACHes.Find(cthd.MA_SACH);
                sach.SL_BAN+=cthd.SL_SACH;
                sach.SL_TON -= cthd.SL_SACH;

                db.SaveChanges();
            }

            Session.Remove("ShoppingCart");

            return Redirect("/Invoice");
        }

        public ActionResult BookOnSale(string id, int? page)
        {
            if (IsLogin() != null) return IsLogin();
            if (id == null) return RedirectToAction("Index");
            if (page == null) page = 1;;
            List<CHI_TIET_KHUYEN_MAI> list = db.CHI_TIET_KHUYEN_MAI.Where(x =>x.DELETED_AT == null && x.MA_KM == id).ToList();
            List<SACH> sach = new List<SACH>();
            foreach (CHI_TIET_KHUYEN_MAI item in list)
            {
                sach.Add(item.SACH);
            }
            int PageSize = 8;
            int PageNumber = (page ?? 1);
            return View(sach.ToPagedList(PageNumber, PageSize));
        }
    }
}