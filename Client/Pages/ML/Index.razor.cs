using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Vorlesung.Shared.ML.Language;

namespace Vorlesung.Client.Pages.ML
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public HttpClient httpClient { get; set; }

        public bool IsLoading { get; set; } = false;

        public LanguagePrediction BestPrediction { get; set; }

        public List<LanguagePrediction> Predictions { get; set; } = new List<LanguagePrediction>();
    
        public async void OnTextChange(string text)
        {
            IsLoading = true;
            StateHasChanged();

            Predictions = new List<LanguagePrediction>();
            BestPrediction = null;

            ModelInput sampleData = new ModelInput()
            {
                Text = text
            };

            var response = await httpClient.PostAsJsonAsync<ModelInput>("/api/predict", sampleData);
            var content = await response.Content.ReadAsStringAsync();

            Predictions = JsonConvert.DeserializeObject<List<LanguagePrediction>>(content);
            BestPrediction = Predictions.Where(x => x.Score == Predictions.Max(y => y.Score)).First();

            IsLoading = false;
            StateHasChanged();
        }
    }
}
