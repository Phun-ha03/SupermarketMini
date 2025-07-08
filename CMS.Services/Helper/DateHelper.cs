using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Helper
{
    public class DateHelper
    {
        public static string getTimespan(DateTime dt)
        {
            TimeSpan span = DateTime.Now.Subtract(dt);
            if (span.Hours <= 0)
            {

                return span.Minutes.ToString() + " min";

            }
            else
            {
                return dt.ToString("dd/MM/yyyy hh:mm");
            }
        }

        public static short getYear(DateTime? dt)
        {
            return (short)(dt == null ? 0 : dt.Value.Year);
        }

        public static short getMonth(DateTime? dt)
        {
            return (short)(dt == null ? 0 : dt.Value.Month);
        }

        public static short getQuater(DateTime? dt)
        {
            if (dt == null) return (short)0;
            var month = dt.Value.Month;
            if (month <= 3)
            {
                return (short)1;
            }
            else if (month <= 6)
            {
                return (short)2;
            }
            else if (month <= 9)
            {
                return (short)3;
            }
            else
            {
                return (short)4;
            }
        }
    }
}
