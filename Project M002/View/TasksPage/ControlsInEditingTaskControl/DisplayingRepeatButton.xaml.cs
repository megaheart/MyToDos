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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDos.View.TasksPage.ControlsInEditingTaskControl
{
    /// <summary>
    /// Interaction logic for DisplayingRepeatButton.xaml
    /// </summary>
    public partial class DisplayingRepeatButton : UserControl
    {
        public DisplayingRepeatButton()
        {
            InitializeComponent();
            App.LanguageChanged += App_LanguageChanged;
        }
        private void App_LanguageChanged()
        {
            //Display UI of Date

        }
        private void OpenPage(object sender, RoutedEventArgs e)
        {

            
        }
        private void RemoveRepeat(object sender, RoutedEventArgs e)
        {

        }
    }
}
