using System;

namespace DrugTimer.Shared.Extensions
{
    public static class DatabaseExtension
    {
        /// <summary>
        /// Handles a database value, returning default value if object is null
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="value">Value to handle</param>
        /// <returns>Either null or casted value</returns>
        public static T HandleNull<T>(this object value)
        {
            T val;
            //need to find the non-nullable version of the type, as you can't convert to nullable type
            var nonNullableType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (value.Equals(DBNull.Value) || value.Equals(null) || value.Equals(""))
                val = default;
            else
                val = (T)Convert.ChangeType(value, nonNullableType);

            return val;
        }
    }
}
