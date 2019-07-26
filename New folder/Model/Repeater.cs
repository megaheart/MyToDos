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
        Once = 4,
        Custom = 5
    }
    public abstract class Repeater
    {
        public event Action RepeaterInfoChanged;
        protected int[] _dates;
        internal int[] Dates { get => _dates; }
        protected RepeaterType _type;
        public RepeaterType Type
        {
            get => _type;
            internal set {
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
            internal set
            {
                if (value != _mode)
                {
                    _mode = value;
                    RepeaterInfoChanged?.Invoke();
                }
            }
        }
        protected void OnRepeaterInfoChanged() => RepeaterInfoChanged?.Invoke();
        internal abstract bool IsUsableOn(DateTime date);
        public abstract Repeater Clone();
        
    }
}
