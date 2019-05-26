using Ex3.Models;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class FlightGearController : Controller
    {
        /* default */
        public ActionResult Index()
        {
            return View();
        }

        /* Sample only one time the position of the plane and displaying it on the map */
        [HttpGet]
        public ActionResult DisplayLocation(string ip, int port)
        {
            DisplayLocation displayLocation = new DisplayLocation(ip,port);
            return View(displayLocation);
        }

        /* Sample 4 times per second the position of the plane and displaying it on the map */
        [HttpGet]
        public ActionResult DisplayRefreshingLocation(string ip, int port, int time)
        {
            DisplayRefreshingLocation displayRefreshingLocation = new DisplayRefreshingLocation(ip, port, time);
            return View();
        }

        /*
        * Sample(with the given rate for the duration received) and save the flight data from the given IP and port.
        * for example, can be accessed via the following uri: /save/127.0.0.1/5400/4/10/flight1
         */
        [HttpGet]
        public ActionResult SaveFlightData(string ip, int port, int time,int duration, string fileName)
        {
            SaveFlightData saveFlightData = new SaveFlightData(ip, port, time,duration,fileName);
            return View();
        }
    }
}