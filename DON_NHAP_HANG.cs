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

    public partial class DON_NHAP_HANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DON_NHAP_HANG()
        {
            this.CHI_TIET_NHAP_HANG = new HashSet<CHI_TIET_NHAP_HANG>();
        }

        [Display(Name = "Mã đơn nhập")]
        public int MA_DON_NHAP { get; set; }
        [Display(Name = "Mã nhân viên")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string MA_NV { get; set; }
        [Display(Name = "Ngày nhập")]
        [Required(ErrorMessage = "Không được để trống!")]
        public Nullable<System.DateTime> NGAY_NHAP { get; set; }
        [Display(Name = "Tổng chi phí")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string TONG_CHI { get; set; }
        [Display(Name = "Mã nhà cung cấp")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string MA_NCC { get; set; }
        public Nullable<System.DateTime> DELETED_AT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_NHAP_HANG> CHI_TIET_NHAP_HANG { get; set; }
        public virtual NHA_CUNG_CAP NHA_CUNG_CAP { get; set; }
        public virtual NHAN_VIEN NHAN_VIEN { get; set; }
    }
}
