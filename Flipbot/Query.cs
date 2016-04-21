using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipbot
{
    class Query
    {
        public string QueryName { get; set; }
        public string RawText { get; set; }
        public double EstimatedMarketValueInChaos { get; set; }
        public string UsedFilter { get; set; }
        public string UsedTemplate { get; set; }
    }
}
