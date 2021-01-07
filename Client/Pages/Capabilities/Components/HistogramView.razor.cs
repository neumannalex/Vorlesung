using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class HistogramView : ComponentBase
    {
        [Parameter]
        public List<MeasurementData> Data { get; set; }

        public UpdateBinModel updateBinModel = new UpdateBinModel();

        public object[] HistogramData = new object[0];
        public IChartComponent _chart;
        public HistogramConfig ChartConfig = new HistogramConfig
        {
            Title = new AntDesign.Charts.Title
            {
                Visible = true,
                Text = "Histogram"
            },
            Description = new Description
            {
                Visible = false,
                Text = "tbd"
            },
            ForceFit = true,
            Padding = "auto",
            BinField = "value",
            BinNumber = 10
        };


        protected override async Task OnParametersSetAsync()
        {
            await UpdateChart();
        }

        public async Task UpdateChart()
        {
            try
            {
                StateHasChanged();

                HistogramData = Data.Select(x => new { value = x.Value }).Cast<object>().ToArray();

                Console.WriteLine($"Building histogram from {HistogramData.Length} data points.");

                ChartConfig.BinNumber = updateBinModel.NumberOfBins;
                await _chart.UpdateConfig(ChartConfig);

                await _chart.ChangeData(HistogramData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
