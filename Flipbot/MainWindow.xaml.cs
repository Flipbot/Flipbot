using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flipbot
{
    public partial class MainWindow : Window
    {
        public static string url = @"http://api.exiletools.com/index/_search?pretty";
        public static string league = "Perandus";
        public static int sleepMsBetweenQuerys = 150;
        public static int sleepMsBetweenScanRoutines = 1000 * 60;

        List<Query> querys = new List<Query>();
        ItemList items = new ItemList();

        WebClient webClient = new WebClient();
        QueryBuilder queryBuilder = new QueryBuilder();
        ResultParser resultParser = new ResultParser();

        public MainWindow()
        {
            InitializeComponent();
            this.Title = this.Title += " - Scanning " + league + " League.";

            ExcecuteSetupSequence();

            ExecuteScanRoutine(null, null);
        }

        private void ExcecuteSetupSequence()
        {
            SetupWebClient();
            SetupQuerys();
            SetupItemGrid();
            SetupTimer();
        }

        private void SetupTimer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(ExecuteScanRoutine);
            aTimer.Interval = sleepMsBetweenScanRoutines;
            aTimer.Enabled = true;
        }

        private void SetupWebClient()
        {
            webClient.Headers[HttpRequestHeader.Authorization] = "DEVELOPMENT-Indexer";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        }

        private void SetupItemGrid()
        {
            dg_ItemGrid.ItemsSource = items;
        }

        private void SetupQuerys()
        {
            querys = queryBuilder.getQuerys();
        }

        private void ExecuteScanRoutine(object source, ElapsedEventArgs e)
        {
            List<Item> resultItems = new List<Item>();
            foreach (Query query in querys)
            {
                string resultJson = webClient.UploadString(url, "POST", query.queryText);


                resultItems.AddRange(resultParser.ParseResultJson(resultJson, query));

                Thread.Sleep(sleepMsBetweenQuerys);
            }
            items.SetItems(resultItems);
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            Item item = dataGrid.SelectedItem as Item;
            if(item != null)
                Clipboard.SetText(item.defaultMessage);
        }
    }
}
