using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.Text.RegularExpressions;

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
                    TempData["msg"] = "<script>alert('Sản phẩm đã hết hàng!');</script>";
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
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.DSSP = LayGioHang();
            //Lấy cước phí theo khu vực
            var lstCuocPhi = db.CuocPhis;
            ViewBag.CuocPhi = lstCuocPhi;
            //Lấy hình thức giao hàng
            var lstHinhThuc = db.HinhThucGiaoHangs;
            ViewBag.HinhThuc = lstHinhThuc;
            //nếu giỏ hàng rỗng
            if(Session["CuocPhi"] != null)
            {
                //int MaDH = Int32.Parse(Session["MaDH"].ToString());
                ViewBag.TamTinh = TongThanhTien();
                int cuocphi = Int32.Parse(Session["CuocPhi"].ToString());
                ViewBag.PhiVC = cuocphi.ToString("#,##");
                ViewBag.TongTT = Session["TongThanhTien"].ToString();
                ViewBag.DiaChi = Session["DiaChi"].ToString();
                ViewBag.Email = Session["Email"].ToString();
                ViewBag.SDT = Session["SDT"].ToString();
            }
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
                    string strScript = "";
                    strScript = "<script>alert('Sản phẩm đã hết hàng, không thể đặt thêm!'); window.location.reload();</script>";
                    TempData["msg"] = strScript;
                    spCheck.SoLuong = soLuongCu;
                }
                if (soluong == 0)
                {
                    lstGioHang[lstGioHang.IndexOf(spCheck)].SoLuong = soluong;
                    Session["GioHang"] = lstGioHang;
                    return Content("0 VND");
                }
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
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
            Session["TongThanhTien"] = ((10 * TinhTongTien() / 100) + TinhTongTien());
            lstThanhToan.Add(tongThanhTien);
            return Content(tongThanhTien);
        }
        //Cập nhật lại số sp ở giỏ hàng trên menu của CartLayout
        public ActionResult CapNhatGioHang()
        {
            ViewBag.TongSoLuong = TinhTongSoLuong();
            return PartialView("GioHangPartial");
        }
        //Cập nhật lại khung Đơn hàng
        public ActionResult DSSPPartial()
        {
            IEnumerable<ItemGioHang> lstGioHang = LayGioHang() as IEnumerable<ItemGioHang>;
            //lấy các sp có số lượng khác 0 lưu vào ViewBag.GioHang
            var tmpLst = lstGioHang.Where(n => n.SoLuong != 0).Select(n=>n);
            return PartialView(tmpLst);
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
        //public ActionResult NhapThongTin(FormCollection form)
        //{
        //    string HoTen = form["txtHoTen"].ToString();
        //    string DiaChi = form["txtDiaChi"].ToString();
        //    string SDT = form["txtSDT"].ToString();
        //    string Email = form["txtEmail"].ToString();
        //    //Nếu là khách vãng lai
        //    KhachHang kh = db.KhachHangs.SingleOrDefault(n=>(n.SoDienThoai == SDT || n.Email == Email) && n.ThanhVien == null);
        //    if (kh == null)
        //    {
        //        kh = new KhachHang();
        //    }
        //    kh.HoTen = HoTen;
        //    kh.DiaChi = DiaChi;
        //    kh.SoDienThoai = SDT;
        //    kh.Email = Email;
        //    Session["KhachHang"] = kh;
        //}
        [HttpPost]   
        public ActionResult DatHang(FormCollection f)
        {
            //Kiểm tra nhập thông tin hợp lệ hay không
            RegexUtilities regex = new RegexUtilities();
            string errorMess = "";
            string email = f["txtEmail"].ToString();
            string hoten = f["txtHoTen"].ToString();
            string diachi = f["txtDiaChi"].ToString();
            string sdt = f["txtSDT"].ToString();
            if (!regex.IsValidEmail(email))
            {
                errorMess = "- Email không hợp lệ.";
            }
            if(sdt.Length > 12 || Regex.IsMatch(sdt, @"^[0-9]*$") == false)
            {
                errorMess += "\r\n- Số điện thoại không hợp lệ.";
            }
            if(!regex.IsValidEmail(email) || sdt.Length > 12 || Regex.IsMatch(sdt, @"^[0-9]*$") == false)
            {
                return Content(errorMess);
            }
            //Lưu thông tin khách hàng
            //Nhớ kiểm tra khách hàng là thành viên đăng nhập hay chưa (code sau)
            //Đây là bước tạo mới 1 khách hàng vãng lai
            KhachHang kh = new KhachHang();
            kh.HoTen = hoten;
            kh.Email = email;
            kh.SoDienThoai = sdt;
            kh.DiaChi = diachi;
            db.KhachHangs.Add(kh);
            db.SaveChanges();
            Session["DiaChi"] = diachi;
            Session["Email"] = email;
            Session["SDT"] = sdt;
            //Thêm đơn hàng
            DonHang dh = new DonHang();
            dh.MaKH = kh.MaKH;
            dh.NgayMua = DateTime.Now;
            dh.UuDai = 0;
            dh.DaThanhToan = false;
            dh.TinhTrangGiaoHang = false;
            dh.DaHuy = false;
            dh.DaXoa = false;
            dh.TongTien = Int32.Parse(Session["TongThanhTien"].ToString());
            dh.MaHinhThuc = Int32.Parse(Session["HinhThuc"].ToString());
            db.DonHangs.Add(dh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            foreach(var item in lstGioHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaSP = item.MaSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                ctdh.TenSP = item.TenSP;
                db.ChiTietDonHangs.Add(ctdh);
            }
            db.SaveChanges();
            //Session["GioHang"] = null;
            return PartialView("DonHangPartial",LayGioHang());
        }
        public ActionResult HinhThucPartial()
        {
            return PartialView();
        }
        public ActionResult KhuVucPartial()
        {
            return PartialView();
        }
        public ActionResult PhiVanChuyenPartial(int cuockv, int cuocphi)
        {
            ViewBag.CuocPhi = cuockv + cuocphi;
            Session["CuocPhi"] = cuockv + cuocphi;
            if (cuocphi == 0)
                Session["HinhThuc"] = 1;
            else
                Session["HinhThuc"] = 2;
            Session["TongThanhTien"] = ((10 * TinhTongTien() / 100) + TinhTongTien()) + cuockv + cuocphi;
            return PartialView();
        }
        public ActionResult TongThanhTienPartial()
        {
            ViewBag.TongThanhTien = Session["TongThanhTien"];
            return PartialView();
        }
        public ActionResult DonHangPartial()
        {
            return PartialView();
        }
       
    }
}