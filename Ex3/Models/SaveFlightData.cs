using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Ex3.Models
{
    public class SaveFlightData
    {
        string ip;
        int port;
        int time;
        int totalTime;
        int elapsedTime;

        DisplayUtils displayUtils;
        Timer timer;

        public SaveFlightData(string ip, int port, int time, int duration, string fileName)
        {
            displayUtils = new DisplayUtils();
            this.ip = ip;
            this.port = port;
            this.time = time;
            InitializeTimer(time);
            totalTime = 10 * 1000;
            elapsedTime = 0;
        }

        private void InitializeTimer(int time)
        {
            timer = new Timer(1000 / time);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (elapsedTime > totalTime)
            {
                return;
            }
            displayUtils.ReadLatAndLon(ip, port);
            elapsedTime += 1000 / time;
        }
    }
}