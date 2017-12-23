using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    public class KhuyenMaiController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: KhuyenMai
        //DANH SACH KHUYEN MAI
        public ActionResult DanhSachKhuyenMai()
        {
            var lstKhuyenMai = db.KhuyenMais.Where(p=>p.MaKhuyenMai != 1);
            ViewBag.ThongBao = (lstKhuyenMai == null) ? "Không có chương trình khuyến mãi nào" : null;
            return View(lstKhuyenMai); 
        }

        //DANH SACH SAN PHAM KHUYEN MAI
        public ActionResult CacSanPhamKhuyenMai()
        {
            var lstSanPhamKhuyenMai = db.SanPhams.Where(n=>n.MaKhuyenMai != 1);
            return View(lstSanPhamKhuyenMai);
        }

        public ActionResult KhuyenMaiDaHuy()
        {
            return View();
        }

        public ActionResult ChinhSuaKhuyenMai(int MaKhuyenMai)
        {
            return View();
        }

        public ActionResult HuyKhuyenMai(int MaKhuyenMai)
        {
            return View();
        }
    }
}