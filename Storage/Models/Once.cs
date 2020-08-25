using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Once : Repeat
    {
        public Once(DateTime date)
        {
            Type = RepeatType.Once;
            Mode = RepeatMode.MultiTimes;
            _dates = new int[] { date.Day, date.Month, date.Year };
        }
        public Once(int[] date)
        {
            Type = RepeatType.Once;
            Mode = RepeatMode.MultiTimes;
            _dates = date;
        }
        internal void SetDate(DateTime date)
        {
            _dates = new int[] { date.Day, date.Month, date.Year };
            OnRepeatInfoChanged();
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return date.Year == _dates[2] && date.Month == _dates[1] && date.Day == _dates[0];
        }
        private Once() { }
        public override Repeat Clone()
        {
            Once Repeat = new Once();
            Repeat._dates = new int[3];
            Array.Copy(this._dates, Repeat._dates, _dates.Length);
            Repeat._type = this._type;
            Repeat._mode = this._mode;
            return Repeat;
        }
    }
}
