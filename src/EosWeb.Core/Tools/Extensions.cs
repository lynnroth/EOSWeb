using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class Extensions
    {
        public static int ToInt32(this object value)
        {
            if (int.TryParse(value.ToString(), out int i))
            {
                return i;
            }

            throw new InvalidOperationException("Unable to parse as int");
        }

        public static double Round(this double value, int digits)
        {
            return Math.Abs(Math.Round(value, digits));
        }
    }
}
