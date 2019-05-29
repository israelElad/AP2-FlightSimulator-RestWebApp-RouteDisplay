using System;
using System.Timers;

namespace Ex3.Models
{

    /* Sample 4 times per second the position of the plane and displaying it on the map */
    public class DisplayRefreshingLocation
    {
        string ip;
        int port;

        Timer timer;

        public DisplayRefreshingLocation(string ip, int port, int time)
        {
            this.ip = ip;
            this.port = port;
            InitializeTimer(time);
        }

        /* Initialize the timer to activate the timed event 'time' times per second */
        private void InitializeTimer(int time)
        {
            // set the timer to 4 times per second
            timer = new Timer(1000 / time);

            // add the function which will be activated at times defined by the timer
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        /* An event that sample the values ​​from the plane only one time */
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //displayUtils.ReadLatAndLon(ip, port);
        }
    }
}