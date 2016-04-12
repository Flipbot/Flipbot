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
        public MainWindow()
        {
            InitializeComponent();
            test();
        }

        public void test()
        {
            string requestBody = File.ReadAllText(@"~\..\..\..\Query\gem_almostlvl20.json");
            string url = @"http://api.exiletools.com/index/_search?pretty";
            string result = @"";            

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Authorization] = "DEVELOPMENT-Indexer";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(url, "POST", requestBody);
            }
            
            List<JToken> items = JObject.Parse(result).SelectToken("hits").Children().ElementAt(2).Values().ToList();

            List<Item> li = new List<Item>();

            foreach (var i in items)
            {
                string id = i.SelectToken("_id").Value<string>();
                string msg = i.SelectToken("_source").SelectToken("shop").SelectToken("defaultMessage").Value<string>();
                Item item = new Item(id, msg);
                Debug.WriteLine(item.ToString());
                Debug.WriteLine("--------------------------------------");
            }
        }
    }
}
