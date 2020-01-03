using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class TimeInfo : NotifiableObject
    {
        private static TimeSpan MaxValueOfDay = new TimeSpan(23, 59, 0);
        public TimeInfo(TimeSpan? activeTimeOfDay, TimeSpan? duration)
        {
            _activeTimeOfDay = _duration = null;
            if (activeTimeOfDay.HasValue)
            {
                if (activeTimeOfDay.Value < TimeSpan.Zero || activeTimeOfDay.Value > MaxValueOfDay)
                    throw new Exception("activeTimeOfDay must be between 0h and 23h59'");
                _activeTimeOfDay = activeTimeOfDay;//new TimeSpan(activeTimeOfDay.Value.Hours, activeTimeOfDay.Value.Minutes, 0);
            }
            if (duration.HasValue)
            {
                _duration = duration;
            }
        }
        //public TimeInfoType Type { get; private set; }
        private TimeSpan? _activeTimeOfDay;
        public TimeSpan? ActiveTimeOfDay {
            set
            {
                if(value != _activeTimeOfDay)
                {
                    if (value.HasValue)
                    {
                        if (value.Value < TimeSpan.Zero || value.Value > MaxValueOfDay)
                            throw new Exception("value must be between 0h and 23h59'");
                        _activeTimeOfDay = value;//new TimeSpan(value.Value.Hours, value.Value.Minutes, 0);
                    }
                    else _activeTimeOfDay = null;
                    OnPropertyChanged("ActiveTimeOfDay");
                }
            }
            get => _activeTimeOfDay;
        }
        private TimeSpan? _duration;
        /// <summary>
        /// The maximum time a Task can be done
        /// </summary>
        public TimeSpan? Duration
        {
            set
            {
                if(_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
            get => _duration;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is TimeInfo)) return false;
            TimeInfo timeInfo = obj as TimeInfo;
            return this._activeTimeOfDay == timeInfo._activeTimeOfDay && this._duration == timeInfo._duration;
        }
        public static bool operator ==(TimeInfo info1, TimeInfo info2)
        {
            return info1._activeTimeOfDay == info2._activeTimeOfDay && info1._duration == info2._duration;
        }
        public static bool operator !=(TimeInfo info1, TimeInfo info2)
        {
            return info1._activeTimeOfDay != info2._activeTimeOfDay || info1._duration != info2._duration;
        }
    }
}
