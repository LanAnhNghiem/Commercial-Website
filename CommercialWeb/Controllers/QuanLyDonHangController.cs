using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommercialWeb.Controllers
{
    [Authorize(Roles = "2_QuanLyDonHang")]
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
            //Đổi tiền từ số qua chữ
            double price = Convert.ToDouble(DonHang.ChiTietDonHangs.Sum(n => n.SanPham.DonGia).Value);
            ViewBag.TienChu = ConvertMoney(price);
           
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
            var lst = db.DonHangs.Where(n => n.DaThanhToan == false && n.DaHuy == false).OrderBy(n => n.NgayMua);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            //Lấy danh sách đơn hàng chưa giao 
            var lstDSDHCG = db.DonHangs.Where(n => n.MaTinhTrang == 2 && n.DaThanhToan == true && n.DaHuy == false).OrderBy(n => n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            //Lấy danh sách đơn hàng chưa giao 
            var lstDSDHCG = db.DonHangs.Where(n => n.MaTinhTrang == 1 && n.DaThanhToan == true && n.DaHuy == false);
            return View(lstDSDHCG);
        }
        [HttpGet]
        public ActionResult DaHuy()
        {
            var lstDaHuy = db.DonHangs.Where(n => n.DaHuy == true);
            return View(lstDaHuy);
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
            //Đổi tiền từ số qua chữ
            double price = Convert.ToDouble(model.ChiTietDonHangs.Sum(n => n.SanPham.DonGia).Value);
            ViewBag.TienChu = ConvertMoney(price);
            //Thời hạn bảo hành
            ViewBag.BaoHanh = lstChiTietDH.Join(db.SanPhams, n => n.MaSP, m => m.MaSP, (n, m) => new { ThoiHanBaoHanh = m.ThoiHanBaoHanh })
                .Select(n => new { n.ThoiHanBaoHanh });
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonHang ddh)
        {
            if(ddh.MaTinhTrang == 0)
            {
                ddh.MaTinhTrang = 2;
            }
            //Truy vấn lấy ra dữ liệu của đơn hàng đó 
            DonHang ddhUpdate = db.DonHangs.Single(n => n.MaDonHang == ddh.MaDonHang);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.MaTinhTrang = ddh.MaTinhTrang;
            ddhUpdate.NgayGiao = DateTime.Now;
            ddhUpdate.DaHuy = ddh.DaHuy;
            db.SaveChanges();

            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonHangs.Where(n => n.MaDonHang == ddh.MaDonHang);
            ViewBag.ListChiTietDH = lstChiTietDH;
            ViewBag.MaTinhTrang = new SelectList(db.TinhTrangDonHangs.OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang", ddhUpdate.TinhTrangDonHang.TenTinhTrang);

            //Đổi tiền từ số qua chữ
            double price = Convert.ToDouble(ddhUpdate.ChiTietDonHangs.Sum(n => n.SanPham.DonGia).Value);
            ViewBag.TienChu = ConvertMoney(price);
            //Gửi khách hàng 1 mail để xác nhận việc thanh toán 
            string tinhTrangThanhToan = ddhUpdate.DaThanhToan ? "Đã thanh toán" : "Chưa thanh toán";
           
            string NoiDungMail = ddhUpdate.DaHuy? "<h2>Xin chào, " + ddhUpdate.KhachHang.HoTen + " </h2><h3>Đơn hàng của mã số " + ddhUpdate.MaDonHang + " bạn đã được hủy.</h3><h3>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</h3>" : "<h2>Xin chào, " + ddhUpdate.KhachHang.HoTen + " </h2><h3>Đơn hàng của mã số "+ddhUpdate.MaDonHang+" bạn đang được xử lý.</h3><p>Tình trạng thanh toán: "+ tinhTrangThanhToan + "</p><p>Tình trạng giao hàng: "+ ddhUpdate.TinhTrangDonHang.TenTinhTrang + "</p><h3>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</h3>";
            //----------------------------------------
            StringBuilder MailContent = new StringBuilder();
            MailContent.Append("<p>Cảm ơn quý khách đã sử dụng dịch vụ của chúng tôi, chúng tôi sẽ liên lạc lại cho quý khách trong thời gian sớm nhất:</p>");
            MailContent.Append("<table>");
            MailContent.Append("<tr><td colspan='2'><h3>Thông tin khách hàng</h3></td></tr>");
            MailContent.Append("<tr><td>Họ và tên:</td><td style='color:blue; font-size:16px; font-weight:bold'>" + ddhUpdate.KhachHang.HoTen+ "</td></tr>");
            MailContent.Append("<tr><td>Số điện thoại:</td><td>"+ddhUpdate.KhachHang.SoDienThoai+"</td></tr>");
            MailContent.Append("<tr><td>Địa chỉ:</td><td>"+ddhUpdate.KhachHang.DiaChi+"</td></tr>");
            MailContent.Append("<tr><td>Email:</td><td>"+ddhUpdate.KhachHang.Email+"</td></tr>");
            MailContent.Append("<br/>");
            MailContent.Append("<tr><td>Tình trạng thanh toán:</td><td style='color:blue; font-size:14px; font-weight:bold'>" + tinhTrangThanhToan + "</td></tr>");
            MailContent.Append("<tr><td>Tình trạng đơn hàng:</td><td style='color:blue; font-size:14px; font-weight:bold'>" + ddhUpdate.TinhTrangDonHang.TenTinhTrang + "</td></tr>");
            MailContent.Append("<tr><td>Liên hệ chúng tôi:</td><td>https://shopcom.com (+84)0123456267</td></tr>");
            MailContent.Append("</table>");
            //---------------------------------------------
            GuiEmail("Xác đơn hàng mã số " + ddhUpdate.MaDonHang +" của hệ thống ShopCOM", "moonprince9x@gmail.com", "nguyenvietthanhchuong@gmail.com", "destiny123!@#", MailContent.ToString());

            ViewBag.Message = "Lưu thành công";

            return View(ddhUpdate);
        }

        public ActionResult HuyDonHang(int? id)
        {
            DonHang dhHuy = db.DonHangs.SingleOrDefault(p => p.MaDonHang == id);
            dhHuy.DaHuy = true;
            db.SaveChanges();
            return RedirectToAction("ChuaGiao");
        }

        public ActionResult HoanTacDonHang(int? id)
        {
            DonHang dhHuy = db.DonHangs.SingleOrDefault(p => p.MaDonHang == id);
            dhHuy.DaHuy = false;
            db.SaveChanges();
            return RedirectToAction("DaHuy", "QuanLyDonHang");
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

        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";
            }
            return Ktach;
        }
        //convert tiền từ số qua chữ
        public static string ConvertMoney(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }
    }
}