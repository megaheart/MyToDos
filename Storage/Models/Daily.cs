using System;

namespace Storage.Model
{
    public class Daily : Repeat
    {
        public Daily()
        {
            _dates = null;
            Type = RepeatType.Daily;
            Mode = RepeatMode.MultiTimes;
        }
        internal override bool IsUsableOn(DateTime date)
        {
            return true;
        }
        public override Repeat Clone()
        {
            return this;
        }
    }
}
