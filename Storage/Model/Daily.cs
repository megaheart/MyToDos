using System;

namespace Storage.Model
{
    public class Daily : Repeater
    {
        public Daily()
        {
            _dates = null;
            Type = RepeaterType.Daily;
            Mode = RepeaterMode.MultiTimes;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return true;
        }
        public override Repeater Clone()
        {
            return this;
        }
    }
}
