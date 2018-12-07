using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xaml.Interactions.Utility
{
    internal static class Utils
    {
        public static bool Between<T>(this T me, T lower, T upper) where T : IComparable<T>
        {
            return me.CompareTo(lower) >= 0 && me.CompareTo(upper) <= 0;
        }
    }
}
