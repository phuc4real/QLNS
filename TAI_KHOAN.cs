//------------------------------------------------------------------------------
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
    
    public partial class TAI_KHOAN
    {
        public string USERNAME { get; set; }
        public string MA_NV { get; set; }
        public string PASSWORD { get; set; }
        public string QUYEN { get; set; }
        public Nullable<System.DateTime> DELETED_AT { get; set; }
    
        public virtual NHAN_VIEN NHAN_VIEN { get; set; }
    }
}
