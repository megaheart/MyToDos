using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Storage.Model;

namespace Storage
{
    public class DataManager
    {
        private static DataManager _current;
        public static DataManager Current {
            get
            {
                if (_current == null) _current = new DataManager();
                return _current;
            }
        }
        public static string AppFolder = System.AppDomain.CurrentDomain.BaseDirectory;
        public static string ResourcesFolder = AppFolder + @"Resources/";
        public static string DataBaseFile = ResourcesFolder + "mega.db";
        public static string DailyWeatherFile = ResourcesFolder + "dailyweather.json";
        public static string CurrentWeatherFile = ResourcesFolder + "currentweather.json";
        public DataManager()
        {
            _sQL = new SQL(DataBaseFile);
        }
        public async System.Threading.Tasks.Task Initialize()
        {
            Tags = await _sQL.GetTagList();
            Tasks = await _sQL.GetTaskList(null, false, Tags);
        }
        private SQL _sQL;
        public ObservableCollection<Tag> Tags { get; private set; }
        public ObservableCollection<Task> Tasks { get; private set; }
        public ObservableCollection<DailyWeatherInformation> DailyWeathers { get; private set; }
        public WeatherInformation CurrentWeather { get; private set; }
        private ObservableCollection<Task> _garbageTasks;
        public async System.Threading.Tasks.Task<ObservableCollection<Task>> GetGarbageTasks()
        {
            if(_garbageTasks == null)
            {
                _garbageTasks = await _sQL.GetTaskList(null, true, Tags);
            }
            return _garbageTasks;
        }
        public void DisposeGarbageTasks()
        {
            _garbageTasks = null;
            GC.Collect();
        }
        //public 
    }
}
