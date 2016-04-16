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
        string url = @"http://api.exiletools.com/index/_search?pretty";
        int sleepMsBetweenQuerys = 150;
        int sleepMsBetweenScanRoutines = 250 * 60 * 1;

        List<Query> querys = new List<Query>();
        List<Item> items = new List<Item>();

        WebClient webClient = new WebClient();
        QueryBuilder queryBuilder = new QueryBuilder();
        ResultParser resultParser = new ResultParser();

        public MainWindow()
        {
            InitializeComponent();

            ExcecuteSetupSequence();

            ExecuteScanRoutine(null, null);            
        }

        private void ExcecuteSetupSequence()
        {
            SetupWebClient();
            SetupTimer();
            SetupItemGrid();
            SetupQuerys();
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
            foreach (Query query in querys)
            {
                string resultJson = webClient.UploadString(url, "POST", query.queryText);

                List<Item> resultItems = resultParser.ParseResultJson(resultJson);

                items.AddRange(resultItems);

                Thread.Sleep(sleepMsBetweenQuerys);
            }

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            Item row = dataGrid.SelectedItem as Item;
            Clipboard.SetText(row.league);
        }
    }
}
