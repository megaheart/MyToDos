using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public enum RepeatMode
    {
        Once,
        MultiTimes
    }
    public enum RepeatType
    {
        Daily,
        Weekly,
        Monthly,
        Once,
        NonRepeater
    }
    public enum TaskType
    {
        Schedule,
        ToDo
    }
    public sealed class Repeater : NotifiableObject
    {
        protected int[] dates;
        private TimeDuration? _time;
        private TimeSpan _duration;
        public TimeDuration? Time
        {
            set
            {
                if (value != _time)
                {
                    if (value.HasValue && value.Value.FinishTime <= value.Value.StartTime) throw new Exception("Can't set finishTime equal to or less then startTime");
                    _time = value;
                    Duration = value.Value.FinishTime - value.Value.StartTime;
                    OnPropertyChanged("Time");
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
                    OnPropertyChanged("Duration");
                }
            }
            get => _duration;
        }
        public RepeatType Type { get; private set; }
        //public RepeatType 
        public TaskType TaskType { private set; get; }
        public bool IsUsableOn(DateTime date)
        {
            if (Type == RepeatType.Daily || Type == RepeatType.NonRepeater) return true;
            if(Type == RepeatType.Weekly)
        }
        public struct TimeDuration
        {
            public TimeSpan StartTime;
            public TimeSpan FinishTime;
            public static bool operator ==(TimeDuration t1, TimeDuration t2)
            {
                return (t1.StartTime == t2.StartTime) && (t1.FinishTime == t2.FinishTime);
            }
            public static bool operator !=(TimeDuration t1, TimeDuration t2)
            {
                return !((t1.StartTime == t2.StartTime) && (t1.FinishTime == t2.FinishTime));
            }
        }
        public static Repeater Parse(string encode)
        {

        }
        public static Repeater TryParse(string encode)
        {

        }
        public string Encode(string encode)
        {

        }
    }
}
