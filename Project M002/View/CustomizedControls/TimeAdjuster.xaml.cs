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
    /// Interaction logic for TimeAdjuster.xaml
    /// </summary>
    public partial class TimeAdjuster : UserControl
    {
        public TimeAdjuster()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TimeOfDayProperty = DependencyProperty.Register("TimeOfDay", typeof(TimeSpan), typeof(TimeAdjuster), new UIPropertyMetadata(new TimeSpan(), TimeOfDayPropertyChanged));
        private static void TimeOfDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //TimeSpan timeOfDay = (TimeSpan)e.NewValue;
            TimeAdjuster TimeAdjuster = d as TimeAdjuster;
            var time = (TimeSpan)e.NewValue;
            TimeAdjuster.Hour.Value = time.Hours;
            TimeAdjuster.Minute.Value = time.Minutes;
            TimeAdjuster.RaiseEvent(new RoutedEventArgs(TimeOfDayChangedEvent));
        }
        //private bool isUserChanged = false;
        public void RefreshTimeOfDay()
        {
            SetValue(TimeOfDayProperty, new TimeSpan(Hour.Value, Minute.Value, 0));
            //isUserChanged = false;
        }
        public TimeSpan TimeOfDay
        {
            set => SetValue(TimeOfDayProperty, value);
            get => new TimeSpan(Hour.Value, Minute.Value, 0);//(TimeSpan) GetValue(TimeOfDayProperty);
        }
        public static readonly RoutedEvent TimeOfDayChangedEvent = EventManager.RegisterRoutedEvent("TimeOfDayChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimeAdjuster));
        public event RoutedEventHandler TimeOfDayChanged
        {
            add { AddHandler(TimeOfDayChangedEvent, value); }
            remove { RemoveHandler(TimeOfDayChangedEvent, value); }
        }
        public static readonly RoutedEvent OKOrCancelPressedEvent = EventManager.RegisterRoutedEvent("OKOrCancelPressed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimeAdjuster));
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
            bool result = int.TryParse(newText, out value) && value > -1 && value < 24;

            return result;
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
