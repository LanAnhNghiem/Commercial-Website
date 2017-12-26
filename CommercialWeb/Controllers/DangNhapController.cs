using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
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
        [HttpGet]
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
        public ActionResult DangNhap(string email, string password)
        {
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.Email == email && n.MatKhau == password);
            if (true)
            {
                Session["TaiKhoan"] = tv;
            }
            return RedirectToAction("ChuaGiao", "QuanLyDonHang");
        }
        
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return View("DangNhap");
        }
    }
}