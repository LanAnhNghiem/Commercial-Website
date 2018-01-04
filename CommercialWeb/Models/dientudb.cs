namespace CommercialWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QuanLyBanHangEntities : DbContext
    {
        public QuanLyBanHangEntities()
            : base("name=QuanLyBanHang")
        {
        }

        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<HinhThucGiaoHang> HinhThucGiaoHangs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<LoaiThanhVien> LoaiThanhViens { get; set; }
        public virtual DbSet<LoaiThanhVien_Quyen> LoaiThanhVien_Quyen { get; set; }
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<ThanhVien> ThanhViens { get; set; }
        public virtual DbSet<TinhTrangDonHang> TinhTrangDonHangs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.DonGia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.TongTien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<LoaiThanhVien>()
                .HasMany(e => e.LoaiThanhVien_Quyen)
                .WithRequired(e => e.LoaiThanhVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quyen>()
                .HasMany(e => e.LoaiThanhVien_Quyen)
                .WithRequired(e => e.Quyen)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.DonGia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.GiaBan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ThanhVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ThanhVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<ThanhVien>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);
        }
    }
}
