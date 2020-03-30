using System;
using System.Collections.Generic;
using System.Globalization;
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
            //TimeSpan timeOfDay = (TimeSpan)e.NewValue;
            TimePicker timePicker = d as TimePicker;
            var time = (TimeSpan)e.NewValue;
            timePicker.Hour.Value = time.Hours;
            timePicker.Minute.Value = time.Minutes;
            timePicker.RaiseEvent(new RoutedEventArgs(TimeOfDayChangedEvent));
        }
        public TimeSpan TimeOfDay
        {
            set => SetValue(TimeOfDayProperty, value);
            get => (TimeSpan) GetValue(TimeOfDayProperty);
        }
        public static readonly RoutedEvent TimeOfDayChangedEvent = EventManager.RegisterRoutedEvent("TimeOfDayChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimePicker));
        public event RoutedEventHandler TimeOfDayChanged
        {
            add { AddHandler(TimeOfDayChangedEvent, value); }
            remove { RemoveHandler(TimeOfDayChangedEvent, value); }
        }
        public static readonly RoutedEvent OKOrCancelPressedEvent = EventManager.RegisterRoutedEvent("OKOrCancelPressed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimePicker));
        public event RoutedEventHandler OKOrCancelPressed
        {
            add { AddHandler(OKOrCancelPressedEvent, value); }
            remove { RemoveHandler(OKOrCancelPressedEvent, value); }
        }
        public void OK(object sender, RoutedEventArgs e)
        {
            TimeOfDay = new TimeSpan(Hour.Value, Minute.Value, 0);
            RaiseEvent(new RoutedEventArgs(OKOrCancelPressedEvent));
        }
        public void Cancel(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OKOrCancelPressedEvent));
        }

        private bool IsHour(string newText, string key)
        {
            int value;
            return int.TryParse(newText, out value) && value > -1 && value < 24;
        }

        private bool IsMinute(string newText, string key)
        {
            int value;
            return int.TryParse(newText, out value) && value > -1 && value < 60;
        }

        private void HourTxt_Changed(object sender, RoutedEventArgs e)
        {
            Hour.Value = int.Parse(HourTxt.Text);
        }

        private void MinuteTxt_Changed(object sender, RoutedEventArgs e)
        {
            Minute.Value = int.Parse(MinuteTxt.Text);
        }

        private void HourValue_Changed(object sender, RoutedEventArgs e)
        {
            HourTxt.Text = Hour.Value.ToString();
        }
        private void MinuteValue_Changed(object sender, RoutedEventArgs e)
        {
            MinuteTxt.Text = Minute.Value.ToString();
        }
    }
    //public class NumberToText : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return value.ToString();
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return int.Parse(value.ToString());
    //    }
    //}
}
