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
    public partial class Calendar : UserControl
    {
        private static readonly string[] nameOfEnglishMonth = new string[] {"January", "February", "March", "April", "May", "June", "July",
            "August", "September", "October", "November", "December" };
        private static readonly string[] shortNameOfEnglishMonth = new string[] {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
            "Aug", "Sep", "Oct", "Nov", "Dec" };
        private CalendarDayButton[] dayButtonList;
        private CalendarMonthButton[] monthButtonList;
        private CalendarDayButton SelectedDayButton;
        CalendarDayButton TodayButton;
        private int currentYearInGridOfDates;
        private int currentYearInGridOfMonths;
        private int currentMonthInGridOfDates;
        public static DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(Calendar), new PropertyMetadata(null, SelectedDatePropertyChanged, null));
        public DateTime? SelectedDate
        {
            set
            {
                SetValue(SelectedDateProperty, value);
            }
            get => (DateTime?)GetValue(SelectedDateProperty);
        }
        private static void SelectedDatePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Calendar calendar = (Calendar)dp;
            DateTime? newValue = (DateTime?)e.NewValue;
            DateTime? oldValue = (DateTime?)e.OldValue;
            if (oldValue != newValue)
            {
                if (newValue.HasValue)
                {
                    if (newValue.Value < calendar.MinValue || newValue.Value > calendar.MaxValue)
                        throw new Exception("Can't set SelectedDate less then MinValue or more then MaxValue");
                    else if ((calendar.currentMonthInGridOfDates == newValue.Value.Month) && (calendar.currentYearInGridOfDates == newValue.Value.Year))
                    {
                        if (calendar.SelectedDayButton != null)
                        {
                            calendar.SelectedDayButton.IsSelected = false;
                            calendar.SelectedDayButton = null;
                        }
                        CalendarDayButton btn = calendar.dayButtonList[newValue.Value.Subtract(calendar.dayButtonList[0].Date).Days];
                        btn.IsSelected = true;
                        calendar.SelectedDayButton = btn;
                    }
                    else calendar.CalendarMoveToMonth(newValue.Value.Month, newValue.Value.Year);
                }
                else if (calendar.SelectedDayButton != null)
                {
                    calendar.SelectedDayButton.IsSelected = false;
                    calendar.SelectedDayButton = null;
                }
            }
        }
        public static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(DateTime), typeof(Calendar), new PropertyMetadata(DateTime.MinValue, MinValuePropertyChanged, null));
        /// <summary>
        /// Default Value: DateTime.MinValue
        /// </summary>
        public DateTime MinValue
        {
            set
            {
                SetValue(MinValueProperty, value);
            }
            get => (DateTime)GetValue(MinValueProperty);
        }
        private static void MinValuePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Calendar calendar = (Calendar)dp;
            DateTime newValue = (DateTime)e.NewValue;
            DateTime oldValue = (DateTime)e.OldValue;
            if (oldValue != newValue)
            {
                if (newValue > calendar.MaxValue)
                    throw new Exception("Can't set MinValue more then MaxValue");
                else if (calendar.SelectedDate.HasValue && (newValue > calendar.SelectedDate))
                    throw new Exception("Can't set MinValue more then SelectedDate");
                int sum = calendar.currentYearInGridOfDates * 12 + calendar.currentMonthInGridOfDates;
                DateTime theFirstDayOfNextMonthInGridOfDates = new DateTime(sum / 12, sum % 12 + 1, 1);
                if (newValue >= theFirstDayOfNextMonthInGridOfDates)
                    calendar.CalendarMoveToMonth(newValue.Month, newValue.Year);
                else calendar.CalendarMoveToMonth(calendar.currentMonthInGridOfDates, calendar.currentYearInGridOfDates);
                if (calendar.GridOfMonths.Visibility == Visibility.Visible)
                {
                    if (calendar.currentYearInGridOfMonths <= newValue.Year)
                        calendar.GridOfMonthsMoveToOtherYear(newValue.Year - calendar.currentYearInGridOfMonths);
                    else calendar.GridOfMonthsMoveToOtherYear(0);
                }
            }
        }
        public static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(DateTime), typeof(Calendar), new PropertyMetadata(DateTime.MaxValue, MaxValuePropertyChanged, null));
        /// <summary>
        /// Default Value: DateTime.MaxValue
        /// </summary>
        public DateTime MaxValue
        {
            set
            {
                SetValue(MaxValueProperty, value);
            }
            get => (DateTime)GetValue(MaxValueProperty);
        }
        private static void MaxValuePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Calendar calendar = (Calendar)dp;
            DateTime newValue = (DateTime)e.NewValue;
            DateTime oldValue = (DateTime)e.OldValue;
            if (oldValue != newValue)
            {
                if (newValue < calendar.MinValue)
                    throw new Exception("Can't set MaxValue less then MinValue");
                else if (calendar.SelectedDate.HasValue && (newValue < calendar.SelectedDate))
                    throw new Exception("Can't set MaxValue less then SelectedDate");
                DateTime theFirstDayOfCurrentMonthInGridOfDates = new DateTime(calendar.currentYearInGridOfDates, calendar.currentMonthInGridOfDates, 1);
                if (newValue < theFirstDayOfCurrentMonthInGridOfDates)
                    calendar.CalendarMoveToMonth(newValue.Month, newValue.Year);
                else calendar.CalendarMoveToMonth(calendar.currentMonthInGridOfDates, calendar.currentYearInGridOfDates);
                if (calendar.GridOfMonths.Visibility == Visibility.Visible)
                {
                    if (calendar.currentYearInGridOfMonths >= newValue.Year)
                        calendar.GridOfMonthsMoveToOtherYear(newValue.Year - calendar.currentYearInGridOfMonths);
                    else calendar.GridOfMonthsMoveToOtherYear(0);
                }
            }
        }
        public Calendar()
        {
            InitializeComponent();
            //Initialize Data Steps
            dayButtonList = new CalendarDayButton[42];
            monthButtonList = new CalendarMonthButton[12];
            for (int i = 0; i < 42; i++)
            {
                dayButtonList[i] = GridOfDates.Children[7 + i] as CalendarDayButton;
                dayButtonList[i].Click += CalendarDayButtonClick;
            }
            for (int i = 0; i < 12; i++)
            {
                monthButtonList[i] = GridOfMonths.Children[i] as CalendarMonthButton;
                monthButtonList[i].Click += CalendarMonthButtonClick;
                monthButtonList[i].Content = shortNameOfEnglishMonth[i];
                monthButtonList[i].Month = i + 1;
            }

            //Initialize Interface
            DateTime today = DateTime.Today;
            CalendarMoveToMonth(today.Month, today.Year);
        }
        private void CalendarMoveToMonth(int month, int year)
        {
            currentMonthInGridOfDates = month;
            currentYearInGridOfDates = year;
            YearOverViewButton.Content = nameOfEnglishMonth[month - 1] + ", " + year;
            DateTime firstDateOfMonth = new DateTime(year, month, 1);
            int startOfMonthIndex = (int)firstDateOfMonth.DayOfWeek;
            for (int i = 0; i < startOfMonthIndex; i++)
            {
                dayButtonList[i].IsOutOfMonth = true;
                dayButtonList[i].Date = firstDateOfMonth.AddDays(i - startOfMonthIndex);
                if (dayButtonList[i].Date < MinValue || dayButtonList[i].Date > MaxValue)
                    dayButtonList[i].IsEnabled = false;
                else dayButtonList[i].IsEnabled = true;
            }
            int endOfMonthIndex = firstDateOfMonth.AddMonths(1).AddDays(-1).Day + startOfMonthIndex;
            for (int i = startOfMonthIndex; i < endOfMonthIndex; i++)
            {
                dayButtonList[i].Date = firstDateOfMonth.AddDays(i - startOfMonthIndex);
                dayButtonList[i].IsOutOfMonth = false;
                if (dayButtonList[i].Date < MinValue || dayButtonList[i].Date > MaxValue)
                    dayButtonList[i].IsEnabled = false;
                else dayButtonList[i].IsEnabled = true;
            }
            for (int i = endOfMonthIndex; i < 42; i++)
            {
                dayButtonList[i].IsOutOfMonth = true;
                dayButtonList[i].Date = firstDateOfMonth.AddDays(i - startOfMonthIndex);
                if (dayButtonList[i].Date < MinValue || dayButtonList[i].Date > MaxValue)
                    dayButtonList[i].IsEnabled = false;
                else dayButtonList[i].IsEnabled = true;
            }
            DateTime today = DateTime.Today;
            if (month == today.Month && year == today.Year)
            {
                CalendarDayButton btn = dayButtonList[today.Subtract(firstDateOfMonth).Days + startOfMonthIndex];
                btn.IsToday = true;
                TodayButton = btn;
            }
            else if (TodayButton != null) TodayButton.IsToday = false;
            if (SelectedDayButton != null)
            {
                SelectedDayButton.IsSelected = false;
                SelectedDayButton = null;
            }
            if (SelectedDate.HasValue && month == SelectedDate.Value.Month && year == SelectedDate.Value.Year)
            {
                CalendarDayButton btn = dayButtonList[SelectedDate.Value.Subtract(firstDateOfMonth).Days + startOfMonthIndex];
                btn.IsSelected = true;
                SelectedDayButton = btn;
            }
            if (year == MinValue.Year && month == MinValue.Month)//Lock GoToPreviousMonthOrYear Button, Disable to move to previous month if currentTime is less then minValue
                GoToPreviousMonthOrYearButton.IsEnabled = false;
            else GoToPreviousMonthOrYearButton.IsEnabled = true;
            if (year == MaxValue.Year && month == MaxValue.Month)//Lock GoToNextMonthOrYear Button, Disable to move to next month if currentTime is more then minValue
                GoToNextMonthOrYearButton.IsEnabled = false;
            else GoToNextMonthOrYearButton.IsEnabled = true;
        }
        private void GridOfMonthsMoveToOtherYear(int year)
        {
            currentYearInGridOfMonths += year;
            YearOverViewButton.Content = currentYearInGridOfMonths.ToString();
            if (currentYearInGridOfMonths == MinValue.Year)//Lock GoToPreviousMonthOrYear Button
                GoToPreviousMonthOrYearButton.IsEnabled = false;
            else GoToPreviousMonthOrYearButton.IsEnabled = true;
            if (currentYearInGridOfMonths == MaxValue.Year)//Lock GoToNextMonthOrYear Button
                GoToNextMonthOrYearButton.IsEnabled = false;
            else GoToNextMonthOrYearButton.IsEnabled = true;
            if (currentYearInGridOfMonths - 5 < MinValue.Year)//Lock GoToFurtherPreviousMonthOrYear Button
                GoToFurtherPreviousYearButton.IsEnabled = false;
            else GoToFurtherPreviousYearButton.IsEnabled = true;
            if (currentYearInGridOfMonths + 5 > MaxValue.Year)//Lock GoToFurtherNextMonthOrYear Button
                GoToFurtherNextYearButton.IsEnabled = false;
            else GoToFurtherNextYearButton.IsEnabled = true;
            for (int i = 0; i < 12; i++)
            {
                if (currentYearInGridOfMonths == MinValue.Year && monthButtonList[i].Month < MinValue.Month)//Lock Month Button whose time is less then minValue
                    monthButtonList[i].IsEnabled = false;
                else monthButtonList[i].IsEnabled = true;
                if (currentYearInGridOfMonths == MaxValue.Year && monthButtonList[i].Month > MaxValue.Month)//Lock Month Button whose time is more then maxValue
                    monthButtonList[i].IsEnabled = false;
                else monthButtonList[i].IsEnabled = true;
            }
        }
        private void OverViewYear(object sender, RoutedEventArgs e)
        {
            if (GridOfDates.Visibility == Visibility.Visible)//Open GridOfMonths
            {
                GridOfMonths.Visibility = Visibility.Visible;
                GridOfDates.Visibility = Visibility.Hidden;
                GridOfMonthsMoveToOtherYear(currentYearInGridOfDates - currentYearInGridOfMonths);
            }
            else//Close Grid of month
            {
                currentYearInGridOfMonths = currentYearInGridOfDates;
                GridOfMonths.Visibility = Visibility.Collapsed;
                GridOfDates.Visibility = Visibility.Visible;
                YearOverViewButton.Content = nameOfEnglishMonth[currentMonthInGridOfDates - 1] + ", " + currentYearInGridOfDates;
            }
        }
        private void CalendarMonthButtonClick(object sender, RoutedEventArgs e)
        {
            CalendarMonthButton btn = sender as CalendarMonthButton;
            CalendarMoveToMonth(btn.Month, currentYearInGridOfMonths);
            GridOfMonths.Visibility = Visibility.Collapsed;
            GridOfDates.Visibility = Visibility.Visible;
        }
        private void CalendarDayButtonClick(object sender, RoutedEventArgs e)
        {
            CalendarDayButton btn = sender as CalendarDayButton;
            SelectedDate = btn.Date;
        }
        private void GoToFurtherPreviousYear(object sender, RoutedEventArgs e)
        {
            GridOfMonthsMoveToOtherYear(-5);
        }
        private void GoToFurtherNextYear(object sender, RoutedEventArgs e)
        {
            GridOfMonthsMoveToOtherYear(5);
        }
        private void GoToPreviousMonthOrYear(object sender, RoutedEventArgs e)
        {
            if (GridOfDates.Visibility == Visibility.Visible)
            {
                int sum = currentYearInGridOfDates * 12 + currentMonthInGridOfDates - 1;
                sum--;
                CalendarMoveToMonth(sum % 12 + 1, sum / 12);
            }
            else GridOfMonthsMoveToOtherYear(-1);
        }
        private void GoToNextMonthOrYear(object sender, RoutedEventArgs e)
        {
            if (GridOfDates.Visibility == Visibility.Visible)
            {
                int sum = currentYearInGridOfDates * 12 + currentMonthInGridOfDates - 1;
                sum++;
                CalendarMoveToMonth(sum % 12 + 1, sum / 12);
            }
            else GridOfMonthsMoveToOtherYear(1);
        }
    }
    public class CalendarDayButton : Button
    {
        public CalendarDayButton() : base()
        {
            _date = new DateTime();
            //IsEnabledChanged += (sender, e) =>
            //{
            //    if(!(bool)e.NewValue) SetValue(IsSelectedProperty, false);
            //};
        }
        //public Calendar Parent { set; get; }
        public static DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CalendarDayButton), new PropertyMetadata(false, null, null));
        //private static void IsSelectedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        public bool IsSelected
        {
            set
            {
                //if(IsEnabled)
                    SetValue(IsSelectedProperty, value);
            }
            get => (bool)GetValue(IsSelectedProperty);
        }
        public static DependencyProperty IsTodayProperty = DependencyProperty.Register("IsToday", typeof(bool), typeof(CalendarDayButton), new PropertyMetadata(false, null, null));
        public bool IsToday
        {
            set
            {
                SetValue(IsTodayProperty, value);
            }
            get => (bool)GetValue(IsTodayProperty);
        }
        public static DependencyProperty IsOutOfMonthProperty = DependencyProperty.Register("IsOutOfMonth", typeof(bool), typeof(CalendarDayButton), new PropertyMetadata(false, null, null));
        public bool IsOutOfMonth
        {
            set
            {
                SetValue(IsOutOfMonthProperty, value);
            }
            get => (bool)GetValue(IsOutOfMonthProperty);
        }
        private DateTime _date;
        public DateTime Date
        {
            set
            {
                DateTime valueDate = value.Date;
                if (valueDate != _date)
                {
                    _date = valueDate;
                    Content = _date.Day.ToString();
                }
            }
            get => _date;
        }

    }
    public class CalendarMonthButton : Button
    {
        public CalendarMonthButton() : base()
        {
            _date = 0;
        }
        //public Calendar Parent { set; get; }
        public static DependencyProperty IsTodayProperty = DependencyProperty.Register("IsToday", typeof(bool), typeof(CalendarMonthButton), new PropertyMetadata(false, null, null));
        public bool IsToday
        {
            set
            {
                SetValue(IsTodayProperty, value);
            }
            get => (bool)GetValue(IsTodayProperty);
        }
        private int _date;
        public int Month
        {
            set
            {
                if (value != _date)
                {
                    _date = value;
                }
            }
            get => _date;
        }

    }
}
