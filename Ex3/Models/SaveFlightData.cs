using System.Timers;

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

        /*
         * Save to file
         */ 
        private void SaveToFile(string[] data)
        {
            //TODO: change path
            System.IO.File.WriteAllLines("D:\\"+this.fileName + ".txt", data);
        }
    }
}