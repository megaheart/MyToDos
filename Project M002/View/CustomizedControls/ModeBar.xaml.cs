using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Collections.Specialized;
using System.Windows.Shapes;
using Storage.Model;
using System.ComponentModel;
using System.Windows.Markup;
using System.Collections;
using System.Windows.Media.Animation;

namespace MyToDos.View.CustomizedControls
{
    // Read summary ↴ ↴ ↴ ↴ ↴ ↴
    /// <summary>
    /// When ModeOption property changed, ModeBar needs to reload layout twice
    /// => ✎✎✎ It needs optimizing ✎✎✎
    /// </summary>
    [ContentProperty("ModeOptions")]
    public partial class ModeBar : UserControl
    {
        private static readonly Thickness VerticalModeThickness = new Thickness(0, 10, 3, 10);

        private static readonly Thickness HorizontalModeThickness = new Thickness(10, 0, 10, 0);
        public ModeBar()
        {
            InitializeComponent();
            //DataContext = this;
            _modeOptions = new ObservableCollection<object>();
            ButtonContainer.ItemsSource = _modeOptions;
            _modeOptions.CollectionChanged += ModeOptions_CollectionChanged;
            //Height = 50;
            //ReloadMainLine();
            //Container = Bu
            Loaded += (sender, e) =>
            {
                heightBinding = new Binding("ActualHeight") { Source = sender };
                widthBinding = new Binding("ActualWidth") { Source = sender };
                MainLine.SetBinding(Line.X2Property, widthBinding);
                MainLine.SetBinding(Line.Y2Property, heightBinding);
                ReloadMainLine();
                if(Mode != -1)
                {
                    var listBoxItem = ButtonContainer.ItemContainerGenerator.ContainerFromIndex(ButtonContainer.SelectedIndex) as ListBoxItem;
                    ReloadSelectionLine(listBoxItem);
                }
                //MessageBox.Show("initialized");
            };
        }
        
        private StackPanel Container = null;
        //private Orientation? _setOrientationWhenContainerIsNull = null;
        private void Container_Intialized(object sender, EventArgs e)
        {
            Container = sender as StackPanel;
            if(Orientation == Orientation.Vertical)
            {
                Container.Orientation = Orientation.Vertical;
                ButtonContainer.Margin = VerticalModeThickness;
            }
            Resize();
        }
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ModeBar), new PropertyMetadata(Orientation.Horizontal, OrientationPropertyChanged));
        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModeBar bar = d as ModeBar;
            //bar.ButtonContainer.ItemsPanel.VisualTree = new StackPanel();
            if (bar.Container != null) 
            {
                bar.Container.Orientation = (Orientation)e.NewValue;
                bar.ReloadMainLine();
                if (bar.Container.Orientation == Orientation.Vertical)
                {
                    bar.ButtonContainer.Margin = VerticalModeThickness;
                }
                else
                {
                    bar.ButtonContainer.Margin = HorizontalModeThickness;
                }
                bar.Resize();
            } 
            //bar._setOrientationWhenContainerIsNull = (Orientation)e.NewValue;
            //bar.ReloadSelectionLine(bar.ButtonContainer.Items[bar.Mode] as RadioButton);
        }
        public Orientation Orientation
        {
            set => SetValue(OrientationProperty, value);
            get => (Orientation)GetValue(OrientationProperty);
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(int), typeof(ModeBar), new PropertyMetadata(-1, ModePropertyChanged));
        private static void ModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ModeBar;
            if((int)e.OldValue != -1)
                bar.RaiseEvent(new ModeChangedEventArgs((int) e.NewValue, ModeChangedEvent));
        }
        /// <summary>
        /// Get or set mode
        /// Return -1 if no modes is selected
        /// </summary>
        public int Mode
        {
            set 
            {
                SetValue(ModeProperty, value);
                ButtonContainer.SelectedIndex = value;
            } 
            get => (int)GetValue(ModeProperty);
        }

        public static readonly RoutedEvent ModeChangedEvent = EventManager.RegisterRoutedEvent("ModeChanged", RoutingStrategy.Bubble, typeof(ModeChangedEventHandler), typeof(ModeBar));
        public event ModeChangedEventHandler ModeChanged
        {
            add { AddHandler(ModeChangedEvent, value); }
            remove { RemoveHandler(ModeChangedEvent, value); }
        }
        private void ModeOptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Container != null)
            {
                Resize();
                var listBoxItem = ButtonContainer.ItemContainerGenerator.ContainerFromIndex(ButtonContainer.SelectedIndex) as ListBoxItem;
                ReloadSelectionLine(listBoxItem);
            }
            //if (_modeOptions.Count > 0 && ButtonContainer.SelectedIndex == -1) ButtonContainer.SelectedIndex = 0;
            //else 
            SetValue(ModeProperty, ButtonContainer.SelectedIndex);
        }
        private void ModeButton_Selected(object sender, SelectionChangedEventArgs e)
        {
            SetValue(ModeProperty, ButtonContainer.SelectedIndex);
            if (Container != null)
            {
                var listBoxItem = ButtonContainer.ItemContainerGenerator.ContainerFromIndex(ButtonContainer.SelectedIndex) as ListBoxItem;
                ReloadSelectionLineAnimation(listBoxItem);
            }
        }
        private Binding heightBinding;
        private Binding widthBinding;
        private void ReloadMainLine()
        {
            if (Orientation == Orientation.Horizontal)
            {
                BindingOperations.ClearBinding(MainLine, Line.X1Property);
                MainLine.X1 = 0;
                MainLine.SetBinding(Line.Y1Property, heightBinding);
                BindingOperations.ClearBinding(SelectionLine, Line.X1Property);
                BindingOperations.ClearBinding(SelectionLine, Line.X2Property);
                SelectionLine.SetBinding(Line.Y1Property, heightBinding);
                SelectionLine.SetBinding(Line.Y2Property, heightBinding);
            }
            else
            {
                BindingOperations.ClearBinding(MainLine, Line.Y1Property);
                MainLine.SetBinding(Line.X1Property, widthBinding);
                MainLine.Y1 = 0;
                BindingOperations.ClearBinding(SelectionLine, Line.Y1Property);
                BindingOperations.ClearBinding(SelectionLine, Line.Y2Property);
                SelectionLine.SetBinding(Line.X1Property, widthBinding);
                SelectionLine.SetBinding(Line.X2Property, widthBinding);
            }
        }
        private static TimeSpan duration = TimeSpan.FromMilliseconds(200);
        private void ReloadSelectionLine(ListBoxItem selection)
        {
            var pt = selection.TranslatePoint(new Point(0, 0), this);
            if (Orientation == Orientation.Horizontal)
            {
                SelectionLine.X1 = pt.X;
                SelectionLine.X2 = pt.X + selection.ActualWidth;
            }
            else
            {
                SelectionLine.Y1 = pt.Y;
                SelectionLine.Y2 = pt.Y + selection.ActualHeight;
            }
        }
        private void ReloadSelectionLineAnimation(ListBoxItem selection)
        {
            Storyboard storyboard = new Storyboard();
            var pt = selection.TranslatePoint(new Point(0, 0), this);
            if(Orientation == Orientation.Horizontal)
            {
                DoubleAnimation doubleAnimationX1 = new DoubleAnimation(pt.X, duration);
                Storyboard.SetTargetName(doubleAnimationX1, "SelectionLine");
                Storyboard.SetTargetProperty(doubleAnimationX1, new PropertyPath(Line.X1Property));
                DoubleAnimation doubleAnimationX2 = new DoubleAnimation(pt.X + selection.ActualWidth, duration);
                Storyboard.SetTargetName(doubleAnimationX2, "SelectionLine");
                Storyboard.SetTargetProperty(doubleAnimationX2, new PropertyPath(Line.X2Property));
                storyboard.Children.Add(doubleAnimationX1);
                storyboard.Children.Add(doubleAnimationX2);
            }
            else
            {
                DoubleAnimation doubleAnimationY1 = new DoubleAnimation(pt.Y, duration);
                Storyboard.SetTargetName(doubleAnimationY1, "SelectionLine");
                Storyboard.SetTargetProperty(doubleAnimationY1, new PropertyPath(Line.Y1Property));
                DoubleAnimation doubleAnimationY2 = new DoubleAnimation(pt.Y + selection.ActualHeight, duration);
                Storyboard.SetTargetName(doubleAnimationY2, "SelectionLine");
                Storyboard.SetTargetProperty(doubleAnimationY2, new PropertyPath(Line.Y2Property));
                storyboard.Children.Add(doubleAnimationY1);
                storyboard.Children.Add(doubleAnimationY2);
            }
            storyboard.Begin(this);
        }
        private ObservableCollection<object> _modeOptions;
        public ObservableCollection<object> ModeOptions
        {
            get => _modeOptions;
            set
            {
                if (value == _modeOptions) return;
                _modeOptions.CollectionChanged -= ModeOptions_CollectionChanged;
                _modeOptions = value;
                ButtonContainer.ItemsSource = _modeOptions;
                _modeOptions.CollectionChanged += ModeOptions_CollectionChanged;
                if (Container != null)
                {
                    Resize();
                    var listBoxItem = ButtonContainer.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
                    ReloadSelectionLine(listBoxItem);
                }
                SetValue(ModeProperty, 0);
            }
        }
        public void Resize()
        {
            Container.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Height = Container.DesiredSize.Height + ButtonContainer.Margin.Top + ButtonContainer.Margin.Bottom;
            Width = Container.DesiredSize.Width + ButtonContainer.Margin.Left + ButtonContainer.Margin.Right;
        }
        
        private void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            Dispatcher.Invoke(App.NullVoid, System.Windows.Threading.DispatcherPriority.Loaded);
            Resize();
            var listBoxItem = ButtonContainer.ItemContainerGenerator.ContainerFromIndex(ButtonContainer.SelectedIndex) as ListBoxItem;
            ReloadSelectionLine(listBoxItem);
        }
    }
    public delegate void ModeChangedEventHandler(object sender, ModeChangedEventArgs e);
    public class ModeChangedEventArgs : RoutedEventArgs
    {
        public ModeChangedEventArgs(int newValue, RoutedEvent routedEvent):base(routedEvent)
        {
            NewValue = newValue;
        }
        public int NewValue { get; private set; }
    }
    public class ModeOption:NotifiableObject
    {
        public ModeOption() { }
        //public RadioButton Button { get; set; }
        public object Icon { get; private set; } = "";
        //private string _materialIconCode = "";
        public string MaterialIconCode
        {
            //get => _materialIconCode;
            set
            {
                TextBlock mIcon = Icon as TextBlock;
                if (mIcon == null) Icon = new TextBlock() { Text = value };
                else mIcon.Text = value;
                OnPropertyChanged("Icon");
            }
            get => "";
        }
        //private Uri _imageIconUrl = null;
        public string ImageIconUrl
        {
            //get => _imageIconUrl;
            set
            {
                Image img = Icon as Image;
                if (img == null) Icon = new Image()
                {
                    Source = new BitmapImage(new Uri(value)),
                    Height = 24
                };
                else
                {
                    var src = img.Source as BitmapImage;
                    src.BeginInit();
                    src.UriSource = new Uri(value);
                    src.EndInit();
                }
                OnPropertyChanged("Icon");
            }
            get => "";
        }
        private string _content = "";
        public string Content
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged("ContentVisibility");
                    OnPropertyChanged("Content");
                }
            }
        }
        public Visibility ContentVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(_content)) return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }
    }
    
}
namespace MyToDos
{
    [ContentProperty("Items")]
    public class UIList : IEnumerable
    {
        public ArrayList Items { set; get; } = new ArrayList();

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
