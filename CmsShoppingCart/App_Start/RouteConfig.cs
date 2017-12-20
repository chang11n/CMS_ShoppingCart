using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmsShoppingCart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional}, new[] { "CmsShoppingCart.Controllers" });

            routes.MapRoute("SidePartial", "Pages/SidebarPartial", new { controller = "Pages", action = "SidebarPartial" }, new[] { "CmsShoppingCart.Controllers" });
            routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "CmsShoppingCart.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "CmsShoppingCart.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "CmsShoppingCart.Controllers" });

            //routes.maproute(
            //    name: "default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "home", action = "index", id = urlparameter.optional }
            //);
        }
    }
}
