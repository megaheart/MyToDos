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
    /// A Special text box with special behaviors
    /// I don't know how to describe more it so you must read 
    /// Views/CustomizedControl/TimeAdjuster.xaml and Views/CustomizedControl/TimeAdjuster.xaml.cs to know more
    /// That is example about how to use this control
    /// </summary>
    public partial class SpecialTextBox : UserControl
    {
        public SpecialTextBox()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty CheckWhileTypingProperty = DependencyProperty.Register("CheckWhileTyping", typeof(bool), typeof(SpecialTextBox), new UIPropertyMetadata(true));

        public bool CheckWhileTyping
        {
            set => SetValue(CheckWhileTypingProperty, value);
            get => (bool)GetValue(CheckWhileTypingProperty);
        }
        public string Text
        {
            get =>  (string)PlaceHolderTxt.Content;
            set
            {
                if (value != MainText.Text || !string.IsNullOrEmpty(value) || ValidationRule == null || ValidationRule(value, ""))
                {
                    //Keyboard.ClearFocus();
                    MainText.Text = "";
                    PlaceHolderTxt.Visibility = Visibility.Visible;
                    PlaceHolderTxt.Content = value;
                }
            }
        }
        public int MaxLength
        {
            set => MainText.MaxLength = value;
        }
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SpecialTextBox));
        public event RoutedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }
        public Thickness TextboxPadding
        {
            set
            {
                MainText.Padding = value;
            }
            get => MainText.Padding;
        }
        public TextAlignment TextAlignment
        {
            set
            {
                MainText.TextAlignment = value;
            }
        }
        /// <summary>
        /// default Validation Rule is "do not save text if user type nothing"
        /// return true to accept input value
        /// return false to decline input value
        /// </summary>
        public event Func<string, string, bool> ValidationRule; //bool ValidationRule(string newText, string key)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlaceHolderTxt.Visibility = Visibility.Collapsed;
            MainText.Focus();
        }
        private void MainText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MainText.Text))
            {
                if (!CheckWhileTyping)
                {
                    if (ValidationRule == null || ValidationRule(MainText.Text, null))
                    {
                        PlaceHolderTxt.Content = MainText.Text;
                        RaiseEvent(new RoutedEventArgs(TextChangedEvent));
                    }
                }
                else
                {
                    PlaceHolderTxt.Content = MainText.Text;
                    RaiseEvent(new RoutedEventArgs(TextChangedEvent));
                }
            }
            MainText.Text = "";
            PlaceHolderTxt.Visibility = Visibility.Visible;
        }

        private void MainText_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!CheckWhileTyping)
            {
                if (e.Text == "\r")
                {
                    if(!string.IsNullOrWhiteSpace(MainText.Text) && (ValidationRule == null || ValidationRule(MainText.Text, null)))
                    {
                        PlaceHolderTxt.Content = MainText.Text;
                        MainText.Text = "";
                        PlaceHolderTxt.Visibility = Visibility.Visible;
                        RaiseEvent(new RoutedEventArgs(TextChangedEvent));
                    }
                    else
                    {
                        MainText.Text = "";
                        PlaceHolderTxt.Visibility = Visibility.Visible;
                    }
                }
                return;
            }
            if (e.Text == "\r")
            {
                if (!string.IsNullOrWhiteSpace(MainText.Text))
                {
                    PlaceHolderTxt.Content = MainText.Text;
                    MainText.Text = "";
                    PlaceHolderTxt.Visibility = Visibility.Visible;
                    RaiseEvent(new RoutedEventArgs(TextChangedEvent));
                }
                else
                {
                    MainText.Text = "";
                    PlaceHolderTxt.Visibility = Visibility.Visible;
                }
                return;
            }
            e.Handled = !((MainText.MaxLength != 0 && MainText.Text.Length == MainText.MaxLength) || ValidationRule == null || ValidationRule(MainText.Text.Insert(MainText.CaretIndex, e.Text), e.Text));
        }

        private void MainText_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!CheckWhileTyping) return;
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                if (!ValidationRule(e.DataObject.GetData(typeof(string)) as string, ""))
                    e.CancelCommand();
            }
            else e.CancelCommand();
        }
    }
}
