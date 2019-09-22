using MyToDos.ViewModel.AppServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MyToDos.ViewModel
{
    class BackgroundServices
    {
        private DispatcherTimer timer;
        /// <summary>
        /// Executed at dd:hh:mm:00
        /// </summary>
        public event Action<AppServiceRunArgs> ExecuteOncePerMinute;
        /// <summary>
        /// Executed at dd:00:00:00
        /// </summary>
        public event Action<AppServiceRunArgs> ExecuteOncePerDay;
        private AppServiceRunArgs BackgroundServiceRunArgs;
        public BackgroundServices()
        {
            BackgroundServiceRunArgs = new AppServiceRunArgs(this);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimeLoop;
        }
        public async Task Initialize()
        {
            //Services start up
            WeatherUpdate.Start(BackgroundServiceRunArgs);
            TaskNotification.Start(BackgroundServiceRunArgs);

            Cleaner.Start(BackgroundServiceRunArgs);//Always Run Finally
            //Timer starts up
            DateTime now = DateTime.Now;
            await Task.Delay((int)checked(60000 - now.Ticks / 10000 % 60000));
            timer.Start();
            //TimeLoop(timer, new EventArgs());
        }
        private void TimeLoop(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (now.Hour == 0 && now.Minute == 0) ExecuteOncePerDay?.Invoke(BackgroundServiceRunArgs);
            ExecuteOncePerMinute?.Invoke(BackgroundServiceRunArgs);
        }
    }
}
