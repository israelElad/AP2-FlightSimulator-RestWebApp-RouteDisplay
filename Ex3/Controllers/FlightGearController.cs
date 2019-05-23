using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult display()
        {
            return View();
        }
    }
}