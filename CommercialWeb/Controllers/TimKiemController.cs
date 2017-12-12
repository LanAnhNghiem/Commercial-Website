using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;
using PagedList;

namespace CommercialWeb.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            //Nhà sản xuất
            var lstNSX = db.NhaSanXuats;
            ViewBag.ListNSX = lstNSX;

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //Tim kiem theo ten san pham
            var listSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.ListSP = listSP;
            ViewBag.TuKhoa = sTuKhoa;
            
            return View(listSP.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult KQTimKiem(String sTuKhoa)
        {
            //Goi ve ham get tim kiem
            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa });
        }
        public ActionResult NhaSXPartial()
        {
            return PartialView();
        }
        public ActionResult DSSanPhamPartial()
        {
           
            return PartialView();
        }
        public ActionResult LoadSanPham(int MaLoai)
        {
            switch (MaLoai)
            {
                case 1:
                    //Tìm kiếm tất cả linh kiện
                    var listLK = db.SanPhams.Where(n => n.MaLoaiSP == 1);
                    ViewBag.ListSP = listLK;
                    break;
                case 2:
                    var listMT = db.SanPhams.Where(n => n.MaLoaiSP == 2);
                    ViewBag.ListSP = listMT;
                    break;
                case 3:
                    //Tìm kiếm tất cả laptop
                    var listLT = db.SanPhams.Where(n => n.MaLoaiSP == 3);
                    ViewBag.ListSP = listLT;
                    break;
                case 10:
                    var listGia1 = db.SanPhams.Where(n => n.DonGia <= 1000000);
                    ViewBag.ListSP = listGia1;
                    break;
                case 11:
                    var listGia2 = db.SanPhams.Where(n => n.DonGia > 1000000 && n.DonGia <= 5000000);
                    ViewBag.ListSP = listGia2;
                    break;
                case 12:
                    var listGia3 = db.SanPhams.Where(n => n.DonGia > 5000000 && n.DonGia <= 10000000);
                    ViewBag.ListSP = listGia3;
                    break;
                case 13:
                    var listGia4 = db.SanPhams.Where(n => n.DonGia > 10000000 && n.DonGia <= 20000000);
                    ViewBag.ListSP = listGia4;
                    break;
                case 14:
                    var listGia5 = db.SanPhams.Where(n => n.DonGia > 20000000);
                    ViewBag.ListSP = listGia5;
                    break;
            }
            return PartialView("DSSanPhamPartial");
        }
        public ActionResult LoadTheoNSX(int MaLoai)
        {
            switch (MaLoai)
            {
                case 1:
                    var listAcer = db.SanPhams.Where(n => n.MaNSX == 1);
                    ViewBag.ListSP = listAcer;
                    break;
                case 2:
                    var listDell = db.SanPhams.Where(n => n.MaNSX == 2);
                    ViewBag.ListSP = listDell;
                    break;
                case 3:
                    var listHp = db.SanPhams.Where(n => n.MaNSX == 3);
                    ViewBag.ListSP = listHp;
                    break;
                case 4:
                    var listLenovo = db.SanPhams.Where(n => n.MaNSX == 4);
                    ViewBag.ListSP = listLenovo;
                    break;
                case 5:
                    var listVN = db.SanPhams.Where(n => n.MaNSX == 5);
                    ViewBag.ListSP = listVN;
                    break;
            }
            return PartialView("DSSanPhamPartial");
        }
    }
}