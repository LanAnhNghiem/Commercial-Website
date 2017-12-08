using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang
        public List<ItemGioHang> LayGioHang()
        {
            //Cart exsited
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            //if Cart null
            if(lstGioHang == null)
            {
                // da sua
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Add cart (normal)
        public ActionResult ThemGioHang(int MaSP, string URL)
        {
            //check if a product exsited in Database or not
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                //Show 404 error, invalid product's url
                Response.StatusCode = 404;
                return null;
            }
            //Get GioHang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //If the product was inserted in Cart
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck != null)
            {
                //Check number of product in database before the customers add more products in their cart
                if(sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("Notification");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(URL);
            }
            
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("Notification");
            }
            lstGioHang.Add(itemGH);
            return Redirect(URL);
        }

        //Total number of product
        public double TinhTongSoLuong()
        {
            //Get GioHang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        //Total (price)
        //public decimal TinhTongTien()
        //{
        //    //Get GioHang
        //    List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
        //    if (lstGioHang == null)
        //    {
        //        return 0;
        //    }
        //    return lstGioHang.Sum(n => n.ThanhTien);
        //}
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();

            return PartialView();
        }
    }
}