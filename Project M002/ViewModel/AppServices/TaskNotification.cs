using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.ViewModel.AppServices
{
    public class TaskNotification : IAppServices<TaskNotification>
    {
        private static TaskNotification _taskNotification =  new TaskNotification();
        public static TaskNotification GetSingleInstance() => _taskNotification;
        private TaskNotification() { }
        public void Start()
        {
            
            throw new NotImplementedException();
        }

        

    }
}
