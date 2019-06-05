using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToDos
{
    /// <summary>
    /// Interaction logic for ConsoleLog.xaml
    /// </summary>
    public partial class ConsoleLog : Window
    {
        private static ConsoleLog console;
        public static void Initialize()
        {
            console = new ConsoleLog();
            console.Show();
        }
        public static void WriteLine(object text)
        {
            console.logs.Dispatcher.BeginInvoke(new Action(()=> {
                console.logs.Items.Add(text.ToString());
            }));
        }
        private ConsoleLog()
        {
            InitializeComponent();
        }
    }
}
