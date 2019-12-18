using System;

namespace Storage.Model
{
    public class UnfinishedTask
    {
        public Task Task {set; get; }
        public TimeInfo TimeInfo {set; get; }
        public TimeSpan NotifiedTime { set; get; }
        public short SqlIndex { set; get; }
        private static short sqlIndexMax;
        public static short SqlIndexMax { 
            set {
                if (sqlIndexMax > -1) throw new Exception("Can only set sqlIndexMax once");
                if (value < 0) throw new Exception("SqlIndexMax must be more than -1");
                sqlIndexMax = value;
            }
            get => sqlIndexMax; 
        }
        public UnfinishedTask(Task task, TimeInfo timeInfo, short index)
        {
            Task = task;
            TimeInfo = timeInfo;
            SqlIndex = index;
        }
        public UnfinishedTask(Task task, TimeInfo timeInfo)
        {
            Task = task;
            TimeInfo = timeInfo;
            SqlIndex = ++sqlIndexMax;
        }
        public long CompareTo(UnfinishedTask unfinishedTask)
        {
            if (this.NotifiedTime == unfinishedTask.NotifiedTime) return this.Task.Index - unfinishedTask.Task.Index;
            return this.NotifiedTime.Ticks - unfinishedTask.NotifiedTime.Ticks;
        }
        public static bool operator < (UnfinishedTask unfinishedTask1, UnfinishedTask unfinishedTask2)
        {
            return unfinishedTask1.CompareTo(unfinishedTask2) < 0;
        }
        public static bool operator > (UnfinishedTask unfinishedTask1, UnfinishedTask unfinishedTask2)
        {
            return unfinishedTask1.CompareTo(unfinishedTask2) > 0;
        }
        public static bool operator <=(UnfinishedTask unfinishedTask1, UnfinishedTask unfinishedTask2)
        {
            return unfinishedTask1.CompareTo(unfinishedTask2) <= 0;
        }
        public static bool operator >=(UnfinishedTask unfinishedTask1, UnfinishedTask unfinishedTask2)
        {
            return unfinishedTask1.CompareTo(unfinishedTask2) >= 0;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(UnfinishedTask)) return CompareTo(obj as UnfinishedTask) == 0;
            return false;
        }
        public static bool operator ==(UnfinishedTask unfinishedTask1, UnfinishedTask unfinishedTask2)
        {
            return unfinishedTask1.CompareTo(unfinishedTask2) == 0;
        }
        public static bool operator !=(UnfinishedTask unfinishedTask1, UnfinishedTask unfinishedTask2)
        {
            return unfinishedTask1.CompareTo(unfinishedTask2) != 0;
        }
    }
}
