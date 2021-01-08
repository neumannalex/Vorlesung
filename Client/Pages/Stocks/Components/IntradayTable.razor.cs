using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Stocks.Components
{
    public partial class IntradayTable : ComponentBase
    {
        [Parameter]
        public IntrayDayDataset Data { get; set; }

        public List<IntradayRecord> Series = new List<IntradayRecord>();

        protected override void OnParametersSet()
        {
            if (Data != null)
                Series = Data.Series;
        }
    }
}
