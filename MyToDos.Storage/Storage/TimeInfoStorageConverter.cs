using MyToDos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Storage
{
    static class TimeInfoStorageConverter
    {
        public static TimeInfo Parse(string s)
        {
            if (s.Length == 8)
            {
                return new TimeInfo(TimeSpan.ParseExact(s.Substring(0, 4), "hhmm", null),
                    TimeSpan.ParseExact(s.Substring(4, 4), "hhmm", null));
            }
            else if (s.Length == 4)
            {
                return new TimeInfo(TimeSpan.ParseExact(s, "hhmm", null), true);
            }
            else if (s.Length == 5)
            {
                return new TimeInfo(TimeSpan.ParseExact(s, "\\~hhmm", null));
            }
            else return new TimeInfo();
        }
        public static string ToString(TimeInfo timeInfo)
        {
            if(timeInfo.Type == TimeInfoType.Schedule)
            {
                return timeInfo.StartTime.Value.ToString("hhmm") + timeInfo.FinishTime.Value.ToString("hhmm");
            }
            else if(timeInfo.Type == TimeInfoType.Alarm)
            {
                return timeInfo.StartTime.Value.ToString("hhmm");
            }
            else //if(timeInfo.Type == TimeInfoType.None)
            {
                if (timeInfo.Duration == TimeSpan.Zero)
                    return "";
                else return timeInfo.Duration.ToString("\\~hhmm");
            }
        }
    }
}
