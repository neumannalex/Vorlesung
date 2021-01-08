using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plotly.Blazor;
using Plotly.Blazor.Traces;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.LayoutLib.XAxisLib;
using Plotly.Blazor.Traces.CandlestickLib;
using Line = Plotly.Blazor.Traces.CandlestickLib.DecreasingLib.Line;

namespace Vorlesung.Client.Pages.Stocks.Components
{
    public partial class IntradayChart : ComponentBase
    {
        [Parameter]
        public IntrayDayDataset Data { get; set; }

        [Parameter]
        public string Title { get; set; } = "Intraday Data";
       
        PlotlyChart Chart;
        Config ChartConfig = new Config
        {
            Responsive = true
        };
        Layout ChartLayout = new Layout
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
                Text = string.Empty
            },
            XAxis = new List<XAxis>
            {
                new XAxis
                {
                    AutoRange = AutoRangeEnum.True,
                    Domain = new List<object> {0, 1},
                    RangeSlider = new RangeSlider
                    {
                        AutoRange = true
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
                    Type = Plotly.Blazor.LayoutLib.YAxisLib.TypeEnum.Linear,
                    Title = new Plotly.Blazor.LayoutLib.YAxisLib.Title
                    {
                        Text = "Value"
                    }
                }
            }
            //Height = 500
        };

        IList<ITrace> ChartData = new List<ITrace>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await UpdateLayout();
                await UpdateData();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await UpdateLayout();
            await UpdateData();
        }

        private async Task UpdateLayout()
        {
            if(Chart != null)
            {
                ChartLayout.Title.Text = Title;

                await Chart.Update();
            }
        }

        private async Task UpdateData()
        {
            if (Data != null && Data.Series != null)
            {
                if(Chart != null)
                {
                    Chart.Data.Clear();

                    await Chart.AddTrace(new Candlestick
                    {
                        Name = Data.MetaData.Symbol,
                        X = Data.Series.Select(x => (object)x.Date).ToList(),
                        Open = Data.Series.Select(x => (object)x.Open).ToList(),
                        High = Data.Series.Select(x => (object)x.High).ToList(),
                        Low = Data.Series.Select(x => (object)x.Low).ToList(),
                        Close = Data.Series.Select(x => (object)x.Close).ToList(),
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

                    await Chart.Update();
                }   
            }
        }
    }
}
