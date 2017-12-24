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
            var lstKhuyenMai = db.KhuyenMais.Where(p=>p.MaKhuyenMai != 1 && p.DaHuy == false);
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
            var lstKMDaHuy = db.KhuyenMais.Where(n => n.DaHuy == true);
            return View(lstKMDaHuy);
        }

        [HttpGet]
        public ActionResult ChinhSuaKhuyenMai(int? MaKhuyenMai)
        {
            KhuyenMai km = db.KhuyenMais.SingleOrDefault(p => p.MaKhuyenMai == MaKhuyenMai);
            return View(km);
        }

        [HttpPost]
        public ActionResult ChinhSuaKhuyenMai(KhuyenMai khuyenmai)
        { 
            return View();
        }

        public ActionResult HuyKhuyenMai(int? MaKhuyenMai)
        {
            KhuyenMai khupdate = db.KhuyenMais.SingleOrDefault(p => p.MaKhuyenMai == MaKhuyenMai);
            khupdate.DaHuy = true;
            db.SaveChanges();
            return RedirectToAction("DanhSachKhuyenMai");
        }

        public ActionResult HuyKhuyenMaiSP(int? MaSP)
        {
            SanPham spUpdate = db.SanPhams.SingleOrDefault(p => p.MaSP == MaSP);
            spUpdate.MaKhuyenMai = 1;
            db.SaveChanges();
            return RedirectToAction("CacSanPhamKhuyenMai");
        }

        public ActionResult HoanTacKhuyenmai(int? MaKhuyenMai)
        {
            KhuyenMai khupdate = db.KhuyenMais.SingleOrDefault(p => p.MaKhuyenMai == MaKhuyenMai);
            khupdate.DaHuy = false;
            db.SaveChanges();
            return RedirectToAction("KhuyenMaiDaHuy");
        }
        [HttpGet]
        public ActionResult ThemMoiKhuyenMai()
        {
            ViewBag.lstSanPham = db.SanPhams.Where(n=>n.MaKhuyenMai == 1);
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiKhuyenMai(KhuyenMai km, IEnumerable<SanPham> lstSP)
        {
            ViewBag.lstSanPham = db.SanPhams.Where(n => n.MaKhuyenMai == 1);
            return View();
        }
    }
}