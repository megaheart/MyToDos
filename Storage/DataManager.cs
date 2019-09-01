using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Storage.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

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
            DailyWeathers = new ObservableCollection<DailyWeatherInformation>();
        }
        private SQL _sQL;
        
        public ObservableCollection<Tag> Tags { get; private set; }
        public void InsertTag(Tag tag)
        {
            //SaveData
            _sQL.Insert(tag);
            //Update Model (UI)
            Tags.Add(tag);
        }
        public void RemoveTag(Tag tag)
        {
            //SaveData
            _sQL.Remove(SQL.Tag, tag.ID);
            //Update Model (UI)
            Tags.Remove(tag);
        }
        public void EditTag<T>(Tag tag, string property, T value)
        {
            //SaveData
            _sQL.Update(SQL.Tag, tag.ID, property, value.ToString());
            //Update Model (UI)
            typeof(Tag).GetProperty(property).SetValue(tag, value);
            throw new Exception("Building");
        }
        public ObservableCollection<Task> Tasks { get; private set; }
        public void InsertTask(Task task)
        {
            //SaveData
            _sQL.Insert(task);
            //Update Model (UI)
            Tasks.Add(task);
        }
        public void MoveTaskToGarbage(Task task)
        {
            //SaveData
            _sQL.MoveToGarbage(SQL.Task, task.ID);
            //Update Model (UI)
            Tasks.Remove(task);
            if (_garbageTasks != null) _garbageTasks.Add(task);
        }
        public void EditTask(Task task, string property, string value)
        {
            //SaveData
            _sQL.Update(SQL.Task, task.ID, property, value);
            //Update Model (UI)
            typeof(Task).GetProperty(property).SetValue(task, value);
            if(property == "Time")
            {

                
            }
            throw new Exception("Building");
        }
        public void EditTask(Task task, string property, DateTime value)
        {
            //SaveData
            _sQL.Update(SQL.Tag, task.ID, property, value);
            //Update Model (UI)
            typeof(Task).GetProperty(property).SetValue(task, value);
            throw new Exception("Building");
        }
        public ObservableCollection<DailyWeatherInformation> DailyWeathers { get; private set; }
        public async System.Threading.Tasks.Task UpdateDailyWeathers(string json)
        {
            JObject jObject = JObject.Parse(json);
            JArray array = jObject.Value<JArray>("DailyForecasts");
            DailyWeathers.Clear();
            foreach (var i in array)
            {
                DailyWeathers.Add(WeatherInfoConverter.GetDailyWeatherInformation(i));
            }
            FileStream file = File.OpenWrite(DailyWeatherFile);
            StreamWriter writer = new StreamWriter(file);
            string dailyweathersJson = JsonConvert.SerializeObject(DailyWeathers);
            await writer.WriteAsync(dailyweathersJson);
            writer.Close();
            file.Close();
        }
        //public ObservableCollection<WeatherInformation> Weathers12Hours { get; private set; }
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
        public void RestoreTaskFromGarbage(Task task)
        {
            //SaveData
            _sQL.RestoreFromGarbage(SQL.Task, task.ID);
            //Update Model (UI)
            _garbageTasks.Remove(task);
            Tasks.Add(task);
        }
        public void RemoveTaskFromGarbage(Task task)
        {
            //SaveData
            _sQL.RemoveFromGarbage(SQL.Task, task.ID);
            //Update Model (UI)
            _garbageTasks.Remove(task);
        }
        public async System.Threading.Tasks.Task<string> GetNoteText(INoteTaking o)
            => await _sQL.GetStringProperty(SQL.Note, "ID=" + o.ID, "Content");
        public void SetNoteText(INoteTaking o, string content)
        {
            if (o.HasNote)
            {
                if (content == "" || content == null)
                    _sQL.Remove(SQL.Note, o.ID);
                else
                    _sQL.Update(SQL.Note, o.ID, "Content", content);
            }
            else
            {
                _sQL.InsertNote(o.ID, content);
            }
        }
        public async System.Threading.Tasks.Task<string> GetExtentedNoteText(INoteTaking o, DateTime time)
            => await _sQL.GetStringProperty(SQL.ExtendedNote, "ID=" + o.ID + " AND " + "Time='" + time.ToString("yyyy-MM-dd HH:mm") + "'", "Content");
        public void SetExtentedNoteText(INoteTaking o, DateTime time, string content)
        {
            if (o.HasNote)
            {
                if (content == "" || content == null)
                    _sQL.Remove(SQL.Note, o.ID);
                else
                    _sQL.SetStringProperty(SQL.ExtendedNote, "ID=" + o.ID + " AND " + "Time='" + time.ToString("yyyy-MM-dd HH:mm") + "'", "Content", content);
            }
            else
            {
                _sQL.InsertExtendedNote(o.ID, time, content);
            }
        }
    }
}
