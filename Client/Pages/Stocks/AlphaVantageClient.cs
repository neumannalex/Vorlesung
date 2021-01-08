using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Stocks
{
    public class AlphaVantageClient
    {
        private readonly HttpClient _httpClient;
        private const string _apiKey = "J2YFN0NELNTMCF02";

        public AlphaVantageClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IntrayDayDataset> GetTimeSeriesIntraday60Min(string symbol)
        {
            var interval = 60;
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval={interval}min&apikey={_apiKey}";

            string json = await _httpClient.GetStringAsync(url);

            try
            {
                var data = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesIntraday60Min>(json);

                var dataset = new IntrayDayDataset
                {
                    MetaData = new IntradayMetaData
                    {
                        Information = data.MetaData.Information,
                        Symbol = data.MetaData.Symbol,
                        LastRefreshed = data.MetaData.LastRefreshed,
                        Interval = data.MetaData.Interval,
                        OutputSize = data.MetaData.OutputSize,
                        TimeZone = data.MetaData.TimeZone
                    },
                    Series = data.Series.OrderBy(x => x.Key).Select(x => new IntradayRecord
                    {
                        Date = x.Key,
                        Open = x.Value.Open,
                        High = x.Value.High,
                        Low = x.Value.Low,
                        Close = x.Value.Close,
                        Volume = x.Value.Volume
                    }).ToList()
                };
                
                return dataset;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            
        }
    
        public async Task<List<AlphaVantageSymbolInfo>> SearchSymbol(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return new List<AlphaVantageSymbolInfo>();

            var url = $"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={keyword}&apikey={_apiKey}";

            string json = await _httpClient.GetStringAsync(url);

            var result = JsonConvert.DeserializeObject<AlphaVantageSymbolSearchResult>(json);

            return result.BestMatches;
        }
    
        public async Task<AlphaVantageCompanyOverview> GetCompanyOverview(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={symbol}&apikey={_apiKey}";

            string json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<AlphaVantageCompanyOverview>(json);
        }
    }
}
