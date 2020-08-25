using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class CustomRepeat : Repeat
    {
        public CustomRepeat(int repeatEveryNDays)
        {
            int days = (int) checked(DateTime.Now.Ticks / 864000000000);
            _dates = new int[] { days, repeatEveryNDays };
            Type = RepeatType.Custom;
            Mode = RepeatMode.MultiTimes;
        }
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
