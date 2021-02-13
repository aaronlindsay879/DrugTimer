using System.Collections.Generic;
using System.Linq;

namespace DrugTimer.Shared.Extensions
{
    public static class EnumerableExtension
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

        /// <summary>
        /// Finds the first info with a given GUID
        /// </summary>
        /// <param name="list">List to search</param>
        /// <param name="guid">GUID to search for</param>
        /// <returns>Info with that GUID</returns>
        public static DrugInfo FirstGuid(this IEnumerable<DrugInfo> list, string guid) => list.First(entry => entry.Guid == guid);
    }
}
