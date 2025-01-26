using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppBarExample.Utilities;
using static System.Environment;

namespace WpfAppBarExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly System.Timers.Timer timer = new();

        public MainWindow()
        {
            InitializeComponent();
            RuningWindows runingWindows = new RuningWindows();
            runingWindows.DoStartWatcher(this);
            try
            {
                timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
                timer.Interval = 2 * 1000;
                //timer.Enabled = true;
            }
            catch (Exception) { }
        }

        private void Timer_Tick(object? sender, ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    if (DesktopExtension.FindDesktop())
                    {
                        AppBarFunctions.SetAppBar(this, ABEdge.None);
                        
                    }
                    else
                    {
                        AppBarFunctions.SetAppBar(this, ABEdge.Top, grid, false);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AppBar_Click(object sender, RoutedEventArgs e)
        {
            AppBar.IsEnabled = false;
            Normal.IsEnabled = true;
            AppBarFunctions.SetAppBar(this, ABEdge.Top, grid, false);
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            Normal.IsEnabled = false;
            AppBar.IsEnabled = true;
            AppBarFunctions.SetAppBar(this, ABEdge.None);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Left = 0;
                Top = 0;
                Width = SystemParameters.WorkArea.Width;
                //Height = SystemParameters.WorkArea.Height;
            }
            catch (Exception ex) { }
        }
    }
}