using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            //Nếu giỏ hàng null
            if(lstGioHang == null)
            {
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public List<String> LayThanhToan()
        {
            List<String> lst = Session["ThanhToan"] as List<String>;
            if(lst == null)
            {
                lst = new List<String>();
                Session["ThanhToan"] = lst;
            }
            return lst;
        }
        public ActionResult ThemGioHangAjax(int MaSP, string URL)
        {
            //Kiểm tra nếu sp có trong giỏ hàng hay chưa
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Hiện lỗi 404 khi đường dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Get GioHang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Nếu sp đã có trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Kiểm tra số lượng tồn trước khi thêm vào giỏ
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    TempData["msg"] = "<script>alert('Change succesfully');</script>";
                    ViewBag.TongSoLuong = TinhTongSoLuong();
                    return PartialView("GioHangPartial");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                TempData["msg"] = "<script>alert('Sản phẩm đã hết hàng!');</script>";
                ViewBag.TongSoLuong = TinhTongSoLuong();
                return PartialView("GioHangPartial");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            return PartialView("GioHangPartial");
        }
        
        public double TinhTongSoLuong()
        {
            //Get GioHang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
       
        public decimal TinhTongTien()
        {
            //Get GioHang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();

            return PartialView();
        }
        public ActionResult TinhItemThanhTien(int masp, decimal dongia, int soluong)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            //Lấy GioHang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Nếu sp đã được thêm vào giỏ
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == masp);
            
            if (spCheck != null)
            {
                int soLuongCu = spCheck.SoLuong;
                spCheck.SoLuong = soluong;
                //Kiểm tra số lượng tồn trước khi thêm vào giỏ
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    TempData["msg"] = "<script>alert('Sản phẩm đã hết hàng, không thể đặt thêm!');</script>";
                    spCheck.SoLuong = soLuongCu;
                }
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
            }
            if(soluong == 0)
            {
                return Content("0 VND");
            }
            return Content((dongia * soluong).ToString("#,##")+" VND");
        }
        public ActionResult TamTinh()
        {
            List<String> lstThanhToan = LayThanhToan();
            String tamTinh = TinhTongTien().ToString("#,##") + " VND";
            lstThanhToan.Add(tamTinh);
            return Content(tamTinh);
        }
        public ActionResult TongThanhTien()
        {
            List<String> lstThanhToan = LayThanhToan();
            String tongThanhTien = ((10 * TinhTongTien() / 100) + TinhTongTien()).ToString("#,##") + " VND";
            lstThanhToan.Add(tongThanhTien);
            return Content(tongThanhTien);
        }
        //Cập nhật lại số sp ở giỏ hàng trên menu của CartLayout
        public ActionResult CapNhatGioHang()
        {
            ViewBag.TongSoLuong = TinhTongSoLuong();
            return PartialView("GioHangPartial");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra nếu sp có trong giỏ hàng hay chưa
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Hiện lỗi 404 khi đường dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Get GioHang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Nếu sp đã có trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa sp
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        
    }
}