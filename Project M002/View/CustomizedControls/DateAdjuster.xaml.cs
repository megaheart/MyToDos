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
using System.Diagnostics;

namespace MyToDos.View.CustomizedControls
{
    /// <summary>
    /// Must use SynchronizeMode() void
    /// </summary>
    public partial class DateAdjuster : UserControl
    {
        public DateAdjuster()
        {
            InitializeComponent();
            Calendar.SelectedDateChanged += (sender, e) =>
            {
                IsValueChanged = true;
            };
            ComboBox.SelectedDateChanged += (sender, e) =>
            {
                IsValueChanged = true;
            };
            ModeControl.HeaderPanel_PreviewMouseLeftButtonDown += () =>
            {
                DidUILoad = true;
            };
        }
        private bool DidUILoad = false;
        private bool IsValueChanged = false;
        //private static Action modeChanged;
        private static int mode = -1;
        /// <summary>
        /// Synchronize mode with user's habit
        /// </summary>
        public void SynchronizeMode()
        {
            //return;
            if (mode == -1)
            {
                Task<string> getValue = DataManager.Current.GetValueAsync("modeOfDateAdjuster");
                getValue.ConfigureAwait(false);
                getValue.ContinueWith(t =>
                {
                    mode = int.Parse(t.Result);
                    ModeControl.Dispatcher.Invoke(() =>
                    {
                        ModeControl.SelectedIndex = mode;
                    });
                });
            }
            else ModeControl.SelectedIndex = mode;
        }
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DateAdjuster), new UIPropertyMetadata(null, SelectedDatePropertyChanged));
        private static void SelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //TimeSpan timeOfDay = (TimeSpan)e.NewValue;
            DateAdjuster DateAdjuster = d as DateAdjuster;
            DateTime? date = e.NewValue as DateTime?;
            if (date == null)
            {
                DateAdjuster.Calendar.SelectedDate = null;
                DateAdjuster.ComboBox.SelectedDate = DateTime.Now;
            }
            else
            {
                DateAdjuster.ComboBox.SelectedDate = date.Value;
                DateAdjuster.Calendar.SelectedDate = date;
            }
            DateAdjuster.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent));
        }
        public void RefreshSelectedDate()
        {
            if (mode != ModeControl.SelectedIndex) throw new Exception("ModeControl.Mode of DateAdjuster is not synchronized");
            if (mode == 0)
            {
                SelectedDate = Calendar.SelectedDate;
            }
            else SelectedDate = ComboBox.SelectedDate;
            IsValueChanged = false;
        }
        /// <summary>
        /// Please use DateAdjuster.RefreshSelectedDate() function before to get correct value, 
        /// sorry about this inconvenience
        /// </summary>
        public DateTime? SelectedDate
        {
            set {
                SetValue(SelectedDateProperty, value);
            }
            get {

                if (IsValueChanged) throw new Exception("Please use DateAdjuster.RefreshSelectedDate() function before to get correct value, sorry about this inconvenience.");
                return (DateTime?)GetValue(SelectedDateProperty); 
            }
        }
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateAdjuster));
        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }
        public static readonly RoutedEvent OKOrCancelPressedEvent = EventManager.RegisterRoutedEvent("OKOrCancelPressed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateAdjuster));
        public event RoutedEventHandler OKOrCancelPressed
        {
            add { AddHandler(OKOrCancelPressedEvent, value); }
            remove { RemoveHandler(OKOrCancelPressedEvent, value); }
        }
        public void OK(object sender, RoutedEventArgs e)
        {
            if (mode != ModeControl.SelectedIndex) throw new Exception("ModeControl.Mode of DateAdjuster is not synchronized");
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

        private void ModeControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mode == -1) return;
            if (ModeControl.SelectedIndex == 0)
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
            if (DidUILoad & mode != ModeControl.SelectedIndex)
            {
                mode = ModeControl.SelectedIndex;
                DataManager.Current.SetValueAsync("modeOfDateAdjuster", mode.ToString()).ConfigureAwait(false);
            }
        }
    }
}
