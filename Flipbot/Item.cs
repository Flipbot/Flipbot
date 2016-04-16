using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Flipbot
{
    class Item
    {
        public string uuid { get; set; }
        public string league { get; set; }
        public string fullName { get; set; }
        public string rarity { get; set; }

        public int hoursSinceModified { get; set; }
        public string defaultMessage { get; set; }
        public double chaosEquiv { get; set; } = 69.23;

        public string QueryName { get; set; }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                SolidColorBrush color;
                switch (rarity)
                {
                    case "Normal": color = SolidColorBrush_FromRGB("#c8c8c8"); break;
                    case "Magic": color = SolidColorBrush_FromRGB("#8888ff"); break;
                    case "Rare": color = SolidColorBrush_FromRGB("#ffff77"); break;
                    case "Unique": color = SolidColorBrush_FromRGB("#af6025"); break;
                    case "Gem": color = SolidColorBrush_FromRGB("#1ba29b"); ; break;
                    case "Currency": color = SolidColorBrush_FromRGB("#aa9e82"); break;
                    case "Divination Card": color = SolidColorBrush_FromRGB("#aa9e82"); break;
                    default: color = SolidColorBrush_FromRGB("#ffffff"); break;
                }
                return color;
            }
        }

        public SolidColorBrush TextColor
        {
            get
            {
                SolidColorBrush color;
                switch (rarity)
                {
                    case "Normal": color = SolidColorBrush_FromRGB("#000000"); break;
                    case "Magic": color = SolidColorBrush_FromRGB("#ffffff"); break;
                    case "Rare": color = SolidColorBrush_FromRGB("#000000"); break;
                    case "Unique": color = SolidColorBrush_FromRGB("#ffffff"); break;
                    case "Gem": color = SolidColorBrush_FromRGB("#ffffff"); ; break;
                    case "Currency": color = SolidColorBrush_FromRGB("#ffffff"); break;
                    case "Divination Card": color = SolidColorBrush_FromRGB("#ffffff"); break;
                    default: color = SolidColorBrush_FromRGB("#ffffff"); break;
                }
                return color;
            }
        }

        public Item()
        {
        }

        public bool Equals(Item item)
        {
            return uuid == item.uuid && 
                hoursSinceModified == item.hoursSinceModified && 
                chaosEquiv == item.chaosEquiv;
        }
       
        private SolidColorBrush SolidColorBrush_FromRGB(string hex)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(hex));
        }
    }
}
