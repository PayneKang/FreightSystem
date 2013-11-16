using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreightSystem.Models.Extra
{
    public static class ExtraFunctions
    {
        public static string ToDateString(this DateTime? dt,string format)
        {
            if (!dt.HasValue)
                return string.Empty;
            else
                return dt.Value.ToString(format);
        }
    }
}
