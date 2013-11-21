using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Export",
                url: "Business/Export/ExcelReport/{DeliverDate}_{ClientName}.xls",
                defaults: new { controller = "Business", action = "Export", ClientName = UrlParameter.Optional, DeliverDate = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Business", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}