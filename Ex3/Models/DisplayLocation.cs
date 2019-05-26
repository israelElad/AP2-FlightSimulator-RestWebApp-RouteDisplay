using System;
using System.Text.RegularExpressions;

namespace Ex3.Models
{
    public class DisplayLocation
    {
        DisplayUtils displayUtils;

        public DisplayLocation(string ip, int port){
            displayUtils = new DisplayUtils();
            displayUtils.ReadLatAndLon(ip,port);
        }
    }
}