﻿using System;
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

        [HttpGet]
        public ActionResult TaoMoiDonhang()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChiTietDonHang(int? MaDonHang)
        {
            if(MaDonHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DonHang DonHang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            if (DonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChiTietDonHang = DonHang.ChiTietDonHangs;
            return View(DonHang);
        }
    }
}