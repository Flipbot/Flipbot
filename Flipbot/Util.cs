using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Flipbot
{
    class Util
    {
        public static DateTime ConvertUnixTimeStamp(string unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(unixTimeStamp));
        }

        public static string RemoveWhiteSpace(string s)
        {
            return Regex.Replace(s, @"\s+", "");
        }

        public static List<string> GetPathOfAllFileInDirectory(string directoryPath)
        {
            return Directory.GetFiles(directoryPath).ToList();
        }
    }
}
