using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Monthly : Repeater
    {
        /// <param name="dates">items must have value from 1 to 31</param>
        public Monthly(int[] dates)
        {
            _dates = dates;
            Type = RepeaterType.Monthly;
            Mode = RepeaterMode.MultiTimes;
        }
        internal void SetDates(int[] dates)
        {
            _dates = dates;
            OnRepeaterInfoChanged();
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return _dates.Contains((int)date.DayOfWeek);
        }
        private Monthly() { }
        public override Repeater Clone()
        {
            Monthly repeater = new Monthly();
            repeater._dates = new int[_dates.Length];
            Array.Copy(this._dates, repeater._dates, _dates.Length);
            repeater._type = this._type;
            repeater._mode = this._mode;
            return repeater;
        }
    }
}
