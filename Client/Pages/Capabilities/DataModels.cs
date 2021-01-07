using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Capabilities
{
    public class CapabilityDetailsModel
    {
        public double? Mean { get; set; }
        public double? StdDeviation { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public long? Count { get; set; }
        public double? Cg { get; set; }
        public double? Cgk { get; set; }
    }

    public class LimitValuesModel
    {
        [Required]
        public double Lower { get; set; } = 10;
        [Required]
        public double Upper { get; set; } = 20;
    }

    public class UpdateBinModel
    {
        [Required]
        public int NumberOfBins { get; set; } = 100;
    }

    public class AddMeasurementModel
    {
        [Required]
        public double Measurement { get; set; }
    }

    public class RandomDataModel
    {
        [Required]
        public double Min { get; set; } = 10;
        [Required]
        public double Max { get; set; } = 20;
        [Required]
        public int Count { get; set; } = 30;
    }

    public class NormalDistributionDataModel
    {
        [Required]
        public double Mean { get; set; } = 15;
        [Required]
        public double StdDeviation { get; set; } = 2;
        [Required]
        public int Count { get; set; } = 30;
    }

    public class StdNormalDistributionDataModel
    {
        [Required]
        public int Count { get; set; } = 30;
    }
}
