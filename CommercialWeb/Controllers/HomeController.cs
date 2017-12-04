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
            var lstSanPhamLaptop = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            ViewBag.ListSanPhamLaptop = lstSanPhamLaptop;

            return View();
        }
    }
}