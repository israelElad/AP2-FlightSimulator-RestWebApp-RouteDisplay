using System;
using System.IO;
using System.Timers;
using System.Web;

namespace Ex3.Models
{
    /*
     * Sample(with the given rate for the duration received) and save the flight data from the given IP and port.
     * for example, can be accessed via the following uri: /save/127.0.0.1/5400/4/10/flight1
     */
    public class SaveFlightData
    {
        string ip;
        int port;
        int time;
        string fileName;
        int totalTime;
        int elapsedTime;

        string[] flightData = new string [512]; //TODO: 512 ?
        int flightDataIndex = 0;

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        Timer timer;

        public SaveFlightData(string ip, int port, int time, int duration, string fileName)
        {
            this.ip = ip;
            this.port = port;
            this.time = time;
            this.fileName = fileName;
            InitializeTimer(time);
            totalTime = duration * 1;
            elapsedTime = 0;
        }

        /*
         * Initialize the timer to activate the timed event 'time' times per second.
         */ 
        private void InitializeTimer(int time)
        {
            timer = new Timer(1000 / time);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        /*
         * Event to run when timer resets:
         * read flight data and when total sampling time exceeds - save the flight data to file
         */
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (elapsedTime >= totalTime)
            {
                //SaveToFile(flightData);
                return;
            }
            //displayUtils.ReadLatAndLon(ip, port);
            //flightData[flightDataIndex] = displayUtils.Lat + "~" + displayUtils.Lon + "~" + displayUtils.Rudder + "~" + displayUtils.Throttle;
            flightDataIndex++;
            elapsedTime += 1000 / time;
        }

        /*
         * Save to file
         */ 
        private void SaveToFile(string data)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, "file1"));
            if (!File.Exists(path))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine("hello");
                }
            }
            else
            {
                //string s = { "hi", "bye" };
                //File.AppendAllLines(s);
            }
        }
    }
}