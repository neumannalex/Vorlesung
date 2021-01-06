using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vorlesung.Shared.Capabilities;
using Vorlesung.Shared.Extensions;


namespace Vorlesung.Client.Pages.Capabilities
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public NotificationService NotificationService { get; set; }

        public List<MeasurementData> Measurements = new List<MeasurementData>();
        public double LowerLimit = 10;
        public double UpperLimit = 20;

        public double? Cg;
        public double? Cgk;

        public double? NewMeasurementValue;

        public async Task CalculateCapabilities()
        {
            var cgCapability = new CgProcessCapability(LowerLimit, UpperLimit);
            var cgkCapability = new CgkProcessCapability(LowerLimit, UpperLimit);

            var data = Measurements.Select(x => x.Value).ToList();

            try
            {
                Cg = cgCapability.Calculate(data);
            }
            catch(Exception ex)
            {
                Cg = null;

                await NotificationService.Error(new NotificationConfig { 
                    Message = "Cg Calculation",
                    Description = ex.Message
                });
            }

            try
            {
                Cgk = cgkCapability.Calculate(data);
            }
            catch (Exception ex)
            {
                Cgk = null;

                await NotificationService.Error(new NotificationConfig
                {
                    Message = "Cgk Calculation",
                    Description = ex.Message
                });
            }

            StateHasChanged();
        }

        public void AddMeasurement()
        {
            if (Measurements == null)
                Measurements = new List<MeasurementData>();

            if(NewMeasurementValue.HasValue)
            {
                Measurements.Add(new MeasurementData
                {
                    Value = NewMeasurementValue.Value
                });
                StateHasChanged();
            }
        }

        public bool CanAddMeasurement()
        {
            if (!NewMeasurementValue.HasValue)
                return false;

            return true;
        }

        public void DeleteMeasurement(string id)
        {
            if (Measurements == null || Measurements.Count <= 0)
                return;

            var item = Measurements.FirstOrDefault(x => x.Id == id);
            Measurements.Remove(item);
        }

        public void GenerateRandomData()
        {
            int n = 20;

            var rnd = new Random();

            var data = new List<MeasurementData>();

            for (int i = 0; i < n; i++)
                data.Add(new MeasurementData { Value = rnd.NextDouble(LowerLimit, UpperLimit) });

            Measurements = data;
        }
    }

    public class MeasurementData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public double Value { get; set; }
    }
}
