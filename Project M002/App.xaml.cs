using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MyToDos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //TimeSpan time = TimeSpan.Zero;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //thread.
        }
        public static void DoEvent()
        {
            App.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate{ }));
        }
    }
}
