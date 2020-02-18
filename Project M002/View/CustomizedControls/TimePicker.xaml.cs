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

namespace MyToDos.View.CustomizedControls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TimeOfDayProperty = DependencyProperty.Register("TimeOfDay", typeof(TimeSpan), typeof(TimePicker), new UIPropertyMetadata(new TimeSpan(), TimeOfDayPropertyChanged));
        private static void TimeOfDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpan timeOfDay = (TimeSpan)e.NewValue;
            TimePicker timePicker = d as TimePicker;

            timePicker.RaiseEvent(new RoutedEventArgs(TimeOfDayChangedEvent));
        }
        public TimeSpan TimeOfDay
        {
            set
            {
                if(value != TimeOfDay)
                {
                    SetValue(TimeOfDayProperty, value);
                }
            }
            get => (TimeSpan) GetValue(TimeOfDayProperty);
        }
        public static readonly RoutedEvent TimeOfDayChangedEvent = EventManager.RegisterRoutedEvent("TimeOfDayChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimePicker));
        public event RoutedEventHandler TimeOfDayChanged
        {
            add { AddHandler(TimeOfDayChangedEvent, value); }
            remove { RemoveHandler(TimeOfDayChangedEvent, value); }
        }
    }
}
