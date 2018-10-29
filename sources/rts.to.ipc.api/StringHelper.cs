using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rts.to.ipc.api
{
    public static class StringHelper
    {
        public static string ToNullStringDash(this string val)
        {
            return !string.IsNullOrEmpty(val) ? val : "-";
        }

        public static string ToNullStringDash(this decimal? val)
        {
            if (val.HasValue)
            {
                return Convert.ToString(val);
            }
            else
            {
                return "-";
            }
        }
    }
}
