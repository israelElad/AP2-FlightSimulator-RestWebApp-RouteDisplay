using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ex3.Models
{
    public class DisplayUtils
    {
        public static double GetDoubleFromString(string str)
        {
            string pattern = "[-+]?[0-9]*\\.?[0-9]+";
            Regex reg = new Regex(pattern);
            Match match = reg.Match(str);
            if (!match.Success)
            {
                Console.WriteLine("No floating number found in the string!");
            }
            double num = Convert.ToDouble(match.Value);
            return num;
        }
    }
}