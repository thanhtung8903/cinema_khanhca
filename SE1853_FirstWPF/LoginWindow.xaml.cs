using Microsoft.Extensions.Configuration;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string name, pass;
            var conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            name = conf.GetSection("Admin:Name").Value;
            pass = conf.GetSection("Admin:Pass").Value;

            if(name == txtName.Text && pass == txtPassword.Password)
            {
                MessageBox.Show("You are login as Administrator!");
                Settings.UserName = name;
                Close();
            } else
            {
                MessageBox.Show("Login failed!");
            }
        }
    }
}
