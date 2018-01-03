namespace CommercialWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        [Key]
        public int MaCTDH { get; set; }

        public int? MaDonHang { get; set; }

        public int? MaSP { get; set; }

        public int? SoLuong { get; set; }

        public decimal? DonGia { get; set; }

        public bool? DaXoa { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
