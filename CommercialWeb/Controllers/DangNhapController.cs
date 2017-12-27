using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.Web.Security;

namespace CommercialWeb.Controllers
{
    public class DangNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: DangNhap
        /// <summary>
        /// Chức năng đăng nhập
        /// Creator:Chương
        /// </summary>
        /// <returns>Trang đăng nhập</returns>
        public ActionResult DangNhap()
        {
            return View();
        }

        /// <summary>
        /// Chức năng đăng nhập
        /// Creator: Chương
        /// </summary>
        /// <param name="email">Nhận vào email</param>
        /// <param name="password">Nhận vào password</param>
        /// <returns>Điều hướng tới trang đơn hàng chưa giao</returns>
        [HttpPost]
        public ActionResult XuLyDangNhap(string email, string password)
        {
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.Email == email && n.MatKhau == password);
            if (tv != null)
            {
                //Lấy ra list quyền của thành viên tương ứng với loại thành viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //Duyệt list quyền
                string Quyen = "";
                if (lstQuyen.Count() != 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        Quyen += item.Quyen.MaQuyen + ",";
                    }
                    Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt dấu "," cuối   
                    PhanQuyen(tv.Email.ToString(), Quyen);
                }
                Session["TaiKhoan"] = tv;
                return JavaScript("window.location = '" + Url.Action("ChuaGiao", "QuanLyDonHang") + "'");
            }
            return Content("LOGIN FAILED !");
        }
        /// <summary>
        /// Hàm phân quyền cho tài khoản đăng nhập hiện thời
        /// Creator: Chương
        /// </summary>
        /// <param name="TaiKhoan">Tên đăng nhập</param>
        /// <param name="Quyen">Danh sách quyền dạng chuỗi  </param>
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                        TaiKhoan, //user
                        DateTime.Now, //begin
                        DateTime.Now.AddHours(3), //timeout
                        false, //remember?
                        Quyen, // permission.. "admin" or for more than one "admin,marketing,sales"
                        FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
        /// <summary>
        /// Đăng xuất khỏi hệ thống trở về trang đăng nhập
        /// Creator: Chương
        /// </summary>
        /// <returns>Trả về trang đăng nhập</returns>
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap");
        }
    }
}