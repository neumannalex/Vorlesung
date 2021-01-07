using System;
using System.Collections.Generic;
using System.Text;

namespace Vorlesung.Shared.Extensions
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random rnd, double minimum, double maximum)
        {
            return rnd.NextDouble() * (maximum - minimum) + minimum;
        }

        public static double NextGaussian(this Random rnd)
        {
            return rnd.NextGaussian(0, 1);
        }

        public static double NextGaussian(this Random rnd, double mean, double stdDeviation)
        {
            double x1 = 1d - rnd.NextDouble();
            double x2 = 1d - rnd.NextDouble();

            double y1 = Math.Sqrt(-2d * Math.Log(x1)) * Math.Cos(2d * Math.PI * x2);

            return mean + y1 * stdDeviation;
        }
    }
}
