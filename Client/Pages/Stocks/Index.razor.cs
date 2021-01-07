using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Vorlesung.Shared.Extensions;

namespace Vorlesung.Client.Pages.Stocks
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public AlphaVantageClient AlphaVantage { get; set; }

        public TimeSeriesIntraday60Min Data = new TimeSeriesIntraday60Min();
        public List<TimeSeriesRecord> Series = new List<TimeSeriesRecord>();
        public bool IsLoading = false;

        public async Task Load()
        {
            IsLoading = true;
            StateHasChanged();

            Data = await AlphaVantage.GetTimeSeriesIntraday60Min("TSLA");
            if(Data != null)
            {
                Series = Data.Series.OrderBy(x => x.Key).Select(x => new TimeSeriesRecord {
                    Date = x.Key,
                    Open = x.Value.Open,
                    High = x.Value.High,
                    Low = x.Value.Low,
                    Close = x.Value.Close,
                    Volume = x.Value.Volume
                }).ToList();
            }

            IsLoading = false;
            StateHasChanged();
        }
    }

    public class TimeSeriesRecord
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public long Volume { get; set; }
    }
}
