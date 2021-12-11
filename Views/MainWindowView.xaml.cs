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
using TDDD49.ViewModels;



namespace TDDD49
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindowView : Window
    {
       
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
