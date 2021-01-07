using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class EvenDistributionGenerator : ComponentBase
    {
        [Parameter]
        public Action<List<MeasurementData>> OnDataChange { get; set; }

        [Parameter]
        public Action<bool> OnGeneratingDataChange { get; set; }

        public RandomDataModel model = new RandomDataModel();

        public async Task GenerateData()
        {
            OnGeneratingDataChange?.Invoke(true);

            var data = await DataGenerator.RandomAsync(model.Count, model.Min, model.Max);

            OnDataChange?.Invoke(data);

            OnGeneratingDataChange?.Invoke(false);
        }
    }
}
