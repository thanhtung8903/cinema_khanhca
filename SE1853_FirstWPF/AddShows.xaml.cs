using System.Windows;
using SE1853_FirstWPF.Models;

namespace SE1853_FirstWPF
{
    public partial class AddShows : Window
    {
        private int _roomId;
        private DateOnly _showDate;
        CinemaContext db = new CinemaContext();

        public AddShows(int roomId, DateOnly showDate)
        {
            InitializeComponent();
            _roomId = roomId;
            _showDate = showDate;
            loadFilm();
            loadSlotEmpty();
        }
        
        private void loadFilm()
        {
            CinemaContext db = new CinemaContext();
            var films = db.Films.ToList();
            cmbFilm.ItemsSource = films;
            cmbFilm.DisplayMemberPath = "Title";
            cmbFilm.SelectedValuePath = "FilmId";
        }
        
        private void loadSlotEmpty()
        {
            var shows = db.Shows.Where(s => s.RoomId == _roomId && s.ShowDate == _showDate).ToList();

            var allSlots = Enumerable.Range(1, 9).ToList();

            var takenSlots = shows.Select(s => s.Slot).ToList();

            var emptySlots = allSlots.Except(takenSlots).ToList();

            cmbSlot.ItemsSource = emptySlots;
        }
        
        private void BtnAddShow_OnClick(object sender, RoutedEventArgs e)
        {
            // Add a new show to the database
            // You can use the selected values from the ComboBoxes and DatePicker
            var newShow = new Show
            {
                RoomId = _roomId,
                FilmId = (int)cmbFilm.SelectedValue,
                ShowDate = _showDate,
                Price = decimal.Parse(txtPrice.Text),
                Status = true,
                Slot = (int)cmbSlot.SelectedValue
            };
            db.Shows.Add(newShow);
            db.SaveChanges();
            MessageBox.Show("Show added successfully!");
        }
    }
}