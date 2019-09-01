using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;

namespace Storage
{
    public static class RepeaterStorageConverter
    {
        public static string ToString(Repeater repeater)
        {
            string output = ((int)repeater.Type).ToString();
            if(repeater.Type != RepeaterType.Daily && repeater.Type != RepeaterType.NonRepeater)
            {
                foreach(var i in repeater.Dates)
                {
                    output += "|" + i;
                }
            }
            return output;
        }
        public static Repeater Parse(string storageStr)
        {
            int[] type_dates = Array.ConvertAll(storageStr.Split('|'), int.Parse);
            int[] dates;
            switch (type_dates[0])
            {
                case 0:
                    return new NonRepeater();
                case 1:
                    return new Daily();
                case 2:
                    dates = new int[type_dates.Length - 1];
                    Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                    return new Weekly(dates);
                case 3:
                    dates = new int[type_dates.Length - 1];
                    Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                    return new Monthly(dates);
                case 4:
                    dates = new int[type_dates.Length - 1];
                    Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                    return new Once(dates);
                default:
                    dates = new int[type_dates.Length - 1];
                    Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                    return new CustomRepeater(dates);
            }
        }
    }
}
