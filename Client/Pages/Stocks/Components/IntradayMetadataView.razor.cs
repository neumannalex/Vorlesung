using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Stocks.Components
{
    public partial class IntradayMetadataView : ComponentBase
    {
        [Parameter]
        public IntrayDayDataset Data { get; set; }
    }
}
