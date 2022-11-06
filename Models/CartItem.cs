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
            double s = (double)(Quantity * SachOrder.GIA_BAN);
            return s;
        }
    }
}