using MyToDos.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
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


namespace MyToDos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowResizer = new WindowResizer(this);
            _pagesTree = new List<UserControl>(1);

            
        }
        private List<UserControl> _pagesTree;
        private void MoveTo(UserControl page)
        {
            _pagesTree.RemoveRange(1, _pagesTree.Count - 1);
            _pagesTree[0] = page;

        }
        private void MoveToTasksPage(object sender, RoutedEventArgs e)
        {
            
        }
        private void MoveToTimetablePage(object sender, RoutedEventArgs e)
        {
        }
        private void MoveToNonSchedulesPage(object sender, RoutedEventArgs e)
        {

        }
        private void MoveToSettingPage(object sender, RoutedEventArgs e)
        {

        }
        WindowResizer WindowResizer;
        // for each rectangle, assign the following method to its PreviewMouseDown event.
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Resize(object sender, MouseButtonEventArgs e)
        {
            WindowResizer.resizeWindow((System.Windows.Controls.Primitives.Thumb)sender);
        }
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class NavigateWindow
    {
        public void Back()
        {

        }
    }
}
