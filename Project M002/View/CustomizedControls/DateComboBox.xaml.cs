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
    /// Interaction logic for DateComboBox.xaml
    /// </summary>
    public partial class DateComboBox : UserControl
    {
        public DateComboBox()
        {
            InitializeComponent();
            int year = DateTime.Now.Year;
            YearBox.MinValue = year;
            YearBox.MaxValue = year + 20;
            SelectedDate = DateTime.Now;
        }
        public DateTime SelectedDate
        {
            set
            {
                YearBox.Value = value.Year;
                MonthBox.Value = value.Month;
                DayBox.Value = value.Day;
            }
            get
            {
                //MessageBox.Show(string.Format("{0}/{1}/{2}", DayBox.Value, MonthBox.Value, YearBox.Value));
                return new DateTime(YearBox.Value, MonthBox.Value, DayBox.Value);
            }
        }
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChangedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateComboBox));
        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }
        public void YearBoxValueChanged(object sender, RoutedEventArgs e)
        {
            if (MonthBox.Value == 2)
            {
                if (YearBox.Value % 4 == 0) DayBox.MaxValue = 29;
                else DayBox.MaxValue = 28;
            }
            RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent));
        }
        public void MonthBoxValueChanged(object sender, RoutedEventArgs e)
        {
            switch (MonthBox.Value)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    DayBox.MaxValue = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    DayBox.MaxValue = 30;
                    break;
                case 2:
                    if(YearBox.Value%4==0) DayBox.MaxValue = 29;
                    else DayBox.MaxValue = 28;
                    break;
            }
            RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent));
        }
        public void DayBoxValueChanged(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent));
        }
    }
}
