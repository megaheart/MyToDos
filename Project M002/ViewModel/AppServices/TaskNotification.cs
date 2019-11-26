using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;
using Storage;
using System.Collections.ObjectModel;

namespace MyToDos.ViewModel.AppServices
{
    class TaskNotification
    {
        /// <summary>
        /// 
        /// </summary>
        private static Tuple<Task,TimeInfo> Active;
        public static ObservableCollection<Task> UnfinishedTasks;
        public static bool IsFocusing = false;
        internal static async void Start(AppServiceRunArgs args)
        {
            UnfinishedTasks = new ObservableCollection<Task>();
            DateTime dateNow = DateTime.Now.Date;
            //Startup - Loading Data
            if (args.LatestStartupDate == dateNow)
            {
                foreach (var i in await DataManager.Current.GetUnfinishedTodayTasksAsync())
                {
                    UnfinishedTasks.Add(DataManager.Current.Tasks[i]);
                }
            }
            else
            {
                //Report finished tasks and unfinished tasks (statistics)
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
                foreach (var i in DataManager.Current.Tasks)
                {
                    if(i.GetStatusOn(dateNow) == TaskStatus.Available)
                    {
                        DataManager.Current.AddUnfinishedTodayTask(i.ID);
                        UnfinishedTasks.Add(i);
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
        /// <summary>
        /// Be called when Notify(), parameters: Active New Task : Tasks, Active New Task's Time Info : TimeInfo
        /// Both Active New Task and Active New Task's Time Info can be null together
        /// </summary>
        public static Action<Task, TimeInfo> Notifying;
        static void Notify(Task activeTask, TimeInfo info)
        {
            if (IsFocusing) return;
            Notifying?.Invoke(activeTask, info);
        }
        public static void Done(Task task)
        {
            Notifying?.Invoke(activeTask, info);
        }
        public static void Snooze()
        {
            Notifying?.Invoke(activeTask, info);
        }
        public static void DoLater(Task task)
        {
            Notifying?.Invoke(activeTask, info);
        }
        public static void FocusOn(Task task)
        {
            Notifying?.Invoke(activeTask, info);
        }
        public static void DoNextTask()
        {
            Notifying?.Invoke(activeTask, info);
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
        private static Tuple<Task, TimeInfo> GetCurrentActiveTask()
        {
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            Task output = null;
            TimeInfo lastest = null;
            for (int i = 0; i < UnfinishedTasks.Count; i++)
            {
                //Find Active task
                foreach (var info in UnfinishedTasks[i].Time)
                {
                    if (info.ActiveTimeOfDay.HasValue)
                    {
                        if (info.ActiveTimeOfDay.Value == timeOfDay) return new Tuple<Task, TimeInfo>(UnfinishedTasks[i], lastest);
                        if(info.ActiveTimeOfDay.Value < timeOfDay)
                        {
                            if (lastest == null || lastest.ActiveTimeOfDay.Value < info.ActiveTimeOfDay.Value ||
                                (lastest.ActiveTimeOfDay.Value == info.ActiveTimeOfDay.Value && output.Index < UnfinishedTasks[i].Index))
                            {
                                output = UnfinishedTasks[i];
                                lastest = info;
                            }
                        }
                    }
                }
            }
            return new Tuple<Task, TimeInfo>(output, lastest);
        }
        private static Tuple<Task, TimeInfo> GetNearbyNextActiveTask()
        {
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            Task output = null;
            TimeInfo nearby = null;
            for (int i = 0; i < UnfinishedTasks.Count; i++)
            {
                //Find Active task
                foreach (var info in UnfinishedTasks[i].Time)
                {
                    if (info.ActiveTimeOfDay.HasValue)
                    {
                        if (info.ActiveTimeOfDay.Value > timeOfDay)
                        {
                            if (nearby == null || nearby.ActiveTimeOfDay.Value > info.ActiveTimeOfDay.Value || 
                                (nearby.ActiveTimeOfDay.Value == info.ActiveTimeOfDay.Value && output.Index < UnfinishedTasks[i].Index))
                            {
                                output = UnfinishedTasks[i];
                                nearby = info;
                            }
                        }
                    }
                }
            }
            return new Tuple<Task, TimeInfo>(output, nearby);
        }
    }
}
