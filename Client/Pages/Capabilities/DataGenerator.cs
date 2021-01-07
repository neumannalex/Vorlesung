using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vorlesung.Shared.Extensions;

namespace Vorlesung.Client.Pages.Capabilities
{
    public class MeasurementData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public double Value { get; set; }
    }

    public static class DataGenerator
    {
        public static async Task<List<MeasurementData>> RandomAsync(int n, double min, double max)
        {
            return await Task.Run(() => { return Random(n, min, max); });
        }
        public static List<MeasurementData> Random(int n, double min, double max)
        {
            var rnd = new Random();

            var data = new List<MeasurementData>();

            for (int i = 0; i < n; i++)
                data.Add(new MeasurementData { Value = rnd.NextDouble(min, max) });

            return data;
        }

        public static async Task<List<MeasurementData>> NormalDistributionAsync(int n, double mean, double stdDeviation)
        {
            return await Task.Run(() => { return NormalDistribution(n, mean, stdDeviation); });
        }
        public static List<MeasurementData> NormalDistribution(int n, double mean, double stdDeviation)
        {
            var rnd = new Random();

            var data = new List<MeasurementData>();
            for(int i = 0; i < n; i++)
            {
                data.Add(new MeasurementData { Value = rnd.NextGaussian(mean, stdDeviation) });
            }

            return data;
        }

        public static async Task<List<MeasurementData>> StandardNormalDistributionAsync(int n)
        {
            return await Task.Run(() => { return StandardNormalDistribution(n); });
        }
        public static List<MeasurementData> StandardNormalDistribution(int n)
        {
            return NormalDistribution(n, 0, 1);
        }
    }
}
