using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("DisplayLocation", "display/{ip}/{port}",
            defaults: new { controller = "FlightGear", action = "DisplayLocation" });

            routes.MapRoute("DisplayRefreshingLocation", "display/{ip}/{port}/{time}",
            defaults: new { controller = "FlightGear", action = "DisplayRefreshingLocation" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FlightGear", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
