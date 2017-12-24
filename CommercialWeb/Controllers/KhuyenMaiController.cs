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
            ViewBag.lstSanPham = db.SanPhams.Where(n => n.MaKhuyenMai == MaKhuyenMai);
            ViewBag.lstSanPhamBT = db.SanPhams.Where(n => n.MaKhuyenMai == 1); 
            return View(km);
        }

        [HttpPost]
        public ActionResult ChinhSuaKhuyenMai(KhuyenMai km, IEnumerable<SanPham> lstSPKM)
        {
            KhuyenMai kmUpdate = db.KhuyenMais.SingleOrDefault(n => n.MaKhuyenMai == km.MaKhuyenMai);
            kmUpdate.TenKhuyenMai = km.TenKhuyenMai;
            kmUpdate.MoTa = km.MoTa;
            db.SaveChanges();

            //Huy khuyen mai tat ca san pham dang khuyen mai tuong ung
            IEnumerable<SanPham> lstSPdangKM = db.SanPhams.Where(n => n.MaKhuyenMai == km.MaKhuyenMai);
            foreach(var sp in lstSPdangKM)
            {
                sp.MaKhuyenMai = 1;
                sp.GiaBan = sp.DonGia;
            }
            db.SaveChanges();
            //Cập nhật lại danh sách sản phẩm khuyến mãi mới
            SanPham spUpdate;
            if(lstSPKM != null)
            {
                foreach (var sp in lstSPKM)
                {
                    spUpdate = db.SanPhams.SingleOrDefault(n => n.MaSP == sp.MaSP);
                    spUpdate.MaKhuyenMai = km.MaKhuyenMai;
                    spUpdate.GiaBan = spUpdate.DonGia * (1 - (decimal)km.MoTa / 100);
                }
            }

            db.SaveChanges();

            ViewBag.lstSanPham = db.SanPhams.Where(n => n.MaKhuyenMai == km.MaKhuyenMai);
            ViewBag.lstSanPhamBT = db.SanPhams.Where(n => n.MaKhuyenMai == 1);
            ViewBag.ThongBao = "Thành công !";
            return View(kmUpdate);
        }

        
        public ActionResult HuyKhuyenMai(int? MaKhuyenMai)
        {
            KhuyenMai kmupdate = db.KhuyenMais.SingleOrDefault(p => p.MaKhuyenMai == MaKhuyenMai);
            IEnumerable<SanPham> lstSP = db.SanPhams.Where(n => n.MaKhuyenMai == kmupdate.MaKhuyenMai);
            foreach (var sp in lstSP)
            {
                sp.MaKhuyenMai = 1;
                sp.GiaBan = sp.DonGia;
            }
            kmupdate.DaHuy = true;
            db.SaveChanges();
            return RedirectToAction("DanhSachKhuyenMai");
        }

        public ActionResult HuyKhuyenMaiSP(int? MaSP)
        {
            SanPham spUpdate = db.SanPhams.SingleOrDefault(p => p.MaSP == MaSP);
            spUpdate.MaKhuyenMai = 1;
            spUpdate.GiaBan = spUpdate.DonGia;
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
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiKhuyenMai(KhuyenMai km, IEnumerable<SanPham> lstSP)
        {
            ViewBag.lstSanPham = db.SanPhams.Where(n => n.MaKhuyenMai == 1);
            //Cap nhat gia va ma khuyen mai cho tat ca sp dc ap dung khuyen mai
            SanPham spUpdate;
            foreach(var sp in lstSP)
            {
                spUpdate = db.SanPhams.SingleOrDefault(n => n.MaSP == sp.MaSP);
                spUpdate.MaKhuyenMai = km.MaKhuyenMai;
                spUpdate.GiaBan = sp.DonGia * (1 - (decimal)km.MoTa/100);
            }
            km.DaHuy = false;
            db.KhuyenMais.Add(km);
            db.SaveChanges();
            ViewBag.ThongBao = "Thành công !";
            return View();
        }
    }
}