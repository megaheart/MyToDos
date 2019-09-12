using Storage.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using t = System.Threading.Tasks;

namespace Storage
{
    /// <summary>
    /// SQL query for application
    /// </summary>
    public class SQL
    {
        //private string _dataSourceAddress;
        //public string Password
        //{
        //    set
        //    {
        //        CONNECTION_STRING = "Data Source=" + _dataSourceAddress + ";Version=3;Password=" + value + ";";
        //    }
        //}
        #region SQL Types
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
        private static SQLType _task = new SQLType("Tasks", true);
        public static SQLType Task
        {
            get
            {
                return _task;
            }
        }
        private static SQLType _note = new SQLType("Notes");
        public static SQLType Note
        {
            get
            {
                return _note;
            }
        }
        private static SQLType _tag = new SQLType("Tags", true);
        public static SQLType Tag
        {
            get
            {
                return _tag;
            }
        }
        private static SQLType _extendedNote = new SQLType("ExtendedNotes", true);
        public static SQLType ExtendedNote
        {
            get
            {
                return _extendedNote;
            }
        }
        #endregion
        private SQLiteConnection _sQLite;
        public SQL(string dataSourceAddress/*, string password*/)
        {
            //_dataSourceAddress = dataSourceAddress;
            _sQLite = new SQLiteConnection("Data Source = " + dataSourceAddress + "; Version = 3;");
            
        }
        #region General Methods
        public async t.Task ExecuteQueryAsync(string cmd)
        {
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = cmd;
            await sQLiteCommand.ExecuteNonQueryAsync();
            _sQLite.Close();
        }
        /// <summary>
        /// Execute many SQL commands
        /// </summary>
        /// <param name="cmds">list of commands</param>
        /// <param name="reportIndex">
        /// parameter of reportIndex is index of command which has just been executed
        /// </param>
        /// <returns></returns>
        public async t.Task ExecuteQueriesAsync(string[] cmds, Action<int> reportIndex)
        {
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            for(int i = 0; i < cmds.Length; ++i)
            {
                sQLiteCommand.CommandText = cmds[i];
                await sQLiteCommand.ExecuteNonQueryAsync();
                reportIndex(i);
            }
            _sQLite.Close();
        }
        public async t.Task UpdateAsync(SQLType type, string id, string property, string newValue)
        {
            string cmd = String.Format("UPDATE {0} SET {2} = '{3}' WHERE ID = '{1}';", type.TableName, id, property, newValue);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task UpdateAsync(SQLType type, string id, string property, DateTime newValue)
        {
            string cmd = String.Format("UPDATE {0} SET {2} = '{3}' WHERE ID = '{1}';",
                type.TableName, id, property, newValue.ToString("yyyy-MM-dd HH:mm:ss"));
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task RemoveAsync(SQLType type, string id)
        {
            //if (type.IsRecyclable) throw new Exception("Can't remove a IRecyclable type, please use SQL.MoveToGarbage method");
            string cmd = String.Format("DELETE FROM {0} WHERE ID='{1}';", type.TableName, id);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task RemoveAsync(SQLType type, string id, string anotherCondition)
        {
            //if (type.IsRecyclable) throw new Exception("Can't remove a IRecyclable type, please use SQL.MoveToGarbage method");
            string cmd = String.Format("DELETE FROM {0} WHERE ID='{1}' AND {2};", type.TableName, id, anotherCondition);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task MoveToGarbageAsync(SQLType type, string id)
        {
            //if (!type.IsRecyclable) throw new Exception("Can't move it to garbage, please use SQL.Remove method");
            string cmd = String.Format("INSERT INTO {0}Garbage SELECT * FROM {0} WHERE ID = '{1}';DELETE FROM {0} WHERE ID = '{1}';",
                type.TableName, id);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task RemoveFromGarbageAsync(SQLType type, string id)
        {
            //if (!type.IsRecyclable) throw new Exception("SQL.RemoveFromGarbage doesn't support unrecyclable type");
            string cmd = String.Format(@"DELETE FROM ExtendedNotes WHERE ID = '{1}';
                                        DELETE FROM Notes WHERE ID = '{1}';
                                        DELETE FROM {0}Garbage WHERE ID = '{1}';", type.TableName, id);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task RemoveAllFromGarbageAsync(SQLType type)
        {
            //if (!type.IsRecyclable) throw new Exception("SQL.RemoveAllFromGarbageAsync doesn't support unrecyclable type");
            string cmd = String.Format(@"
                        DELETE FROM ExtendedNotes WHERE ID IN (SELECT {0}.ID FROM {0} INNER JOIN ExtendedNotes ON {0}.ID = ExtendedNotes.ID);
                        DELETE FROM Notes WHERE ID IN (SELECT {0}.ID FROM {0} INNER JOIN Notes ON {0}.ID = Notes.ID);
                        DELETE FROM {0}Garbage;", type.TableName);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task RestoreFromGarbageAsync(SQLType type, string id)
        {
            //if (!type.IsRecyclable) throw new Exception("SQL.RestoreFromGarbage doesn't support unrecyclable type");
            string cmd = String.Format(@"INSERT INTO {0} SELECT * FROM {0}Garbage WHERE ID = '{1}';DELETE FROM {0}Garbage WHERE ID = '{1}';
                                        PRAGMA foreign_keys = 0;
                                        CREATE TABLE _{0} AS SELECT * FROM {0} ORDER BY LENGTH(ID),ID;
                                        DROP TABLE {0};
                                        CREATE TABLE {0} AS SELECT * FROM _{0};
                                        DROP TABLE _{0};
                                        PRAGMA foreign_keys = 1;",
                type.TableName, id);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task<string> GetStringPropertyAsync(SQLType type, string condition, string property)
        {
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            string queryCmd = "SELECT " + property + " FROM " + type.TableName + " WHERE " + condition + ';';
            sQLiteCommand.CommandText = queryCmd;
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            string output = reader[0].ToString();
            _sQLite.Close();
            return output;
        }
        public async t.Task SetStringPropertyAsync(SQLType type, string condition, string property, string newValue)
        {
            string cmd = String.Format("UPDATE {0} SET {1} = '{2}' WHERE {3};", type.TableName, property, newValue, condition);
            await ExecuteQueryAsync(cmd);
        }
        #endregion
        #region Task Query Method
        public async t.Task InsertAsync(Task task)
        {
            string tags;
            if (task.Tags.Count == 0) tags = "";
            else
            {
                tags = task.Tags[0].ID;
                for (var i = 1; i < task.Tags.Count; i++)
                {
                    tags += "," + task.Tags[i].ID;
                }
            }
            string cmd = String.Format(@"INSERT INTO tasks(Title, ID, Repeater, ActivatedTime, ExpiryTime, Time, Tags, WebAddress) VALUES 
                                        ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');",
                task.Title, task.ID, RepeaterStorageConverter.ToString(task.Repeater), task.ActivatedTime.ToString("yyyy-MM-dd HH:mm"),
                task.ExpiryTime.ToString("yyyy-MM-dd HH:mm"), TimeInfosStorageConverter.ToString(task.Time), tags, task.WebAddress);
            await ExecuteQueryAsync(cmd);
        }
        ///
        // use SQL.Update to update Task
        ///
        // use SQL.MoveToGarbage to remove Task
        /// 
        // use SQL.RestoreFromGarbage to restore Task
        ///
        // use SQL.RemoveFromGarbage to remove Task completely
        ///


        // <summary>
        // Query a list of tasks which predicate(task) returns true :)
        // </summary>
        // <param name="predicate">
        // The boolean function to filter Tasks
        // </param>
        // <returns></returns>
        //public async t.Task<List<Task>>
        //    GetTaskListAsync(Predicate<Task> predicate, List<Tag> tagList)
        //{
        //    List<Task> tasks = new List<Task>();
        //    await _sQLite.OpenAsync();
        //    SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
        //    sQLiteCommand.CommandText = "SELECT * FROM Tasks;";
        //    DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
        //    if (predicate == null) predicate = t => true;
        //    var emptyArray = new Tag[0];
        //    while (await reader.ReadAsync())
        //    {
        //        string title = reader[0].ToString();
        //        string ID = reader[1].ToString();
        //        Repeater repeater = RepeaterStorageConverter.Parse(reader[2].ToString());
        //        DateTime activatedTime = reader.GetDateTime(3);
        //        DateTime expiryTime = reader.GetDateTime(4);
        //        ObservableCollection<TimeInfo> time = TimeInfosStorageConverter.Parse(reader[5].ToString());
        //        Tag[] tags = emptyArray;
        //        string s_tags = reader[6].ToString();
        //        if (s_tags != "") tags = Array.ConvertAll(s_tags.Split(','), x => tagList.First(y => y.ID == x));
        //        string webAddress = reader[7].ToString();
        //        Task task = new Task(title, ID, repeater, activatedTime, expiryTime, time, new ObservableCollection<Tag>(tags), webAddress);
        //        if (predicate(task)) tasks.Add(task);
        //    }
        //    _sQLite.Close();
        //    return tasks;
        //}
        public async t.Task<List<Task>> GetTaskListAsync(string conditions, CollectionForIdentificationObject<Tag> tagList)
        {
            List<Task> tasks = new List<Task>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = "SELECT *, EXISTS (SELECT ID FROM Notes WHERE Tasks.ID = Notes.ID) AS HasNote FROM Tasks WHERE " + conditions + ";";
            var reader = await sQLiteCommand.ExecuteReaderAsync();
            var emptyArray = new Tag[0];
            while (await reader.ReadAsync())
            {
                string title = reader[0].ToString();
                string ID = reader[1].ToString();
                Repeater repeater = RepeaterStorageConverter.Parse(reader[2].ToString());
                DateTime activatedTime = reader.GetDateTime(3);
                DateTime expiryTime = reader.GetDateTime(4);
                ObservableCollection<TimeInfo> time = TimeInfosStorageConverter.Parse(reader[5].ToString());
                Tag[] tags = emptyArray;
                string s_tags = reader[6].ToString();
                if (s_tags != "") tags = Array.ConvertAll(s_tags.Split(','), x => tagList[tagList.IndexOfID(x)]);
                string webAddress = reader[7].ToString();
                Task task = new Task(title, ID, repeater, activatedTime, expiryTime, time, new ObservableCollection<Tag>(tags), webAddress);
                task.HasNote = reader.GetBoolean(8);
                tasks.Add(task);
            }
            _sQLite.Close();
            return tasks;
        }
        public async t.Task<List<Task>> GetGarbageTaskCollectionAsync()
        {
            List<Task> tasks = new List<Task>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = "SELECT Title,ID FROM TasksGarbage;";
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string title = reader[0].ToString();
                string ID = reader[1].ToString();
                Task task = new Task(title, ID);
                tasks.Add(task);
            }
            _sQLite.Close();
            return tasks;
        }
        #endregion
        #region Tag Query Methods
        public async t.Task InsertAsync(Tag tag)
        {
            string cmd = String.Format(@"INSERT INTO tags(Title, ID, IsDefault, Color) VALUES('{0}', '{1}', {2}, '{3}');",
                tag.Title, tag.ID, tag.IsDefault, tag.Color);
            await ExecuteQueryAsync(cmd);
        }
        ///
        // use SQL.Update to update Tag
        ///
        // use SQL.Remove to remove Tag
        ///

        public async t.Task<List<Tag>> GetTagListAsync()
        {
            List<Tag> tags = new List<Tag>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = String.Format(@"SELECT * FROM Tags;");
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string title = reader[0].ToString();
                string id = reader[1].ToString();
                bool isDefault = reader.GetBoolean(2);
                string color = reader[3].ToString();
                tags.Add(new Tag(isDefault, title, id, color));
            }
            _sQLite.Close();
            return tags;
        }
        #endregion
        //
        //
        //------ Cooming soon -------begin--------------------------
        #region ToDoComment Query Method 
        public async t.Task InsertToDoCommentAsync(ToDoComment comment)
        {
            string cmd = String.Format(@"INSERT INTO ToDoComments(Time, Content) VALUES ('{0}', '{1}');",
                comment.Time, comment.Content);
            await ExecuteQueryAsync(cmd);
        }
        ///<summary>
        /// typeparameter T must be <c>ToDoComment</c>
        ///</summary>
        public async t.Task UpdateToDoCommandAsync(string id, ToDoComment comment)
        {
            string cmd = String.Format("UPDATE ToDoComments SET Content = '{0}' WHERE ID = '{1}' AND Time = '{2}';",
                comment.Content, id, comment.Time);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task RemoveToDoCommentAsync(string id, ToDoComment comment)
        {
            string cmd = String.Format(@"DELETE FROM ToDoComments WHERE ID = '{0}' AND Time = '{1}';",
                id, comment.Time);
            await ExecuteQueryAsync(cmd);
        }
        public async t.Task<ObservableCollection<ToDoComment>> GetCommentListAsync(string id, DateTime startTime, DateTime lastTime)
        {
            ObservableCollection<ToDoComment> comments = new ObservableCollection<ToDoComment>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = String.Format(@"SELECT Time,Content FROM ToDoComments WHERE ID = '{0}' AND Time BETWEEN 
                '{1}' AND '{2}';", id, startTime.ToString("yyyy-MM-dd HH:mm"), startTime.ToString("yyyy-MM-dd HH:mm"));
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
        #endregion
        //------ Cooming soon -------end----------------------------
        //
        //
        #region Note Query Method
        /// <summary>
        /// Save new Note
        /// </summary>
        /// <param name="id">ID of Task or Event</param>
        /// <param name="content">Note's content</param>
        public async t.Task InsertNoteAsync(string id, string content)
        {
            string cmd = "INSERT INTO Notes(ID,Content) VALUES('" + id + "','" + content + "');";
            await ExecuteQueryAsync(cmd);
        }

        ///
        // use SQL.Update to update Note 
        ///
        // use SQL.Remove to remove Note
        ///
        // use SQL.GetStringProperty to get Content of Note
        ///
        #endregion
        #region Extended Note Query Method
        /// <summary>
        /// Save new Extened Note
        /// </summary>
        /// <param name="id">ID of Task or Event</param>
        /// <param name="time">Time of Extened Note</param>
        /// <param name="content">Extened Note's content</param>
        public async t.Task InsertExtendedNoteAsync(string id, DateTime time, string content)
        {
            string cmd = "INSERT INTO Notes(ID,Time,Content) VALUES('" + id + "','" + time.ToString("yyyy-MM-dd HH:mm") + "','" + content + "');";
            await ExecuteQueryAsync(cmd);
        }
        ///
        // use SQL.Update to update Task
        ///
        // use SQL.Remove to remove Task
        ///
        public async t.Task<NoteInfo[]> GetExistExtendedNoteInfosAsync()
        {
            List<NoteInfo> noteInfos = new List<NoteInfo>();
            await _sQLite.OpenAsync();
            SQLiteCommand sQLiteCommand = _sQLite.CreateCommand();
            sQLiteCommand.CommandText = String.Format(@"SELECT ID,Time FROM ExtendedNotes;");
            DbDataReader reader = await sQLiteCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string id = reader[0].ToString();
                DateTime time = reader.GetDateTime(1);
                noteInfos.Add(new NoteInfo() { ID = id, Time = time });
            }
            _sQLite.Close();
            return noteInfos.ToArray();
        }
        #endregion
    }
}
