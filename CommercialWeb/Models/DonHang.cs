//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommercialWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
    
        public int MaDonHang { get; set; }
        public Nullable<int> MaKH { get; set; }
        public Nullable<System.DateTime> NgayMua { get; set; }
        public Nullable<System.DateTime> NgayGiao { get; set; }
        public Nullable<decimal> TongTien { get; set; }
        public Nullable<int> MaTinhTrang { get; set; }
        public Nullable<int> MaHinhThuc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual HinhThucGiaoHang HinhThucGiaoHang { get; set; }
        public virtual KhachHang KhachHang { get; set; }
        public virtual TinhTrangDonHang TinhTrangDonHang { get; set; }
    }
}
