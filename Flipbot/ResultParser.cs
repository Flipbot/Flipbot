﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipbot
{
    class ResultParser
    {
        public List<Item> ParseResultJson(string resultJson)
        {
            List<Item> items = new List<Item>();
            List<JToken> itemTokens = ExtractItemJtoken(resultJson);

            foreach (JToken token in itemTokens)
            {                
                items.Add(ParseJToken(token));
            }

            return items;
        }


        private List<JToken> ExtractItemJtoken(string resultJSON)
        {
            return JObject.Parse(resultJSON).SelectToken("hits").Children().ElementAt(2).Values().ToList();
        }        

        private Item ParseJToken(JToken itemJtoken)
        {
            Item item = new Item();

            item.uuid = itemJtoken
                .SelectToken("_id")
                .Value<string>();

            item.fullName = itemJtoken
                .SelectToken("_source")
                .SelectToken("info")
                .SelectToken("fullName")
                .Value<string>();

            item.defaultMessage = itemJtoken
                .SelectToken("_source")
                .SelectToken("shop")
                .SelectToken("defaultMessage")
                .Value<string>();

          // double.Parse("3.5", System.Globalization.NumberStyles.AllowDecimalPoint)


            item.chaosEquiv = double.Parse(itemJtoken
                .SelectToken("_source")
                .SelectToken("shop")
                .SelectToken("chaosEquiv")
                .Value<string>(), CultureInfo.InvariantCulture);

            item.rarity = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("rarity")
                .Value<string>();

            string epochMili = itemJtoken
                    .SelectToken("_source")
                    .SelectToken("shop")
                    .SelectToken("modified")
                    .Value<string>();
            item.hoursSinceModified = (DateTime.Now.ToUniversalTime() - ConvertUnixTimeStamp(epochMili)).Hours;

            item.league = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("league")
                .Value<string>();
            
            return item;
        }

        private static DateTime ConvertUnixTimeStamp(string unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(unixTimeStamp));
        }
    }
}
