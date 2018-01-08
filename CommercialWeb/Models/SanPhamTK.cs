using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CommercialWeb.Models
{
    public class SanPhamTK
    {
        [JsonProperty("MaSP")]
        public int MaSP { get; set; }
        [JsonProperty("TenSP")]
        public string TenSP { get; set; }
        [JsonProperty("SoLuong")]
        public int SoLuong { get; set; }
        public SanPhamTK() { }
        public SanPhamTK(int masp, string tensp, int soluong)
        {
            this.MaSP = masp;
            this.TenSP = tensp;
            this.SoLuong = soluong;
        }
    }

}