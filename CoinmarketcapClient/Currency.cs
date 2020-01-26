using System;
using Newtonsoft.Json;

namespace NoobsMuc.Coinmarketcap.Client
{
    public class Currency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Rank { get; set; }
        public double Price { get; set; }
        public double Volume24hUsd { get; set; }
        public double MarketCapUsd { get; set; }
        public double PercentChange1h { get; set; }
        public double PercentChange24h { get; set; }
        public double PercentChange7d { get; set; }
        public DateTime LastUpdated { get; set; }
        public Double MarketCapConvert { get; set; }
        public string ConvertCurrency { get; set; }
    }
}