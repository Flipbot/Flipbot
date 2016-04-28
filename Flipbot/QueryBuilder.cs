using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private enum FilterScanState { AddintToMust, AddingToShould, AddingToMustNot, init };

        private void BuildQuerys()
        {
            foreach (KeyValuePair<string, string> kp in filters)
            {
                string filterText = RemoveCommentedLines(kp.Value);

                StringReader reader = new StringReader(filterText);

                if (!filterText.Contains(">>"))
                    throw new Exception("bad filter detected");

                //associate the file name of the filter to the query name.
                string filterName = kp.Key.Split('.')[0];

                string templateToUse;
                if (filterText.Contains(">>MUSTNOT"))
                    templateToUse = "MUSTNOT";
                else if (filterText.Contains(">>SHOULD"))
                    templateToUse = "SHOULD";
                else
                    templateToUse = "DEFAULT";

                ////////////////////////////////////////////////////

                string line;
                Query query = null;
                string must = "";
                string should = "";
                string mustnot = "";
                FilterScanState filterScanState = FilterScanState.init;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(">>ITEM"))
                    {
                        must = "";
                        should = "";
                        mustnot = "";
                        filterScanState = FilterScanState.init;

                        query = new Query();
                        query.UsedTemplate = templateToUse;
                        query.UsedFilter = filterName;
                        query.QueryName = line.Split(':')[1];

                        line = reader.ReadLine();
                        query.EstimatedMarketValueInChaos = Double.Parse(line.Split(':')[1], CultureInfo.InvariantCulture);
                    }
                    else if (line.Contains(">>MUSTNOT"))
                    {
                        filterScanState = FilterScanState.AddingToMustNot;
                    }
                    else if (line.Contains(">>MUST"))
                    {
                        filterScanState = FilterScanState.AddintToMust;
                    }
                    else if (line.Contains(">>SHOULD"))
                    {
                        filterScanState = FilterScanState.AddingToShould;
                    }
                    //else if (line.Contains(">>END") || Util.RemoveWhiteSpace(line) == "")
                    else if (Util.RemoveWhiteSpace(line) == "")
                    {
                        string temp = filterTemplates[templateToUse];

                        if (mustnot != "")
                            temp = temp.Replace(">>MUSTNOT", mustnot);

                        if (must != "")
                            temp = temp.Replace(">>MUST", must);

                        if (should != "")
                            temp = temp.Replace(">>SHOULD", should);

                        query.RawText = temp;
                        querys.Add(query);
                    }
                    else if (filterScanState == FilterScanState.AddintToMust)
                    {
                        must += line;
                    }
                    else if (filterScanState == FilterScanState.AddingToShould)
                    {
                        should += line;
                    }
                    else if (filterScanState == FilterScanState.AddingToMustNot)
                    {
                        mustnot += line;
                    }

                }
            }
        }

        private void GetTemplates()
        {
            foreach (string filePath in Util.GetPathOfAllFileInDirectory(Config.templateDirectoryPath))
            {
                string templateText = File.ReadAllText(filePath);
                templateText = templateText.Replace("<<LEAGUE>>", Config.league);
                filterTemplates.Add(Path.GetFileName(filePath).Split('.')[0], templateText);
            }
        }

        private void GetQuerysDefinitions()
        {
            foreach (string filePath in Util.GetPathOfAllFileInDirectory(Config.queryDirectoryPath))
            {
                filters.Add(Path.GetFileName(filePath), File.ReadAllText(filePath));
            }
        }

        private string RemoveCommentedLines(string text)
        {
            StringReader reader = new StringReader(text);
            StringWriter writer = new StringWriter();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if(!line.StartsWith("//"))
                    writer.WriteLine(line);
            }

            return writer.ToString();
        }
    }
}
