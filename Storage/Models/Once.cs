using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Once : Repeat
    {
        public Once()
        {
            _dates = null;
            Type = RepeatType.Once;
            Mode = RepeatMode.Once;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return date.Year == _dates[2] && date.Month == _dates[1] && date.Day == _dates[0];
        }
        public override Repeat Clone()
        {
            Once Repeat = new Once();
            return Repeat;
        }
    }
}
