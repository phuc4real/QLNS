using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class CartItem
    { 
        public SACH SachOrder { get; set; }
        public int Quantity { get; set; }
        public double SumPrice()
        {
            double s = (double)(Quantity * SachOrder.DON_GIA.OrderByDescending(e => e.NGAY_AP_DUNG).First().SO_TIEN);
            return s;
        }
    }
}