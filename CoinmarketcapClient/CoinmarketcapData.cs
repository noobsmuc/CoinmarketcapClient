using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NoobsMuc.Coinmarketcap.Client
{
    public class CoinmarketcapItemData
    {
        public Status Status { get; set; }

        public CoinmarketcapItemData()
        {
            DataList = new List<ItemData>();
        }

        [JsonProperty(PropertyName = "data")]
        public List<ItemData> DataList { get; set; }

        private Dictionary<string, ItemData> _dataItemList;
        [JsonProperty(PropertyName = "dataItem")]
        public Dictionary<string, ItemData> DataItemList
        {
            get { return _dataItemList; }
            set
            {
                _dataItemList = value;
                DataList = value.Values.ToList();
            }
        }
    }

    public class ItemData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public int num_market_pairs { get; set; }
        public DateTime date_added { get; set; }
        public List<object> tags { get; set; }
        public double? max_supply { get; set; }
        public double? circulating_supply { get; set; }
        public double? total_supply { get; set; }
        public CurrenyInfo platform { get; set; }
        public int cmc_rank { get; set; }
        public DateTime last_updated { get; set; }
        public Quote quote { get; set; }
    }

    public class Status
    {
        public DateTime timestamp { get; set; }
        public int error_code { get; set; }
        public object error_message { get; set; }
        public int elapsed { get; set; }
        public int credit_count { get; set; }
        public object notice { get; set; }
    }

    public class CurrenyInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public string token_address { get; set; }
    }

    public class CurrenyPriceInfo
    {
        public double? price { get; set; }
        public double? volume_24h { get; set; }
        public double? percent_change_1h { get; set; }
        public double? percent_change_24h { get; set; }
        public double? percent_change_7d { get; set; }
        public double? market_cap { get; set; }
        public DateTime last_updated { get; set; }
    }

    public class Quote
    {
        public CurrenyPriceInfo CurrenyPriceInfo { get; set; }
    }
}