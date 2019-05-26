using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ex3.Models
{
    public class DisplayLocation
    {
        public double Lat {get;set;}
        public double Lon {get;set;}

        public DisplayLocation(string ip, int port){
            ReadLatAndLon(ip,port);
        }

        public void ReadLatAndLon(string ip, int port)
        {
            Client.Instance.ConnectToServer(ip, port);
            Client.Instance.WriteToServer("get /position/latitude-deg\r\n");
            string latStr = Client.Instance.ReadAnswerFromServer();
            Lat = getDoubleFromString(latStr);

            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            string lonStr = Client.Instance.ReadAnswerFromServer();
            Lon = getDoubleFromString(lonStr);
            Console.WriteLine(Lat + "----" + Lon);
        }

        public double getDoubleFromString(string str)
        {
            string pattern = "[-+]?[0-9]*\\.?[0-9]+";
            Regex reg =new Regex(pattern);
            Match match = reg.Match(str);
            if (!match.Success)
            {
                Console.WriteLine("No floating number found in the string!");
            }
            double num = Convert.ToDouble(match.Value);
            return num;
        }
    }
}