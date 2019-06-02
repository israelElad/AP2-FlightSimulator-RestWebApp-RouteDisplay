using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Ex3.Models
{
    public class InfoModel
    {
        public DisplayFlight DF { get; set; }
        private Mutex mutex;
        public string FileName { get; set; }
        public string[] flightData;
        private int flightDataIndex;

        public const string SCENARIO_FILE = "App_Data/{0}.txt";           // The Path of the Secnario

        // instance for singleton pattern
        private static InfoModel instance = null;

        private InfoModel()
        {
            DF = new DisplayFlight();
            mutex = new Mutex();
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
            // disconnect if connected
            if (Client.Instance.IsConnected)
            {
                Client.Instance.CloseClient();
            }
            //connect to FlightGear's server
            Client.Instance.ConnectToServer(ip, port);
            Read();
            Client.Instance.CloseClient();
        }

        public void ReadAlways(string ip, int port)
        {
            // disconnect if connected
            if (Client.Instance.IsConnected)
            {
                Client.Instance.CloseClient();
            }
            //connect to FlightGear's server
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
            DF.Lat = GetDoubleFromString(latStr);

            // Read Lon's value
            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            string lonStr = Client.Instance.ReadAnswerFromServer();
            DF.Lon = GetDoubleFromString(lonStr);

            // Read Rudder's value
            Client.Instance.WriteToServer("get /controls/flight/rudder\r\n");
            string rudderStr = Client.Instance.ReadAnswerFromServer();
            DF.Rudder = GetDoubleFromString(rudderStr);

            // Read Throttle's value
            Client.Instance.WriteToServer("get /controls/engines/current-engine/throttle\r\n");
            string throttleStr = Client.Instance.ReadAnswerFromServer();
            DF.Throttle = GetDoubleFromString(throttleStr);

            System.Diagnostics.Debug.WriteLine(DF.Lat + "----" + DF.Lon + "----" + DF.Rudder + "----" + DF.Throttle);
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

        /* Save to file */
        public void SaveFlightDataToFile()
        {
            string data = DF.Lon + "~" + DF.Lat + "~" + DF.Rudder + "~" + DF.Throttle;

            string pathFormat = String.Format(SCENARIO_FILE, FileName);
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, pathFormat);
            // If the file does not exist - create it and write to it
            if (!File.Exists(filePath))
            {
                using (StreamWriter file = new StreamWriter(filePath, true))
                {
                    file.WriteLine(data);
                }
            }
            // Else, just write to it
            else
            {
                string[] line = new string[1];
                line[0] = string.Copy(data);
                File.AppendAllLines(filePath, line);
            }
        }

        /* load from file */
        public void LoadFlightDataFromFile()
        {
            string pathFormat = String.Format(SCENARIO_FILE, FileName);
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, pathFormat);
            // reading all the lines of the file
            flightData = System.IO.File.ReadAllLines(filePath);        
            flightDataIndex =0;
        }

        // Updating LAT and LON once by file
        public void UpdateFlightDataInDisplayFlight()
        {
            // If the file is finished
            if (flightDataIndex >= flightData.Length)
            {
                DF.EOF = "true";
                InfoModel.Instance.flightData = null;
                return;
            }
            string[] line = flightData[flightDataIndex].Split('~');
            DF.Lon = Convert.ToDouble(line[0]);
            DF.Lat = Convert.ToDouble(line[1]);
            DF.EOF = "false";
            flightDataIndex++;
            System.Diagnostics.Debug.WriteLine(DF.Lon + "///" +DF.Lat);
        }
    }
}