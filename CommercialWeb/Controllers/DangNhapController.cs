using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommercialWeb.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: DangNhap
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(int i)
        {
            return RedirectToAction("ChuaGiao", "QuanLyDonHang");
        }
    }
}