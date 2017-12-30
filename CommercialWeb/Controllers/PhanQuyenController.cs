using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    [Authorize(Roles = "7_PhanQuyen")]
    public class PhanQuyenController : Controller
    {
        
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();  
        // GET: PhanQuyen
        /// <summary>
        /// Load danh sách loại thành viên
        /// Creator: Chương
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.LoaiThanhViens.Where(n=>n.MaLoaiTV != 4).OrderBy(n=>n.TenLoai));
        }

        /// <summary>
        /// Lấy danh sách quyền của loại thành viên đổ ra checkbox
        /// Creator: Chương
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PhanQuyen(int? id)
        {
            //Lấy mã loại tv dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null)
            {
                return HttpNotFound();
            }
            //Lấy ra danh sách quyền để load ra check box
            ViewBag.MaQuyen = db.Quyens;
            //Lấy ra danh sách quyền của loại thành viên đó
            //Bước 1: Lấy ra những quyền thuộc loại thành viên đó dựa vào bảng LoaiThanhVien_Quyen
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);
            return View(ltv);
        }

        [HttpPost]
        public ActionResult PhanQuyen(int? MaLTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {

            //Trường hợp : Nếu đã đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //Bước 1: Xóa những quyền cũa thuộc loại TV đó
            var lstDaPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV);
            if (lstDaPhanQuyen.Count() != 0)
            {
                db.LoaiThanhVien_Quyen.RemoveRange(lstDaPhanQuyen);
                db.SaveChanges();
            }
            if (lstPhanQuyen != null)
            {
                //Kiểm tra list danh sách quyền được check
                foreach (var item in lstPhanQuyen)
                {
                    item.MaLoaiTV = int.Parse(MaLTV.ToString());
                    //Nếu được check thì insert dữ liệu vào bảng phân quyền
                    db.LoaiThanhVien_Quyen.Add(item);


                }
                db.SaveChanges();
            }
            ViewBag.ThongBao = "Thành công !";
            //Lấy ra danh sách quyền để load ra check box
            ViewBag.MaQuyen = db.Quyens;
            //Lấy ra danh sách quyền của loại thành viên đó
            //Bước 1: Lấy ra những quyền thuộc loại thành viên đó dựa vào bảng LoaiThanhVien_Quyen
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV);
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == MaLTV);
            return View(ltv);
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