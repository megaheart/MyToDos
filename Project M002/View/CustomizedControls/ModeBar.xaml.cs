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

namespace MyToDos.View.CustomizedControls
{
    /// <summary>
    /// Interaction logic for ModeBar.xaml
    /// </summary>
    public partial class ModeBar : UserControl
    {
        public ModeBar()
        {
            InitializeComponent();
            ModeOptions = new ObservableCollection<ModeOption>();
            ModeOptions.CollectionChanged += ModeOptions_CollectionChanged;
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(int), typeof(ModeBar), new PropertyMetadata(0, ModePropertyChanged));
        private static void ModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModeBar bar = d as ModeBar;

            bar.RaiseEvent(new ModeChangedEventArgs((int)e.NewValue, ModeChangedEvent));
        }
        public int Mode
        {
            set => SetValue(ModeProperty, value);
            get => (int)GetValue(ModeProperty);
        }

        public static readonly RoutedEvent ModeChangedEvent = EventManager.RegisterRoutedEvent("ModeChanged", RoutingStrategy.Bubble, typeof(ModeChangedEventHandler), typeof(ModeBar));
        public event ModeChangedEventHandler ModeChanged
        {
            add { AddHandler(ModeChangedEvent, value); }
            remove { RemoveHandler(ModeChangedEvent, value); }
        }
        private void ModeOptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    break;
                case NotifyCollectionChangedAction.Remove:

                    break;
                case NotifyCollectionChangedAction.Replace:

                    break;
                case NotifyCollectionChangedAction.Move:

                    break;
                case NotifyCollectionChangedAction.Reset:

                    break;
            }
        }
        private void ModeButton_Clicked(object sender, RoutedEventArgs e)
        {
            var mode = (sender as Button).DataContext as ModeOption;
            Mode = ModeOptions.IndexOf(mode);
        }
        private void ModeOption_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ModeOption modeOption = sender as ModeOption;
            var content = (modeOption.Button.Content as StackPanel).Children[0] as ContentControl;
            Image img = content.Content as Image;
            TextBlock mIcon = content.Content as TextBlock;
            switch (e.PropertyName)
            {
                case "1":
                    if (mIcon == null) content.Content = new TextBlock() { Text = modeOption.MaterialIconCode };
                    else mIcon.Text = modeOption.MaterialIconCode;
                    break;
                case "2":
                    if (img == null) content.Content = new Image()
                    {
                        Source = new BitmapImage(modeOption.ImageIconUrl)
                    };
                    else
                    {
                        var src = img.Source as BitmapImage;
                        src.BeginInit();
                        src.UriSource = modeOption.ImageIconUrl;
                        src.EndInit();
                    }
                    break;

            }
        }
        private RadioButton GetNewModeButton(ModeOption modeOption)
        {
            StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new ContentControl() { FontFamily = new FontFamily("Material Icons") });
            var text = new TextBlock();
            text.SetBinding(TextBlock.TextProperty, new Binding("Text"));
            stackPanel.Children.Add(text);
            var btn = new RadioButton()
            {
                Content = stackPanel,
                DataContext = modeOption,
                
            };
            btn.Click += ModeButton_Clicked;
            return btn;
        }
        public ObservableCollection<ModeOption> ModeOptions { get; private set; }
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
        public RadioButton Button { get; set; }
        private string _materialIconCode = "";
        public string MaterialIconCode
        {
            get => _materialIconCode;
            set
            {
                if(value != _materialIconCode)
                {
                    _materialIconCode = value;
                    OnPropertyChanged("1");
                }
            }
        }
        private Uri _imageIconUrl = null;
        public Uri ImageIconUrl
        {
            get => _imageIconUrl;
            set
            {
                if (value != _imageIconUrl)
                {
                    _imageIconUrl = value;
                    OnPropertyChanged("2");
                }
            }
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
                    OnPropertyChanged("Content");
                }
            }
        }
    }
}
