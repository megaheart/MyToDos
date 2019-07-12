﻿using System;
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
        public class SQLType
        {
            public SQLType(string tableName, bool isRecyclable = false)
            {
                TableName = tableName;
                IsRecyclable = isRecyclable;
            }
            public string TableName { get; private set; }
            public bool IsRecyclable { get; private set; }
        }
        private SQLType _task = new SQLType("Tasks", true);
        internal SQLType Task
        {
            get
            {
                return _task;
            }
        }
        private SQLType _note = new SQLType("Notes");
        internal SQLType Note
        {
            get
            {
                return _note;
            }
        }
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
        internal void Update(SQLType type, string id, string property, string newValue)
        {
            string cmd = String.Format("UPDATE {0} SET {2} = '{3}' WHERE ID = '{1}';", type.TableName, id, property, newValue);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void Remove(SQLType type, string id)
        {
            if (type.IsRecyclable) throw new Exception("Can't remove a IRecyclable type, please use SQL.MoveToGarbage method");
            string cmd = String.Format("DELETE FROM {0} WHERE ID = '{1}';", type.TableName, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void MoveToGarbage(SQLType type, string id)
        {
            if (!type.IsRecyclable) throw new Exception("Can't move it to garbage, please use SQL.Remove method");
            string cmd = String.Format("INSERT INTO {0}Garbage SELECT * FROM {0} WHERE ID = '{1}';DELETE FROM {0} WHERE ID = '{1}';", 
                type.TableName, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void RemoveFromGarbage(SQLType type, string id)
        {
            string cmd = String.Format(@"DELETE FROM {0}Garbage WHERE ID = '{1}';", type.TableName, id);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void RestoreFromGarbage(SQLType type, string id)
        {
            string cmd = String.Format(@"INSERT INTO {0} SELECT * FROM {0}Garbage WHERE ID = '{1}';DELETE FROM {0}Garbage WHERE ID = '{1}';", 
                type.TableName, id);
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
            sQLiteCommand.CommandText = "SELECT Note FROM Notes WHERE ID = '" + id + "';";
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            string output = reader["Note"].ToString();
            _sQLite.Close();
            return output;
        }
        private static bool AlwaysTrue(object o) => true;
        internal async System.Threading.Tasks.Task<ObservableCollection<Task>> GetTaskList(Predicate<Task> predicate = null, bool isGarbage = false)
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
                string title = reader[0].ToString();
                string ID = reader[1].ToString();
                Repeater repeater = RepeaterStorageConverter.Parse(reader[2].ToString());
                DateTime activatedTime = reader.GetDateTime(3);
                DateTime expiryTime = reader.GetDateTime(4);
                TimeInfo time = TimeInfoStorageConverter.Parse(reader[5].ToString());
                Task task = new Task(title, ID, repeater, activatedTime, expiryTime, time);
                if (predicate(task)) tasks.Add(task);
            }
            _sQLite.Close();
            return tasks;
        }
        internal void InsertToDoComment(ToDoComment comment)
        {
            string cmd = String.Format(@"INSERT INTO ToDoComments(Time, Content) VALUES (DATE('{0}'), '{1}');",
                comment.Time, comment.Content);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        ///<summary>
        /// typeparameter T must be <c>ToDoComment</c>
        ///</summary>
        internal void UpdateToDoCommand(string id, ToDoComment comment)
        {
            string cmd = String.Format("UPDATE ToDoComments SET Content = '{0}' WHERE ID = '{1}' AND Time = DATE('{2}');", 
                comment.Content, id, comment.Time);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal void RemoveToDoComment(string id, ToDoComment comment)
        {
            string cmd = String.Format(@"DELETE FROM ToDoComments WHERE ID = '{0}' AND Time = DATE('{1}');",
                id, comment.Time);
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal async System.Threading.Tasks.Task<ObservableCollection<ToDoComment>> GetCommentList(string id, DateTime startTime, DateTime lastTime)
        {
            ObservableCollection<ToDoComment> comments = new ObservableCollection<ToDoComment>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = String.Format(@"SELECT Time,Content FROM ToDoComments WHERE ID = '{0}' AND Time BETWEEN 
                DATE('{1}') AND DATE('{2}');", id, startTime.ToString("yyyy-MM-dd HH:mm"), startTime.ToString("yyyy-MM-dd HH:mm"));
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string content = reader[1].ToString();
                DateTime time = reader.GetDateTime(0);
                comments.Add(new ToDoComment(time, content));
            }
            _sQLite.Close();
            return comments;
        }
    }
}
