using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Storage.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using t = System.Threading.Tasks;

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
        private SQL _sQL;
        public DataManager()
        {
            _sQL = new SQL(DataBaseFile);
        }
        public async t.Task Initialize()
        {
            Tags = new SQLCollection<Tag>(await _sQL.GetTagListAsync());
            var getTaskList = _sQL.GetTaskListAsync("True", Tags);
            var getDailyWeathersData = GetDailyWeathersDataAsync();
            Tags.SQLInsertsItem += InsertTag;
            Tags.SQLRemovesItem += RemoveTag;
            Tags.SQLUpdatesItem += EditTag;
            Tasks = new SQLCollection<Task>(await getTaskList);
            Tasks.SQLInsertsItem += InsertTask;
            Tasks.SQLRemovesItem += MoveTaskToGarbage;
            Tasks.SQLUpdatesItem += EditTask;
            DailyWeathers = await getDailyWeathersData;
        }
        public SQLCollection<Tag> Tags { get; private set; }
        public void InsertTag(SQLCollectionChangedArgs e)
        {
            _sQL.InsertAsync(e.Item as Tag);
        }
        public void RemoveTag(SQLCollectionChangedArgs e)
        {
            Tag _tag = e.Item as Tag;
            foreach(var i in Tasks)
            {
                i.Tags.Remove(_tag);
            }
            _sQL.RemoveAsync(SQL.Tag, _tag.ID);
        }
        public void EditTag(SQLCollectionChangedArgs e)
        {
            _sQL.UpdateAsync(SQL.Tag, e.Item.ID, e.Property, e.NewValue);
        }
        public SQLCollection<Task> Tasks { get; private set; }
        public void InsertTask(SQLCollectionChangedArgs e)
        {
            _sQL.InsertAsync(e.Item as Task);
        }
        public void MoveTaskToGarbage(SQLCollectionChangedArgs e)
        {
            _sQL.RemoveAsync(SQL.Task, e.Item.ID);
        }
        public void EditTask(SQLCollectionChangedArgs e)
        {
            _sQL.UpdateAsync(SQL.Task, e.Item.ID, e.Property, e.NewValue);
        }
        public ObservableCollection<DailyWeatherInformation> DailyWeathers { get; private set; }
        private async t.Task<ObservableCollection<DailyWeatherInformation>> GetDailyWeathersDataAsync()
        {
            FileStream file = File.Open(DailyWeatherFile, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(file);
            var json = await reader.ReadToEndAsync();
            reader.Close();
            file.Close();
            return JsonConvert.DeserializeObject<ObservableCollection<DailyWeatherInformation>>(json);
        }
        public async t.Task UpdateDailyWeathersAsync(string json)
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
        public async t.Task<SQLGarbageCollection<Task>> GetGarbageTasksAsync()
        {
            var garbageTasks = new SQLGarbageCollection<Task>(await _sQL.GetGarbageTaskCollectionAsync());
            garbageTasks.SQLClearsAllItems += GarbageTasksClearsAllItems;
            garbageTasks.SQLRemovesItem += GarbageTasksRemovesItem;
            garbageTasks.SQLRestoresItem += GarbageTasksRestoresItem;
            return garbageTasks;
        }

        private async void GarbageTasksRestoresItem(SQLCollectionChangedArgs e)
        {
            await _sQL.RestoreFromGarbageAsync(SQL.Task, e.Item.ID);
            List<Task> tasks = await _sQL.GetTaskListAsync("ID=" + e.Item.ID, Tags);
            Tasks.RestoreItem(tasks[0]);
        }

        private void GarbageTasksRemovesItem(SQLCollectionChangedArgs e)
        {
            _sQL.RemoveFromGarbageAsync(SQL.Task, e.Item.ID).ContinueWith(t=> { if (t.IsFaulted) throw t.Exception; });
        }

        private void GarbageTasksClearsAllItems()
        {
            _sQL.RemoveAllFromGarbageAsync(SQL.Task).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; }); ;
        }
        public async t.Task<string> GetNoteTextAsync(NoteTaking o)
        {
            if (o.HasNote)
                return await _sQL.GetStringPropertyAsync(SQL.Note, "ID='" + o.ID + "'", "Content");
            return "";
        }
        public async t.Task SetNoteTextAsync(NoteTaking o, string content)
        {
            if (o.HasNote)
            {
                if (content == "" || content == null)
                {
                    await _sQL.RemoveAsync(SQL.Note, o.ID);
                    o.HasNote = false;
                }
                else
                    await _sQL.UpdateAsync(SQL.Note, o.ID, "Content", content);
            }
            else
            {
                await _sQL.InsertNoteAsync(o.ID, content);
                o.HasNote = true;
            }
        }
        public async t.Task<NoteInfo[]> GetExistExtendedNoteInfosAsync()
            => await _sQL.GetExistExtendedNoteInfosAsync();
        public async t.Task<string> GetExtentedNoteTextAsync(NoteTaking o, DateTime time)
            => await _sQL.GetStringPropertyAsync(SQL.ExtendedNote, "ID='" + o.ID + "' AND " + "Time='" + time.ToString("yyyy-MM-dd HH:mm") + "'", "Content");
        public async t.Task SetExtentedNoteTextAsync(NoteTaking o, DateTime time, string content)
        {
            await _sQL.SetStringPropertyAsync(SQL.ExtendedNote, "ID='" + o.ID + "' AND " + "Time='" + time.ToString("yyyy-MM-dd HH:mm") + "'", "Content", content);
        }
        public async t.Task RemoveExtendedNoteAsync(NoteTaking o, DateTime time)
        {
            await _sQL.RemoveAsync(SQL.Note, o.ID, "Time='" + time.ToString("yyyy-MM-dd HH:mm") + "'");
        }
    }
}
