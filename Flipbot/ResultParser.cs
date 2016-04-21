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

            item.RawResponseText = itemJtoken.ToString();

            item.DateModified = Util.ConvertUnixTimeStamp(
                    itemJtoken
                    .SelectToken("_source")
                    .SelectToken("shop")
                    .SelectToken("modified")
                    .Value<string>());

            item.DateAdded = Util.ConvertUnixTimeStamp(
                    itemJtoken
                    .SelectToken("_source")
                    .SelectToken("shop")
                    .SelectToken("added")
                    .Value<string>());

            item.DateUpdated = Util.ConvertUnixTimeStamp(
                    itemJtoken
                    .SelectToken("_source")
                    .SelectToken("shop")
                    .SelectToken("updated")
                    .Value<string>());

            DateTime mostRecentDate = DateTime.Compare(item.DateUpdated, item.DateModified) <= 0 ? item.DateUpdated : item.DateModified;

            item.HoursSinceModified = ((int)(DateTime.Now.ToUniversalTime() - mostRecentDate).TotalHours) - 1;

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

            if (item.PriceInChaos > query.EstimatedMarketValueInChaos * (1.0 - Config.acceptableProfitMargin))
               return null;

            item.ProfitMarginInChaos = query.EstimatedMarketValueInChaos - item.PriceInChaos;

            item.QueryName = query.QueryName;

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

            item.league = itemJtoken
                .SelectToken("_source")
                .SelectToken("attributes")
                .SelectToken("league")
                .Value<string>();

            return item;
        }

    }
}
