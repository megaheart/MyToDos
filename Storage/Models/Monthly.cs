using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Monthly : Repeat
    {
        /// <param name="dates">items must have value from 1 to 31</param>
        public Monthly(int[] dates)
        {
            _dates = dates;
            Type = RepeatType.Monthly;
            Mode = RepeatMode.MultiTimes;
        }
        internal void SetDates(int[] dates)
        {
            _dates = dates;
            OnRepeatInfoChanged();
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return _dates.Contains((int)date.DayOfWeek);
        }
        private Monthly() { }
        public override Repeat Clone()
        {
            Monthly Repeat = new Monthly();
            Repeat._dates = new int[_dates.Length];
            Array.Copy(this._dates, Repeat._dates, _dates.Length);
            Repeat._type = this._type;
            Repeat._mode = this._mode;
            return Repeat;
        }
    }
}
