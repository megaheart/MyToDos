using Storage.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public static class TimeInfosStorageConverter
    {
        public static ObservableCollection<TimeInfo> Parse(string s)
        {
            ObservableCollection<TimeInfo> ouput = new ObservableCollection<TimeInfo>();
            string[] list = s.Split('|');
            for(int i = 0; i < list.Length; i ++)
            {
                ouput.Add(ParseToTimeInfo(list[i]));
            }
            return ouput;
        }
        public static string ToString(ObservableCollection<TimeInfo> timeInfos)
        {
            string output = "";
            foreach(var i in timeInfos)
            {
                output += ToString(i) + "|";
            }
            if (output.Length > 0) output = output.Remove(output.Length - 1, 1);
            return output;
        }
        public static TimeInfo ParseToTimeInfo(string s)
        {
            var list = s.Split(',');
            TimeSpan? activeTimeOfDay = null;
            if (list[0].Length != 0) activeTimeOfDay = TimeSpan.FromMinutes(int.Parse(list[0]));
            TimeSpan? limit = null;
            if (list.Length > 1) limit = TimeSpan.FromMinutes(int.Parse(list[1]));
            return new TimeInfo(activeTimeOfDay, limit);
        }
        public static string ToString(TimeInfo timeInfo/*, bool withoutDuration = false*/)
        {
            string output;
            if (timeInfo.ActiveTimeOfDay.HasValue) output = timeInfo.ActiveTimeOfDay.Value.TotalMinutes.ToString();
            else output = "";
            //if (withoutDuration) return output;
            if (timeInfo.Duration.HasValue) output += ',' + timeInfo.Duration.Value.TotalMinutes;
            return output;
        }
    }
}
