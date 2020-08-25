using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            App.LanguageChanged += App_LanguageChanged;
        }
        private void App_LanguageChanged()
        {
            //Display UI of Date
            if (SelectedDate.HasValue)
            {
                var time = SelectedDate.Value.Date - System.DateTime.Now.Date;
                if (time > TimeSpan.Zero)//Future
                {
                    if (time.Days == 1) DateTimeTxt.Text = FindResource("Lang_Tomorrow").ToString();
                    else
                        DateTimeTxt.Text = string.Format(FindResource("Lang_DateTimePicker_001").ToString(), time.Days);
                }
                else if (time == TimeSpan.Zero)//Today
                {
                    DateTimeTxt.Text = FindResource("Lang_Today").ToString();
                }
                else//Past
                {
                    if (time.Days == -1) DateTimeTxt.Text = FindResource("Lang_Yesterday").ToString();
                    else
                        DateTimeTxt.Text = string.Format(FindResource("Lang_DateTimePicker_002").ToString(), -time.Days);
                }
                DateTimeNumber.Text = SelectedDate.Value.ToString("dd/MM/yyyy");

            }
            else
            {
                TitleTxt.Text = FindResource("Lang_Pick").ToString() + Title;
            }
        }



        public FontFamily IconFontFamily
        {
            get => Icon.FontFamily;
            set => Icon.FontFamily = value;
        }
        public string IconContent
        {
            get => Icon.Text;
            set => Icon.Text = value;
        }
        public double IconFontSize
        {
            get => Icon.FontSize;
            set => Icon.FontSize = value;
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DatePicker), new UIPropertyMetadata("Date", TitlePropertyChanged));
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DatePicker ctrl = d as DatePicker;
            string title = e.NewValue.ToString();
            if (ctrl.SelectedDate.HasValue)
            {
                ctrl.TitleTxt.Text = title + ":";
            }
            else
            {
                ctrl.TitleTxt.Text = ctrl.FindResource("Lang_Pick").ToString() + " " + title;
            }
        }
        public string Title
        {
            set => SetValue(TitleProperty, value);
            get => (string)GetValue(TitleProperty);
        }
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new UIPropertyMetadata(null, SelectedDatePropertyChanged));
        private static void SelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DatePicker ctrl = d as DatePicker;
            DateTime? newDatetime = (DateTime?)e.NewValue;
            DateTime? oldDatetime = (DateTime?)e.OldValue;
            if (newDatetime.HasValue)
            {
                if (!oldDatetime.HasValue)
                {
                    ctrl.TitleTxt.Text = ctrl.Title + ":";
                    ctrl.DateTimeNumber.Visibility = Visibility.Visible;
                    ctrl.RemoveDateBtn.Visibility = Visibility.Visible;
                }
                var time = newDatetime.Value.Date - System.DateTime.Now.Date;
                if (time > TimeSpan.Zero)//Future
                {
                    if (time.Days == 1) ctrl.DateTimeTxt.Text = ctrl.FindResource("Lang_Tomorrow").ToString();
                    else
                        ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_DateTimePicker_001").ToString(), time.Days);
                }
                else if (time == TimeSpan.Zero)//Today
                {
                    ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_Today").ToString() + ctrl.FindResource("Lang_DateTimePicker_003").ToString(),
                        null, newDatetime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                else//Past
                {
                    if (time.Days == -1) ctrl.DateTimeTxt.Text = ctrl.FindResource("Lang_Yesterday").ToString();
                    else
                        ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_DateTimePicker_002").ToString(), -time.Days);
                }
                ctrl.DateTimeNumber.Text = newDatetime.Value.ToString("dd/MM/yyyy");

            }
            else
            {
                ctrl.TitleTxt.Text = ctrl.FindResource("Lang_Pick").ToString() + " " + ctrl.Title;
                ctrl.DateTimeTxt.Text = "";
                ctrl.DateTimeNumber.Visibility = Visibility.Collapsed;
                ctrl.RemoveDateBtn.Visibility = Visibility.Collapsed;
            }
            ctrl.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent));
        }
        public DateTime? SelectedDate
        {
            set => SetValue(SelectedDateProperty, value);
            get => (DateTime?)GetValue(SelectedDateProperty);
        }
        //private bool SetDateTimePrivately = false;
        //private void PrivateSetDateTime(DateTime? datetime)
        //{
        //    SetDateTimePrivately = true;
        //    DateTime = datetime;
        //    SetDateTimePrivately = false;
        //}
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DatePicker));
        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }

        private void OpenPopupBox(object sender, RoutedEventArgs e)
        {
            DateAdjusterControl.SynchronizeMode();
            if (SelectedDate.HasValue)
            {
                DateAdjusterControl.SelectedDate = SelectedDate;
            }
            else
            {
                DateTime now = DateTime.Now;
                DateAdjusterControl.SelectedDate = now;
            }
            PopupBox.IsOpen = true;
        }
        private void RemoveSelectedDate(object sender, RoutedEventArgs e)
        {
            SelectedDate = null;
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            DateAdjusterControl.RefreshSelectedDate();
            SelectedDate = DateAdjusterControl.SelectedDate.Value;
            PopupBox.IsOpen = false;
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            PopupBox.IsOpen = false;
        }
    }
}

