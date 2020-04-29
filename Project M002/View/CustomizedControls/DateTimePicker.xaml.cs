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
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
            App.LanguageChanged += App_LanguageChanged;
        }

        private void App_LanguageChanged()
        {
            if (SelectedDateTime.HasValue)
            {
                var time = SelectedDateTime.Value.Date - System.DateTime.Now.Date;
                if (time > TimeSpan.Zero)//Future
                {
                    if (time.Days == 1) DateTimeTxt.Text = string.Format(FindResource("Lang_Tomorrow").ToString() + FindResource("Lang_DateTimePicker_003").ToString(),
                        null, SelectedDateTime.Value.TimeOfDay.ToString("hh\\:mm"));
                    DateTimeTxt.Text = string.Format(FindResource("Lang_DateTimePicker_001").ToString() + FindResource("Lang_DateTimePicker_003").ToString(),
                        time.Days, SelectedDateTime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                else if (time == TimeSpan.Zero)//Today
                {
                    DateTimeTxt.Text = string.Format(FindResource("Lang_Today").ToString() + FindResource("Lang_DateTimePicker_003").ToString(),
                        null, SelectedDateTime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                else//Past
                {
                    if (time.Days == -1) DateTimeTxt.Text = string.Format(FindResource("Lang_Yesterday").ToString() + FindResource("Lang_DateTimePicker_003").ToString(),
                        null, SelectedDateTime.Value.TimeOfDay.ToString("hh\\:mm"));
                    DateTimeTxt.Text = string.Format(FindResource("Lang_DateTimePicker_002").ToString() + FindResource("Lang_DateTimePicker_003").ToString(),
                        -time.Days, SelectedDateTime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                DateTimeNumber.Text = SelectedDateTime.Value.ToString("HH:mm dd/MM/yyyy");

            }
            else
            {
                TitleTxt.Text = FindResource("Lang_Pick").ToString() + Title;
            }
            if (DateTimeTabControl.SelectedIndex == 1)
            {
                DateTitle.Text = FindResource("Lang_Date").ToString();
            }
            else
            {
                TimeTitle.Text = FindResource("Lang_Time").ToString();
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
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DateTimePicker), new UIPropertyMetadata("DateTime", TitlePropertyChanged));
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker ctrl = d as DateTimePicker;
            string title = e.NewValue.ToString();
            if (ctrl.SelectedDateTime.HasValue)
            {
                ctrl.TitleTxt.Text = title + ":";
            }
            else
            {
                ctrl.TitleTxt.Text = ctrl.FindResource("Lang_Pick").ToString() + title;
            }
        }
        public string Title
        {
            set => SetValue(TitleProperty, value);
            get => (string)GetValue(TitleProperty);
        }
        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(DateTimePicker), new UIPropertyMetadata(null, SelectedDateTimePropertyChanged));
        private static void SelectedDateTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker ctrl = d as DateTimePicker;
            DateTime? newDatetime = (DateTime?)e.NewValue;
            DateTime? oldDatetime = (DateTime?)e.OldValue;
            if (newDatetime.HasValue)
            {
                if(!oldDatetime.HasValue)
                {
                    ctrl.TitleTxt.Text = ctrl.Title + ":";
                    ctrl.DateTimeNumber.Visibility = Visibility.Visible;
                }
                var time = newDatetime.Value.Date - System.DateTime.Now.Date;
                if(time > TimeSpan.Zero)//Future
                {
                    if (time.Days == 1) ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_Tomorrow").ToString() + ctrl.FindResource("Lang_DateTimePicker_003").ToString(),
                        null, newDatetime.Value.TimeOfDay.ToString("hh\\:mm"));
                    ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_DateTimePicker_001").ToString() + ctrl.FindResource("Lang_DateTimePicker_003").ToString(),
                        time.Days, newDatetime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                else if(time == TimeSpan.Zero)//Today
                {
                    ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_Today").ToString() + ctrl.FindResource("Lang_DateTimePicker_003").ToString(),
                        null, newDatetime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                else//Past
                {
                    if (time.Days == -1) ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_Yesterday").ToString() + ctrl.FindResource("Lang_DateTimePicker_003").ToString(),
                        null, newDatetime.Value.TimeOfDay.ToString("hh\\:mm"));
                    ctrl.DateTimeTxt.Text = string.Format(ctrl.FindResource("Lang_DateTimePicker_002").ToString() + ctrl.FindResource("Lang_DateTimePicker_003").ToString(),
                        -time.Days, newDatetime.Value.TimeOfDay.ToString("hh\\:mm"));
                }
                ctrl.DateTimeNumber.Text = newDatetime.Value.ToString("HH:mm dd/MM/yyyy");

            }
            else
            {
                ctrl.TitleTxt.Text = ctrl.FindResource("Lang_Pick").ToString() + ctrl.Title;
                ctrl.DateTimeNumber.Visibility = Visibility.Collapsed;
            }
            ctrl.RaiseEvent(new RoutedEventArgs(SelectedDateTimeChangedEvent));
        }
        public DateTime? SelectedDateTime
        {
            set => SetValue(SelectedDateTimeProperty, value);
            get => (DateTime?)GetValue(SelectedDateTimeProperty);
        }
        //private bool SetDateTimePrivately = false;
        //private void PrivateSetDateTime(DateTime? datetime)
        //{
        //    SetDateTimePrivately = true;
        //    DateTime = datetime;
        //    SetDateTimePrivately = false;
        //}
        public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateTimeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateTimePicker));
        public event RoutedEventHandler SelectedDateTimeChanged
        {
            add { AddHandler(SelectedDateTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedDateTimeChangedEvent, value); }
        }
        
        private void OpenPopupBox(object sender, RoutedEventArgs e)
        {
            if (SelectedDateTime.HasValue)
            {
                DateAdjusterControl.SelectedDate = SelectedDateTime;
                TimeAdjusterControl.TimeOfDay = SelectedDateTime.Value.TimeOfDay;
            }
            else
            {
                DateTime now = DateTime.Now;
                DateAdjusterControl.SelectedDate = now;
                TimeAdjusterControl.TimeOfDay = TimeSpan.Zero;
            }
            DateTimeTabControl.SelectedIndex = 1;
            PopupBox.IsOpen = true;
        }
        private void RemoveSelectedDateTime(object sender, RoutedEventArgs e)
        {
            Button removeBtn = sender as Button;
            removeBtn.Visibility = Visibility.Collapsed;
            SelectedDateTime = null;
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = DateAdjusterControl.SelectedDate.Value.Add(TimeAdjusterControl.TimeOfDay);
            PopupBox.IsOpen = false;
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            PopupBox.IsOpen = false;
        }

        private void CustomizedTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomizedTabControl tabControl = sender as CustomizedTabControl;
            if(tabControl.SelectedIndex == 1)
            {
                DateTitle.Text = FindResource("Lang_Date").ToString();
                TimeTitle.Text = TimeAdjusterControl.TimeOfDay.ToString("hh\\:mm");
            }
            else
            {
                DateTitle.Text = DateAdjusterControl.SelectedDate.Value.ToString("dd/MM/yyyy");
                TimeTitle.Text = FindResource("Lang_Time").ToString();
            }
        }
    }
}
