using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;
using Storage;
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
        public static void UnfinishedTasks_Add(UnfinishedTask task)
        {
            int index = 0;
            while(index < UnfinishedTasks.Count && task > UnfinishedTasks[index])
            {
                index++;
            }
            DataManager.Current.AddUnfinishedTodayTask(task);
            UnfinishedTasks.Insert(index, task);
        }
        #endregion
        public static ObservableCollection<UnfinishedTask> UnfinishedAndDoneLaterTasks;
        public static bool IsFocusing { set; get; } = false;
        internal static async void Start(AppServiceRunArgs args)
        {
            CurrentIndex = -1;
            UnfinishedTasks = new ObservableCollection<UnfinishedTask>();
            UnfinishedAndDoneLaterTasks = new ObservableCollection<UnfinishedTask>();
            DateTime dateNow = DateTime.Now.Date;
            //Startup - Loading Data
            if (args.LatestStartupDate == dateNow)
            {
                await DataManager.Current.GetUnfinishedTodayTasksAsync((id,index, activeTimeOfDayTotalMinutes) => {
                    var activeTimeOfDay = TimeSpan.FromMinutes(activeTimeOfDayTotalMinutes);
                    var task = DataManager.Current.Tasks[id];
                    var timeInfo = task.Time.First(t => t.ActiveTimeOfDay == activeTimeOfDay);
                    UnfinishedTasks_Add(new UnfinishedTask(task, timeInfo, index));
                    if (index > UnfinishedTask.SqlIndexMax) UnfinishedTask.SqlIndexMax = index;
                });
            }
            else
            {
                //Report finished tasks and unfinished tasks (statistics)
                // Continue working............................... (workmark)//////////=============-------------\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\{{{{{{{{}}}}}}}}}}
                List<Tuple<string, bool>> todayTasks = await DataManager.Current.GetTodayTasksListAsync();
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
                    todayTasks.Count, finishedTasks, unfinishedTasks);
                await DataManager.Current.ClearTodayTasksAsync();//Clear TodayTasks table
                //Renew Unfinished Today Tasks
                foreach (var i in DataManager.Current.Tasks)
                {
                    if(i.Type == TaskType.Schedule && i.GetStatusOn(dateNow) == TaskStatus.Available)
                    {
                        foreach(var j in i.Time)
                        {
                            UnfinishedTask unfinishedTask = new UnfinishedTask(i, j);
                            DataManager.Current.AddUnfinishedTodayTask(unfinishedTask);
                            UnfinishedTasks_Add(unfinishedTask);
                        }
                    }
                }

            }
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
            CurrentIndex = -CurrentIndex - 1;
        }
        public static void Snooze(UnfinishedTask unfinishedTask, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero) throw new Exception("duration must be more than zero");
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
            }
            throw new Exception("UnfinishedTasks doesn't contain it to do later");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance">
        /// 
        /// </param>
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
            TimeSpan timeOfDay = now.TimeOfDay;
            Task output = Active.Item1;
            TimeInfo lastest = Active.Item2;
            if (addedTask.GetStatusOn(now) == TaskStatus.Available)
            {
                foreach (var info in addedTask.Time)
                {
                    if (info.ActiveTimeOfDay.Value == timeOfDay)
                    {
                        output = addedTask;
                        lastest = info;
                        break;
                    }
                    if (info.ActiveTimeOfDay.Value < timeOfDay)
                    {
                        if (lastest == null || lastest.ActiveTimeOfDay.Value < info.ActiveTimeOfDay.Value ||
                            (lastest.ActiveTimeOfDay.Value == info.ActiveTimeOfDay.Value && output.Index < addedTask.Index))
                        {
                            output = addedTask;
                            lastest = info;
                        }
                    }
                }
            }
            if (output == addedTask)
            {
                Active = new Tuple<Task, TimeInfo>(output, lastest);
                //Task starts - Notify
                Notify(Active.Item1, Active.Item2);
            }
        }
        internal static void CheckRemovedTask(SQLCollectionChangedArgs e)
        {
            if(Active.Item1 == e.Item)//Task starts - Notify
            {
                var currentActive = GetCurrentActiveTask();
                Active = currentActive;
                if (Active.Item1 == null)//Rest time starts
                {
                    Notify(null, null);
                }
                else//Task starts - Notify
                {
                    Notify(Active.Item1, Active.Item2);
                }
            }
        }
        internal static void CheckChangedTask(SQLCollectionChangedArgs e)
        {
            Task changedTask = e.Item as Task;
            if (e.Property == "Time")
            {
                if (changedTask == Active.Item1)
                {
                    var currentActive = GetCurrentActiveTask();
                    if (currentActive.Item1 != Active.Item1)
                    {
                        Active = currentActive;
                        if (Active.Item1 == null)//Rest time starts
                        {
                            Notify(null, null);
                        }
                        else//Task starts - Notify
                        {
                            Notify(Active.Item1, Active.Item2);
                        }
                    }
                }
                else
                {
                    DateTime now = DateTime.Now;
                    TimeSpan timeOfDay = now.TimeOfDay;
                    Task output = Active.Item1;
                    TimeInfo lastest = Active.Item2;
                    if (changedTask.GetStatusOn(now) == TaskStatus.Available)
                    {
                        foreach (var info in changedTask.Time)
                        {
                            if (info.ActiveTimeOfDay.Value == timeOfDay)
                            {
                                output = changedTask;
                                lastest = info;
                                break;
                            }
                            if (info.ActiveTimeOfDay.Value < timeOfDay)
                            {
                                if (lastest == null || lastest.ActiveTimeOfDay.Value < info.ActiveTimeOfDay.Value ||
                                    (lastest.ActiveTimeOfDay.Value == info.ActiveTimeOfDay.Value && output.Index < changedTask.Index))
                                {
                                    output = changedTask;
                                    lastest = info;
                                }
                            }
                        }
                    }
                    if (output == changedTask)
                    {
                        Active = new Tuple<Task, TimeInfo>(output, lastest);
                        //Task starts - Notify
                        Notify(Active.Item1, Active.Item2);
                    }
                }
            }
            else if (e.Property == "Repeater")
            {
                if (changedTask == Active.Item1)
                {
                    if (changedTask.GetStatusOn(DateTime.Now) != TaskStatus.Available)
                    {
                        var currentActive = GetCurrentActiveTask();
                        Active = currentActive;
                        if (Active.Item1 == null)//Rest time starts
                        {
                            Notify(null, null);
                        }
                        else//Task starts - Notify
                        {
                            Notify(Active.Item1, Active.Item2);
                        }
                    }
                }
                else
                {
                    DateTime now = DateTime.Now;
                    TimeSpan timeOfDay = now.TimeOfDay;
                    Task output = Active.Item1;
                    TimeInfo lastest = Active.Item2;
                    if (changedTask.GetStatusOn(now) == TaskStatus.Available)
                    {
                        foreach (var info in changedTask.Time)
                        {
                            if (info.ActiveTimeOfDay.Value == timeOfDay)
                            {
                                output = changedTask;
                                lastest = info;
                                break;
                            }
                            if (info.ActiveTimeOfDay.Value < timeOfDay)
                            {
                                if (lastest == null || lastest.ActiveTimeOfDay.Value < info.ActiveTimeOfDay.Value ||
                                    (lastest.ActiveTimeOfDay.Value == info.ActiveTimeOfDay.Value && output.Index < changedTask.Index))
                                {
                                    output = changedTask;
                                    lastest = info;
                                }
                            }
                        }
                    }
                    if (output == changedTask)
                    {
                        Active = new Tuple<Task, TimeInfo>(output, lastest);
                        //Task starts - Notify
                        Notify(Active.Item1, Active.Item2);
                    }
                }
            }
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
}
