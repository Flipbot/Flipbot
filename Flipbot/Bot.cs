using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace Flipbot
{
    class Bot
    {
        List<Query> querys = new List<Query>();
        ResultParser resultParser = new ResultParser();
        QueryBuilder queryBuilder = new QueryBuilder();
        JsonHttpClient jsonHttpClient = new JsonHttpClient(Config.url, Config.sleepMsBetweenQuerys);
        ItemList items;

        public Bot(ItemList items)
        {
            this.items = items;
        }

        public void Run()
        {
            ExcecuteSetupSequence();
        }

        private void ExcecuteSetupSequence()
        {
            SetupQuerys();
            StartTimer();
        }

        private void SetupQuerys()
        {
            querys = queryBuilder.getQuerys();
        }

        private void StartTimer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(ExecuteScanRoutine);
            aTimer.Interval = Config.sleepMsBetweenScanRoutines;
            aTimer.Enabled = true;

            ExecuteScanRoutine(null, null);
        }

        private void ExecuteScanRoutine(object source, ElapsedEventArgs e)
        {
            List<Item> resultItems = new List<Item>();
            foreach (Query query in querys)
            {
                string resultJson = jsonHttpClient.ExecutePostRequest(query.RawText);

                resultItems.AddRange(resultParser.ParseResultJson(resultJson, query));

                Thread.Sleep(Config.sleepMsBetweenQuerys);
            }
            items.SetItems(resultItems);
        }
    }
}
