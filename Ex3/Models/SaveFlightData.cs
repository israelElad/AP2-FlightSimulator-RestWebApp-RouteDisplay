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
        string fileName;
        int totalTime;
        int elapsedTime;

        string[] flightData = new string [512]; //TODO:
        int flightDataIndex = 0;

        DisplayUtils displayUtils;
        Timer timer;

        public SaveFlightData(string ip, int port, int time, int duration, string fileName)
        {
            displayUtils = new DisplayUtils();
            this.ip = ip;
            this.port = port;
            this.time = time;
            this.fileName = fileName;
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
                SaveToFile(flightData);
                return;
            }
            displayUtils.ReadLatAndLon(ip, port);
            flightData[flightDataIndex] = displayUtils.Lat + "~" + displayUtils.Lon;
            flightDataIndex++;
            elapsedTime += 1000 / time;
        }

        private void SaveToFile(string[] data)
        {
            //TODO: change path
            System.IO.File.WriteAllLines("D:\\"+this.fileName + ".txt", data);
        }
    }
}