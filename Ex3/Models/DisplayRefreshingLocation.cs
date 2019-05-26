using System;
using System.Timers;

namespace Ex3.Models
{
    public class DisplayRefreshingLocation
    {
        public double Lat { get; set; }
        public double Lon { get; set; }

        public string IP { get; set; }
        public int Port { get; set; }

        Timer timer;

        public DisplayRefreshingLocation(string ip, int port, int time)
        {
            IP = ip;
            Port = port;
            timer = new Timer(time * 1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ReadLatAndLon(IP, Port);
        }

        public void ReadLatAndLon(string ip, int port)
        {
            Client.Instance.ConnectToServer(ip, port);
            Client.Instance.WriteToServer("get /position/latitude-deg\r\n");
            string latStr = Client.Instance.ReadAnswerFromServer();
            Lat = DisplayUtils.GetDoubleFromString(latStr);

            Client.Instance.WriteToServer("get /position/longitude-deg\r\n");
            string lonStr = Client.Instance.ReadAnswerFromServer();
            Lon = DisplayUtils.GetDoubleFromString(lonStr);
            System.Diagnostics.Debug.WriteLine(Lat + "----" + Lon);
        }
    }
}