using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Daily : Repeater
    {
        public Daily()
        {
            Duration = TimeSpan.Zero;
            Time = null;
            _dates = null;
            Type = RepeaterType.Daily;
            Mode = RepeaterMode.MultiTimes;
        }
        public Daily(TimeSpan duration)
        {
            Duration = duration;
            Time = null;
            _dates = null;
            Type = RepeaterType.Daily;
            Mode = RepeaterMode.MultiTimes;
        }
        public Daily(TimeDuration time)
        {
            Time = time;
            _dates = null;
            Type = RepeaterType.Daily;
            Mode = RepeaterMode.MultiTimes;
        }
        public override bool IsUsableOn(DateTime date)
        {
            return true;
        }
        public override Repeater Clone()
        {
            Daily repeater = new Daily();
            repeater._duration = this._duration;
            repeater._time = this._time;
            return repeater;
        }
    }
}
