

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Utilities.Helpers
{
    public class CSSHelper
    {
        public static string IndentAdder(int level)
        {
            string result = (level * 15).ToString();
            result += "px";
            return result;
        }
        public static string BoldTextAdder(int level)
        {
            if (level <= 2)
            {
                return "bold";
            }
            return "";
        }
        //public static string ColorSelector(Color color)
        //{

        //}
        public static string FontStyleAdder(int level)
        {
            if (level == 2 || level == 4)
            {
                return "italic";
            }
            return "normal";
        }
    }
}
