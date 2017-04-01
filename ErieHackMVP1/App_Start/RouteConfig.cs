using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErieHackMVP1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "OnlyAction",
                "{action}",
                new { controller = "Home", action = "Index" }
            );

            routes.MapRoute("Browse", "Browse/{query}/{startIndex}",
                        new
                        {
                            controller = "Reports",
                            action = "Browse",
                            startIndex = 0,
                            pageSize = 20
                        });

            routes.MapRoute("BrowseBy", "Browse/{query}/{startIndex}",
                        new
                        {
                            controller = "Reports",
                            action = "BrowseBy",
                            startIndex = 0,
                            pageSize = 20
                        });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}
