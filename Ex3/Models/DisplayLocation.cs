using System;
using System.Text.RegularExpressions;

namespace Ex3.Models
{

    /* Sample only one time the position of the plane and displaying it on the map */
    public class DisplayLocation
    {
        DisplayUtils displayUtils;

        public DisplayLocation(string ip, int port){
            displayUtils = new DisplayUtils();
            displayUtils.ReadLatAndLon(ip,port);
        }
    }
}