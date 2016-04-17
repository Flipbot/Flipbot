using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flipbot
{
    class QueryBuilder
    {
        private string queryDirectoryPath = @"~\..\..\..\Query\";
        private string templateDirectoryPath = @"~\..\..\..\QueryTemplates\";
        private Dictionary<string, string> filterTemplates = new Dictionary<string, string>();
        private Dictionary<string, string> filters = new Dictionary<string, string>();
        private List<Query> querys = new List<Query>();

        public List<Query> getQuerys()
        {
            return querys;
        }

        public QueryBuilder()
        {
            GetTemplates();
            GetQuerysDefinitions();
            BuildQuerys();
        }

        public void BuildQuerys()
        {
            foreach (KeyValuePair<string, string> kp in filters)
            {
                StringReader strReader = new StringReader(kp.Value);
                Query newQuery = new Query();
                newQuery.Name = kp.Key.Split('.')[0];

                if (!kp.Value.Contains("<<"))
                {
                    newQuery.RawText = strReader.ReadToEnd();
                }
                else
                {
                    string templateName = Util.RemoveWhiteSpace(strReader.ReadLine().Replace("<<", "").Replace(">>", ""));
                    string tmp = Util.RemoveWhiteSpace(strReader.ReadLine().Replace("<<", "").Replace(">>", ""));

                    newQuery.MaxPriceInChaos = Double.Parse(tmp.Split(':')[0], CultureInfo.InvariantCulture);
                    //newQuery.PotentialProfitInChaos = Double.Parse(tmp.Split(':')[1]) - newQuery.MaxPriceInChaos;

                    string queryDesc = strReader.ReadToEnd();
                    newQuery.RawText = filterTemplates[templateName].Replace("<<MUST>>", queryDesc);
                }
                querys.Add(newQuery);
            }
        }

        public void GetTemplates()
        {
            foreach (string filePath in Util.GetPathOfAllFileInDirectory(templateDirectoryPath))
            {
                string templateText = File.ReadAllText(filePath);
                templateText = templateText.Replace("<<LEAGUE>>", MainWindow.league);
                filterTemplates.Add(Path.GetFileName(filePath), templateText);
            }
        }

        public void GetQuerysDefinitions()
        {
            foreach (string filePath in Util.GetPathOfAllFileInDirectory(queryDirectoryPath))
            {
                filters.Add(Path.GetFileName(filePath), File.ReadAllText(filePath));
            }
        }
    }
}
