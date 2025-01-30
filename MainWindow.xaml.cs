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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RuningWindows runingWindows = new RuningWindows();
            runingWindows.DoStartWatcher(this);
            SideBanner sideBanner = new SideBanner(this);
            sideBanner.Show();
        }

        public void ShowAppBar()
        {
            AppBarFunctions.SetAppBar(this, ABEdge.Top, grid, false);
        }

        public void HideAppBar()
        {
            AppBarFunctions.SetAppBar(this, ABEdge.None);
        }

        private void AppBar_Click(object sender, RoutedEventArgs e)
        {
            AppBar.IsEnabled = false;
            Normal.IsEnabled = true;
            ShowAppBar();
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            Normal.IsEnabled = false;
            AppBar.IsEnabled = true;
            HideAppBar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Left = 0;
                Top = 0;
                Width = SystemParameters.WorkArea.Width;
            }
            catch (Exception ex) { }
        }
    }
}