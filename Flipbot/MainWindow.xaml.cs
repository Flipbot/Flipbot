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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = @"http://api.exiletools.com/index/_search?pretty";
        string directoryPath = @"~\..\..\..\Query\";
        List<string> querys = new List<string>();
        WebClient webClient;

        public MainWindow()
        {
            InitializeComponent();

            webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = "DEVELOPMENT-Indexer";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            foreach (string queryPath in Directory.GetFiles(directoryPath))
            {
                querys.Add(File.ReadAllText(queryPath));
            }

            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(time_elapsed);
            aTimer.Interval = 1000 * 60 * 1;
            aTimer.Enabled = true;

            time_elapsed(null, null);
        }

        public void time_elapsed(object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("=================Report Start================");

            foreach (string requestBody in querys)
            {
                string resultJson = webClient.UploadString(url, "POST", requestBody);

                List<JToken> itemTokens = ExtractItemJtoken(resultJson);
                foreach (var token in itemTokens)
                {
                    Item item = new Item(token);
                    Debug.WriteLine(item.ToString());
                    Debug.WriteLine("--------------------------------------");
                }
            }

            Debug.WriteLine("=================Report End=================");
        }

        public List<JToken> ExtractItemJtoken(string resultJSON)
        {
            return JObject.Parse(resultJSON).SelectToken("hits").Children().ElementAt(2).Values().ToList();
        }
    }
}
