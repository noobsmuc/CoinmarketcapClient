using System.Threading.Tasks;
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


        public T MakeRequest<T>(string resource, Method method)
        {
                var request = new RestRequest(resource, Method.GET);
                Task<IRestResponse<T>> task = RestClient.Execute<T>(request);
                return task.Result.Data;
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
