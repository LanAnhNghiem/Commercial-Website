using CommercialWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommercialWeb.Controllers
{
    [Authorize(Roles = "3_QuanLyKhachHang")]
    public class QuanLyKhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyKhachHang
        public ActionResult Index()
        {
            return View(db.KhachHangs.OrderByDescending(n => n.MaKH));
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lấy sản phẩm cần chỉnh  sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                return HttpNotFound();
            }

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaThanhVien = new SelectList(db.ThanhViens.OrderBy(n => n.HoTen), "MaThanhVien", "HoTen");
            return View(kh);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {

            ViewBag.MaThanhVien = new SelectList(db.ThanhViens.OrderBy(n => n.HoTen), "MaThanhVien", "HoTen");
            //Nếu dữ liệu đầu vào chắn chắn ok 
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}