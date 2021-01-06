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
    }
}
