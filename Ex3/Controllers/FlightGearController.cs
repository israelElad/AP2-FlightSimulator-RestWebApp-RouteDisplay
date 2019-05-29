using Ex3.Models;
using System.Text;
using System.Web.Mvc;
using System.Xml;

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
            InfoModel.Instance.ReadOnce(ip, port);

            Session["Lon"] = InfoModel.Instance.Lon;
            Session["Lat"] = InfoModel.Instance.Lat;

            return View(InfoModel.Instance);
        }

        /* Sample 4 times per second the position of the plane and displaying it on the map */
        [HttpGet]
        public ActionResult DisplayRefreshingLocation(string ip, int port, int time)
        {
            Session["time"] = time;

            InfoModel.Instance.ReadAlways(ip, port);
            return View(InfoModel.Instance);
        }

        [HttpPost]
        public string GetFlightData()
        {
            DisplayFlight displayFlight = InfoModel.Instance.DisplayFlight;

            return ToXml(displayFlight);
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

        private string ToXml(DisplayFlight display)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Display");

            display.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
    }
}