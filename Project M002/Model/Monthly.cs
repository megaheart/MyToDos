using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Monthly : Repeater
    {
        /// <param name="dates">items must have value from 1 to 31</param>
        public Monthly(int[] dates)
        {
            Duration = TimeSpan.Zero;
            Time = null;
            _dates = dates;
            Type = RepeaterType.Monthly;
            Mode = RepeaterMode.MultiTimes;
        }
        /// <param name="dates">items must have value from 1 to 31</param>
        public Monthly(int[] dates, TimeSpan duration)
        {
            Duration = duration;
            Time = null;
            _dates = dates;
            Type = RepeaterType.Monthly;
            Mode = RepeaterMode.MultiTimes;
        }
        /// <param name="dates">items must have value from 1 to 31</param>
        public Monthly(int[] dates, TimeDuration time)
        {
            Time = time;
            _dates = dates;
            Type = RepeaterType.Monthly;
            Mode = RepeaterMode.MultiTimes;
        }
        public void SetDates(int[] dates)
        {
            _dates = dates;
            OnRepeaterInfoChanged();
        }
        public override bool IsUsableOn(DateTime date)
        {
            return _dates.Contains((int)date.DayOfWeek);
        }
    }
}
