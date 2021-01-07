using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Stocks
{
    public class TimeSeriesIntraday60Min
    {
        [JsonProperty(PropertyName = "Meta Data")]
        public TimeSeriesIntradayMetaData MetaData { get; set; }

        [JsonProperty(PropertyName = "Time Series (60min)")]
        public Dictionary<DateTime, TimeSeriesIntradayValue> Series { get; set; } = new Dictionary<DateTime, TimeSeriesIntradayValue>();
    }

    public class TimeSeriesIntradayMetaData
    {
        [JsonProperty(PropertyName = "1. Information")]
        public string Information { get; set; }

        [JsonProperty(PropertyName = "2. Symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonProperty(PropertyName = "4. Interval")]
        public string Interval { get; set; }

        [JsonProperty(PropertyName = "5. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty(PropertyName = "6. Time Zone")]
        public string TimeZone { get; set; }
    }

    public class TimeSeriesIntradayValue
    {
        [JsonProperty(PropertyName = "1. open")]
        public double Open { get; set; }

        [JsonProperty(PropertyName = "2. high")]
        public double High { get; set; }

        [JsonProperty(PropertyName = "3. low")]
        public double Low { get; set; }

        [JsonProperty(PropertyName = "4. close")]
        public double Close { get; set; }

        [JsonProperty(PropertyName = "5. volume")]
        public long Volume { get; set; }

    }
}
