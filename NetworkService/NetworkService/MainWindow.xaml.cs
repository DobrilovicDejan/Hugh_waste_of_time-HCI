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

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btn_E.IsEnabled = false;
        }

        private void TextBlock_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void btn_E_Click(object sender, RoutedEventArgs e)
        {
            btn_E.IsEnabled = false;
            btn_D.IsEnabled = true;
            btn_G.IsEnabled = true;
        }

        private void btn_D_Click(object sender, RoutedEventArgs e)
        {
            btn_E.IsEnabled = true;
            btn_D.IsEnabled = false;
            btn_G.IsEnabled = true;
        }

        private void btn_G_Click(object sender, RoutedEventArgs e)
        {
            btn_E.IsEnabled = true;
            btn_D.IsEnabled = true;
            btn_G.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
