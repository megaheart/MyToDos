using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MyToDos.ViewModel;
using Storage;

namespace MyToDos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient HttpClient { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            HttpClient = new HttpClient();
            App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            (new MainWindow()).ShowDialog();
            //MyToDos.Properties.Settings.Default.
            //DataManager.Current.Initialize().Wait();//.ContinueWith(t=> {
            //    if (t.IsFaulted) throw t.Exception;
            //    else MessageBox.Show("successful");
            //});
        }

        //private void Application_Exit(object sender, ExitEventArgs e)
        //{
        //    //thread.
        //}
        public static void DoEvent()
        {
            App.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate{ }));
        }
    }
}
