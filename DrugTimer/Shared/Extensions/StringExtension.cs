using System.Linq;

namespace DrugTimer.Shared.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Strips all non-numeric chars from a given string
        /// </summary>
        /// <param name="str">String to remove non-numeric chars from</param>
        /// <returns>Formatted string</returns>
        public static string ToNumeric(this string str)
        {
            var numericChar = str.Where(char.IsDigit).ToArray();

            return new string(numericChar);
        }
    }
}
