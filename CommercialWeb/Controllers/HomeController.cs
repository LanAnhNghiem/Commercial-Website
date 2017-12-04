using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Home
        public ActionResult Index()
        {
            //New products
            var lstSanPhamMoi = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            ViewBag.ListSanPhamMoi = lstSanPhamMoi;

            //Laptop
            var lstLaptopNoiBat = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            ViewBag.ListLaptopNoiBat = lstLaptopNoiBat;

            //San pham khuyen mai
            var lstSanPhamKhuyenMai = db.SanPhams.Where(n => n.MaLoaiSP == 2);
            ViewBag.ListSanPhamKhuyenMai = lstSanPhamKhuyenMai;

            //Danh sách laptop ở trang chủ
            var lstLaptop = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            ViewBag.ListLaptop = lstLaptop;

            //Danh sách laptop ở trang chủ
            var lstMayTinh = db.SanPhams.Where(n => n.MaLoaiSP == 2);
            ViewBag.LisLaptop = lstMayTinh;

            //Danh sách laptop ở trang chủ
            var lstLinhKien = db.SanPhams.Where(n => n.MaLoaiSP == 1);
            ViewBag.LisLaptop = lstLinhKien;

            return View();
        }
    }
}