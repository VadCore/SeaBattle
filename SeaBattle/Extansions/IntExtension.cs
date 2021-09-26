using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Extansions
{
    public static class IntExtension
    {
        public static int ExceptionIfNotBetweenMinMax(this int value, int min = int.MinValue, int max = int.MaxValue)
        {
            if(value >= min && value <= max)
            {
                return value;
            }
            else
            {
                throw new ArgumentException(string.Format($"Value must be between {0} and {1}", min, max));
            }
        }
    }
}
