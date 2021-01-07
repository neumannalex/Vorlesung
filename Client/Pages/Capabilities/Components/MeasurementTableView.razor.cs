using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class MeasurementTableView : ComponentBase
    {
        [Parameter]
        public List<MeasurementData> Data { get; set; }

        [Parameter]
        public Action<List<MeasurementData>> OnDataChange { get; set; }

        public AddMeasurementModel addMeasurementModel = new AddMeasurementModel();

        public AntDesign.Form<AddMeasurementModel> form;

        public void AddMeasurement()
        {
            try
            {
                var isValid = form.Validate();

                Console.WriteLine($"Is valid: {isValid}");
                Console.WriteLine($"Data: {JsonConvert.SerializeObject(addMeasurementModel)}");

                if (Data != null && isValid)
                {
                    Data.Add(new MeasurementData { Value = addMeasurementModel.Measurement });
                    OnDataChange?.Invoke(Data);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ClearData()
        {
            if (Data != null)
            {
                Data = new List<MeasurementData>();
                OnDataChange?.Invoke(Data);
            }
        }

        public void DeleteMeasurement(string id)
        {
            if (Data == null || Data.Count <= 0)
                return;

            var item = Data.FirstOrDefault(x => x.Id == id);
            Data.Remove(item);

            OnDataChange?.Invoke(Data);
        }
    }
}
