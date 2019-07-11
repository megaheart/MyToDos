using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public enum TimeInfoType
    {
        Schedule,
        Alarm,
        None
    }
    class TimeInfo
    {
        private static TimeSpan MaxValueOfDay = new TimeSpan(23, 59, 0);
        public TimeInfo()
        {
            _startTime = _finishTime = null;
            _duration = TimeSpan.Zero;
            Type = TimeInfoType.None;
        }
        public TimeInfo(TimeSpan startTime, TimeSpan finishTime)
        {
            if (startTime < TimeSpan.Zero || finishTime > MaxValueOfDay)
                throw new Exception("FinishTime or startTime must be between 0h and 23h59'");
            _duration = finishTime - startTime;
            if (_duration <= TimeSpan.Zero) throw new Exception("Can't set finishTime equal to or less then startTime");
            _startTime = startTime;
            _finishTime = finishTime;
            Type = TimeInfoType.Schedule;
        }
        public TimeInfo(TimeSpan duration)
        {
            if (_duration <= TimeSpan.Zero || _duration > MaxValueOfDay)
                throw new Exception("Duration must be more than TimeSpan.Zero");
            _duration = duration;
            _startTime = _finishTime = null;
            Type = TimeInfoType.None;
        }
        public TimeInfo(TimeSpan startTime, bool onlyHaveStartTime)
        {
            if (startTime < TimeSpan.Zero || startTime > MaxValueOfDay)
                throw new Exception("StartTime must be between 0h and 23h59'");
            _startTime = startTime;
            _duration = TimeSpan.Zero;
            _finishTime = null;
            Type = TimeInfoType.Alarm;
        }
        //public event Action TimeInfoChanged;
        public TimeInfoType Type { get; private set; }
        private TimeSpan? _startTime;
        public TimeSpan? StartTime {
            get => _startTime;
            //internal set
            //{
            //    if (_startTime != value)
            //    {}
            //}
        }
        private TimeSpan? _finishTime;
        public TimeSpan? FinishTime
        {
            get => _finishTime;
            //internal set
            //{

            //}
        }
        protected TimeSpan _duration;
        public TimeSpan Duration
        {
            //set
            //{
            //    if (_time.HasValue) throw new Exception("Can't set Duration when Time property has value");
            //    if (_duration != value)
            //    {
            //        _duration = value;
            //        RepeaterInfoChanged?.Invoke();
            //    }
            //}
            get => _duration;
        }
        public bool IsActive(TimeSpan now)
        {
            if (_startTime.HasValue)
            {
                if (_finishTime.HasValue) return _startTime <= now && _finishTime > now;
                else return _startTime <= now;
            }
            return true;
        }
    }
}
