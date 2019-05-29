using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace Ex3.Models
{
    public class InfoModel
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }

        public DisplayFlight DisplayFlight { get; set; }

        private Mutex mutex;

        // instance for singleton pattern
        private static InfoModel instance = null;

        private InfoModel()
        {
            mutex = new Mutex();
            DisplayFlight = new DisplayFlight();
        }

        /* instance method for singleton pattern */
        public static InfoModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InfoModel();
                }
                return instance;
            }
        }

        public void ReadOnce(string ip, int port)
        {
            // connect the plane
            Client.Instance.ConnectToServer(ip, port);

            Read();            
        }

        public void ReadAlways(string ip, int port)
        {
            // connect the plane
            Client.Instance.ConnectToServer(ip, port);

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Read();
                }
            }); thread.Start();
        }

        private void Read()
        {
            mutex.WaitOne();
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
            mutex.ReleaseMutex();
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
    }
}