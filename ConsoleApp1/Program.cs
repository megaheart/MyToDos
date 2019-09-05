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
            test.SQL_GetNote();
            Console.ReadLine();
        }
    }
    class ExceptionTest
    {
        public void DataManager_Initialize()
        {
            SQL sQL = new SQL(DataManager.DataBaseFile);
            List<Tag> Tags = sQL.GetTagListAsync().Result;
            var a = SQL_GetTaskList(null, false, Tags);
        }
        public ObservableCollection<Task> SQL_GetTaskList(Predicate<Task> predicate, bool isGarbage, List<Tag> tagList)
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
            if (predicate == null) predicate = o => true;
            var emptyArray = new Tag[0];
            while (reader.ReadAsync().Result)
            {
                string title = reader[0].ToString();
                string ID = reader[1].ToString();
                Repeater repeater = RepeaterStorageConverter.Parse(reader[2].ToString());
                DateTime activatedTime = reader.GetDateTime(3);
                DateTime expiryTime = reader.GetDateTime(4);
                ObservableCollection<TimeInfo> time = TimeInfosStorageConverter.Parse(reader[5].ToString());
                Tag[] tags = emptyArray;
                string s_tags = reader[6].ToString();
                if (s_tags != "") tags = Array.ConvertAll(s_tags.Split(','), x => tagList.First(y => y.ID == x));
                string webAddress = reader[7].ToString();
                Task task = new Task(title, ID, repeater, activatedTime, expiryTime, time, new ObservableCollection<Tag>(tags), webAddress);
                if (predicate(task)) tasks.Add(task);
            }
            _sQLite.Close();
            return tasks;
        }
        public void SQL_InsertTasks()
        {
            Random random = new Random();
            Repeater daily = new Daily();
            Repeater nonRepeater = new NonRepeater();
            Func<Repeater> monthly = () => new Monthly(new int[] { random.Next(1, 32) });
            Func<Repeater> customRepeater = () => new CustomRepeater(random.Next(1, 8));
            Func<Repeater> weekly = () => new Weekly(new int[] { random.Next(0, 7) });
            Func<Repeater> once = () => new Once(new int[] { random.Next(1, 29), random.Next(1, 13), 2020 });
            Func<DateTime> startTime = () => DateTime.Now.Date.Add(new TimeSpan(random.Next(3, 100), random.Next(0, 24), random.Next(0, 60), 0));
            Func<DateTime> endTime = () => DateTime.Now.Date.Add(new TimeSpan(random.Next(100, 109), random.Next(0, 24), random.Next(0, 60), 0));
            Func<string> id = () =>
            {
                long i = checked((long)random.Next() * random.Next());
                string s = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                string o = "";
                while (i != 0)
                {
                    o = s[checked((int)(i % 62))] + o;
                    i /= 62;
                }
                if (o == "") throw new Exception("output must be different from null");
                Console.WriteLine(o);
                return o;
            };
            ObservableCollection<Task> tasks = new ObservableCollection<Task>()
            {
                new Task("aaaa",id(),nonRepeater, startTime(), endTime(),
                    new ObservableCollection<TimeInfo>()
                    {
                        new TimeInfo(new TimeSpan(12,59,0),new TimeSpan(1,0,0))
                    },
                    new ObservableCollection<Tag>()
                    {
                        new Tag(){Title ="linh dep trai",ID = "xa4"}
                    },
                    @"https://google.com"
                ),
                new Task("Linh is handsome",id(), monthly(), startTime(),endTime(),
                    new ObservableCollection<TimeInfo>()
                    {
                        new TimeInfo(new TimeSpan(12,59,0),new TimeSpan(1,0,0)),
                        new TimeInfo(new TimeSpan(13,59,0),new TimeSpan(1,0,0)),
                        new TimeInfo(new TimeSpan(14,59,0),new TimeSpan(1,0,0)),
                        new TimeInfo(new TimeSpan(15,59,0),new TimeSpan(1,0,0))
                    },
                    new ObservableCollection<Tag>()
                    {
                    },
                    @"https://www.youtube.com"
                ),
                new Task("CCCC AAA bbb xxx",id(), customRepeater(), startTime(),endTime(),
                    new ObservableCollection<TimeInfo>()
                    {
                    },
                    new ObservableCollection<Tag>()
                    {
                        new Tag(){Title ="lilili",ID = "cva4"},
                        new Tag(){Title ="cc",ID = "cca4"},
                        new Tag(){Title ="bvc",ID = "xds"},
                        new Tag(){Title ="lcds",ID = "ca4x"},
                        new Tag(){Title ="netCore",ID = "874"},
                        new Tag(){Title ="xxnhxx",ID = "696969"}
                    },
                    @"https://www.youtube.com"
                ),
                new Task("BBBB XGDs",id(), daily, startTime(),endTime(),
                    new ObservableCollection<TimeInfo>()
                    {
                    },
                    new ObservableCollection<Tag>()
                    {
                        new Tag(){Title ="linh",ID = "ca4"}
                    },
                    @"https://www.youtube.com"
                ),
                new Task("Hayayay",id(), once(), startTime(),endTime(),
                    new ObservableCollection<TimeInfo>()
                    {
                    },
                    new ObservableCollection<Tag>()
                    {
                        new Tag(){Title ="linh",ID = "ca4"}
                    },
                    @"https://www.youtube.com"
                ),
                new Task("Frosesa",id(), weekly(), startTime(),endTime(),
                    new ObservableCollection<TimeInfo>()
                    {
                    },
                    new ObservableCollection<Tag>()
                    {
                        new Tag(){Title ="linh",ID = "ca4"}
                    },
                    @"https://www.youtube.com"
                )
            };
            SQL sQL = new SQL(DataManager.DataBaseFile);
            foreach(var i in tasks)
            {
                sQL.InsertAsync(i);
            }
            
        }
        public void SQL_InsertTags()
        {
            ObservableCollection<Tag> tags = new ObservableCollection<Tag>()
            {
                new Tag(){Title ="linh",ID = "ca4",Color="#ffffff"},
                new Tag(){Title ="lilili",ID = "cva4"},
                new Tag(){Title ="cc",ID = "cca4"},
                new Tag(){Title ="bvc",ID = "xds"},
                        new Tag(){Title ="lcds",ID = "ca4x",Color="#ffffff"},
                        new Tag(){Title ="netCore",ID = "874"},
                        new Tag(){Title ="xxnhxx",ID = "696969"},
                        new Tag(){Title ="linh dep trai",ID = "xa4",Color="#ffffff"}
            };
            SQL sQL = new SQL(DataManager.DataBaseFile);
            foreach (var i in tags)
            {
                sQL.InsertAsync(i);
            }

        }
        public void SQL_GetNote()
        {
            DataManager data = new DataManager();
            string content;
            string[] ids = new string[] { "1vzpgk5Cjg", "CZnuyEZXRO", "1I8CGu4xHl2", "xxxx" };
            foreach(var i in ids)
            {
                data.GetNoteTextAsync(new NoteTaking() { ID = i }).ContinueWith(t =>
                {

                    
                });
            }
        }
    }
    class NoteTaking : INoteTaking
    {
        public bool HasNote => throw new NotImplementedException();

        public string ID { get; set; }
    }
}
