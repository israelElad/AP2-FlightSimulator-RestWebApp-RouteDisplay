using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ex3.Models
{
    public class DisplayUtils
    {
        public double Lat { get; set; }
        public double Lon { get; set; }

        public double GetDoubleFromString(string str)
        {
            string pattern = "[-+]?[0-9]*\\.?[0-9]+";
            Regex reg = new Regex(pattern);
            Match match = reg.Match(str);
            if (!match.Success)
            {
                Console.WriteLine("No floating number found in the string!");
            }
            double num = Convert.ToDouble(match.Value);
            return num;
        }

        public void ReadLatAndLon(string ip, int port)
        {
            Client.Instance.ConnectToServer(ip, port);
            Client.Instance.WriteToServer("get /position/latitude-deg\r\n");
            string latStr = Client.Instance.ReadAnswerFromServer();
            Lat = GetDoubleFromString(latStr);

            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            string lonStr = Client.Instance.ReadAnswerFromServer();
            Lon = GetDoubleFromString(lonStr);
            System.Diagnostics.Debug.WriteLine(Lat + "----" + Lon);
        }
    }
}