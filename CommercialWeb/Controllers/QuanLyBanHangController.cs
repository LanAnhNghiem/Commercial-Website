using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using Newtonsoft.Json;

namespace CommercialWeb.Controllers
{
    public class QuanLyBanHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyBanHang
        [HttpGet]
        public ActionResult TaoDonHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getProductList()
        {
            var dict = db.SanPhams.ToDictionary(s => s.MaSP, s => s.TenSP);
            return Json(dict.ToList(), JsonRequestBehavior.AllowGet);
        }
        public List<ItemDonHang> LayDonHang()
        {
            List<ItemDonHang> lstDonHang = Session["DonHang"] as List<ItemDonHang>; 
            if(lstDonHang == null)
            {
                lstDonHang = new List<ItemDonHang>();
                Session["DonHang"] = lstDonHang;
            }
            return lstDonHang;
        }
        [HttpPost]
        public ActionResult addNewProduct(int MaSP, int SoLuong)
        {
            //var sp = db.SanPhams.Where(n=>n.MaSP == MaSP).Select(n=> new {n.MaSP, n.TenSP, n.ThoiHanBaoHanh, n.DonGia, n.GiaBan});
            var sp = db.SanPhams.Where(n => n.MaSP == MaSP).Join(db.KhuyenMais, km => km.MaKhuyenMai, sanpham => sanpham.MaKhuyenMai,
                (km, sanpham) => new {km.MaSP, km.TenSP, km.ThoiHanBaoHanh, km.DonGia, sanpham.MoTa,sanpham.TenKhuyenMai, km.GiaBan})
                .Select(n=> new { n.MaSP, n.TenSP, n.ThoiHanBaoHanh, n.DonGia, n.MoTa, n.TenKhuyenMai, n.GiaBan});
            
            //var items = db.Words.Take(1).ToList();
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string converted = JsonConvert.SerializeObject(sp, null, jsSettings).Remove(0,1);
            converted = (converted.Remove(converted.LastIndexOf("}]"), 2));
            converted += ",\"SoLuong\":" + SoLuong + "}";
            var newItem = JsonConvert.DeserializeObject<ItemDonHang>(converted);
            List<ItemDonHang> lstDonHang = LayDonHang();
            lstDonHang.Add(newItem);
            Session["DonHang"] = lstDonHang;
            //var listDH = JsonConvert.SerializeObject(lstDonHang, null, jsSettings);
            ViewBag.SoLuong = SoLuong;
            return PartialView("listSanPhamPartial", LayDonHang());
        }
        public ActionResult listSanPhamPartial()
        {
            return PartialView();
        }
    }
}