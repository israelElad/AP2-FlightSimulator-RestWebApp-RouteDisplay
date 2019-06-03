using Ex3.Models;
using System.Net;
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

        /* Sample only one time the position of the plane and displaying it on the map *or* load flight
         * data from file and display it like an animation */
        [HttpGet]
        public ActionResult DisplayLocationOrLoadRefreshingLocation(string IPOrFileName, int PortOrTime)
        {
            IPAddress ip;
            // If the IP address is correct -> view = DisplayLocation
            if (IPAddress.TryParse(IPOrFileName, out ip))
            {
                // disconnect if connected
                if (Client.Instance.IsConnected)
                {
                    Client.Instance.CloseClient();
                }
                Client.Instance.ConnectToServer(IPOrFileName, PortOrTime);

                InfoModel.Instance.ReadOnce();

                Session["Lon"] = InfoModel.Instance.DF.Lon;
                Session["Lat"] = InfoModel.Instance.DF.Lat;

                return View("DisplayLocation");
            }
            // Else, view = LoadRefreshingLocation
            Session["Time"] = PortOrTime;
            InfoModel.Instance.FileName = IPOrFileName;
            return View("LoadRefreshingLocation");
        }

        /* Sample 4 times per second the position of the plane and displaying it on the map */
        [HttpGet]
        public ActionResult DisplayRefreshingLocation(string ip, int port, int time)
        {
            Session["Time"] = time;

            // disconnect if connected
            if (Client.Instance.IsConnected)
            {
                Client.Instance.CloseClient();
            }
            Client.Instance.ConnectToServer(ip, port);

            return View();
        }

        /*
        * Sample(with the given rate for the duration received) and save the flight data from the given IP and port.
        * for example, can be accessed via the following uri: /save/127.0.0.1/5400/4/10/flight1
         */
        [HttpGet]
        public ActionResult SaveRefreshingLocation(string ip, int port, int time, int duration, string fileName)
        {
            InfoModel.Instance.ClearFile();

            Session["Time"] = time;
            Session["Duration"] = duration;
            InfoModel.Instance.FileName = fileName;

            // disconnect if connected
            if (Client.Instance.IsConnected)
            {
                Client.Instance.CloseClient();
            }
            Client.Instance.ConnectToServer(ip, port);

            return View();
        }

        /*
         * Called every 'time' seconds from the view,
         * each time reading the flight's data from the server,
         * and passing their value as xml to the view.
         */
        [HttpPost]
        public string GetFlightData()
        {
            DisplayFlight displayFlight = InfoModel.Instance.DF;

            InfoModel.Instance.ReadOnce();

            return ToXml(displayFlight);
        }

        /*
         * Called every 'time' seconds for 'duration' seconds from the view,
         * each time reading the flight's data from the server, saving them to a file,
         * and passing their value as xml to the view.
         */
        [HttpPost]
        public string SaveFlightData()
        {
            DisplayFlight displayFlight = InfoModel.Instance.DF;

            InfoModel.Instance.ReadOnce();
            InfoModel.Instance.SaveFlightDataToFile();

            return ToXml(displayFlight);
        }

        /*
         * Called every 'time' seconds from the view,
         * each time reading the flight's data from the file,
         * and passing their value as xml to the view.
         */
        [HttpPost]
        public string LoadFlightData()
        {
            DisplayFlight displayFlight = InfoModel.Instance.DF;
            //only load the file to the List at the first time that the function is called
            if (InfoModel.Instance.flightData==null) 
            {
                InfoModel.Instance.LoadFlightDataFromFile();
            }
            InfoModel.Instance.UpdateFlightDataInDisplayFlight();
            return ToXml(displayFlight);
        }

        /*
         * Writes all data to a xml file.
         */
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
    }
}