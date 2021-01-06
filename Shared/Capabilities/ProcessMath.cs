using System;
using System.Collections.Generic;
using System.Text;

namespace Vorlesung.Shared.Capabilities
{
    public static class ProcessMath
    {
        public static double Average(IList<double> measurements)
        {
            if (measurements == null)
                throw new ArgumentNullException("Input data may not be null.");

            var numberOfMeasurements = measurements.Count;

            if (numberOfMeasurements <= 0)
                throw (new ArgumentOutOfRangeException("There must be at least one measurement to calculate Average."));

            var sum = 0d;

            foreach (var measurement in measurements)
            {
                sum += measurement;
            }

            return sum / (double)numberOfMeasurements;
        }

        public static double StdDeviation(IList<double> measurements)
        {
            if (measurements == null)
                throw new ArgumentNullException("Input data may not be null.");

            var numberOfMeasurements = measurements.Count;

            if (numberOfMeasurements <= 1)
                throw (new ArgumentOutOfRangeException("There must be at least two measurement to calculate StdDeviation."));

            try
            {
                var average = Average(measurements);

                var squaredDifference = 0d;

                foreach (var measurement in measurements)
                {
                    squaredDifference += Math.Pow(measurement - average, 2);
                }

                return Math.Sqrt(squaredDifference / (double)(numberOfMeasurements - 1));
            }
            catch
            {
                throw;
            }
        }
    }
}
