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
using System.Windows.Shapes;

namespace WpfAppBarExample
{
    public partial class SideBanner : Window
    {
        MainWindow parentWindow;
        public SideBanner(MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Left = SystemParameters.WorkArea.Width - Width;
                Top = parentWindow.Height;
                Height = SystemParameters.WorkArea.Height - parentWindow.Height;
            }
            catch (Exception ex) { }
        }
    }
}
