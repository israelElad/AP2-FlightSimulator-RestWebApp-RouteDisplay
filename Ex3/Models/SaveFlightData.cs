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
        int time;
        int totalTime;
        int elapsedTime;

        int flightDataIndex = 0;

        Timer timer;

        public SaveFlightData(string ip, int port, int time, int duration, string fileName)
        {
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
    }
}