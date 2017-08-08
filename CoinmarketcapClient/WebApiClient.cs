using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace NoobsMuc.Coinmarketcap.Client
{
    public class WebApiClient
    {
        private IRestClient RestClient { get; set; }

        public WebApiClient(string url)
        {
            RestClient = new RestClient(url);
        }

        public List<Currency> MakeRequest(string resource, Method method, string convert)
        {
            var request = new RestRequest(resource, method);
            Task<IRestResponse> task = RestClient.Execute(request);
            var content = task.Result.Content;

            if (!string.IsNullOrEmpty(convert))
            {
                content = content.Replace("price_" + convert.ToLower(), "price_convert");
                content = content.Replace("24h_volume_" + convert.ToLower(), "24h_volume_convert");
                content = content.Replace("market_cap_" + convert.ToLower(), "market_cap_convert");
            }

            List<Currency> result = JsonConvert.DeserializeObject<List<Currency>>(content);
            result.ForEach(x => x.ConvertCurrency = convert);
            return result; 
        }

        public static RestRequest CreateRequest(string resource, Method method)
        {
            var taskRequest = new RestRequest(resource, method);
            taskRequest.Parameters.Clear();

            //here you can add some headers
            //taskRequest.AddHeader("Authorization", $"Bearer myToken");

            return taskRequest;
        }
    }
}
