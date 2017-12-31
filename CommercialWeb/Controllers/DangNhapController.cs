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
        
        public ActionResult DangNhap()
         {
            if (Session["TaiKhoan"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

    
        [HttpPost]
        public ActionResult XuLyDangNhap(string email, string password)
        {
            //Nếu đã đăng nhập thì chuyển thẳng tới
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
                return JavaScript("window.location = '" + Url.Action("Index", "Admin") + "'");
            }
            return Content("LOGIN FAILED !");
        }
       
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
        
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap");
        }
    }
}