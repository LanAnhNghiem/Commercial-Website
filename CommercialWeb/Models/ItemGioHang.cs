using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommercialWeb.Models
{
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSp { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public ItemGioHang(int masp, int soluong)
        {
            using(QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = masp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == masp);
                this.TenSp = sp.TenSP;
                this.HinhAnh = sp.HinhAnh1;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = soluong;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang(int masp)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = masp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == masp);
                this.TenSp = sp.TenSP;
                this.HinhAnh = sp.HinhAnh1;
                this.SoLuong = 1;
                this.DonGia = sp.DonGia.Value;
                this.ThanhTien = DonGia * SoLuong;
            }
        }

        public ItemGioHang() { }
    }
}