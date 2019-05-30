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
            Session["Lon"] = InfoModel.Instance.DF.Lon;
            Session["Lat"] = InfoModel.Instance.DF.Lat;

            return View();
        }

        /* Sample 4 times per second the position of the plane and displaying it on the map */
        [HttpGet]
        public ActionResult DisplayRefreshingLocation(string ip, int port, int time)
        {
            Session["Time"] = time;
            Session["Duration"] = -1;
            InfoModel.Instance.ReadAlways(ip, port);

            return View("DisplayOrSaveRefreshingLocation");
        }

        [HttpPost]
        public string GetFlightData()
        {
            DisplayFlight displayFlight = InfoModel.Instance.DF;

            return ToXml(displayFlight);
        }

        private string ToXml(DisplayFlight display)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Displays");

            display.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        /*
        * Sample(with the given rate for the duration received) and save the flight data from the given IP and port.
        * for example, can be accessed via the following uri: /save/127.0.0.1/5400/4/10/flight1
         */
        [HttpGet]
        public ActionResult SaveFlightData(string ip, int port, int time,int duration, string fileName)
        {
            InfoModel.Instance.Save = true;

            Session["Time"] = time;
            Session["Duration"] = duration;
            InfoModel.Instance.FileName = fileName;
            InfoModel.Instance.ReadAlways(ip, port);

            return View("DisplayOrSaveRefreshingLocation");
        }
    }
}