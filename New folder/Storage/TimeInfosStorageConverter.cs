using MyToDos.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Storage
{
    static class TimeInfosStorageConverter
    {
        public static ObservableCollection<TimeInfo> Parse(string s)
        {
            ObservableCollection<TimeInfo> ouput = new ObservableCollection<TimeInfo>();
            string[] list = s.Split('|');
            for(int i = 1; i < s.Length; i += 2)
            {
                TimeSpan? activeTimeOfDay = null;
                if (list[i - 1].Length != 0) activeTimeOfDay = TimeSpan.ParseExact(list[i - 1], "hhmm", null);
                TimeSpan? limit = null;
                if (list[i].Length != 0) limit = TimeSpan.ParseExact(list[i], "hhmm", null);
                ouput.Add(new TimeInfo(activeTimeOfDay, limit));
            }
            return ouput;
        }
        public static string ToString(ObservableCollection<TimeInfo> timeInfos)
        {
            if(timeInfos.Count > 0)
            {
                string output;
                if (timeInfos[0].ActiveTimeOfDay.HasValue) output = timeInfos[0].ActiveTimeOfDay.Value.ToString("hhmm");
                else output = "";
                if (timeInfos[0].Limit.HasValue) output += '|' + timeInfos[0].Limit.Value.ToString("hhmm");
                else output += '|';
                for (int i = 1; i < timeInfos.Count; i ++)
                {
                    if (timeInfos[i].ActiveTimeOfDay.HasValue) output += timeInfos[i].ActiveTimeOfDay.Value.ToString("hhmm");
                    if (timeInfos[i].Limit.HasValue) output += '|' + timeInfos[i].Limit.Value.ToString("hhmm");
                    else output += '|';
                }
                return output;
            }
            return "";
        }
    }
}
