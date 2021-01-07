using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Vorlesung.Client.Pages.Capabilities
{
    public partial class Index : ComponentBase
    {
        public List<MeasurementData> Measurements = new List<MeasurementData>();

        public bool IsGeneratingData = false;

        public void OnDataChanged(List<MeasurementData> data)
        {
            Measurements = data;
            StateHasChanged();
        }

        public void OnGeneratingDataChanged(bool state)
        {
            IsGeneratingData = state;
            StateHasChanged();
        }
    }    
}
