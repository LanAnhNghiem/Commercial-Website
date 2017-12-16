using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    public class QuanLyDonDatHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonDatHang
        public ActionResult QuanLyDonHang()
        {
            return View(db.DonHangs.Where(n=>n.MaTinhTrang == 1));
        }
    }
}