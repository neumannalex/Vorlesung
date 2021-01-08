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

        public bool IsLoading = false;
        public IntrayDayDataset Data = new IntrayDayDataset();
        public AlphaVantageCompanyOverview CompanyOverviewData = null;
        
        public AlphaVantageSymbolInfo SelectedSymbol;

        public async Task<IEnumerable<AlphaVantageSymbolInfo>> SearchSymbols(string searchText)
        {
            return await  AlphaVantage.SearchSymbol(searchText);
        }

        public async Task Load()
        {
            IsLoading = true;
            StateHasChanged();

            var symbol = SelectedSymbol.Symbol;

            Data = await AlphaVantage.GetTimeSeriesIntraday60Min(symbol);

            CompanyOverviewData = await AlphaVantage.GetCompanyOverview(symbol);

            IsLoading = false;
            StateHasChanged();
        }
    }
}
