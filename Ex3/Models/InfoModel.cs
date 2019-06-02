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
        public bool Save { get; set; }

        public const string SCENARIO_FILE = "App_Data/{0}.txt";           // The Path of the Secnario

        // instance for singleton pattern
        private static InfoModel instance = null;

        private InfoModel()
        {
            DF = new DisplayFlight();
            mutex = new Mutex();
            Save = false;
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
            if (!Client.Instance.IsConnected)
            {
                Client.Instance.ConnectToServer(ip, port);
            }
            Read();          
        }

        public void ReadAlways(string ip, int port)
        {
            // connect the plane
            if (!Client.Instance.IsConnected)
            {
                Client.Instance.ConnectToServer(ip, port);
            }
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

            string data = DF.Lon + "~" + DF.Lat + "~" + DF.Rudder + "~" + DF.Throttle;

            if (Save)
            {
                SaveToFile(data);
            }

            //System.Diagnostics.Debug.WriteLine(DF.Lat + "----" + DF.Lon + "----" + DF.Rudder + "----" + DF.Throttle);
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
        private void SaveToFile(string data)
        {
            string pathFormat = String.Format(SCENARIO_FILE, FileName);
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, pathFormat);
            if (!File.Exists(filePath))
            {
                using (StreamWriter file = new StreamWriter(filePath, true))
                {
                    file.WriteLine(data);
                }
            }
            else
            {
                string[] line = new string[1];
                line[0] = string.Copy(data);
                File.AppendAllLines(filePath, line);
            }
        }

        /* load from file */
        public void LoadFromFile()
        {
            string pathFormat = String.Format(SCENARIO_FILE, FileName);
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, pathFormat);

            string[] lines = System.IO.File.ReadAllLines(filePath);        // reading all the lines of the file
            string[] line = lines[0].Split('~');
            InfoModel.instance.DF.Lon = Convert.ToDouble(line[0]);
            InfoModel.instance.DF.Lat = Convert.ToDouble(line[1]);

            System.Diagnostics.Debug.WriteLine(InfoModel.instance.DF.Lon + "///" + InfoModel.instance.DF.Lat);
        }
    }
}