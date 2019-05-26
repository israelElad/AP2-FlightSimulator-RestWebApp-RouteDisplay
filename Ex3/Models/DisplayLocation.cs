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
            byte[] buffer;
            Client.Instance.ConnectToServer(ip, port);
            Client.Instance.WriteToServer("get /position/latitude-deg\r\n");
            Client.Instance.ReadAnswerFromServer(buffer);
            string latStr= System.Text.Encoding.Default.GetString(buffer);
            Lat = Convert.ToDouble(latStr);

            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            Client.Instance.ReadAnswerFromServer(buffer);
            string lonStr = System.Text.Encoding.Default.GetString(buffer);
            Lat = Convert.ToDouble(latStr);
        }
    }
}