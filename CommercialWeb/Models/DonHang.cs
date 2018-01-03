namespace CommercialWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        [Key]
        public int MaDonHang { get; set; }

        public int? MaKH { get; set; }

        public DateTime? NgayMua { get; set; }

        public DateTime NgayGiao { get; set; }

        public decimal? TongTien { get; set; }

        public int? MaTinhTrang { get; set; }

        public int? MaHinhThuc { get; set; }

        public bool? DaXoa { get; set; }

        public bool DaThanhToan { get; set; }

        public bool DaHuy { get; set; }

        public int? MaThanhVIen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual HinhThucGiaoHang HinhThucGiaoHang { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual ThanhVien ThanhVien { get; set; }

        public virtual TinhTrangDonHang TinhTrangDonHang { get; set; }
    }
}
