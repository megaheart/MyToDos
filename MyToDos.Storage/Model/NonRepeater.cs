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
            _dates = null;
            Type = RepeaterType.NonRepeater;
            Mode = RepeaterMode.Once;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }
        public override Repeater Clone()
        {
            NonRepeater repeater = this;
            return repeater;
        }
    }
}
