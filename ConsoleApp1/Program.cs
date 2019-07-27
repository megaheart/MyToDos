using Storage;
using Storage.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static ExceptionTest test = new ExceptionTest();
        static void Main(string[] args)
        {
            test.DataManager_InitializeTest();
        }
    }
    class ExceptionTest
    {
        public void DataManager_InitializeTest()
        {
            SQL sQL = new SQL(DataManager.DataBaseFile);
            ObservableCollection<Tag> Tags = sQL.GetTagList().Result;
            SQL_GetTaskListTest(null, false, Tags);
        }
        public ObservableCollection<Task> SQL_GetTaskListTest(Predicate<Task> predicate, bool isGarbage, ObservableCollection<Tag> tagList)
        {
            SQLiteConnection _sQLite = new SQLiteConnection("Data Source = " + DataManager.DataBaseFile + "; Version = 3;");
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            _sQLite.OpenAsync().Wait();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            if (isGarbage)
            {
                sQLiteCommand.CommandText = "SELECT * FROM TasksGarbage;";
            }
            else
            {
                sQLiteCommand.CommandText = "SELECT * FROM Tasks;";
            }
            DbDataReader reader = sQLiteCommand.ExecuteReaderAsync().Result;
            if (predicate == null) predicate = o => true; ;
            while (reader.ReadAsync().Result)
            {
                string title = reader[0].ToString();
                string ID = reader[1].ToString();
                Repeater repeater = RepeaterStorageConverter.Parse(reader[2].ToString());
                DateTime activatedTime = reader.GetDateTime(3);
                DateTime expiryTime = reader.GetDateTime(4);
                ObservableCollection<TimeInfo> time = TimeInfosStorageConverter.Parse(reader[5].ToString());
                Tag[] tags = Array.ConvertAll<string, Tag>(reader[6].ToString().Split(','), x => tagList.First(y => y.ID == x));
                string webAddress = reader[7].ToString();
                Task task = new Task(title, ID, repeater, activatedTime, expiryTime, time, new ObservableCollection<Tag>(tags), webAddress);
                if (predicate(task)) tasks.Add(task);
            }
            _sQLite.Close();
            return tasks;
        }
    }
}
