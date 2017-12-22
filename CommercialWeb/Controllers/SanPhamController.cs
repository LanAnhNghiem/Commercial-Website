using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using System.Net;

namespace CommercialWeb.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        [ChildActionOnly]
        public ActionResult SanPhamMoiPartial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult LaptopNoiBatPartial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamKhuyenMaiPartial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult LaptopPartial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MayTinhPartial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult LinhKienPartial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SliderBannerPartial()
        {
            return PartialView();
        }


        // xây dựng trang xem chi tiết
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            //Kiểm tra tham số truyền vào có rổng hay không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            return View(sp);
        }
    }
}