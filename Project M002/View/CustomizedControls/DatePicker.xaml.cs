using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Storage;
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
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl
    {
        public DatePicker()
        {
            InitializeComponent();
            if (mode == -1)
            {
                DataManager.Current.GetValueAsync("modeOfDatePicker").ContinueWith(t =>
                {
                    mode = int.Parse(t.Result);
                    ModeControl.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,(Action) (() =>
                    {
                        ModeControl.Mode = mode;
                    }));
                    //MessageBox.Show(t.Result);
                });
            }
            else ModeControl.Mode = mode;
        }

        //private static Action modeChanged;
        private static int mode = -1;
        public void Refresh()
        {
            ModeControl.Mode = mode;
            //if (ModeControl.Mode != mode)
            //{
            //    ModeControl.Mode = mode;
            //    if(mode == 0)
            //    {
            //        Calendar.SelectedDate = SelectedDate;
            //    }
            //    else
            //    {
            //        if (SelectedDate.HasValue)
            //        {
            //            ComboBox.SelectedDate = SelectedDate.Value;
            //        }
            //        else
            //        {
            //            ComboBox.SelectedDate = DateTime.Now;
            //        }
            //    }
            //}
        }
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new UIPropertyMetadata(null, SelectedDatePropertyChanged));
        private static void SelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //TimeSpan timeOfDay = (TimeSpan)e.NewValue;
            DatePicker datePicker = d as DatePicker;
            DateTime? date = e.NewValue as DateTime?;
            if (date == null)
            {
                datePicker.Calendar.SelectedDate = null;
                datePicker.ComboBox.SelectedDate = DateTime.Now;
            }
            else
            {
                datePicker.ComboBox.SelectedDate = date.Value;
                datePicker.Calendar.SelectedDate = date;
            }
            datePicker.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent));
        }
        public DateTime? SelectedDate
        {
            set => SetValue(SelectedDateProperty, value);
            get => (DateTime?)GetValue(SelectedDateProperty);
        }
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DatePicker));
        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }
        public static readonly RoutedEvent OKOrCancelPressedEvent = EventManager.RegisterRoutedEvent("OKOrCancelPressed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DatePicker));
        public event RoutedEventHandler OKOrCancelPressed
        {
            add { AddHandler(OKOrCancelPressedEvent, value); }
            remove { RemoveHandler(OKOrCancelPressedEvent, value); }
        }
        public void OK(object sender, RoutedEventArgs e)
        {
            if (mode != ModeControl.Mode) throw new Exception("ModeControl.Mode of DatePicker is not synchronized");
            if (mode == 0)
            {
                SelectedDate = Calendar.SelectedDate;
            }
            else SelectedDate = ComboBox.SelectedDate;
            RaiseEvent(new RoutedEventArgs(OKOrCancelPressedEvent));
        }
        public void Cancel(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OKOrCancelPressedEvent));
        }

        private void ModeControl_ModeChanged(object sender, ModeChangedEventArgs e)
        {
            if(mode != e.NewValue)
            {
                mode = e.NewValue;
                DataManager.Current.SetValueAsync("modeOfDatePicker", mode.ToString());
            }
            if (mode == 0)
            {
                Calendar.SelectedDate = ComboBox.SelectedDate;
            }
            else
            {
                if (Calendar.SelectedDate.HasValue)
                {
                    ComboBox.SelectedDate = Calendar.SelectedDate.Value;
                }
                else
                {
                    ComboBox.SelectedDate = DateTime.Now;
                }
            }
            //MessageBox.Show(mode.ToString());
        }
    }
}
