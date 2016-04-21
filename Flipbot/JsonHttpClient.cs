using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Flipbot
{
    class JsonHttpClient
    {
        private WebClient webClient = new WebClient();
        private string url;
        private int coolDownTimeMs;

        public JsonHttpClient(string url, int coolDownTimeMs)
        {
            this.url = url;
            this.coolDownTimeMs = coolDownTimeMs;
            SetupWebClient();
        }

        public string ExecutePostRequest(string requestBody)
        {
            return webClient.UploadString(url, "POST", requestBody);
        }

        private void SetupWebClient()
        {
            webClient.Headers[HttpRequestHeader.Authorization] = "DEVELOPMENT-Indexer";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        }
    }
}
