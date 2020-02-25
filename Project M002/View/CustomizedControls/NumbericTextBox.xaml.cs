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
    /// Interaction logic for NumbericBox.xaml
    /// </summary>
    public partial class NumbericTextBox : UserControl
    {
        public NumbericTextBox()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(NumbericTextBox), new UIPropertyMetadata(23, MaxValuePropertyChanged));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(NumbericTextBox), new UIPropertyMetadata(0, MinValuePropertyChanged));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumbericTextBox), new UIPropertyMetadata(0, ValuePropertyChanged));
        //public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(NumbericTextBox));
        private static void MaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumbericTextBox n = d as NumbericTextBox;
            n.PreviousBtn2.Content = n.SafeAdd(n.Value, -2).ToString();
            n.PreviousBtn1.Content = n.SafeAdd(n.Value, -1).ToString();
            //n.NextBtn1.Content = n.SafeAdd(n.Value, 1).ToString();
            //n.NextBtn2.Content = n.SafeAdd(n.Value, 2).ToString();
        }
        public int MaxValue
        {
            set
            {
                if (value != MaxValue)
                {
                    SetValue(MaxValueProperty, value);
                }
            }
            get
            {
                return (int)GetValue(MaxValueProperty);
            }
        }
        private static void MinValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumbericTextBox n = d as NumbericTextBox;
            //n.PreviousBtn2.Content = n.SafeAdd(n.Value, -2).ToString();
            //n.PreviousBtn1.Content = n.SafeAdd(n.Value, -1).ToString();
            n.NextBtn1.Content = n.SafeAdd(n.Value, 1).ToString();
            n.NextBtn2.Content = n.SafeAdd(n.Value, 2).ToString();
        }
        public int MinValue
        {
            set
            {
                if (value != MinValue)
                {
                    SetValue(MinValueProperty, value);
                }
            }
            get
            {
                return (int)GetValue(MinValueProperty);
            }
        }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int value = int.Parse(e.NewValue.ToString());
            NumbericTextBox n = d as NumbericTextBox;
            n.PreviousBtn2.Content = n.SafeAdd(value, -2).ToString();
            n.PreviousBtn1.Content = n.SafeAdd(value, -1).ToString();
            n.MainTextBox.Text = value.ToString();
            n.NextBtn1.Content = n.SafeAdd(value, 1).ToString();
            n.NextBtn2.Content = n.SafeAdd(value, 2).ToString();
            n.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }
        public int Value
        {
            set
            {
                if (value > MaxValue || value < MinValue) throw new Exception("Can't set value property out of min or max value");
                if (value != Value)
                {
                    SetValue(ValueProperty, value);
                }
            }
            get
            {
                return (int)GetValue(ValueProperty);
            }
        }
        private static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumbericTextBox));
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        public Brush ThemeColor
        {
            set => MainTextBox.Background = value;
        }
        public void IncreaseOne(object sender, RoutedEventArgs e)
        {
            Value = SafeAdd(Value, 1);
        }
        public void IncreaseTwo(object sender, RoutedEventArgs e)
        {
            Value = SafeAdd(Value, 2);
        }
        public void DecreaseOne(object sender, RoutedEventArgs e)
        {
            Value = SafeAdd(Value, -1);
        }
        public void DecreaseTwo(object sender, RoutedEventArgs e)
        {
            Value = SafeAdd(Value, -2);
        }
        private int SafeAdd(int value, int i)
        {
            int length = MaxValue - MinValue + 1;
            return (value - MinValue + i + length) % length;
        }
        bool loop = false;
        private async void IncreaseContinuity_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            loop = true;
            Value = SafeAdd(Value, 1);
            await Task.Delay(140);
            while (loop)
            {
                await Task.Delay(20);
                Value = SafeAdd(Value, 1);
            }
        }

        private void IncreaseContinuity_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            loop = false;
        }

        private async void DecreaseContinuity_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            loop = true;
            Value = SafeAdd(Value, -1);
            await Task.Delay(140);
            while (loop)
            {
                await Task.Delay(20);
                Value = SafeAdd(Value, -1);
            }
        }

        private void DecreaseContinuity_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            loop = false;
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                Value = SafeAdd(Value, 1);
            else Value = SafeAdd(Value, -1);
        }
    }
}
