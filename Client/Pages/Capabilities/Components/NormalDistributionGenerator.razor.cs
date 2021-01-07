using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class NormalDistributionGenerator : ComponentBase
    {
        [Parameter]
        public Action<List<MeasurementData>> OnDataChange { get; set; }

        [Parameter]
        public Action<bool> OnGeneratingDataChange { get; set; }

        public NormalDistributionDataModel model = new NormalDistributionDataModel();

        public async Task GenerateData()
        {
            OnGeneratingDataChange?.Invoke(true);

            var data = await DataGenerator.NormalDistributionAsync(model.Count, model.Mean, model.StdDeviation);

            OnDataChange?.Invoke(data);

            OnGeneratingDataChange?.Invoke(false);
        }
    }
}
