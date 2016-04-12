using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipbot
{
    class Item
    {
        public string id;
        public string defaultMessage;

        public Item(string id, string defaultMessage)
        {
            this.id = id;
            this.defaultMessage = defaultMessage;
        }

        public string ToString()
        {
            return defaultMessage;
        }
    }
}
