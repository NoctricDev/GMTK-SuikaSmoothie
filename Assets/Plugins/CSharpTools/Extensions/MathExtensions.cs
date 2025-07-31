using System;

namespace CSharpTools.Extensions
{
    public static class MathExtensions
    {
        public static int FloorToInt(this double value) => (int)Math.Floor(value);

        public static int CeilToInt(this double value) => (int)Math.Ceiling(value);

        public static int RoundToInt(this double value) => (int)Math.Round(value);
        
        public static double Clamp01(this double value) => Clamp(value, 0, 1);
        public static double Clamp(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));
        
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (max.CompareTo(min) < 0)
                max = min;
            
            if (val.CompareTo(min) < 0)
                return min;
            
            return val.CompareTo(max) > 0 ? max : val;
        }
        
        public static double Remap(this double value, double low, double high, double newLow, double newHigh)
            => (value - low) / (high - low) * (newHigh - newLow) + newLow;
        
        public static float Remap(this float value, float low, float high, float newLow, float newHigh)
        {
            double temp = value;
            return (float)temp.Remap(low, high, newLow, newHigh);
        }

        public static float RemapToFloat(this int value, int low, int high, float newLow, float newHigh)
            => (float)((value - low) / (double)(high - low) * (newHigh - newLow) + newLow);

        public static float RemapToFloat(this long value, long low, long high, float newLow, float newHigh)
            => (float)((value - low) / (double)(high - low) * (newHigh - newLow) + newLow);
    }
}
