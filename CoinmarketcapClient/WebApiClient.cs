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


        public List<Currency> MakeRequest(Method method, string convert, 
            bool oneItemonly, bool isSymbol, IReadOnlyList<string> searchString)
        {
            if (string.IsNullOrEmpty(convert))
                throw new ArgumentException("currency not set.");

            var request = CreateRequest(method);
            Task<IRestResponse> task = RestClient.Execute(request);
            var content = task.Result.Content;

            List<Currency> currencyList = new List<Currency>();
            if (!isSymbol)
            {
                GetNonSymbolData(convert, oneItemonly, content, currencyList);
            }
            else
            {
                GetSymbolData(convert, searchString, content, currencyList);
            }

            return currencyList;
        }

        private static void GetSymbolData(string convert, IReadOnlyList<string> searchString, 
            string content, List<Currency> currencyList)
        {
            foreach (string item in searchString)
            {
                string replaceString = $"\"{item}\":[";
                content = content.Replace(replaceString, "\"CurrenyData\":[");
            }

            string replaceString1 = $"\"quote\":{{\"{convert}\":";
            content = content.Replace(replaceString1, "\"quote\":{\"CurrenyPriceInfo\":");

            content = content.Replace("data", "dataItem");

            CoinmarketcapDataSymbol result1 = JsonConvert.DeserializeObject<CoinmarketcapDataSymbol>(content);
            foreach (CurrenyData data in result1.dataItem.CurrenyData)
            {
                Currency item = new Currency
                {
                    Id = data.id.ToString(),
                    Name = data.name,
                    Symbol = data.symbol,
                    Rank = data.cmc_rank.ToString(),
                    Price = data.quote.CurrenyPriceInfo.price ?? 0,
                    Volume24hUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    MarketCapUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    PercentChange1h = data.quote.CurrenyPriceInfo.percent_change_1h ?? 0,
                    PercentChange24h = data.quote.CurrenyPriceInfo.percent_change_24h ?? 0,
                    PercentChange7d = data.quote.CurrenyPriceInfo.percent_change_7d ?? 0,
                    LastUpdated = data.quote.CurrenyPriceInfo.last_updated,
                    MarketCapConvert = data.quote.CurrenyPriceInfo.market_cap ?? 0,
                    ConvertCurrency = convert
                };

                currencyList.Add(item);
            }
        }

        private static string GetNonSymbolData(string convert, bool oneItemonly, string content, List<Currency> currencyList)
        {
            content = content.Replace(convert, "CurrenyPriceInfo");
            if (oneItemonly)
                content = content.Replace("data", "dataItem");

            CoinmarketcapItemData result = JsonConvert.DeserializeObject<CoinmarketcapItemData>(content);

            foreach (ItemData data in result.DataList)
            {
                Currency item = new Currency
                {
                    Id = data.id.ToString(),
                    Name = data.name,
                    Symbol = data.symbol,
                    Rank = data.cmc_rank.ToString(),
                    Price = data.quote.CurrenyPriceInfo.price ?? 0,
                    Volume24hUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    MarketCapUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    PercentChange1h = data.quote.CurrenyPriceInfo.percent_change_1h ?? 0,
                    PercentChange24h = data.quote.CurrenyPriceInfo.percent_change_24h ?? 0,
                    PercentChange7d = data.quote.CurrenyPriceInfo.percent_change_7d ?? 0,
                    LastUpdated = data.quote.CurrenyPriceInfo.last_updated,
                    MarketCapConvert = data.quote.CurrenyPriceInfo.market_cap ?? 0,
                    ConvertCurrency = convert
                };

                currencyList.Add(item);
            }

            return content;
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