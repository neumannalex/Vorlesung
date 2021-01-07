using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class StandardNormalDistributionGenerator : ComponentBase
    {
        [Parameter]
        public Action<List<MeasurementData>> OnDataChange { get; set; }

        [Parameter]
        public Action<bool> OnGeneratingDataChange { get; set; }

        public StdNormalDistributionDataModel model = new StdNormalDistributionDataModel();

        public async Task GenerateData()
        {
            OnGeneratingDataChange?.Invoke(true);

            var data = await DataGenerator.StandardNormalDistributionAsync(model.Count);

            OnDataChange?.Invoke(data);

            OnGeneratingDataChange?.Invoke(false);
        }
    }
}
