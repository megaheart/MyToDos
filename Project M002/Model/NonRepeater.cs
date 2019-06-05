using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class NonRepeater : Repeater
    {
        public NonRepeater()
        {
            Time = null;
            Duration = TimeSpan.Zero;
            _dates = null;
            Type = RepeaterType.NonRepeater;
            Mode = RepeaterMode.Once;
        }
        public override bool IsUsableOn(DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }
    }
}
