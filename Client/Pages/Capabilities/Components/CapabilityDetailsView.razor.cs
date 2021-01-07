using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vorlesung.Shared.Capabilities;

namespace Vorlesung.Client.Pages.Capabilities.Components
{
    public partial class CapabilityDetailsView : ComponentBase
    {
        [Inject]
        public NotificationService NotificationService { get; set; }

        [Parameter]
        public List<MeasurementData> Data { get; set; }

        public LimitValuesModel limitValuesModel = new LimitValuesModel();
        public CapabilityDetailsModel capabilityDetails = new CapabilityDetailsModel();

        public string CgColor = null;
        public string CgkColor = null;

        public Dictionary<string, string> Colors = new Dictionary<string, string>
        {
            {"empty", ""},
            {"green", "color: #3f8600" },
            {"red", "color: #cf1322" }
        };

        protected override void OnParametersSet()
        {
            Calculate();
        }

        public void Calculate()
        {
            if (Data == null || Data.Count < 2)
            {
                capabilityDetails = new CapabilityDetailsModel();

                CgColor = Colors["empty"];
                CgkColor = Colors["empty"];

                //await NotificationService.Error(new NotificationConfig
                //{
                //    Message = "Capability Calculation",
                //    Description = "There must be at least two measurements to calculate the capabilities."
                //});

                return;
            }

            var cgCapability = new CgProcessCapability(limitValuesModel.Lower, limitValuesModel.Upper);
            var cgkCapability = new CgkProcessCapability(limitValuesModel.Lower, limitValuesModel.Upper);

            var data = Data.Select(x => x.Value).ToList();

            try
            {
                capabilityDetails.Mean = ProcessMath.Mean(data);
            }
            catch (Exception ex)
            {
                capabilityDetails.Mean = null;
            }

            try
            {
                capabilityDetails.StdDeviation = ProcessMath.StdDeviation(data);
            }
            catch (Exception ex)
            {
                capabilityDetails.StdDeviation = null;
            }

            try
            {
                capabilityDetails.Cg = cgCapability.Calculate(data);
                CgColor = capabilityDetails.Cg >= 1.67 ? Colors["green"] : Colors["red"];
            }
            catch (Exception ex)
            {
                capabilityDetails.Cg = null;
                CgColor = Colors["empty"];
            }

            try
            {
                capabilityDetails.Cgk = cgkCapability.Calculate(data);
                CgkColor = capabilityDetails.Cgk >= 1.67 ? Colors["green"] : Colors["red"];
            }
            catch (Exception ex)
            {
                capabilityDetails.Cgk = null;
                CgkColor = Colors["empty"];
            }

            capabilityDetails.Min = data.Min();
            capabilityDetails.Max = data.Max();
            capabilityDetails.Count = data.Count;
        }
    }
}
