using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace NoobsMuc.Coinmarketcap.Client
{
    public class WebApiClient
    {
        private static string _ApiKey; 
        private IRestClient RestClient { get; set; }

        public WebApiClient(UriBuilder baseUrl, Dictionary<string, string> queryArguments, string apiKey)
        {
            _ApiKey = apiKey;
            string url = QueryHelpers.AddQueryString(baseUrl.ToString(), queryArguments);
            RestClient = new RestClient(url);
        }


        public List<Currency> MakeRequest(Method method, string convert, bool oneItemonly)
        {
            if (string.IsNullOrEmpty(convert))
                throw new ArgumentException("currency not set.");

            var request = CreateRequest(method);
            Task<IRestResponse> task = RestClient.Execute(request);
            var content = task.Result.Content;

            content = content.Replace(convert, "CurrenyPriceInfo");
            if (oneItemonly)
                content = content.Replace("data", "dataItem");

            CoinmarketcapItemData result = JsonConvert.DeserializeObject<CoinmarketcapItemData>(content);

            List<Currency> currencyList = new List<Currency>();
            foreach (ItemData data in result.DataList)
            {
                Currency item = new Currency
                {
                    Id = data.id.ToString(),
                    Name = data.name,
                    Symbol = data.symbol,
                    Rank = data.cmc_rank.ToString(),
                    Price = data.quote.CurrenyPriceInfo.price,
                    Volume24hUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    MarketCapUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    PercentChange1h = data.quote.CurrenyPriceInfo.percent_change_1h ?? 0,
                    PercentChange24h = data.quote.CurrenyPriceInfo.percent_change_24h ?? 0,
                    PercentChange7d = data.quote.CurrenyPriceInfo.percent_change_7d ?? 0,
                    LastUpdated = data.quote.CurrenyPriceInfo.last_updated,
                    MarketCapConvert = data.quote.CurrenyPriceInfo.market_cap,
                    ConvertCurrency = convert
                };

                currencyList.Add(item);
            }

            return currencyList;
        }

        public static RestRequest CreateRequest(Method method)
        {
            var taskRequest = new RestRequest(method);
            taskRequest.Parameters.Clear();

            //here you can add some headers
            taskRequest.AddHeader("X-CMC_PRO_API_KEY", _ApiKey);
            taskRequest.AddHeader("Accepts", "application/json");

            return taskRequest;
        }
    }
}
