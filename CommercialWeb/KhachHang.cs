namespace CommercialWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            DonHangs = new HashSet<DonHang>();
        }

        [Key]
        public int MaKH { get; set; }

        public string HoTen { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(12)]
        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        public int? MaThanhVien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }

        public virtual ThanhVien ThanhVien { get; set; }
    }
}
