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

    public partial class SACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SACH()
        {
            this.CHI_TIET_HOA_DON = new HashSet<CHI_TIET_HOA_DON>();
            this.CHI_TIET_KHUYEN_MAI = new HashSet<CHI_TIET_KHUYEN_MAI>();
            this.CHI_TIET_NHAP_HANG = new HashSet<CHI_TIET_NHAP_HANG>();
            this.DON_GIA = new HashSet<DON_GIA>();
        }

        [Display(Name = "Mã sách")]
        [Required(ErrorMessage = "Không được để trống!")]
        [MaxLength(20, ErrorMessage = "Mã sách tối đa 20 kí tự!")]
        public string MA_SACH { get; set; }
        [Display(Name = "Dạng sách")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string MA_DS { get; set; }
        [Display(Name = "Độ tuôi")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string MA_DT { get; set; }
        [Display(Name = "Thể loại")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string MA_TL { get; set; }
        [Display(Name = "Tên sách")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string TEN_SACH { get; set; }
        [Display(Name = "Mô tả sách")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string MO_TA_SACH { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string ANH_BIA { get; set; }
        [Display(Name = "Tác giả")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string TAC_GIA { get; set; }
        [Display(Name = "Nhà xuất bản")]
        [Required(ErrorMessage = "Không được để trống!")]
        public string NHA_XB { get; set; }
        [Display(Name = "Năm xuất bản")]
        public Nullable<System.DateTime> NAM_XB { get; set; }
        [Display(Name = "Số lượng đã bán")]
        [Range(0, int.MaxValue, ErrorMessage = "Hãy nhập số lượng hợp lệ!")]
        public Nullable<int> SL_BAN { get; set; }
        [Display(Name = "Số lượng tồn kho")]
        [Range(0, int.MaxValue, ErrorMessage = "Hãy nhập số lượng hợp lệ!")]
        public Nullable<int> SL_TON { get; set; }
        public Nullable<System.DateTime> DELETED_AT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_HOA_DON> CHI_TIET_HOA_DON { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_KHUYEN_MAI> CHI_TIET_KHUYEN_MAI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_NHAP_HANG> CHI_TIET_NHAP_HANG { get; set; }
        public virtual DANG_SACH DANG_SACH { get; set; }
        public virtual DO_TUOI DO_TUOI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DON_GIA> DON_GIA { get; set; }
        public virtual THE_LOAI THE_LOAI { get; set; }
    }
}
