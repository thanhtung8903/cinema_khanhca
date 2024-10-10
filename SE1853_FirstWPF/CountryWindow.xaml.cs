using SE1853_FirstWPF.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE1853_FirstWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CountryWindow : Window
    {
        CinemaContext context;
        public CountryWindow()
        {
            InitializeComponent();
            context = new CinemaContext();
            dgCountries.ItemsSource = context.Countries
                .Select(c => new {c.CountryCode,
                                  c.CountryName})
                .ToList();
            cboCountryCode.ItemsSource = context.Countries.ToList();
            cboCountryCode.DisplayMemberPath = "CountryName";
            cboCountryCode.SelectedValuePath = "CountryCode";
        }

        private void dgCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic country = dgCountries.SelectedItem;
            if (country != null)
            {
                txtCountryCode.Text = country.CountryCode;
                txtCountryName.Text = country.CountryName;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Country country = new Country
                {
                    CountryCode = txtCountryCode.Text,
                    CountryName = txtCountryName.Text
                };
                context.Countries.Add(country);
                context.SaveChanges();
                MessageBox.Show("Added!");
                dgCountries.ItemsSource = context.Countries
                .Select(c => new {
                    c.CountryCode,
                    c.CountryName
                })
                .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            dynamic country = dgCountries.SelectedItem;
            if (country == null)
            {
                MessageBox.Show("You must select a Country!");
                return;
            }
            try
            {
       
                Country country1 = context.Countries.Find(country.CountryCode);
                context.Countries.Remove(country1);
                context.SaveChanges();
                MessageBox.Show("Deleted");
                dgCountries.ItemsSource = context.Countries
                .Select(c => new {
                    c.CountryCode,
                    c.CountryName
                })
                .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            dynamic country = dgCountries.SelectedItem;
            if (country == null)
            {
                MessageBox.Show("You must select a country!");
                return;
            }
            try
            {
                Country country1 = context.Countries.Find(country.CountryCode);
                country1.CountryName = txtCountryName.Text;
                context.Countries.Update(country1);
                context.SaveChanges ();
                MessageBox.Show("Updated!");
                dgCountries.ItemsSource = context.Countries
                    .Select(c => new {
                            c.CountryCode,
                            c.CountryName
                    })
                    .ToList();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboCountryCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgCountries.ItemsSource =
                context.Countries.Where(c => c.CountryCode ==
                cboCountryCode.SelectedValue)
                .Select(c => new
                {
                    c.CountryCode,
                    c.CountryName
                }).ToList();
        }
    }
}