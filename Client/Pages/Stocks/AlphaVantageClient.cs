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

        public async Task<TimeSeriesIntraday60Min> GetTimeSeriesIntraday60Min(string symbol)
        {
            var interval = 60;
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval={interval}min&apikey={_apiKey}";

            string json = await _httpClient.GetStringAsync(url);

            try
            {
                var data = JsonConvert.DeserializeObject<TimeSeriesIntraday60Min>(json);
                return data;
                //Console.WriteLine(data.MetaData.Information);

                //foreach(var item in data.Series)
                //{
                //    Console.WriteLine($"{item.Key} | {item.Value.Open} | {item.Value.High} | {item.Value.Low} | {item.Value.Close} | {item.Value.Volume}");
                //}
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            
        }
    }
}
