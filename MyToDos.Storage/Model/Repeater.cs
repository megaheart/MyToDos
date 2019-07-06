using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public enum RepeaterMode
    {
        Once,
        MultiTimes
    }
    public enum RepeaterType
    {
        NonRepeater = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Once = 4
    }
    public abstract class Repeater
    {
        public int Index
        {
            get
            {
                int index = 0;
                if (Time.HasValue) index = 2;
                else if (Duration != TimeSpan.Zero) index = 1;
                return ((int)Type) * 3 + index;
            }
        }
        public event Action RepeaterInfoChanged;
        protected int[] _dates;
        protected TimeDuration? _time;
        protected TimeSpan _duration;
        public TimeDuration? Time
        {
            set
            {
                if (value != _time)
                {
                    if (value.HasValue)
                    {
                        if (value.Value.FinishTime <= value.Value.StartTime) throw new Exception("Can't set finishTime equal to or less then startTime");
                        _time = value;
                        Duration = value.Value.FinishTime - value.Value.StartTime;
                        RepeaterInfoChanged?.Invoke();
                    }
                    else
                    {
                        _time = value;
                        Duration = TimeSpan.Zero;
                        RepeaterInfoChanged?.Invoke();
                    }
                }
            }
            get => _time;
        }
        public TimeSpan Duration
        {
            set
            {
                if (_time.HasValue) throw new Exception("Can't set Duration when Time property has value");
                if(_duration != value)
                {
                    _duration = value;
                    RepeaterInfoChanged?.Invoke();
                }
            }
            get => _duration;
        }
        protected RepeaterType _type;
        public RepeaterType Type
        {
            get => _type;
            set {
                if (value != _type)
                {
                    _type = value;
                    RepeaterInfoChanged?.Invoke();
                }
            }
        }
        protected RepeaterMode _mode;
        public RepeaterMode Mode
        {
            get => _mode;
            set
            {
                if (value != _mode)
                {
                    _mode = value;
                    RepeaterInfoChanged?.Invoke();
                }
            }
        }
        protected void OnRepeaterInfoChanged() => RepeaterInfoChanged?.Invoke();
        public abstract bool IsUsableOn(DateTime date);
        public abstract Repeater Clone();
        public struct TimeDuration
        {
            public TimeDuration(TimeSpan startTime, TimeSpan finishTime)
            {
                StartTime = startTime;
                FinishTime = finishTime;
            }
            public TimeSpan StartTime { get; private set; }
            public TimeSpan FinishTime { get; private set; }
            public static bool operator ==(TimeDuration t1, TimeDuration t2)
            {
                return (t1.StartTime == t2.StartTime) && (t1.FinishTime == t2.FinishTime);
            }
            public static bool operator !=(TimeDuration t1, TimeDuration t2)
            {
                return !((t1.StartTime == t2.StartTime) && (t1.FinishTime == t2.FinishTime));
            }
            public override bool Equals(object obj)
            {
                if(obj.GetType() == typeof(TimeDuration))
                {
                    return this == (TimeDuration)obj;
                }
                return false;
            }
        }
        //public abstract string ToDurationString();
    }
}
