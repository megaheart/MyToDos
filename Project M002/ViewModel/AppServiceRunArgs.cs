using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.ViewModel
{
    class AppServiceRunArgs
    {
        public BackgroundServices Sender { get; private set; }
        public DateTime LatestStartupDate { get; private set; }
        public AppServiceRunArgs(BackgroundServices sender, DateTime latestStartupDate)
        {
            Sender = sender;
            LatestStartupDate = latestStartupDate;
        }
        
    }
}
