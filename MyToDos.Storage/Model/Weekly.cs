using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Weekly : Repeater
    {
        /// <param name="dates">Sun,Mon,Tue,...,Sat -> 0,1,2,...,6</param>
        public Weekly(int[] dates)
        {
            _dates = dates;
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        public Weekly(DayOfWeek[] dates)
        {
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            Type = RepeaterType.Weekly;
            Mode = RepeaterMode.MultiTimes;
        }
        internal void SetDates(int[] dates)
        {
            _dates = dates;
            OnRepeaterInfoChanged();
        }
        internal void SetDates(DayOfWeek[] dates)
        {
            _dates = Array.ConvertAll(dates, IntFromDayOfWeek);
            OnRepeaterInfoChanged();
        }
        static int IntFromDayOfWeek(DayOfWeek day) => (int)day;
        internal override bool IsUsableOn(DateTime date)
        {
            return _dates.Contains((int)date.DayOfWeek);
        }
        private Weekly() { }
        public override Repeater Clone()
        {
            Weekly repeater = new Weekly();
            repeater._dates = new int[_dates.Length];
            Array.Copy(this._dates, repeater._dates, _dates.Length);
            repeater._type = this._type;
            repeater._mode = this._mode;
            return repeater;
        }
    }
}
