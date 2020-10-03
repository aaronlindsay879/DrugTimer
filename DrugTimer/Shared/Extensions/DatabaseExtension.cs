using System;
using System.Collections.Generic;
using System.Text;

namespace DrugTimer.Shared.Extensions
{
    public static class DatabaseExtension
    {
        public static T HandleNull<T>(this object value)
        {
            T val;
            //need to find the non-nullable version of the type, as you can't convert to nullable type
            var nonNullableType = Nullable.GetUnderlyingType(typeof(T));

            if (value.Equals(DBNull.Value))
                val = default;
            else
                val = (T)Convert.ChangeType(value, nonNullableType);

            return val;
        }
    }
}
