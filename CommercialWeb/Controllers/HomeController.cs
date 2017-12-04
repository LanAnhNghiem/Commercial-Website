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
            var lstSanPhamMoi = db.SanPhams.Where(n => n.DonGia > 10000000);
            ViewBag.ListSanPhamMoi = lstSanPhamMoi;
            return View();
        }
    }
}