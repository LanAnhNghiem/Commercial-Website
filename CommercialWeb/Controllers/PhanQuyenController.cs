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
        public ActionResult Index()
        {
            return View(db.LoaiThanhViens.OrderBy(n=>n.TenLoai));
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