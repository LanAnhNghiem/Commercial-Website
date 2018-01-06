using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using Newtonsoft.Json;

namespace CommercialWeb.Controllers
{
    [Authorize(Roles = "1_TaoDonHang")]
    public class QuanLyBanHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyBanHang
        [HttpGet]
        public ActionResult TaoDonHang()
        {
            return View(LayDonHang());
        }

        [HttpPost]
        public ActionResult getProductList()
        {
            var dict = db.SanPhams.Where(n=> n.DaXoa == false).ToDictionary(s => s.MaSP, s => s.TenSP);
            return Json(dict.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult getThanhVienList()
        {
            var dict = db.ThanhViens.Where(n => n.MaLoaiTV == 4)
                .ToDictionary(n => n.MaThanhVien, n => n.HoTen + " - " + n.SoDienThoai + " - " + n.Email + " - " + n.DiaChi);
            return Json(dict.ToList(), JsonRequestBehavior.AllowGet);
        }
        public List<ItemDonHang> LayDonHang()
        {
            List<ItemDonHang> lstDonHang = Session["DonHang"] as List<ItemDonHang>;
            if (lstDonHang == null)
            {
                lstDonHang = new List<ItemDonHang>();
                Session["DonHang"] = lstDonHang;
            }
            return lstDonHang;
        }
        [HttpPost]
        public ActionResult addNewProduct(int MaSP, int SoLuong)
        {
            var product = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            var sp = db.SanPhams.Where(n => n.MaSP == MaSP).Join(db.KhuyenMais, km => km.MaKhuyenMai, sanpham => sanpham.MaKhuyenMai,
                (km, sanpham) => new { km.MaSP, km.TenSP, km.ThoiHanBaoHanh, km.DonGia, sanpham.MoTa, sanpham.TenKhuyenMai, km.GiaBan })
                .Select(n => new { n.MaSP, n.TenSP, n.ThoiHanBaoHanh, n.DonGia, n.MoTa, n.TenKhuyenMai, n.GiaBan });

            //var items = db.Words.Take(1).ToList();
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string converted = JsonConvert.SerializeObject(sp, null, jsSettings).Remove(0, 1);
            converted = (converted.Remove(converted.LastIndexOf("}]"), 2));
            converted += ",\"SoLuong\":" + SoLuong + "}";
            var newItem = JsonConvert.DeserializeObject<ItemDonHang>(converted);
            List<ItemDonHang> lstDonHang = LayDonHang();
            ItemDonHang spCheck = lstDonHang.SingleOrDefault(n => n.MaSP == newItem.MaSP);
            if (spCheck != null)
            {
                if (product.SoLuongTon < SoLuong)
                {
                    return Content("alert");
                }
                else
                {
                    spCheck.SoLuong += SoLuong;
                    return PartialView("listSanPhamPartial", lstDonHang);
                }
            }
            if (product.SoLuongTon < SoLuong)
            {
                return Content("alert");
            }
            lstDonHang.Add(newItem);
            Session["DonHang"] = lstDonHang;
            //var listDH = JsonConvert.SerializeObject(lstDonHang, null, jsSettings);
            ViewBag.SoLuong = SoLuong;
            ViewBag.TongTien = TinhTongTien();
            return PartialView("listSanPhamPartial", lstDonHang);
        }

        [HttpPost]
        public ActionResult DatHang(int MaKH, bool IsThanhToan, bool IsGiaoHang)
        {
            ThanhVien tvSession = (ThanhVien)Session["TaiKhoan"];
            List<ItemDonHang> lstDonHang = LayDonHang();
            if (lstDonHang == null || lstDonHang.Count == 0)
            {
                return Content("Giỏ hàng rỗng !!!");
            }
            DonHang dh = new DonHang();
            dh.MaKH = MaKH;
            dh.NgayMua = DateTime.Now;
            dh.NgayGiao = DateTime.Now;
            dh.TongTien = TinhTongTien();
            if (IsThanhToan == true)
            {
                dh.DaThanhToan = true;
            }
            else
            {
                dh.DaThanhToan = false;
            }
            if (IsGiaoHang == true)
            {
                dh.MaTinhTrang = 1;
            }
            else
            {
                dh.MaTinhTrang = 2;
            }
            dh.MaHinhThuc = 1;
            dh.DaXoa = false;
            dh.DaHuy = false;
            dh.MaThanhVIen = tvSession.MaThanhVien;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in lstDonHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaSP = item.MaSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                ctdh.DaXoa = false;
                db.ChiTietDonHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["DonHang"] = null;
            return Content("Thêm đơn hàng thành công. Tổng giá trị đơn hàng là: " + TinhTongTien().ToString("#,##"));
        }
        public decimal TinhTongTien()
        {
            //Get GioHang
            List<ItemDonHang> lstGioHang = Session["DonHang"] as List<ItemDonHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            decimal sum = 0;
            foreach (var item in lstGioHang)
            {
                sum += (decimal)item.GiaBan * item.SoLuong;
            }
            return sum;
        }
        public ActionResult listSanPhamPartial()
        {
            return PartialView();
        }
        public ActionResult XoaItemDonHang(int MaSP)
        {
            List<ItemDonHang> lstDonHang = LayDonHang();
            //Nếu sp đã có trong giỏ hàng
            ItemDonHang spCheck = lstDonHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("TaoDonHang", "QuanLyBanHang");
            }
            //Xóa sp
            lstDonHang.Remove(spCheck);
            return RedirectToAction("TaoDonHang");
        }

        public ActionResult HienTongThanhTien()
        {
            return Content(TinhTongTien().ToString("#,##"));
        }

        [HttpPost]
        public ActionResult DatHangMoi(string HoTen, string Email, string SDT, string DiaChi, bool IsThanhToan, bool IsGiaoHang)
        {
            ThanhVien tvSession = (ThanhVien)Session["TaiKhoan"];
            List<ItemDonHang> lstDonHang = LayDonHang();
            if (lstDonHang == null || lstDonHang.Count == 0)
            {
                return Content("Giỏ hàng rỗng !!!");
            }
            KhachHang kh = new KhachHang();
            kh.HoTen = HoTen;
            kh.Email = Email;
            kh.DiaChi = DiaChi;
            kh.SoDienThoai = SDT;
            db.KhachHangs.Add(kh);
            db.SaveChanges();
            DonHang dh = new DonHang();
            dh.MaKH = kh.MaKH;
            dh.NgayMua = DateTime.Now;
            dh.NgayGiao = DateTime.Now;
            dh.TongTien = TinhTongTien();
            if (IsThanhToan == true)
            {
                dh.DaThanhToan = true;
            }
            else
            {
                dh.DaThanhToan = false;
            }
            if (IsGiaoHang == true)
            {
                dh.MaTinhTrang = 1;
            }
            else
            {
                dh.MaTinhTrang = 2;
            }
            dh.MaHinhThuc = 1;
            dh.DaXoa = false;
            dh.DaHuy = false;
            dh.MaThanhVIen = tvSession.MaThanhVien;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in lstDonHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaSP = item.MaSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                ctdh.DaXoa = false;
                db.ChiTietDonHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["DonHang"] = null;
            return Content("Thêm đơn hàng thành công.");
        }
    }
}