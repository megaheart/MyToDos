using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;

namespace Storage
{
    internal static class RepeaterStorageConverter
    {
        internal static string ToString(Repeater repeater)
        {
            string output = ((int)repeater.Type).ToString();
            if(repeater.Type != RepeaterType.Daily && repeater.Type == RepeaterType.NonRepeater)
            {
                foreach(var i in repeater.Dates)
                {
                    output += "|" + i;
                }
            }
            return output;
        }
        internal static Repeater Parse(string storageStr)
        {
            int[] type_dates = Array.ConvertAll(storageStr.Split('|'), int.Parse);
            if (type_dates[0] == 1)
            {
                return new Daily();
            }
            else if (type_dates[0] == 0)
            {
                return new NonRepeater();
            }
            else if (type_dates[0] == 2)
            {
                int[] dates = new int[type_dates.Length - 1];
                Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                return new Weekly(dates);
            }
            else if (type_dates[0] == 3)
            {
                int[] dates = new int[type_dates.Length - 1];
                Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                return new Monthly(dates);
            }
            else if (type_dates[0] == 4)
            {
                int[] dates = new int[type_dates.Length - 1];
                Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                return new Once(dates);
            }
            else //if (type_dates[0] == 5)
            {
                int[] dates = new int[type_dates.Length - 1];
                Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                return new CustomRepeater(dates);
            }
        }
    }
}
