using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Client.Extensions
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Takes a number of elements from a list, after reversing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iEnumerable">IEnumerable to use</param>
        /// <param name="num">Number of elements to take</param>
        /// <returns>Elements from the list</returns>
        public static IEnumerable<T> TakeReversed<T>(this IEnumerable<T> iEnumerable, int num)
        {
            var newEnumerable = iEnumerable.Reverse();

            return newEnumerable.Take(num);
        }
    }
}
