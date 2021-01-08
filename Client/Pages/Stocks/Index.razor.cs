using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.LayoutLib.XAxisLib;
using Plotly.Blazor.Traces.CandlestickLib;
using Line = Plotly.Blazor.Traces.CandlestickLib.DecreasingLib.Line;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Vorlesung.Shared.Extensions;
using Plotly.Blazor;
using Plotly.Blazor.Traces;

namespace Vorlesung.Client.Pages.Stocks
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public AlphaVantageClient AlphaVantage { get; set; }

        public TimeSeriesIntraday60Min Data = new TimeSeriesIntraday60Min();
        public List<TimeSeriesRecord> Series = new List<TimeSeriesRecord>();
        public bool IsLoading = false;

        // Plotly
        PlotlyChart chart;
        Config config = new Config
        {
            Responsive = true
        };
        Layout layout = new Layout
        {
            DragMode = DragModeEnum.Zoom,
            Margin = new Margin
            {
                R = 10,
                T = 25,
                B = 40,
                L = 60
            },
            ShowLegend = true,
            Title = new Plotly.Blazor.LayoutLib.Title
            {
                Text = "Intraday Data"
            },
            XAxis = new List<XAxis>
            {
                new XAxis
                {
                    AutoRange = AutoRangeEnum.True,
                    Domain = new List<object> {0, 1},
                    //Range = new List<object> {"2017-01-03 12:00", "2017-02-15 12:00"},
                    RangeSlider = new RangeSlider
                    {
                        AutoRange = true
                        //Range = new object[] {"2017-01-03 12:00", "2017-02-15 12:00"}
                    },
                    Title = new Plotly.Blazor.LayoutLib.XAxisLib.Title
                    {
                        Text = "Date"
                    },
                    Type = TypeEnum.Date
                }
            },
            YAxis = new List<YAxis>
            {
                new YAxis
                {
                    AutoRange = Plotly.Blazor.LayoutLib.YAxisLib.AutoRangeEnum.True,
                    Domain = new List<object> {0, 1},
                    //Range = new List<object> {114.609999778, 137.410004222},
                    Type = Plotly.Blazor.LayoutLib.YAxisLib.TypeEnum.Linear
                }
            },
            Height = 500
        };

        IList<ITrace> data = new List<ITrace>();

        public async Task Load()
        {
            IsLoading = true;
            StateHasChanged();
            
            var symbol = "TSLA";

            Data = await AlphaVantage.GetTimeSeriesIntraday60Min(symbol);
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


                chart.Data.Clear();

                await chart.AddTrace(new Candlestick
                {
                    Name = symbol,
                    X = Series.Select(x => (object)x.Date).ToList(),
                    Open = Series.Select(x => (object)x.Open).ToList(),
                    High = Series.Select(x => (object)x.High).ToList(),
                    Low = Series.Select(x => (object)x.Low).ToList(),
                    Close = Series.Select(x => (object)x.Close).ToList(),
                    Decreasing = new Decreasing
                    {
                        Line = new Line
                        {
                            Color = "#7F7F7F"
                        }
                    },
                    Increasing = new Increasing
                    {
                        Line = new Plotly.Blazor.Traces.CandlestickLib.IncreasingLib.Line
                        {
                            Color = "rgba(31,119,180,1)"
                        }
                    },
                    XAxis = "x",
                    YAxis = "y"
                });

                await chart.Update();
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
