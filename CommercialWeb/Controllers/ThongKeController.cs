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
            return View();
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