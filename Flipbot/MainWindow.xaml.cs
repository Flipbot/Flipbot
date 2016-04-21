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
        ItemList items;
        Bot bot;

        public MainWindow()
        {
            InitializeComponent();

            Title = "Flipbot - Scanning " + Config.league + " League.";

            items = new ItemList();

            SetupItemGrid(items);

            bot = new Bot(items);

            bot.Run();
        }

        private void SetupItemGrid(ItemList items)
        {
            dg_ItemGrid.ItemsSource = items;
        }

        //Puts the item selected default message in the clipboard
        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            Item item = dataGrid.SelectedItem as Item;
            if (item != null)               
                Clipboard.SetText(item.DefaultMessage);
        }
    }
}
