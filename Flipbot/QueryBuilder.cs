using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipbot
{
    class QueryBuilder
    {
        string queryDirectoryPath = @"~\..\..\..\Query\";
        string templateDirectoryPath = @"~\..\..\..\QueryTemplates\";

        Dictionary<string, string> templates = new Dictionary<string, string>();
        Dictionary<string, string> queryDefinitions = new Dictionary<string, string>();
        List<Query> querys = new List<Query>();

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
            foreach (KeyValuePair<string, string> kp in queryDefinitions)
            {
                StringReader strReader = new StringReader(kp.Value);
                Query newQuery = new Query();                
                newQuery.queryName = kp.Key.Split('.')[0];
                
                if (!kp.Value.Contains("<<"))
                {
                    newQuery.queryText = strReader.ReadToEnd();
                }
                else
                {
                    string templateName = strReader.ReadLine().Replace("<<", "").Replace(">>", "");
                    string queryDesc = strReader.ReadToEnd();
                    newQuery.queryText = templates[templateName].Replace("<<MUST>>", queryDesc);
                }
                querys.Add(newQuery);
            }
        }

        public void GetTemplates()
        {
            foreach (string filePath in GetPathOfAllFileInDirectory(templateDirectoryPath))
            {
                string templateText = File.ReadAllText(filePath);
                templateText = templateText.Replace("<<LEAGUE>>", MainWindow.league);
                templates.Add(Path.GetFileName(filePath), templateText);
            }
        }

        public void GetQuerysDefinitions()
        {
            foreach (string filePath in GetPathOfAllFileInDirectory(queryDirectoryPath))
            {
                queryDefinitions.Add(Path.GetFileName(filePath), File.ReadAllText(filePath));
            }
        }

        public List<string> GetPathOfAllFileInDirectory(string directoryPath)
        {
            return Directory.GetFiles(directoryPath).ToList();
        }
    }
}
