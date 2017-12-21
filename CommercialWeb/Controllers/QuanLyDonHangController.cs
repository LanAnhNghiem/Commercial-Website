using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.Net;
using System.Net.Mail;

namespace CommercialWeb.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonHang
        public ActionResult QuanLyDonHang()
        {
            return View(db.DonHangs.Where(n=>n.MaTinhTrang == 1 && n.DaXoa == false));
        }

        [HttpGet]
        public ActionResult TaoMoiDonhang()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChiTietDonHang(int? MaDonHang)
        {
            if(MaDonHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DonHang DonHang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            if (DonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChiTietDonHang = DonHang.ChiTietDonHangs;
            return View(DonHang);
        }
        [HttpPost]
        public ActionResult XoaDonHang(int? MaDonHang)
        {
            if (MaDonHang == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang DonHang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            if (DonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChiTietDonHang = DonHang.ChiTietDonHangs;
            DonHang.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("QuanLyDonHang", "QuanLyDonHang");
        }

        [HttpGet]
        public ActionResult XoaDonHang(int? MaDonHang, FormCollection form)
        {
            if (MaDonHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DonHang DonHang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            if (DonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChiTietDonHang = DonHang.ChiTietDonHangs;
            return View(DonHang);
        }
        [HttpGet]
        public ActionResult ThemDonHang()
        {
            return View();
        }
        //--------------------------------
        public ActionResult ChuaThanhToan()
        {
            //Lấy danh sách các đơn hàng Chưa duyệt
            var lst = db.DonHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayMua);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            //Lấy danh sách đơn hàng chưa giao 
            var lstDSDHCG = db.DonHangs.Where(n => n.MaTinhTrang == 2 && n.DaThanhToan == true).OrderBy(n => n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            //Lấy danh sách đơn hàng chưa giao 
            var lstDSDHCG = db.DonHangs.Where(n => n.MaTinhTrang == 1 && n.DaThanhToan == true);
            return View(lstDSDHCG);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //Kiểm tra xem id hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang model = db.DonHangs.SingleOrDefault(n => n.MaDonHang == id);
            //Kiểm tra đơn hàng có tồn tại không
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonHangs.Where(n => n.MaDonHang == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            ViewBag.MaTinhTrang = new SelectList(db.TinhTrangDonHangs.OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang", model.TinhTrangDonHang.TenTinhTrang);
            return View(model);
        }
        [HttpGet]
        public ActionResult DaHuy()
        {
            var lstDaHuy = db.DonHangs.Where(n => n.DaHuy == true);
            return View(lstDaHuy);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonHang ddh)
        {
            if(ddh.MaTinhTrang == 0)
            {
                ddh.MaTinhTrang = 2;
            }
            //Truy vấn lấy ra dữ liệu của đơn hàn đó 
            DonHang ddhUpdate = db.DonHangs.Single(n => n.MaDonHang == ddh.MaDonHang);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.MaTinhTrang = ddh.MaTinhTrang;
            ddhUpdate.DaHuy = ddh.DaHuy;
            db.SaveChanges();

            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonHangs.Where(n => n.MaDonHang == ddh.MaDonHang);
            ViewBag.ListChiTietDH = lstChiTietDH;
            ViewBag.MaTinhTrang = new SelectList(db.TinhTrangDonHangs.OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang", ddhUpdate.TinhTrangDonHang.TenTinhTrang);

            //Gửi khách hàng 1 mail để xác nhận việc thanh toán 
            string tinhTrangThanhToan = ddhUpdate.DaThanhToan ? "Đã thanh toán" : "Chưa thanh toán";
           
            string NoiDungMail = ddhUpdate.DaHuy? "<h2>Xin chào, " + ddhUpdate.KhachHang.HoTen + " </h2><h3>Đơn hàng của mã số " + ddhUpdate.MaDonHang + " bạn đã được hủy.</h3><h3>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</h3>" : "<h2>Xin chào, " + ddhUpdate.KhachHang.HoTen + " </h2><h3>Đơn hàng của mã số "+ddhUpdate.MaDonHang+" bạn đang được xử lý.</h3><p>Tình trạng thanh toán: "+ tinhTrangThanhToan + "</p><p>Tình trạng giao hàng: "+ ddhUpdate.TinhTrangDonHang.TenTinhTrang + "</p><h3>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</h3>";
            GuiEmail("Xác đơn hàng mã số " + ddhUpdate.MaDonHang +" của hệ thống ShopCOM", "moonprince9x@gmail.com", "nguyenvietthanhchuong@gmail.com", "destiny123!@#", NoiDungMail);
            return View(ddhUpdate);
        }
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title;  // tiêu đề gửi
            mail.Body = Content;                 // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;               //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true;   //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail);   //Gửi mail đi
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}