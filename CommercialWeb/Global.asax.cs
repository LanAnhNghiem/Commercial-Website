using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CommercialWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["SoLuongTruyCap"] = 0;
            Application["Online"] = 0;
        }

        protected void Session_Start()
        {
            Application.Lock(); // Dùng để đồng bộ hóa
            Application["SoLuongTruyCap"] = (int)Application["SoLuongTruyCap"] + 1;
            Application["Online"] = (int)Application["Online"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock(); // Dùng để đồng bộ hóa
            Application["Online"] = (int)Application["Online"] -1;
            Application.UnLock();
        }

    }
}
