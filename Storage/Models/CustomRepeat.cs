using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class CustomRepeat : Repeat
    {
        /// <summary>
        /// Constructor of <seealso cref="CustomRepeat"/>
        /// </summary>
        /// <param name="milestone"></param>
        /// <param name="repeatEveryNDays"></param>
        public CustomRepeat(DateTime milestone, int repeatEveryNDays)
        {
            int _milestone = (int) checked(milestone.Date.Ticks / 864000000000);
            _dates = new int[] { _milestone, repeatEveryNDays };
            Type = RepeatType.Custom;
            Mode = RepeatMode.MultiTimes;
        }
        /// <summary>
        /// Constructor of <seealso cref="CustomRepeat"/>. Be careful when using it!
        /// </summary>
        /// <param name="dates"></param>
        public CustomRepeat(int[] dates)
        {
            _dates = dates;
            Type = RepeatType.Custom;
            Mode = RepeatMode.MultiTimes;
        }
        private CustomRepeat(){}
        public override Repeat Clone()
        {
            CustomRepeat Repeat = new CustomRepeat();
            Repeat._dates = new int[2];
            Array.Copy(this._dates, Repeat._dates, _dates.Length);
            Repeat._type = this._type;
            Repeat._mode = this._mode;
            return Repeat;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return (date.Date.Ticks / 864000000000 - _dates[0]) % _dates[1] == 0;
        }
    }
}
