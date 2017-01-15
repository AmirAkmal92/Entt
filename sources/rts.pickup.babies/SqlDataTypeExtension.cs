using System;

namespace Bespoke.PosEntt.CustomActions
{
    public static class SqlDataTypeExtension
    {
        public static DateTime? ToValidSqlDateTime(this DateTime? value)
        {
            if (null == value) return null;
            if (value.Value == DateTime.MinValue)
                return null;
            if (value.Value.Year <= 1753)
                return null;
            return value.Value;
        }
        public static DateTime ToValidSqlDateTime(this DateTime value)
        {
            if (value == DateTime.MinValue)
                return new DateTime(1753, 1, 1);
            if (value.Year <= 1753)
                return new DateTime(1753, 1, 1);
            return value;
        }
    }
}
