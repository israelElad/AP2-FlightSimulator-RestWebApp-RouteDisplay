using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Ex3.Models
{
    public class DisplayUtils
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }

        Mutex mutex;

        public DisplayUtils()
        {
            Lat = 0;
            Lon = 0;
            Rudder = 0;
            Throttle = 0;

            mutex = new Mutex();
        }

        /* Get a double from a string by using a regular expression */
        public double GetDoubleFromString(string str)
        {
            string pattern = "[-+]?[0-9]*\\.?[0-9]+";
            Regex reg = new Regex(pattern);
            Match match = reg.Match(str);
            // if there is no double in the string
            if (!match.Success)
            {
                Console.WriteLine("No floating number found in the string!");
            }
            double num = Convert.ToDouble(match.Value);
            return num;
        }

        /* Read the values of Lat and Lon from the plane */
        public void ReadLatAndLon(string ip, int port)
        {
            mutex.WaitOne();
            // connect the plane
            Client.Instance.ConnectToServer(ip, port);
            mutex.ReleaseMutex();

            // Read Lat's value
            Client.Instance.WriteToServer("get /position/latitude-deg\r\n");
            string latStr = Client.Instance.ReadAnswerFromServer();
            Lat = GetDoubleFromString(latStr);

            // Read Lon's value
            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            string lonStr = Client.Instance.ReadAnswerFromServer();
            Lon = GetDoubleFromString(lonStr);

            // Read Rudder's value
            Client.Instance.WriteToServer("get /controls/flight/rudder\r\n");
            string rudderStr = Client.Instance.ReadAnswerFromServer();
            Rudder = GetDoubleFromString(rudderStr);

            // Read Throttle's value
            Client.Instance.WriteToServer("get /controls/engines/current-engine/throttle\r\n");
            string throttleStr = Client.Instance.ReadAnswerFromServer();
            Throttle = GetDoubleFromString(throttleStr);

            System.Diagnostics.Debug.WriteLine(Lat + "----" + Lon + "----" + Rudder + "----" + Throttle);
        }
    }
}