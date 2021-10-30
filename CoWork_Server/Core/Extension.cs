using System;

namespace Co_Work.Core
{
    static class RandomExtension
    {
        public static double NextDouble(this Random random,double minValue,double maxValue)
        {
            return minValue + (random.NextDouble()) * (maxValue - minValue);
        }
    }
}