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
        public AppServiceRunArgs(BackgroundServices sender)
        {
            Sender = sender;
        }
        
    }
}
