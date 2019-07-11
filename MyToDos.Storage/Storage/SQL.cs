using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Security;
using MyToDos.Model;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace MyToDos.Storage
{
    internal class SQL
    {
        //private string _dataSourceAddress;
        //public string Password
        //{
        //    set
        //    {
        //        CONNECTION_STRING = "Data Source=" + _dataSourceAddress + ";Version=3;Password=" + value + ";";
        //    }
        //}
        private SQLiteConnection _sQLite;
        internal SQL(string dataSourceAddress, string password)
        {
            //_dataSourceAddress = dataSourceAddress;
            _sQLite = new SQLiteConnection("Data Source = " + dataSourceAddress + "; Version = 3;");
        }
        internal void Insert(Task task)
        {
            string cmd = String.Format(@"INSERT INTO tasks(Title, ID, Repeater, ActivatedTime, ExpiryTime, Time) VALUES 
                                        ('{0}', '{1}', '{2}', Date('{3}'), Date('{4}', '{5}'));",
                task.Title, task.ID, RepeaterStorageConverter.ToString(task.Repeater), task.ActivatedTime.ToString("yyyy-MM-dd HH:mm"), 
                task.ExpiryTime.ToString("yyyy-MM-dd HH:mm"), TimeInfoStorageConverter.ToString(task.Time));
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void Update<T>(string id, string property, string newValue)
        {
            string dataBase = typeof(T).Name + "s";
            string cmd = String.Format("UPDATE {0} SET {2} = '{3}' WHERE {1} = 0;", dataBase, id, property, newValue);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void Remove<T>(string id)
        {
            string dataBase = typeof(T).Name + "s";
            string cmd = String.Format("DELETE FROM {0} WHERE ID = {1};", dataBase, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void MoveToGarbage<T>(string id)
            where T:IRecyclable
        {
            string dataBase = typeof(T).Name + "s";
            string cmd = String.Format("INSERT INTO {0}Garbage SELECT * FROM {0} WHERE id = {1};DELETE FROM {0} WHERE ID = {1};", 
                dataBase, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void RemoveFromGarbage<T>(string id)
            where T : IRecyclable
        {
            string garbage = typeof(T).Name + "sGarbage";
            string cmd = String.Format(@"DELETE FROM {0} WHERE ID = {1};", garbage, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void RestoreFromGarbage<T>(string id)
            where T : IRecyclable
        {
            string dataBase = typeof(T).Name + "s";
            string cmd = String.Format(@"INSERT INTO {0} SELECT * FROM {0}Garbage WHERE id = {1};DELETE FROM {0}Garbage WHERE ID = {1};", 
                dataBase, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal async System.Threading.Tasks.Task ExecuteQuery(string cmd)
        {
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = cmd;
            await sQLiteCommand.ExecuteNonQueryAsync();
            _sQLite.Close();
        }
        internal async System.Threading.Tasks.Task<string> GetNoteText(string id)
        {
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = "SELECT Note FROM Notes WHERE ID = " + id + ";";
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            string output = reader["Note"].ToString();
            _sQLite.Close();
            return output;
        }
        private static bool AlwaysTrue(object o) => true;
        internal async System.Threading.Tasks.Task<ObservableCollection<Task>> GetTaskList(Predicate<Task> predicate, bool isGarbage = false)
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            if (isGarbage)
            {
                sQLiteCommand.CommandText = "SELECT * FROM TasksGarbage;";
            }
            else
            {
                sQLiteCommand.CommandText = "SELECT * FROM Tasks;";
            }
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            if (predicate == null) predicate = AlwaysTrue;
            while (await reader.ReadAsync())
            {
                string title = reader.GetString(0);
                string ID = reader[1].ToString();
                Repeater repeater = RepeaterStorageConverter.Parse(reader.GetString(2));
                DateTime activatedTime = reader.GetDateTime(3);
                DateTime expiryTime = reader.GetDateTime(4);
                TimeInfo time = TimeInfoStorageConverter.Parse(reader.GetString(5));
                Task task = new Task(title, ID, repeater, activatedTime, expiryTime, time);
                if (predicate(task)) tasks.Add(task);
            }
            _sQLite.Close();
            return tasks;
        }
    }
}
