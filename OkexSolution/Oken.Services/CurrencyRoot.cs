using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oken.Services
{
    public class CurrencyRoot
    {
        public int code { get; set; }
        public List<CurrencyDatum> data { get; set; }
        public string detailMsg { get; set; }
        public string msg { get; set; }
    }
    public class CurrencyDatum
    {
        public int baseCurrency { get; set; }
        public string collect { get; set; }
        public bool isMarginOpen { get; set; }
        public int listDisplay { get; set; }
        public float marginRiskPreRatio { get; set; }
        public float marginRiskRatio { get; set; }
        public int marketFrom { get; set; }
        public int maxMarginLeverage { get; set; }
        public int maxPriceDigit { get; set; }
        public int maxSizeDigit { get; set; }
        public float minTradeSize { get; set; }
        public int online { get; set; }
        public int productId { get; set; }
        public int quoteCurrency { get; set; }
        public float quoteIncrement { get; set; }
        public int quotePrecision { get; set; }
        public int sort { get; set; }
        public string symbol { get; set; }
    }

}
