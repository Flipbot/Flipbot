using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Flipbot
{
    class ItemList : ObservableCollection<Item>
    {
        public void SetItems(List<Item> resultItems)
        {
            // source: http://stackoverflow.com/questions/18331723/this-type-of-collectionview-does-not-support-changes-to-its-sourcecollection-fro
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.Clear();
                foreach (Item i in resultItems)
                {
                    this.Add(i);
                }

            });
        }
    }
}
