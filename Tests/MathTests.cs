using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Vorlesung.Shared.Capabilities;
using Xunit;

namespace Tests
{
    public class MeasurementData
    {
        public List<double> Measurements;
        public double ExpectedMean;
        public double ExpectedSigma;
        public double Precision;

        public static MeasurementData Set1 = new MeasurementData
        {
            Measurements = new List<double> { 13.39, 14.16, 14.82, 19.43, 15.65, 12.58, 15.79, 15.32, 19.59, 14.72, 16.98, 16.06, 14.78, 15.98, 15.38, 12.28, 17.96, 14.16, 15.04, 11.12 },
            ExpectedMean = 15.2595,
            ExpectedSigma = 2.150628978,
            Precision = 9
        };

        public static MeasurementData Set2 = new MeasurementData
        {
            Measurements = new List<double> { 2.93, 7.74, -2.18, -6.23, -8.95, -5.32, -6.42, -6.95, -9.37, -0.66, 1.10, 5.85, 9.45, 6.12, 5.46, -8.19, -8.67, -1.32, -5.49, 3.00 },
            ExpectedMean = -1.405,
            ExpectedSigma = 6.214846825,
            Precision = 9
        };

        public static MeasurementData Set3 = new MeasurementData
        {
            Measurements = new List<double> { -10.41, -10.94, -13.32, -11.54, -19.75, -16.88, -18.45, -15.91, -17.2, -12.59, -10.99, -11.67, -17.09, -18.3, -12.48, -14.28, -18.11, -14.21, -10.87, -12.39 },
            ExpectedMean = -14.369,
            ExpectedSigma = 3.054928546,
            Precision = 9
        };



        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { MeasurementData.Set1 },
                new object[] { MeasurementData.Set2 },
                new object[] { MeasurementData.Set3 }
            };
    }

    public class MathTests
    {
        [Fact]
        public void Nonsense()
        {
            Assert.True(true);
        }

        [Fact]
        public void MeanThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => ProcessMath.Mean(null));
        }

        [Fact]
        public void MeanThrowsOnEmpty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessMath.Mean(new List<double>()));
        }

        [Fact]
        public void StdDeviationThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => ProcessMath.StdDeviation(null));
        }

        [Fact]
        public void StdDeviationThrowsOnEmpty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessMath.StdDeviation(new List<double>()));
        }

        [Fact]
        public void StdDeviationThrowsOnTooFew()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessMath.StdDeviation(new List<double> { 1.0 }));
        }

        [Theory]
        [MemberData(nameof(MeasurementData.Data), MemberType = typeof(MeasurementData))]
        public void MeanIsCorrect(MeasurementData data)
        {
            var avg = ProcessMath.Mean(data.Measurements);
            Assert.Equal(data.ExpectedMean, avg, 9);
        }

        [Theory]
        [MemberData(nameof(MeasurementData.Data), MemberType = typeof(MeasurementData))]
        public void SigmaIsCorrect(MeasurementData data)
        {
            var stdDeviation = ProcessMath.StdDeviation(data.Measurements);
            Assert.Equal(data.ExpectedSigma, stdDeviation, 9);
        }
    }
}
