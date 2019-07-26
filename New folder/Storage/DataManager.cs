using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyToDos.Model;

namespace MyToDos.Storage
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
        private static string AppFolder = System.AppDomain.CurrentDomain.BaseDirectory;
        private static string ResourcesFolder = AppFolder + @"Resources/";
        private static string DataBaseFile = ResourcesFolder + "mega.db";
        private static string DailyWeatherFile = ResourcesFolder + "dailyweather.json";
        private static string CurrentWeatherFile = ResourcesFolder + "currentweather.json";
        public DataManager(){}
        public async System.Threading.Tasks.Task Initialize()
        {
            _sQL = new SQL(ResourcesFolder);
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
