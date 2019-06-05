using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Weekly : Repeater
    {
        /// <param name="dates">Sun,Mon,Tue,...,Sat -> 0,1,2,...,6</param>
        public Weekly(int[] dates)
        {
            Duration = TimeSpan.Zero;
            Time = null;
            _dates = dates;
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        /// <param name="dates">Sun,Mon,Tue,...,Sat -> 0,1,2,...,6</param>
        public Weekly(int[] dates, TimeSpan duration)
        {
            Duration = duration;
            Time = null;
            _dates = dates;
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        /// <param name="dates">Sun,Mon,Tue,...,Sat -> 0,1,2,...,6</param>
        public Weekly(int[] dates, TimeDuration time)
        {
            Time = time;
            _dates = dates;
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        public Weekly(DayOfWeek[] dates)
        {
            Duration = TimeSpan.Zero;
            Time = null;
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        public Weekly(DayOfWeek[] dates, TimeSpan duration)
        {
            Duration = duration;
            Time = null;
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        public Weekly(DayOfWeek[] dates, TimeDuration time)
        {
            Time = time;
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        public void SetDates(int[] dates)
        {
            _dates = dates;
            OnRepeaterInfoChanged();
        }
        public void SetDates(DayOfWeek[] dates)
        {
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            OnRepeaterInfoChanged();
        }
        static int IntFromDayOfWeek(DayOfWeek day) => (int)day;
        public override bool IsUsableOn(DateTime date)
        {
            return _dates.Contains((int)date.DayOfWeek);
        }
    }
}
