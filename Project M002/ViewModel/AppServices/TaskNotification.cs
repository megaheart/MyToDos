using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;
using Storage;
using Storage.QueryMethods;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MyToDos.ViewModel.AppServices
{
    class TaskNotification
    {
        /// <summary>
        /// CurrentIndex <  -1 => free time -> lastest active task index = -CurrentIndex - 2 (the indexs of tasks missed are from 0 to this index)
        /// CurrentIndex >=  0 => work time -> current active task index = CurrentIndex
        /// CurrentIndex == -1 => work hard ~  don't miss any tasks before
        /// </summary>
        private static int CurrentIndex;
        public static ObservableCollection<UnfinishedTask> UnfinishedTasks;
        #region UnfinishedTasks Interacting Methods
        // Interacting and saving data 
        /// <summary>
        /// Add unfinished tasks at correct index
        /// </summary>
        /// <returns>
        /// index of task added in UnfinishedTasks
        /// </returns>
        //public static int UnfinishedTasks_Add(int index, UnfinishedTask task)
        //{
        //    int index = Extensions.BinarySearchRelatively(UnfinishedTasks, task, (t1, t2) =>
        //    {
        //        long c = t1.CompareTo(t2);
        //        if (c > 0) return 1;
        //        if (c == 0) return 0;
        //        return -1;
        //    });
        //    DataManager.Current.AddUnfinishedTodayTask(task);
        //    UnfinishedTasks.Insert(index, task);
        //    return index;
        //}
        #endregion
        public static ObservableCollection<UnfinishedTask> UnfinishedAndDoneLaterTasks;
        public static ObservableCollection<SnoozedTask> SnoozedTasks;
        public static bool IsFocusing { set; get; } = false;
        internal static async void Start(AppServiceRunArgs args)
        {
            CurrentIndex = -1;
            UnfinishedTasks = new ObservableCollection<UnfinishedTask>();
            UnfinishedAndDoneLaterTasks = new ObservableCollection<UnfinishedTask>();
            DateTime dateNow = DateTime.Now.Date;
            //Startup - Loading Data
            if (args.LatestStartupDate != dateNow)
            {
                //Report finished tasks and unfinished tasks (statistics)
                // Continue working............................... (workmark)//////////=============-------------\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\{{{{{{{{}}}}}}}}}}
                /*List<Tuple<string, bool>> todayTasks = await DataManager.Current.GetTodayTasksListAsync();
                int numberOfFinishedTasks = 0;
                string unfinishedTasks = "";
                string finishedTasks = "";
                foreach(var i in todayTasks)
                {
                    if (i.Item2)
                    {
                        numberOfFinishedTasks++;
                        finishedTasks += i.Item1;
                    }
                    else unfinishedTasks += i.Item2;
                }
                DataManager.Current.ReportInteractingTasksStatAsync(args.LatestStartupDate, numberOfFinishedTasks,
                    todayTasks.Count, finishedTasks, unfinishedTasks);*/
                await DataManager.Current.ClearTodayTasksAsync();//Clear TodayTasks table
            }
            //Renew Unfinished Today Tasks
            //[Optimize] by using Quick Sort
            foreach (var i in DataManager.Current.Tasks)
            {
                if (i.Type == TaskType.Schedule && i.GetStatusOn(dateNow) == TaskStatus.Available)
                {
                    AddAllUnfinishedTaskOf(i);
                }
            }
            //Remove all unfinished tasks which is done
            await DataManager.Current.GetFinishedTodayTasksAsync((id, activeTimeOfDayTotalMinutes) =>
            {
                TimeSpan activeTime = TimeSpan.FromMinutes(activeTimeOfDayTotalMinutes);
                int index = Extensions.BinarySearch(UnfinishedTasks, x => x.NotifiedTime, activeTime, TimeSpan.Compare);
                if (index == -1) return;
                int i = index;
                while(i>-1 && UnfinishedTasks[i].NotifiedTime == activeTime)
                {
                    if(UnfinishedTasks[i].Task.ID == id)
                    {
                        UnfinishedTasks.RemoveAt(i);
                        return;
                    }
                    i--;
                }
                i = index + 1;
                while (i < UnfinishedTasks.Count && UnfinishedTasks[i].NotifiedTime == activeTime)
                {
                    if (UnfinishedTasks[i].Task.ID == id)
                    {
                        UnfinishedTasks.RemoveAt(i);
                        return;
                    }
                    i++;
                }
            });
            //Execute
            Run(args);
            DataManager.Current.Tasks.SQLInsertsItem += CheckInsertedTask;
            DataManager.Current.Tasks.SQLRemovesItem += CheckRemovedTask;
            DataManager.Current.Tasks.SQLUpdatesItem += CheckChangedTask;
            args.Sender.ExecuteOncePerMinute += Run;
        }
        internal static void Run(AppServiceRunArgs args)
        {
            if (IsFocusing) return;
            var index = GetCurrentActiveTaskIndex();
            if (index != GetLastestActiveTaskIndex())
            {
                CurrentIndex = index;
                Notify(UnfinishedTasks[index]);
            }
        }
        /// <summary>
        /// Be called when Notify(), parameters: Active New Task : Tasks, Active New Task's Time Info : TimeInfo
        /// Both Active New Task and Active New Task's Time Info can be null together
        /// </summary>
        public static Action<UnfinishedTask> Notifying;
        static void Notify(UnfinishedTask task)
        {
            if (IsFocusing) return;
            Notifying?.Invoke(task);
        }
        public static void Done(UnfinishedTask task)
        {
            DataManager.Current.FinishTodayTasksAsync(task);
            if (!UnfinishedTasks.Remove(task)) UnfinishedAndDoneLaterTasks.Remove(task);
            else
            {
                int index = 0;
                while (index < SnoozedTasks.Count)
                {
                    if (SnoozedTasks[index].UnfinishedTask == task)
                    {
                        break;
                    }
                    index++;
                }
                if (index != SnoozedTasks.Count)
                {
                    SnoozedTasks.RemoveAt(index);
                }
            }
            CurrentIndex = -CurrentIndex - 1;
            
        }
        public static void Snooze(UnfinishedTask unfinishedTask, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero) throw new Exception("duration must be more than zero");
            if(!SnoozedTasks.Any(x=>x.UnfinishedTask == unfinishedTask))
            {
                SnoozedTasks.Add(new SnoozedTask() { UnfinishedTask = unfinishedTask, OriginalNotifiedTime = unfinishedTask.NotifiedTime });
            }
            unfinishedTask.NotifiedTime += duration;
            int index = UnfinishedTasks.IndexOf(unfinishedTask);
            if (index == -1) throw new Exception("UnfinishedTasks doesn't contain it to snooze");
            var oldIndex = index;
            index++;
            while (index < UnfinishedTasks.Count && unfinishedTask > UnfinishedTasks[index])
            {
                index++;
            }
            index--;
            UnfinishedTasks.Move(oldIndex, index);
        }
        public static void DoLater(UnfinishedTask task)
        {
            if (UnfinishedTasks.Remove(task))
            {
                if (UnfinishedAndDoneLaterTasks.Contains(task))
                    throw new Exception("UnfinishedAndDoneLaterTasks is containing it now"); ;
                UnfinishedAndDoneLaterTasks.Add(task);
                int index = 0;
                while(index < SnoozedTasks.Count)
                {
                    if(SnoozedTasks[index].UnfinishedTask == task)
                    {
                        break;
                    }
                    index++;
                }
                if (index != SnoozedTasks.Count)
                {
                    SnoozedTasks.RemoveAt(index);
                }
            }
            throw new Exception("UnfinishedTasks doesn't contain it to do later");
        }
        /// <summary>
        /// Get unfinished task whose index = current unfinished task's index + distance
        /// </summary>
        public static UnfinishedTask GetNextOrPreviousUnfinishedTask(int distance)
        {
            distance = GetLastestActiveTaskIndex() + distance;
            if (distance < 0 || distance >= UnfinishedTasks.Count) return null;
            return UnfinishedTasks[distance];
        }
        internal static void CheckInsertedTask(SQLCollectionChangedArgs e)
        {
            Task addedTask = e.Item as Task;
            DateTime now = DateTime.Now;
            if (addedTask.Type == TaskType.Schedule && addedTask.GetStatusOn(now) == TaskStatus.Available)
            {
                if (AddAllUnfinishedTaskOf(addedTask)) Notify(UnfinishedTasks[CurrentIndex]);
            }
        }
        /// <returns>
        /// true: notify new active task
        /// false: nothing to notify
        /// </returns>
        public static bool AddAllUnfinishedTaskOf(Task task)
        {
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            bool isCurrentIndexChanged = false;
            int index = 0;
            int timeInfoIndex = 0;
            UnfinishedTask unfinishedTask = null;
            while (timeInfoIndex < task.Time.Count && index < UnfinishedTasks.Count)
            {
                if (unfinishedTask == null) unfinishedTask = new UnfinishedTask(task, task.Time[timeInfoIndex]);
                if (unfinishedTask < UnfinishedTasks[index])
                {
                    UnfinishedTasks.Insert(index, unfinishedTask);
                    isCurrentIndexChanged = isCurrentIndexChanged || RenewCurrentActiveTaskIndexAfterAdding(index, timeOfDay);
                    timeInfoIndex++;
                    unfinishedTask = null;
                }
                index++;
            }
            while (timeInfoIndex < task.Time.Count)
            {
                if (task.Time[timeInfoIndex].ActiveTimeOfDay.Value <= timeOfDay)
                {
                    CurrentIndex = UnfinishedTasks.Count;
                    isCurrentIndexChanged = true;
                }
                UnfinishedTasks.Insert(UnfinishedTasks.Count, new UnfinishedTask(task, task.Time[timeInfoIndex]));
            }
            return isCurrentIndexChanged;
        }
        /// <returns>
        /// true: notify new active task
        /// false: nothing to notify
        /// </returns>
        public static bool RemoveAllUnfinishedTaskOf(Task task)
        {
            //List<TimeInfo> time = removedTask.Time.ToList();
            bool isCurrentIndexChanged = false;
            int i = 0;
            int count = task.Time.Count;
            while (count != 0 && i < UnfinishedTasks.Count)
            {
                if (UnfinishedTasks[i].Task != task)
                {
                    i++;
                    continue;
                }
                UnfinishedTasks.RemoveAt(i);
                isCurrentIndexChanged = isCurrentIndexChanged || RenewCurrentActiveTaskIndexAfterRemoving(i);
                count--;
                int index = 0;
                while (index < SnoozedTasks.Count)
                {
                    if (SnoozedTasks[index].UnfinishedTask == UnfinishedTasks[i])
                    {
                        break;
                    }
                    index++;
                }
                if (index != SnoozedTasks.Count)
                {
                    SnoozedTasks.RemoveAt(index);
                }
            }
            i = 0;
            while (count != 0 && i < UnfinishedAndDoneLaterTasks.Count)
            {
                if (UnfinishedTasks[i].Task != task)
                {
                    i++;
                    continue;
                }
                UnfinishedAndDoneLaterTasks.RemoveAt(i);
                count--;
            }
            return isCurrentIndexChanged;
        }
        internal static void CheckRemovedTask(SQLCollectionChangedArgs e)
        {
            Task removedTask = e.Item as Task;
            DateTime now = DateTime.Now;
            if (removedTask.Type == TaskType.Schedule && removedTask.GetStatusOn(now) == TaskStatus.Available)
            {
                if (RemoveAllUnfinishedTaskOf(removedTask)) Notify(UnfinishedTasks[CurrentIndex]);
            }
        }
        internal static void CheckChangedTask(SQLCollectionChangedArgs e)
        {
            Task changedTask = e.Item as Task;
            if (e.Property == "Time")
            {
                DateTime now = DateTime.Now;
                if (changedTask.Type == TaskType.Schedule && changedTask.GetStatusOn(now) == TaskStatus.Available)
                {
                    int index = 0;
                    while(index < SnoozedTasks.Count)
                    {
                        if (SnoozedTasks[index].UnfinishedTask.Task == changedTask)
                        {
                            SnoozedTasks.RemoveAt(index);
                        }
                        else index++;
                    }
                    bool isfree = CurrentIndex < 0;
                    UnfinishedTask oldActiveTask = null;
                    if (CurrentIndex != -1)
                    {
                        oldActiveTask = UnfinishedTasks[GetLastestActiveTaskIndex()];
                        oldActiveTask.NotifiedTime = oldActiveTask.TimeInfo.ActiveTimeOfDay.Value;
                    }
                    RemoveAllUnfinishedTaskOf(changedTask);
                    AddAllUnfinishedTaskOf(changedTask);
                    if (CurrentIndex > -1)
                    {
                        if (oldActiveTask != UnfinishedTasks[CurrentIndex]) Notify(UnfinishedTasks[CurrentIndex]);
                        else if (isfree) CurrentIndex = -CurrentIndex - 2;
                    }
                    else if(CurrentIndex == -1 && oldActiveTask != null) Notify(null);
                }

            }
            else if (e.Property == "Repeater")
            {
                DateTime now = DateTime.Now;
                if (changedTask.Type == TaskType.Schedule && changedTask.GetStatusOn(now) == TaskStatus.Available)
                {
                    if(!UnfinishedTasks.Any(x=>x.Task == changedTask))
                    {
                        if (AddAllUnfinishedTaskOf(changedTask)) Notify(UnfinishedTasks[CurrentIndex]);
                    }
                }
                else
                {
                    if (UnfinishedTasks.Any(x => x.Task == changedTask))
                    {
                        if (RemoveAllUnfinishedTaskOf(changedTask)) Notify(UnfinishedTasks[CurrentIndex]);
                    }
                }
            }
        }

        /// <returns>
        /// true: notify new active task
        /// false: nothing to notify
        /// </returns>
        public static bool RenewCurrentActiveTaskIndexAfterAdding(int index, TimeSpan timeOfDayNow)
        {
            var oldCurrentIndex = GetLastestActiveTaskIndex();
            if (index > oldCurrentIndex)
            {
                if (UnfinishedTasks[index].NotifiedTime <= timeOfDayNow)
                {
                    CurrentIndex = index;
                    return true;
                }
            }
            else
            {
                if (CurrentIndex < 0) CurrentIndex--;
                else CurrentIndex++;
            }
            return false;
        }
        /// <returns>
        /// true: notify new active task
        /// false: nothing to notify
        /// </returns>
        public static bool RenewCurrentActiveTaskIndexAfterRemoving(int index)
        {
            var oldCurrentIndex = GetLastestActiveTaskIndex();
            if (index <= oldCurrentIndex)
            {
                if (index == CurrentIndex)
                {
                    CurrentIndex--;
                    return true;
                }
                else
                {
                    if (CurrentIndex < 0) CurrentIndex++;
                    else CurrentIndex--;
                }
            }
            return false;
        }
        private static int GetCurrentActiveTaskIndex()
        {
            var timeOfDayNow = DateTime.Now.TimeOfDay;
            var index = GetLastestActiveTaskIndex();
            //if (index == -1) return -1;
            while (index < UnfinishedTasks.Count - 1 && UnfinishedTasks[index + 1].NotifiedTime >= timeOfDayNow) index++;
            return index;
        }
        private static int GetLastestActiveTaskIndex()
        {
            if (CurrentIndex < -1) return -CurrentIndex - 2;
            return CurrentIndex;
        }
    }
    public class SnoozedTask
    {
        public UnfinishedTask UnfinishedTask;
        public TimeSpan OriginalNotifiedTime;
    }
}
