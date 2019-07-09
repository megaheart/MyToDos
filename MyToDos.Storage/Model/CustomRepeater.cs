using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    class CustomRepeater : Repeater
    {
        public CustomRepeater(int repeatEveryNDays)
        {
            int days = (int) checked(DateTime.Now.Ticks / 864000000000);
            _dates = new int[] { days, repeatEveryNDays };
            Type = RepeaterType.Custom;
            Mode = RepeaterMode.MultiTimes;
        }
        public CustomRepeater(int[] dates)
        {
            _dates = dates;
            Type = RepeaterType.Custom;
            Mode = RepeaterMode.MultiTimes;
        }
        private CustomRepeater(){}
        public override Repeater Clone()
        {
            CustomRepeater repeater = new CustomRepeater();
            repeater._dates = new int[2];
            Array.Copy(this._dates, repeater._dates, _dates.Length);
            repeater._type = this._type;
            repeater._mode = this._mode;
            return repeater;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return (date.Date.Ticks / 864000000000 - _dates[0]) % _dates[1] == 0;
        }
    }
}
