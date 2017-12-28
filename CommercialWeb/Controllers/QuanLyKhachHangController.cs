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
        public ActionResult IndexQuanLyKH()
        {
            return View(db.KhachHangs.OrderByDescending(n => n.MaKH));
        }

        /// <summary>
        /// Creator: Bửu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creator: Bửu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(KhachHang model)
        {
            //Nếu dữ liệu đầu vào chắn chắn ok 
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == model.MaKH);
            kh.HoTen = model.HoTen;
            kh.DiaChi = model.DiaChi;
            kh.SoDienThoai = model.SoDienThoai;
            kh.Email = model.Email;
            db.SaveChanges();
            ViewBag.ThongBao = "Thành công !";
            return View(kh);
        }

        /// <summary>
        /// Tìm kiếm khách hàng bằng tên hoặc số đt
        /// Creator: Chương
        /// </summary>
        /// <param name="sTuKhoa"></param>
        /// <returns>Danh sách khách hàng</returns>
        public ActionResult TimKiem(string sTuKhoa)
        {
            IEnumerable<KhachHang> lstKH = db.KhachHangs.Where(n => n.HoTen.Contains(sTuKhoa) || n.SoDienThoai.Contains(sTuKhoa) || n.Email.Contains(sTuKhoa) || n.DiaChi.Contains(sTuKhoa));
            return View("IndexQuanLyKH", lstKH);
        }
    }
}