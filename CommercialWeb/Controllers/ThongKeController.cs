using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
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
            var thang = DateTime.Now.Month;
            return View();
        }
        [HttpPost]
        public ActionResult ThongKeTheoThang(int thang, int nam)
        {
            ViewBag.TongTien = db.DonHangs.Where(n => n.NgayGiao.Month == thang && n.NgayGiao.Year == nam).Sum(n => n.TongTien);
            return View();
        }
        //Thống kê đơn hàng
        public double ThongKeDonHang()
        {
            double SoLuongDonHang = db.DonHangs.Count();
            return SoLuongDonHang; 
        }

        //Thống kê tổng doanh thu
        public decimal ThongKeTongDoanhThu()
        {
            //Thong ke theo tat ca doanh thu
            decimal TongDoanhThu = db.DonHangs.Sum(n => n.TongTien);
            return TongDoanhThu;
        }

        //Thống kê doanh thu theo tháng
        public decimal ThongKeDoanhThuTheoThang(int Thang, int Nam)
        {

            //var lstDonHang = db.DonHangs.Where(n => n.NgayGiao.Month == Thang && n.NgayGiao.Year == Nam);
            //decimal DoanhThuTheoThang = 0;
            //foreach (var item in lstDonHang)
            //{
            //    DoanhThuTheoThang += item.TongTien;
            //}

            decimal DoanhThuTheoThang = db.DonHangs.Where(n => n.NgayGiao.Month == Thang && n.NgayGiao.Year == Nam).Sum(n => n.TongTien);
            return DoanhThuTheoThang;
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