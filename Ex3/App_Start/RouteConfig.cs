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
            routes.MapRoute("DisplayLocationOrLoadFlightData", "display/{IPOrFileName}/{PortOrTime}",
            defaults: new { controller = "FlightGear", action = "DisplayLocationOrLoadFlightData" });

            // /display/127.0.0.1/5400/4 
            routes.MapRoute("DisplayRefreshingLocation", "display/{ip}/{port}/{time}",
            defaults: new { controller = "FlightGear", action = "DisplayRefreshingLocation" });

            // /save/127.0.0.1/5400/4/10/flight1 
            routes.MapRoute("SaveRefreshingLocation", "save/{ip}/{port}/{time}/{duration}/{fileName}",
            defaults: new { controller = "FlightGear", action = "SaveRefreshingLocation" });

            // default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FlightGear", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
