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

namespace SE1853_FirstWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mnuLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if(Settings.UserName != "" && Settings.UserName != null)
            {
                mnuFilm.IsEnabled = true;
                mnuShow.IsEnabled = true;
                mnuLogin.Header = $"Logout({Settings.UserName})";
            }    
            else
            {
                mnuFilm.IsEnabled = false;
                mnuShow.IsEnabled = false;
                mnuLogin.Header = "Login";


            }
        }

        private void mnuFilm_Click(object sender, RoutedEventArgs e)
        {
            phim  p = new phim();
            p.ShowDialog();
        }

        private void mnuShows_Click(object sender, RoutedEventArgs e)
        {
            Shows s = new Shows();
            s.ShowDialog();
        }
    }
}
