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
    class Config
    {
        /* http://apikey:DEVELOPMENT-Indexer@api.exiletools.com/index/_search?pretty */
        public static string url = @"http://api.exiletools.com/index/_search?pretty";
        public static string league = "Perandus";
        public static int sleepMsBetweenQuerys = 1000;
        public static int sleepMsBetweenScanRoutines = 1000 * 60 * 5;

        public static string queryDirectoryPath = @"~\..\..\..\Query\";
        public static string templateDirectoryPath = @"~\..\..\..\QueryTemplates\";

        public static double acceptableProfitMargin = 0.1;
        public static double minimumProfitChaos = 5;
    }
}
