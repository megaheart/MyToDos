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
using Storage.Model;

namespace MyToDos.View.TasksPage.ControlsInAddingTaskControl
{
    /// <summary>
    /// Interaction logic for EditingRepeatControl.xaml
    /// </summary>
    public partial class EditingRepeatControl : UserControl
    {
        public EditingRepeatControl()
        {
            InitializeComponent();
            App.LanguageChanged += App_LanguageChanged;
        }
        private void App_LanguageChanged()
        {
            //Display UI of Date
            
        }

        public static readonly DependencyProperty RepeatProperty = DependencyProperty.Register("Repeat", typeof(Repeat), typeof(EditingRepeatControl), new UIPropertyMetadata(null, RepeatPropertyChanged));
        private static void RepeatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EditingRepeatControl ctrl = d as EditingRepeatControl;
            
            ctrl.RaiseEvent(new RoutedEventArgs(RepeatChangedEvent));
        }
        public Repeat Repeat
        {
            set => SetValue(RepeatProperty, value);
            get => (Repeat)GetValue(RepeatProperty);
        }
        public static readonly RoutedEvent RepeatChangedEvent = EventManager.RegisterRoutedEvent("RepeatChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EditingRepeatControl));
        public event RoutedEventHandler RepeatChanged
        {
            add { AddHandler(RepeatChangedEvent, value); }
            remove { RemoveHandler(RepeatChangedEvent, value); }
        }

        private void OpenPopupBox(object sender, RoutedEventArgs e)
        {
            
            PopupBox.IsOpen = true;
        }
        private void RemoveRepeat(object sender, RoutedEventArgs e)
        {
            
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            
            PopupBox.IsOpen = false;
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            PopupBox.IsOpen = false;
        }
    }
}
