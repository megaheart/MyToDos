using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Security;


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
        internal void InsertTaskToDB(MyToDos.Model.Task task)
        {
            string cmd = String.Format(@"INSERT INTO tasks(Title, ID, Repeater, ActivatedTime, ExpiryTime) VALUES 
                                        ('{0}', '{1}', '{2}', 'Date({3})', 'Date({4})');",
                task.Title, task.ID, RepeaterStorageConverter.RepeaterToString(task.Repeater), 
                task.ActivatedTime.ToString("yyyy-MM-dd HH:mm"), task.ExpiryTime.ToString("yyyy-MM-dd HH:mm"));
            ExecuteQuery(cmd).ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
        }
        internal async Task ExecuteQuery(string cmd)
        {
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = cmd;
            await sQLiteCommand.ExecuteNonQueryAsync();
            _sQLite.Close();
        }
    }
}
