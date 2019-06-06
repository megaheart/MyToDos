using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Once : Repeater
    {
        public Once(DateTime date)
        {
            Duration = TimeSpan.Zero;
            Time = null;
            Type = RepeaterType.Once;
            Mode = RepeaterMode.MultiTimes;
            _dates = new int[] { date.Day, date.Month, date.Year };
        }
        public Once(DateTime date, TimeSpan duration)
        {
            Duration = duration;
            Time = null;
            Type = RepeaterType.Once;
            Mode = RepeaterMode.MultiTimes;
            _dates = new int[] { date.Day, date.Month, date.Year };
        }
        public Once(DateTime date, TimeDuration time)
        {
            Time = time;
            Type = RepeaterType.Once;
            Mode = RepeaterMode.MultiTimes;
            _dates = new int[] { date.Day, date.Month, date.Year };
        }
        public void SetDate(DateTime date)
        {
            _dates = new int[] { date.Day, date.Month, date.Year };
            OnRepeaterInfoChanged();
        }
        public override bool IsUsableOn(DateTime date)
        {
            return date.Year == _dates[2] && date.Month == _dates[1] && date.Day == _dates[0];
        }
        //public override string ToString()
        //{
        //    string output;
        //    if (Time.HasValue)
        //    {
        //        return 
        //    }
        //    return output;
        //}
    }
}
