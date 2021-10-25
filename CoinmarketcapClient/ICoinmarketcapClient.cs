using System.Collections.Generic;

namespace NoobsMuc.Coinmarketcap.Client
{
    public interface ICoinmarketcapClient
    {
        List<string> GetConvertCurrencyList();

        Currency GetCurrencyBySlug(string slug);
        Currency GetCurrencyBySymbol(string symbol);
        Currency GetCurrencyBySlug(string slug, string convertCurrency);
        Currency GetCurrencyBySymbol(string symbol, string convertCurrency);

        IEnumerable<Currency> GetCurrencyBySlugList(string[] slugList);
        IEnumerable<Currency> GetCurrencyBySlugList(string[] slugList, string convertCurrency);

        IEnumerable<Currency> GetCurrencyBySymbolList(string[] symbolList);
        IEnumerable<Currency> GetCurrencyBySymbolList(string[] symbolList, string convertCurrency);

        IEnumerable<Currency> GetCurrencies();
        IEnumerable<Currency> GetCurrencies(string convertCurrency);
        IEnumerable<Currency> GetCurrencies(int limit);
        IEnumerable<Currency> GetCurrencies(int limit, string convertCurrency);
    }
}
