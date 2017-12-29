using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommercialWeb.Models
{
    public class ItemDonHang
    {
        [JsonProperty("MaSP")]
        public int MaSP { get; set; }
        [JsonProperty("TenSP")]
        public string TenSP { get; set; }
        [JsonProperty("ThoiHanBaoHanh")]
        public string ThoiHanBaoHanh { get; set; }
        [JsonProperty("DonGia")]
        public decimal? DonGia { get; set; }
        [JsonProperty("MoTa")]
        public int? MoTa { get; set; }
        [JsonProperty("TenKhuyenMai")]
        public string TenKhuyenMai { get; set; }
        [JsonProperty("GiaBan")]
        public decimal? GiaBan { get; set; }
        [JsonProperty("SoLuong")]
        public int SoLuong { get; set; }
        public ItemDonHang()
        {

        }
        public ItemDonHang(int masp, string tensp, string baohanh, decimal? dongia, int? mota, string tenkm, decimal? giaban, int soluong)
        {
            this.MaSP = masp;
            this.TenSP = tensp;
            this.ThoiHanBaoHanh = baohanh;
            this.DonGia = dongia;
            this.MoTa = mota;
            this.TenKhuyenMai = tenkm;
            this.GiaBan = giaban;
            this.SoLuong = soluong;
        }
    }

}