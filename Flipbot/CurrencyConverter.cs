using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipbot
{
    class CurrencyConverter
    {
        // Perandus league rates as of 2016-04-16 taken from http://www.poebase.xyz/
        private static Dictionary<string, double> CurrencyChaosEquivalences = new Dictionary<string, double>()
        {
            {"ScrollofWisdom", 0.006},
            {"PortalScroll",0.006},
            {"PerandusCoins",0.0056},
            {"OrbofAlteration",0.073},
            {"ChromaticOrb",0.1},
            {"JewellersOrb",0.1429},
            {"OrbofChance",0.16},
            {"CartographersChisel",0.4348},
            {"OrbofAlchemy",0.344},
            {"OrbofFusing",0.600},
            {"OrbofScouring",0.500},
            {"BlessedOrb",0.500},
            {"VaalOrb",1.600},
            {"ChaosOrb",1.000},
            {"OrbofRegret",1.000},
            {"GemcuttersPrism",1.600},
            {"RegalOrb",1.000},
            {"DivineOrb",14.000},
            {"ExaltedOrb",60.00},
            {"MirrorofKalandra",8000.000},
            {"EternalOrb",10000.000} // arbitrary value, since none exists in perandus
        };

        public static double CovertToChaosValue(string currencyType, double currencyAmount)
        {
            return currencyAmount * CurrencyChaosEquivalences[Util.RemoveWhiteSpace(currencyType)];
        }
    }
}
