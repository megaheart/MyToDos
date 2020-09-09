using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;

namespace Storage
{
    public static class RepeatStorageConverter
    {
        public static string ToString(Repeat Repeat)
        {
            string output = ((int)Repeat.Type).ToString();
            if(Repeat.Type != RepeatType.Daily && Repeat.Type != RepeatType.Once)
            {
                foreach(var i in Repeat.Dates)
                {
                    output += "|" + i;
                }
            }
            return output;
        }
        public static Repeat Parse(string storageStr)
        {
            int[] type_dates = Array.ConvertAll(storageStr.Split('|'), int.Parse);
            int[] dates;
            switch (type_dates[0])
            {
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
                    return new Once();
                default:
                    dates = new int[type_dates.Length - 1];
                    Array.Copy(type_dates, 1, dates, 0, type_dates.Length - 1);
                    return new CustomRepeat(dates);
            }
        }
    }
}
