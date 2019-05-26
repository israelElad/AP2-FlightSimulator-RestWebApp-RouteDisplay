using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // /display/127.0.0.1/5400 
            routes.MapRoute("DisplayLocation", "display/{ip}/{port}",
            defaults: new { controller = "FlightGear", action = "DisplayLocation" });

            // /display/127.0.0.1/5400/4 
            routes.MapRoute("DisplayRefreshingLocation", "display/{ip}/{port}/{time}",
            defaults: new { controller = "FlightGear", action = "DisplayRefreshingLocation" });

            // /save/127.0.0.1/5400/4/10/flight1 
            routes.MapRoute("SaveFlightData", "save/{ip}/{port}/{time}/{duration}/{fileName}",
            defaults: new { controller = "FlightGear", action = "SaveFlightData" });

            // default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FlightGear", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
