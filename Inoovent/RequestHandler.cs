using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

namespace Inoovent
{

    public class RequestHandler
    {
        private static string PLOOMES_API_PATH = "http://api2.ploomes.com/";
        public static HttpClient ploomesClient;
        private static string uk = "EA003A7E7F32C3FC9738DBA9B63B7512747D5EA3AAD59D9D4CCA6B379FB2594A72915B403163107578D02F999F19109D518AD421FC36FE919BC58F3E146006E1";

        public static void instantiatePloomesConnection()
        {
            ploomesClient = new HttpClient();
            ploomesClient.DefaultRequestHeaders.Add("User-Key", uk);
            ploomesClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static JArray MakePloomesRequest(string url, Method method, JObject json = null)
        {
            instantiatePloomesConnection();

            System.Threading.Thread.Sleep(1000);
            string response = string.Empty;
            url = PLOOMES_API_PATH + url;

            if (method == Method.GET)
                response = ploomesClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            else if (method == Method.POST)
            {
                if (json != null)
                    response = ploomesClient.PostAsync(url, new StringContent(json.ToString())).Result.Content.ReadAsStringAsync().Result;
                else
                    response = ploomesClient.PostAsync(url, new StringContent(new JObject().ToString())).Result.Content.ReadAsStringAsync().Result;
            }
            else if (method == Method.DELETE)
            {
                ploomesClient.DeleteAsync(url);
                return null;
            }

            else if (method == Method.PATCH)
            {
                var content = new ObjectContent<JObject>(json, new JsonMediaTypeFormatter());
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
                ploomesClient.SendAsync(request).Wait();

                return null;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(response)["value"] as JArray;
        }
    }
    public enum Method
    {
        GET,
        POST,
        PATCH,
        DELETE
    }
}

