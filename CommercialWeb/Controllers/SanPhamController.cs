using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommercialWeb.Models;

namespace CommercialWeb.Controllers
{
    public class SanPhamController : Controller
    {
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
    }
}