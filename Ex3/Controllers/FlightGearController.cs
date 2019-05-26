using Ex3.Models;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class FlightGearController : Controller
    {
        // GET: FlightGear
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DisplayLocation(string ip, int port)
        {
            DisplayLocation displayLocation = new DisplayLocation(ip,port);
            return View(displayLocation);
        }

        [HttpGet]
        public ActionResult DisplayRefreshingLocation(string ip, int port, int time)
        {
            DisplayRefreshingLocation displayRefreshingLocation = new DisplayRefreshingLocation(ip, port, time);
            return View();
        }
    }
}