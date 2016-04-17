using Newtonsoft.Json.Linq;
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
        public List<Item> ParseResultJson(string resultJson, Query query)
        {
            List<Item> items = new List<Item>();
            List<JToken> itemTokens = ExtractItemJtoken(resultJson);

            foreach (JToken token in itemTokens)
            {
                Item newItem = ParseJToken(token, query);

                if (newItem != null)
                    items.Add(newItem);
            }

            return items;
        }


        private List<JToken> ExtractItemJtoken(string resultJSON)
        {
            return JObject.Parse(resultJSON).SelectToken("hits").Children().ElementAt(2).Values().ToList();
        }

        private Item ParseJToken(JToken itemJtoken, Query query)
        {
            Item item = new Item();

            item.currencyType = itemJtoken
                .SelectToken("_source")
                .SelectToken("shop")
                .SelectToken("currency")
                .Value<string>();

            item.currencyAmount = double.Parse(itemJtoken
                .SelectToken("_source")
                .SelectToken("shop")
                .SelectToken("amount")
                .Value<string>(), CultureInfo.InvariantCulture);

            item.PriceInChaos = CurrencyConverter.CovertToChaosValue(item.currencyType, item.currencyAmount);

            if (item.PriceInChaos > query.MaxPriceInChaos)
               return null;

            item.ProfitMarginInChaos = query.MaxPriceInChaos - item.PriceInChaos;

            item.QueryName = query.Name;

            item.uuid = itemJtoken
                .SelectToken("_id")
                .Value<string>();

            item.FullName = itemJtoken
                .SelectToken("_source")
                .SelectToken("info")
                .SelectToken("fullName")
                .Value<string>();

            item.DefaultMessage = itemJtoken
                .SelectToken("_source")
                .SelectToken("shop")
                .SelectToken("defaultMessage")
                .Value<string>();

            item.Rarity = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("rarity")
                .Value<string>();

            string epochMili = itemJtoken
                    .SelectToken("_source")
                    .SelectToken("shop")
                    .SelectToken("modified")
                    .Value<string>();
            item.HoursSinceModified = (DateTime.Now.ToUniversalTime() - Util.ConvertUnixTimeStamp(epochMili)).Hours;

            item.league = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("league")
                .Value<string>();

            return item;
        }

    }
}
