using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Oken.Services
{
    public class Contract
    {
        public int binary { get; set; }
        public string channel { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string high { get; set; }
        public string limitLow { get; set; }
        public string vol { get; set; }
        public string last { get; set; }
        public string low { get; set; }
        public string buy { get; set; }
        public string hold_amount { get; set; }
        public string sell { get; set; }
        public long contractId { get; set; }
        public string unitAmount { get; set; }
        public string limitHigh { get; set; }


        public decimal Buy()
        {
            decimal.TryParse(buy, out var number);
            return number;
        }
        public decimal Sell()
        {
            decimal.TryParse(sell, out var number);
            return number;
        }
    }

    public class Rootobject
    {
        [JsonProperty(PropertyName = "event")]
        public string EventName { get; set; }
        public string channel { get; set; }

        public static Rootobject CreateNew(string bName)
        {
            var rootobject = new Rootobject();
            rootobject.channel = $"ok_sub_spot_{bName}_ticker_next_week";
            rootobject.EventName = "addChannel";
            return rootobject;
        }

        public static readonly List<string> CurrencieNames = new List<string>() { "usdt", "btc", "eth" };

        public static List<Rootobject> CreateNewList(string bName)
        {
            var rootobjects = new List<Rootobject>();
            foreach (var currencieName in CurrencieNames)
            {
                var rootobject = new Rootobject();
                rootobject.channel = $"ok_sub_spot_{bName.ToLower()}_{currencieName}_ticker_next_week";
                rootobject.EventName = "addChannel";
                rootobjects.Add(rootobject);
            }
            return rootobjects;
        }
    }

}
