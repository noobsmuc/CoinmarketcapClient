using System.Collections.Generic;
using System.Linq;
using RestSharp.Portable;

namespace NoobsMuc.Coinmarketcap.Client
{
    public class CoinmarketcapClient : ICoinmarketcapClient
    {
        private const string Url = "http://api.coinmarketcap.com";
        private const string Path = "/v1/ticker";

        List<string> ICoinmarketcapClient.GetConvertCurrencyList()
        {
            return new List<string>{"AUD", "BRL", "CAD", "CHF", "CNY", "EUR", "GBP", "HKD", "IDR", "INR", "JPY", "KRW", "MXN", "RUB"};
        }

        Currency ICoinmarketcapClient.GetCurrencyById(string id)
        {
            return CurrencyById(id, string.Empty);
        }

        Currency ICoinmarketcapClient.GetCurrencyById(string id, string convertCurrency)
        {
            return CurrencyById(id, convertCurrency);
        }
        
        private Currency CurrencyById(string id, string convertCurrency)
        {
            string path = "/" + id;
            if (!string.IsNullOrEmpty(convertCurrency))
                path += "/?convert=" + convertCurrency;

            var client = new WebApiClient(Url);
            var result = client.MakeRequest(Path + path, Method.GET, convertCurrency);
            
            return result.First();
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
            string seperator = string.Empty;
            string path = "?";
            if (limit > 0)
            {
                path += "limit=" + limit;
                seperator = "&";
            }

            if(!string.IsNullOrEmpty(convertCurrency))
                path += seperator +"convert=" + convertCurrency;
            
            var client = new WebApiClient(Url);
            var result = client.MakeRequest(Path + path, Method.GET, convertCurrency);
            return result;
        }
    }
}
