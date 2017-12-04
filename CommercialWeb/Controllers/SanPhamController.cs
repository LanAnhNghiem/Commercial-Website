using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult SanPhamLaptopPartial()
        {
            return PartialView();
        }
    }
}