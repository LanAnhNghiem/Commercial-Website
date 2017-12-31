using CommercialWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CommercialWeb.Controllers
{
    public class Product_DetailController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //
        // GET: /Product_Detail/
        public ActionResult Detail(int? id, string tensp)
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