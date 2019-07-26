using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class TimeInfo : NotifiableObject
    {
        private static TimeSpan MaxValueOfDay = new TimeSpan(23, 59, 0);
        public TimeInfo(TimeSpan? activeTimeOfDay, TimeSpan? limit)
        {
            _activeTimeOfDay = _limit = null;
            if (activeTimeOfDay.HasValue)
            {
                if (activeTimeOfDay.Value < TimeSpan.Zero || activeTimeOfDay.Value > MaxValueOfDay)
                    throw new Exception("activeTimeOfDay must be between 0h and 23h59'");
                _activeTimeOfDay = activeTimeOfDay;//new TimeSpan(activeTimeOfDay.Value.Hours, activeTimeOfDay.Value.Minutes, 0);
            }
            if (limit.HasValue)
            {
                _limit = limit;
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
        private TimeSpan? _limit;
        public TimeSpan? Limit
        {
            set
            {
                if(_limit != value)
                {
                    _limit = value;
                    OnPropertyChanged("Limit");
                }
            }
            get => _limit;
        }
    }
}
