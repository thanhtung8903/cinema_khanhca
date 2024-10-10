using SE1853_FirstWPF.Models;
using System.Windows;
using System.Windows.Controls;

namespace SE1853_FirstWPF
{
    public partial class phim : Window
    {
        public phim()
        {
            InitializeComponent();
            loadData();
            loadGenres();
        }

        private void txtUri_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void cmbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        CinemaContext db = new CinemaContext();


        private void loadGenres()
        {
            var genres = db.Genres.ToList();
            cmbGenre.ItemsSource = genres;
            cmbGenre.DisplayMemberPath = "Name";
            cmbGenre.SelectedValuePath = "GenreId";
        }

        public void loadData()
        {
            var dt = from fil in db.Films
                join gen in db.Genres on fil.GenreId equals gen.GenreId
                select new
                {
                    ID = fil.FilmId,
                    Genre = gen.Name,
                    Title = fil.Title,
                    Year = fil.Year,
                    CountryCode = fil.CountryCode,
                    FilmUri = fil.FilmUrl
                };

            var data2 = dt.ToList();


            dgFilm.ItemsSource = data2;
        }

        private void dgFilm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFilm.SelectedItem == null)
            {
                return;
            }

            var selectedFilm = dgFilm.SelectedItem;

            // Lấy giá trị từ đối tượng kiểu ẩn danh bằng reflection
            if (selectedFilm != null)
            {
                txtID.Text = selectedFilm.GetType().GetProperty("ID")?.GetValue(selectedFilm, null)?.ToString();
                txtTitle.Text = selectedFilm.GetType().GetProperty("Title")?.GetValue(selectedFilm, null)?.ToString();
                txtYear.Text = selectedFilm.GetType().GetProperty("Year")?.GetValue(selectedFilm, null)?.ToString();
                txtCountry.Text = selectedFilm.GetType().GetProperty("CountryCode")?.GetValue(selectedFilm, null)
                    ?.ToString();
                cmbGenre.Text = selectedFilm.GetType().GetProperty("Genre")?.GetValue(selectedFilm, null)?.ToString();
                txtUri.Text = selectedFilm.GetType().GetProperty("FilmUri")?.GetValue(selectedFilm, null)?.ToString();
            }
        }


        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            //if (!isConfirm())
            //{
            //    return;
            //}

            //if (!isValidate())
            //{
            //    return;
            //}

            Film film = new Film();
            film.Title = txtTitle.Text;
            film.Year = int.Parse(txtYear.Text);
            film.CountryCode = txtCountry.Text;
            film.GenreId = (int)cmbGenre.SelectedValue;
            film.FilmUrl = txtUri.Text;
            db.Films.Add(film);
            db.SaveChanges();
            loadData();
        }


        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            //if (!isConfirm())
            //{
            //    return;
            //}

            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Please select a film to delete");
                return;
            }

            int id = int.Parse(txtID.Text);
            var film = db.Films.Find(id);
            db.Films.Remove(film);
            db.SaveChanges();
            loadData();
        }

        private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            //if (!isConfirm())
            //{
            //    return;
            //}

            //if (!isValidate())
            //{
            //    return;
            //}

            int id = int.Parse(txtID.Text);
            var film = db.Films.Find(id);
            film.Title = txtTitle.Text;
            film.Year = int.Parse(txtYear.Text);
            film.CountryCode = txtCountry.Text;
            film.GenreId = (int)cmbGenre.SelectedValue;
            film.FilmUrl = txtUri.Text;
            db.SaveChanges();
            loadData();
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var dt = from fil in db.Films
                join gen in db.Genres on fil.GenreId equals gen.GenreId
                where fil.Title.Contains(txtTitle.Text)
                select new
                {
                    ID = fil.FilmId,
                    Genre = gen.Name,
                    Title = fil.Title,
                    Year = fil.Year,
                    CountryCode = fil.CountryCode,
                    FilmUri = fil.FilmUrl
                };

            var data2 = dt.ToList();
            dgFilm.ItemsSource = data2;
        }
    }
}