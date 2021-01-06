using System;
using System.Collections.Generic;
using System.Linq;
using Vorlesung.Shared.Capabilities;
using Xunit;

namespace Tests
{
    public class CapabilityData
    {
        public List<double> Measurements = new List<double>();
        public double Lower;
        public double Upper;
        public double ExpectedCg;
        public double ExpectedCgk;
        public int Precision;

        public static CapabilityData Set1 = new CapabilityData
        {
            Measurements = new List<double> { 13.39, 14.16, 14.82, 19.43, 15.65, 12.58, 15.79, 15.32, 19.59, 14.72, 16.98, 16.06, 14.78, 15.98, 15.38, 12.28, 17.96, 14.16, 15.04, 11.12 },
            Upper = 15.5,
            Lower = 14.5,
            ExpectedCg = 0.077496708,
            ExpectedCgk = 0.037275917,
            Precision = 9
        };

        public static CapabilityData Set2 = new CapabilityData
        {
            Measurements = new List<double> { 2.93, 7.74, -2.18, -6.23, -8.95, -5.32, -6.42, -6.95, -9.37, -0.66, 1.10, 5.85, 9.45, 6.12, 5.46, -8.19, -8.67, -1.32, -5.49, 3.00 },
            Upper = 1.0,
            Lower = -1.0,
            ExpectedCg = 0.053635004,
            ExpectedCgk = -0.021722177,
            Precision = 9
        };

        public static CapabilityData Set3 = new CapabilityData
        {
            Measurements = new List<double> { -10.41, -10.94, -13.32, -11.54, -19.75, -16.88, -18.45, -15.91, -17.2, -12.59, -10.99, -11.67, -17.09, -18.3, -12.48, -14.28, -18.11, -14.21, -10.87, -12.39 },
            Upper = -10.0,
            Lower = -20.0,
            ExpectedCg = 0.545566497,
            ExpectedCgk = 0.476716005,
            Precision = 9
        };

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { CapabilityData.Set1 },
                new object[] { CapabilityData.Set2 },
                new object[] { CapabilityData.Set3 }
            };
    }

    public class ProcessCapabilityTests
    {
        [Fact]
        public void LimitsAreSet()
        {
            var lower = 10;
            var upper = 20;
            var cg = new CgProcessCapability(lower, upper);

            Assert.Equal(lower, cg.LowerLimitValue);
            Assert.Equal(upper, cg.UpperLimitValue);
        }

        [Theory]
        [InlineData(10, 20, 10)]
        public void LimitSpanIsCalculatedCorrectly(double lower, double upper, double span)
        {
            var cg = new CgProcessCapability(lower, upper);

            Assert.Equal(lower, cg.LowerLimitValue);
            Assert.Equal(upper, cg.UpperLimitValue);
            Assert.Equal(span, cg.LimitSpan);
        }

        [Fact]
        public void CgThrowsOnMeasurementsNull()
        {
            var cg = new CgProcessCapability(10, 20);
            
            Assert.Throws<ArgumentNullException>(() => cg.Calculate(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void CgThrowsOnTooFewMeasurements(int numberOfMeasurements)
        {
            var cg = new CgProcessCapability(10, 20);

            var measurements = Enumerable
                                    .Range(1, numberOfMeasurements)
                                    .Select(x => (double)x)
                                    .ToList();

            Assert.Throws<ArgumentOutOfRangeException>(() => cg.Calculate(measurements));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1000)]
        public void CgDoesNotThrowOnManyMeasurements(int numberOfMeasurements)
        {
            var cg = new CgProcessCapability(10, 20);

            var measurements = Enumerable
                                    .Range(1, numberOfMeasurements)
                                    .Select(x => (double)x)
                                    .ToList();

            var value = cg.Calculate(measurements);
            Assert.IsType<double>(value);
        }
    
        [Theory]
        [MemberData(nameof(CapabilityData.Data), MemberType = typeof(CapabilityData))]
        public void CgIsCorrect(CapabilityData data)
        {
            //var data = CapabilityData.Set1;

            var cg = new CgProcessCapability(data.Lower, data.Upper);
            var value = cg.Calculate(data.Measurements);

            Assert.Equal(data.ExpectedCg, value, data.Precision);
        }

        [Theory]
        [MemberData(nameof(CapabilityData.Data), MemberType = typeof(CapabilityData))]
        public void CgkIsCorrect(CapabilityData data)
        {
            var cgk = new CgkProcessCapability(data.Lower, data.Upper);
            var value = cgk.Calculate(data.Measurements);

            Assert.Equal(data.ExpectedCgk, value, data.Precision);
        }
    }
}
