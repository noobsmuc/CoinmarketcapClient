using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using RestSharp.Portable;

namespace NoobsMuc.Coinmarketcap.Client
{
    public class CoinmarketcapClient : ICoinmarketcapClient
    {
        private const string UrlBase = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/";
        private const string UrlPartList = "listings/latest";
        private const string UrlPartItem = "quotes/latest";
        private string _ApiKey;

        public CoinmarketcapClient(string apiKey)
        {
            _ApiKey = apiKey;
        }
        List<string> ICoinmarketcapClient.GetConvertCurrencyList()
        {
            return new List<string>{"AUD", "BRL", "CAD", "CHF", "CNY", "EUR", "GBP", "HKD", "IDR", "INR", "JPY", "KRW", "MXN", "RUB"};
        }

        Currency ICoinmarketcapClient.GetCurrencyBySlug(string slug)
        {
            return CurrencyBySlugList(new List<string> {slug}, string.Empty).First();
        }

        Currency ICoinmarketcapClient.GetCurrencyBySymbol(string symbol)
        {
            return CurrencyBySymbolList(new List<string> { symbol }, string.Empty).First();
        }

        Currency ICoinmarketcapClient.GetCurrencyBySlug(string slug, string convertCurrency)
        {
            return CurrencyBySlugList(new List<string> { slug }, convertCurrency).First();
        }

        Currency ICoinmarketcapClient.GetCurrencyBySymbol(string symbol, string convertCurrency)
        {
            return CurrencyBySymbolList(new List<string> { symbol }, convertCurrency).First();
        }

        public IEnumerable<Currency> GetCurrencyBySlugList(string[] slugList)
        {
            return CurrencyBySlugList(slugList.ToList(), string.Empty);
        }

        public IEnumerable<Currency> GetCurrencyBySlugList(string[] slugList, string convertCurrency)
        {
            return CurrencyBySlugList(slugList.ToList(), convertCurrency);
        }

        public IEnumerable<Currency> GetCurrencyBySymbolList(string[] symbolList)
        {
            return CurrencyBySymbolList(symbolList.ToList(), string.Empty);
        }

        public IEnumerable<Currency> GetCurrencyBySymbolList(string[] symbolList, string convertCurrency)
        {
            return CurrencyBySymbolList(symbolList.ToList(), convertCurrency);
        }

        private IEnumerable<Currency> CurrencyBySlugList(List<string> slugList, string convertCurrency)
        {
            var queryArguments = new Dictionary<string, string>
            {
                {"slug", string.Join(",", slugList.Select(item => item.ToLower()))}
            };
 
            var client = GetWebApiClient(UrlPartItem, ref convertCurrency, queryArguments);
            var result = client.MakeRequest(Method.GET, convertCurrency, true);

            return result;
        }

        private IEnumerable<Currency> CurrencyBySymbolList(List<string> symbolList, string convertCurrency)
        {
            var queryArguments = new Dictionary<string, string>
            {
                {"symbol", string.Join(",", symbolList.Select(item => item.ToLower()))}
            };

            var client = GetWebApiClient(UrlPartItem, ref convertCurrency, queryArguments);
            var result = client.MakeRequest(Method.GET, convertCurrency, true);

            return result;
        }

        private WebApiClient GetWebApiClient(
            string urlPart, ref string convertCurrency, Dictionary<string, string> queryArguments)
        {
            if (string.IsNullOrEmpty(convertCurrency))
                convertCurrency = "USD";

            queryArguments.Add("convert", convertCurrency);

            UriBuilder uri = new UriBuilder(UrlBase + urlPart);
            var client = new WebApiClient(uri, queryArguments, _ApiKey);
            return client;
        }

        IEnumerable<Currency> ICoinmarketcapClient.GetCurrencies()
        {
            return Currencies(-1, string.Empty);
        }

        IEnumerable<Currency> ICoinmarketcapClient.GetCurrencies(string convertCurrency)
        {
            return Currencies(-1, convertCurrency);
        }

        IEnumerable<Currency> ICoinmarketcapClient.GetCurrencies(int limit)
        {
            return Currencies(limit, string.Empty);
        }

        IEnumerable<Currency> ICoinmarketcapClient.GetCurrencies(int limit, string convertCurrency)
        {
            return Currencies(limit, convertCurrency);
        }

        private List<Currency> Currencies(int limit, string convertCurrency)
        {
            var queryArguments = new Dictionary<string, string>
            {
                {"start", "1"}
            };

            if (limit > 0)
                queryArguments.Add("limit", limit.ToString());
            else
                queryArguments.Add("limit", "100");
            
            var client = GetWebApiClient(UrlPartList, ref convertCurrency, queryArguments);

            var result = client.MakeRequest( Method.GET, convertCurrency, false);
            return result;
        }
    }
}
