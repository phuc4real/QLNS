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
    
    public partial class KHUYEN_MAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHUYEN_MAI()
        {
            this.CHI_TIET_KHUYEN_MAI = new HashSet<CHI_TIET_KHUYEN_MAI>();
        }
    
        public string MA_KM { get; set; }
        public string TEN_KM { get; set; }
        public Nullable<System.DateTime> BAT_DAU { get; set; }
        public Nullable<System.DateTime> KET_THUC { get; set; }
        public Nullable<double> TI_LE_GIAM { get; set; }
        public string MO_TA_KM { get; set; }
        public string DIEU_KIEN { get; set; }
        public Nullable<System.DateTime> DELETED_AT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_KHUYEN_MAI> CHI_TIET_KHUYEN_MAI { get; set; }
    }
}
