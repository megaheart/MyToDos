using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Weekly : Repeat
    {
        /// <param name="dates">Sun,Mon,Tue,...,Sat -> 0,1,2,...,6</param>
        public Weekly(int[] dates)
        {
            _dates = dates;
            Type = RepeatType.Weekly;
            Mode = RepeatMode.MultiTimes;
        }
        public Weekly(DayOfWeek[] dates)
        {
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            Type = RepeatType.Weekly;
            Mode = RepeatMode.MultiTimes;
        }
        internal void SetDates(int[] dates)
        {
            _dates = dates;
            OnRepeatInfoChanged();
        }
        internal void SetDates(DayOfWeek[] dates)
        {
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            OnRepeatInfoChanged();
        }
        static int IntFromDayOfWeek(DayOfWeek day) => (int)day;
        internal override bool IsUsableOn(DateTime date)
        {
            return _dates.Contains((int)date.DayOfWeek);
        }
        private Weekly() { }
        public override Repeat Clone()
        {
            Weekly Repeat = new Weekly();
            Repeat._dates = new int[_dates.Length];
            Array.Copy(this._dates, Repeat._dates, _dates.Length);
            Repeat._type = this._type;
            Repeat._mode = this._mode;
            return Repeat;
        }
    }
}
