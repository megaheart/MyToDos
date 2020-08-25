using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class NonRepeat : Repeat
    {
        public NonRepeat()
        {
            _dates = null;
            Type = RepeatType.NonRepeat;
            Mode = RepeatMode.Once;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }
        public override Repeat Clone()
        {
            NonRepeat Repeat = this;
            return Repeat;
        }
    }
}
