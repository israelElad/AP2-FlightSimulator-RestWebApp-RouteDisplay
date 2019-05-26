using System;
using System.Collections.Generic;
using System.Linq;
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
            Lat = Convert.ToDouble(latStr);

            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            string lonStr = Client.Instance.ReadAnswerFromServer();
            Lat = Convert.ToDouble(latStr);

            Console.WriteLine(Lat + "----" + Lon);
        }
    }
}