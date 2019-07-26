using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Once : Repeater
    {
        public Once(DateTime date)
        {
            Type = RepeaterType.Once;
            Mode = RepeaterMode.MultiTimes;
            _dates = new int[] { date.Day, date.Month, date.Year };
        }
        public Once(int[] date)
        {
            Type = RepeaterType.Once;
            Mode = RepeaterMode.MultiTimes;
            _dates = date;
        }
        internal void SetDate(DateTime date)
        {
            _dates = new int[] { date.Day, date.Month, date.Year };
            OnRepeaterInfoChanged();
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return date.Year == _dates[2] && date.Month == _dates[1] && date.Day == _dates[0];
        }
        private Once() { }
        public override Repeater Clone()
        {
            Once repeater = new Once();
            repeater._dates = new int[3];
            Array.Copy(this._dates, repeater._dates, _dates.Length);
            repeater._type = this._type;
            repeater._mode = this._mode;
            return repeater;
        }
    }
}
