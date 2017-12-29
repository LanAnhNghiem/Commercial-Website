using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.IO;
using System.Net;
using System.Data.Entity.Migrations;


namespace CommercialWeb.Controllers
{
    [Authorize(Roles = "4_QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLySanPham
        public ActionResult IndexQuanLySP()
        {
            return View(db.SanPhams.OrderByDescending(n=>n.MaSP));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            // Load dropdownlist nhà cung cấp và nhà sản xuất
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");

            return View();
        }

        
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            // Load dropdownlist nhà cung cấp và nhà sản xuất
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");

            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiểm tra nội dung hình ảnh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //Kiểm tra định dạng hình ảnh
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload += "Hình ảnh" + i + " không hợp lệ <br />";
                            loi++;
                        }
                        else
                        {
                            //Kiểm tra hình ảnh tồn tại

                            //Lấy tên hình ảnh
                            var fileName = Path.GetFileName(HinhAnh[0].FileName);
                            //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                            var path = Path.Combine(Server.MapPath("~/Content/ProductImages"), fileName);
                            //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload1 = "Hình " + i + "đã tồn tại <br />";
                                loi++;
                            }
                        }
                    }
                }

            }
            if (loi > 0)
            {
                return View(sp);
            }
            sp.HinhAnh1 = HinhAnh[0].FileName;
            sp.HinhAnh2 = HinhAnh[1].FileName;
            sp.HinhAnh3 = HinhAnh[2].FileName;

            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i].ContentLength > 0)
                {
                    //Lấy tên hình ảnh
                    var fileName = Path.GetFileName(HinhAnh[i].FileName);
                    //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                    var path = Path.Combine(Server.MapPath("~/Content/ProductImages"), fileName);
                    HinhAnh[i].SaveAs(path);
                }
            }
            sp.MaKhuyenMai = 1;
            sp.GiaBan = sp.DonGia;
            sp.SoLuongTon = 0;
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("IndexQuanLySP");
        }
        
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            SanPham tp = db.SanPhams.SingleOrDefault(n => n.MaSP == sp.MaSP);
            // Lưu ảnh
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiểm tra nội dung hình ảnh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //Lấy tên hình ảnh
                        var fileName = Path.GetFileName(HinhAnh[i].FileName);
                        //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                        var path = Path.Combine(Server.MapPath("~/Content/ProductImages"), fileName);
                        //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        HinhAnh[i].SaveAs(path);
                    }
                }
            }

           
            if (HinhAnh[0] != null)
            {
                sp.HinhAnh1 = HinhAnh[0].FileName;
            }
            else
            {
                sp.HinhAnh1 = tp.HinhAnh1;
            }
            if (HinhAnh[1] != null)
            {
                sp.HinhAnh2 = HinhAnh[1].FileName;
            }
            else
            {
                sp.HinhAnh2 = tp.HinhAnh2;
            }
            if (HinhAnh[2] != null)
            {
                sp.HinhAnh3 = HinhAnh[2].FileName;
            }
            else
            {
                sp.HinhAnh3 = tp.HinhAnh3;
            }
            if(sp.MaKhuyenMai == null)
            {
                sp.MaKhuyenMai = 1;
                sp.GiaBan = sp.DonGia;
            }
            db.Set<SanPham>().AddOrUpdate(sp);
            db.SaveChanges();
            return RedirectToAction("IndexQuanLySP");
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Xóa thành công";

            return RedirectToAction("IndexQuanLySP");
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

        public  ActionResult TimKiem(string sTuKhoa)
        {
            IEnumerable<SanPham> lstSP = db.SanPhams.Where(n => n.TenSP == sTuKhoa || n.NhaSanXuat.TenNSX == sTuKhoa);
            return View("IndexQuanLySP", lstSP);
        }
    }
}