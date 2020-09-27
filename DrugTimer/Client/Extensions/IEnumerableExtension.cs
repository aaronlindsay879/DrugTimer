using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Client.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> TakeReversed<T>(this IEnumerable<T> iEnumerable, int num)
        {
            var newEnumerable = iEnumerable.Reverse();

            return newEnumerable.Take(num);
        }
    }
}
