using System;
using System.Collections.Generic;
using System.Text;

namespace Vorlesung.Shared.Capabilities
{
    public interface IProcessCapability
    {
        double LowerLimitValue { get; }
        double UpperLimitValue { get; }
        double LimitSpan { get; }
        double Calculate(IList<double> measurements);
    }

    public abstract class ProcessCapabilityBase : IProcessCapability
    {
        public double LowerLimitValue { get; protected set; }

        public double UpperLimitValue { get; protected set; }

        public double LimitSpan
        {
            get
            {
                return UpperLimitValue - LowerLimitValue;
            }
        }

        public abstract double Calculate(IList<double> measurements);
    }

    public class CgProcessCapability : ProcessCapabilityBase
    {
        public CgProcessCapability(double lowerLimit, double upperLimit)
        {
            LowerLimitValue = lowerLimit;
            UpperLimitValue = upperLimit;
        }

        public override double Calculate(IList<double> measurements)
        {
            try
            {
                var sigma = ProcessMath.StdDeviation(measurements);

                return LimitSpan / (6d * sigma);
            }
            catch
            {
                throw;
            }
        }
    }

    public class CgkProcessCapability : ProcessCapabilityBase
    {
        public CgkProcessCapability(double lowerLimit, double upperLimit)
        {
            LowerLimitValue = lowerLimit;
            UpperLimitValue = upperLimit;
        }

        public override double Calculate(IList<double> measurements)
        {
            try
            {
                var sigma = ProcessMath.StdDeviation(measurements);

                var average = ProcessMath.Average(measurements);

                return Math.Min(UpperLimitValue - average, average - LowerLimitValue) / (3d * sigma);
            }
            catch
            {
                throw;
            }
        }
    }
}
