using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrugTimer.Shared.Extensions
{
    public static class StringExtension
    {
        public static string ToNumeric(this string str)
        {
            var numericChar = str.Where(c => char.IsDigit(c)).ToArray();

            return new string(numericChar);
        }
    }
}
