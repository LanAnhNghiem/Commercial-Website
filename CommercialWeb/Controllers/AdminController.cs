using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.Web.Security;

namespace CommercialWeb.Controllers
{
    public class AdminController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Render menu quản lý
        /// Creator: Chương
        /// </summary>
        /// <returns>Partial view cho menu quản lý</returns>
        public ActionResult MenuQuanLy()
        {
            if (Session["TaiKhoan"] != null)
            {
                ThanhVien tv = (ThanhVien)Session["TaiKhoan"];
                List<String> lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV).OrderByDescending(n => n.MaQuyen).Select(n => n.MaQuyen).ToList();
                ViewBag.lstQuyen = lstQuyen;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return PartialView();
        }

       
    }
}