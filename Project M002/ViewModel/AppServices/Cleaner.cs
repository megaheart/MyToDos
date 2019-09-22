using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.ViewModel.AppServices
{
    /// <summary>
    /// Run finally
    /// </summary>
    class Cleaner
    {
        public static void Start(AppServiceRunArgs args)
        {
            Run(args);
            args.Sender.ExecuteOncePerMinute += Run;
        }
        public static void Run(AppServiceRunArgs args)
        {
            DateTime now = DateTime.Now;
            var tasks = DataManager.Current.Tasks;
            for (int i = 0; i < tasks.Count; i++)
            {
                //Remove expiried task
                if (tasks[i].ExpiryTime <= now)
                {
                    tasks.Remove(tasks[i]);
                    i--;
                    continue;
                }
            }

            
        }
    }
}
