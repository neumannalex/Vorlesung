using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vorlesung.Shared.ML.Language;

namespace Vorlesung.Client.Pages.ML.Components
{
    public partial class PredictionDisplay : ComponentBase
    {
        [Parameter]
        public List<LanguagePrediction> Predictions { get; set; } = new List<LanguagePrediction>();



        protected override void OnParametersSet()
        {
            
        }
    }
}
