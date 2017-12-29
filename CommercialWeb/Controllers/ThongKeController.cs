using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    [Authorize(Roles = "6_ThongKe")]
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: ThongKe
        public ActionResult ThongKe()
        {
            ViewBag.SoLuongTruyCap = HttpContext.Application["SoLuongTruyCap"].ToString();
            ViewBag.Online = HttpContext.Application["Online"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();
            ViewBag.TongDonHang = ThongKeDonHang();

            return View();
        }

        [HttpGet]
        public ActionResult ThongKeTheoThang()
        {
            ViewBag.TongTien = 0;
            ViewBag.TongSanPham = 0;
            ViewBag.Thang = "__";
            ViewBag.Nam = "__";
            return View();
        }
        [HttpPost]
        public ActionResult ThongKeTheoThang(int thang, int nam)
        {
            ViewBag.TongSanPham = db.DonHangs.Where(n => n.NgayGiao.Month == thang && n.NgayGiao.Year == nam && n.HinhThucGiaoHang.MaHinhThuc == 1).Sum(n=>n.ChiTietDonHangs.Sum(q=>(int?)q.SoLuong)) ?? 0;
            ViewBag.TongTien = db.DonHangs.Where(n => n.NgayGiao.Month == thang && n.NgayGiao.Year == nam && n.HinhThucGiaoHang.MaHinhThuc == 1).Sum(n => n.TongTien) ?? 0;
            ViewBag.Thang = thang;
            ViewBag.Nam = nam;
            return View();
        }
        //Thống kê đơn hàng
        public double ThongKeDonHang()
        {
            double SoLuongDonHang = db.DonHangs.Count();
            return SoLuongDonHang; 
        }

        //Thống kê tổng doanh thu
        public decimal? ThongKeTongDoanhThu()
        {
            //Thong ke theo tat ca doanh thu
            decimal? TongDoanhThu = db.DonHangs.Sum(n => n.TongTien);
            return TongDoanhThu;
        }

        //Thống kê doanh thu theo tháng
        public decimal? ThongKeDoanhThuTheoThang(int Thang, int Nam)
        {

            //var lstDonHang = db.DonHangs.Where(n => n.NgayGiao.Month == Thang && n.NgayGiao.Year == Nam);
            //decimal DoanhThuTheoThang = 0;
            //foreach (var item in lstDonHang)
            //{
            //    DoanhThuTheoThang += item.TongTien;
            //}

            decimal? DoanhThuTheoThang = db.DonHangs.Where(n => n.NgayGiao.Month == Thang && n.NgayGiao.Year == Nam).Sum(n => n.TongTien);
            return DoanhThuTheoThang;
        }

        [HttpGet]
        public ActionResult ThongKeTheoNam()
        {
            ViewBag.TongTien = 0;
            ViewBag.TongSanPham = 0;
            ViewBag.Nam = "__";
            return View();
        }

        [HttpPost]
        public ActionResult ThongKeTheoNam(int nam)
        {
            ViewBag.TongSanPham = db.DonHangs.Where(n => n.NgayGiao.Year == nam && n.HinhThucGiaoHang.MaHinhThuc == 1).Sum(n => n.ChiTietDonHangs.Sum(q => q.SoLuong)) ?? 0;
            ViewBag.TongTien = db.DonHangs.Where(n => n.NgayGiao.Year == nam && n.HinhThucGiaoHang.MaHinhThuc == 1).Sum(n => n.TongTien) ?? 0;
            ViewBag.Nam = nam;

            return View();
        }
        [HttpGet]
        public ActionResult ThongKeTheoQuy()
        {
            ViewBag.TongSanPham = 0;
            ViewBag.TongTien = 0;
            ViewBag.Nam = "__";
            ViewBag.Quy = "__";

            return View();
        }

        [HttpPost]
        public ActionResult ThongKeTheoQuy(int quy, int nam)
        {
            //quý n có min = (n - 1) * 3 + 1;
            var minMonth = (quy - 1) * 3 + 1;
            ViewBag.TongSanPham = db.DonHangs.Where(n => n.NgayGiao.Month >= minMonth && n.NgayGiao.Month <= (minMonth + 2) && n.NgayGiao.Year == nam && n.HinhThucGiaoHang.MaHinhThuc == 1).Sum(n => n.ChiTietDonHangs.Sum(q => q.SoLuong)) ?? 0;
            ViewBag.TongTien = db.DonHangs.Where(n => n.NgayGiao.Month >= minMonth && n.NgayGiao.Month <= (minMonth + 2) && n.HinhThucGiaoHang.MaHinhThuc == 1).Sum(n => n.TongTien) ?? 0;
            ViewBag.Nam = nam;
            ViewBag.Quy = quy;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}