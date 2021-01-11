using AntDesign;
using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vorlesung.Shared.Capabilities;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class HistogramView : ComponentBase
    {
        [Parameter]
        public List<MeasurementData> Data { get; set; }

        [Parameter]
        public LimitValuesModel Limits { get; set; } = null;

        public UpdateBinModel updateBinModel = new UpdateBinModel();

        public PlotlyChart _chart;

        public Config ChartConfig = new Config
        {
            Responsive = true
        };

        public Plotly.Blazor.Layout ChartLayout = new Plotly.Blazor.Layout
        {
            Title = new Plotly.Blazor.LayoutLib.Title
            {
                Text = "Test"
            },
            YAxis = new List<YAxis>
            {
                new YAxis
                {
                    Title = new Plotly.Blazor.LayoutLib.YAxisLib.Title
                    {
                        Text = "Counts"
                    },
                    AutoRange = Plotly.Blazor.LayoutLib.YAxisLib.AutoRangeEnum.True,
                    RangeMode = Plotly.Blazor.LayoutLib.YAxisLib.RangeModeEnum.ToZero
                },
                new YAxis
                {
                    Visible = false,
                    AutoRange = Plotly.Blazor.LayoutLib.YAxisLib.AutoRangeEnum.True,
                    RangeMode = Plotly.Blazor.LayoutLib.YAxisLib.RangeModeEnum.ToZero,
                    Overlaying = "y",
                    Side = Plotly.Blazor.LayoutLib.YAxisLib.SideEnum.Right
                }
            },
            Shapes = new List<Shape>
            { 
                new Shape
                {
                    Type = Plotly.Blazor.LayoutLib.ShapeLib.TypeEnum.Rect,
                    XRef = "x",
                    YRef = "y2",
                    X0 = 0,
                    Y0 = 0,
                    X1 = 0,
                    Y1 = 0,
                    FillColor = "#008000",
                    Opacity = (decimal)0.1,
                    Name = "+- 3 Sigma",
                    Line = new Plotly.Blazor.LayoutLib.ShapeLib.Line
                    {
                        Width = 0
                    }
                },
                new Shape
                {
                    Type = Plotly.Blazor.LayoutLib.ShapeLib.TypeEnum.Line,
                    XRef = "x",
                    YRef = "y2",
                    X0 = 0,
                    Y0 = 0,
                    X1 = 0,
                    Y1 = 0,
                    FillColor = "#ff0000",
                    Line = new Plotly.Blazor.LayoutLib.ShapeLib.Line
                    {
                        Width = 2,
                        Color = "#ff0000"
                    },
                    Opacity = (decimal)1.0,
                    Name = "Lower Limit"
                },
                new Shape
                {
                    Type = Plotly.Blazor.LayoutLib.ShapeLib.TypeEnum.Line,
                    XRef = "x",
                    YRef = "y2",
                    X0 = 0,
                    Y0 = 0,
                    X1 = 0,
                    Y1 = 0,
                    FillColor = "#ff0000",
                    Line = new Plotly.Blazor.LayoutLib.ShapeLib.Line
                    {
                        Width = 2,
                        Color = "#ff0000"
                    },
                    Opacity = (decimal)1.0,
                    Name = "Upper Limit"
                }
            }
        };

        public List<ITrace> ChartData = new List<ITrace>
        {
            new Plotly.Blazor.Traces.Histogram
            {
                Name = "Counts",
                NBinsX = 10,
                X = new List<object>()
            },
            new Plotly.Blazor.Traces.Scatter
            {
                Name = "Fit",
                YAxis = "y2",
                X = new List<object>(),
                Y = new List<object>()
            }
        };

        protected override async Task OnParametersSetAsync()
        {
            await UpdateChart();
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if(firstRender)
                await UpdateChart();
        }

        public async Task UpdateChart()
        {
            //Console.WriteLine("UpdateChart");
            try
            {
                StateHasChanged();
                
                if (!(_chart.Data[0] is Plotly.Blazor.Traces.Histogram histogram)) return;
                if (!(_chart.Data[1] is Plotly.Blazor.Traces.Scatter scatter)) return;

                // Fitted line
                var mu = ProcessMath.Mean(Data.Select(x => x.Value).ToList());
                var sigma = ProcessMath.StdDeviation(Data.Select(x => x.Value).ToList());

                var nSigma = 6;
                var min = mu - (double)nSigma * sigma;
                var max = mu + (double)nSigma * sigma;

                var nSteps = 100;
                var step = (max - min) / (double)nSteps;

                var fittedX = new object[nSteps];
                var fittedY = new object[nSteps];

                for(int i = 0; i < updateBinModel.NumberOfBins; i++)
                {
                    var x = min + i * step;
                    var y = (1d / Math.Sqrt(2d * Math.PI * sigma * sigma)) * Math.Exp(-((x - mu) * (x - mu)) / (2d * sigma * sigma));

                    fittedX[i] = x;
                    fittedY[i] = y;
                }

                scatter.X.Clear();
                scatter.Y.Clear();
                scatter.X.AddRange(fittedX);
                scatter.Y.AddRange(fittedY);
                //ChartLayout.XAxis[0].Range = new List<object> { min, max };

                // Histogram
                histogram.X.Clear();
                histogram.X.AddRange(Data.Select(x => (object)x.Value).ToList());
                histogram.NBinsX = updateBinModel.NumberOfBins;
                //ChartLayout.XAxis[0].Range = new List<object> { min, max };

                var overshoot = 0.0;
                var shapeMinY = (double)fittedY.Max() * (0d - overshoot);
                var shapeMaxY = (double)fittedY.Max() * (1d + overshoot);

                // Shapes
                ChartLayout.Shapes[0].X0 = mu - 3d * sigma;
                ChartLayout.Shapes[0].Y0 = shapeMinY;
                ChartLayout.Shapes[0].X1 = mu + 3d * sigma;
                ChartLayout.Shapes[0].Y1 = shapeMaxY;

                

                if (Limits != null)
                {
                    ChartLayout.Shapes[1].Visible = true;
                    ChartLayout.Shapes[1].X0 = Limits.Lower;
                    ChartLayout.Shapes[1].Y0 = shapeMinY;
                    ChartLayout.Shapes[1].X1 = Limits.Lower;
                    ChartLayout.Shapes[1].Y1 = shapeMaxY;

                    ChartLayout.Shapes[2].Visible = true;
                    ChartLayout.Shapes[2].X0 = Limits.Upper;
                    ChartLayout.Shapes[2].Y0 = shapeMinY;
                    ChartLayout.Shapes[2].X1 = Limits.Upper;
                    ChartLayout.Shapes[2].Y1 = shapeMaxY;
                }
                else
                {
                    ChartLayout.Shapes[1].Visible = false;
                    ChartLayout.Shapes[2].Visible = false;
                }
                

                await _chart.React();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
