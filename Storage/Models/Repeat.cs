using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public enum RepeatMode
    {
        Once,
        MultiTimes
    }
    public enum RepeatType
    {
        //NonRepeat = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Once = 4,
        Custom = 5
    }
    public abstract class Repeat
    {
        public event Action RepeatInfoChanged;
        protected int[] _dates;
        internal int[] Dates { get => _dates; }
        protected RepeatType _type;
        public RepeatType Type
        {
            get => _type;
            internal set {
                if (value != _type)
                {
                    _type = value;
                    RepeatInfoChanged?.Invoke();
                }
            }
        }
        protected RepeatMode _mode;
        public RepeatMode Mode
        {
            get => _mode;
            internal set
            {
                if (value != _mode)
                {
                    _mode = value;
                    RepeatInfoChanged?.Invoke();
                }
            }
        }
        protected void OnRepeatInfoChanged() => RepeatInfoChanged?.Invoke();
        internal abstract bool IsUsableOn(DateTime date);
        public abstract Repeat Clone();
        
    }
}
