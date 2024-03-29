﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DON_GIA
    {
        [Display(Name = "Ngày nhập")]
        public int MA_DG { get; set; }
        [Display(Name = "Giá bán")]
        [Required(ErrorMessage = "Phải nhập vào một số!")]
        [DisplayFormat(DataFormatString = "{0:#,###.## vnđ}")]
        [Range(0, int.MaxValue, ErrorMessage = "Hãy nhập số tiền hợp lệ!")]
        public Nullable<double> SO_TIEN { get; set; }
        [Display(Name = "Ngày áp dụng")]
        [Required(ErrorMessage = "Phải nhập vào một số!")]
        public Nullable<System.DateTime> NGAY_AP_DUNG { get; set; }
        [Display(Name = "Mã sách")]
        [Required(ErrorMessage = "Phải nhập vào một số!")]
        public string MA_SACH { get; set; }
        public Nullable<System.DateTime> DELETED_AT { get; set; }
    
        public virtual SACH SACH { get; set; }
    }
}
