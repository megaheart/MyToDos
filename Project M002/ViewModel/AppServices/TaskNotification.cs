using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.Model;
using Storage;

namespace MyToDos.ViewModel.AppServices
{
    class TaskNotification
    {
        private static Tuple<Task,TimeInfo> Active;
        private static List<Task> UnfinishedTasks;
        internal static async System.Threading.Tasks.Task Start(AppServiceRunArgs args)
        {
            UnfinishedTasks = new List<Task>();
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

                //Clear TodayTasks table
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
                if(Active.Item1 == null)//Rest time starts
                {
                    //Notify();
                }
                else//Task starts - Notify
                {
                    //Notify();
                }
            }
        }
        static void Notify()
        {

        }
        //internal static void CheckInsertedTask(SQLCollectionChangedArgs e)
        //{

        //}
        internal static void CheckRemovedTask(SQLCollectionChangedArgs e)
        {
            if(Active.Item1 == e.Item)//Task starts - Notify
            {
                var currentActive = GetCurrentActiveTask();
                Active = currentActive;
                if (Active.Item1 == null)//Rest time starts
                {

                }
                else//Task starts - Notify
                {

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

                        }
                        else//Task starts - Notify
                        {

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
                            if (info.ActiveTimeOfDay.HasValue && info.ActiveTimeOfDay.Value <= timeOfDay)
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

                        }
                        else//Task starts - Notify
                        {

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
                            if (info.ActiveTimeOfDay.HasValue && info.ActiveTimeOfDay.Value <= timeOfDay)
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

                    }
                }
            }
        }
        private static Tuple<Task, TimeInfo> GetCurrentActiveTask()
        {
            DateTime now = DateTime.Now;
            TimeSpan timeOfDay = now.TimeOfDay;
            var tasks = DataManager.Current.Tasks;
            if (tasks.Count == 0) return null;
            Task output = null;
            TimeInfo lastest = null;
            for (int i = 0; i < tasks.Count; i++)
            {
                //Find Active task
                if (tasks[i].GetStatusOn(now) == TaskStatus.Available)
                {
                    foreach (var info in tasks[i].Time)
                    {
                        if (info.ActiveTimeOfDay.HasValue && info.ActiveTimeOfDay.Value <= timeOfDay)
                        {
                            if (lastest == null || lastest.ActiveTimeOfDay.Value < info.ActiveTimeOfDay.Value ||
                                (lastest.ActiveTimeOfDay.Value == info.ActiveTimeOfDay.Value && output.Index < tasks[i].Index))
                            {
                                output = tasks[i];
                                lastest = info;
                            }
                        }
                    }
                }
            }
            return new Tuple<Task, TimeInfo>(output, lastest);
        }
    }
}
