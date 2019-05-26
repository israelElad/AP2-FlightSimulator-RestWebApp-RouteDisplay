using System;
using System.Timers;

namespace Ex3.Models
{
    public class DisplayRefreshingLocation
    {
        public string IP { get; set; }
        public int Port { get; set; }

        DisplayUtils displayUtils;
        Timer timer;

        public DisplayRefreshingLocation(string ip, int port, int time)
        {
            displayUtils = new DisplayUtils();
            IP = ip;
            Port = port;
            timer = new Timer(time * 1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            displayUtils.ReadLatAndLon(IP, Port);
        }
    }
}