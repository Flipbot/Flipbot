using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipbot
{
    class Item
    {
        public string uuid;
        public string fullName;
        public string defaultMessage;
        public string rarity;
        public string hoursSinceModified;
        public string league;
        
        public Item(JToken itemJtoken)
        {
            this.uuid = itemJtoken
                .SelectToken("_id")
                .Value<string>();

            this.fullName = itemJtoken
                .SelectToken("_source")
                .SelectToken("info")
                .SelectToken("fullName")
                .Value<string>();

            this.defaultMessage = itemJtoken
                .SelectToken("_source")
                .SelectToken("shop")
                .SelectToken("defaultMessage")
                .Value<string>();

            this.rarity = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("rarity")
                .Value<string>();

            string epochMili = itemJtoken
                    .SelectToken("_source")
                    .SelectToken("shop")
                    .SelectToken("modified")
                    .Value<string>();
            this.hoursSinceModified = (DateTime.Now - ConvertUnixTimeStamp(epochMili)).Hours.ToString();

            this.league = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("league")
                .Value<string>();
        }

        public string ToString()
        {
            return fullName + "\t" + hoursSinceModified + "\r\n\t" + defaultMessage;
        }

        public static DateTime ConvertUnixTimeStamp(string unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(unixTimeStamp));
        }
    }
}
