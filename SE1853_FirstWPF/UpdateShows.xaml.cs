using System.Windows;
using SE1853_FirstWPF.Models;

namespace SE1853_FirstWPF;

public partial class UpdateShows : Window
{
    private int _roomId;
    private DateOnly _showDate;
    private int _showId;
    private Show _currentShow;
    CinemaContext db = new CinemaContext();

    public UpdateShows(int showId, int roomId, DateOnly showDate)
    {
        InitializeComponent();
        _roomId = roomId;
        _showDate = showDate;
        _showId = showId;
        loadFilm();
        LoadCurrentShow();
        loadSlotEmpty();
    }

    private void LoadCurrentShow()
    {
        _currentShow = db.Shows.Find(_showId);
        if (_currentShow != null)
        {
            cmbFilm.SelectedValue = _currentShow.FilmId;
            txtPrice.Text = _currentShow.Price.ToString();
            cmbSlot.SelectedValue = _currentShow.Slot;
        }
    }

    private void loadFilm()
    {
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

        // Bỏ comment phần này để giữ lại slot hiện tại của show đang được cập nhật
        if (_currentShow != null)
        {
            takenSlots.Remove(_currentShow.Slot);
        }

        var availableSlots = allSlots.Except(takenSlots).ToList();

        // Thêm slot hiện tại vào danh sách các slot có sẵn
        if (_currentShow != null && !availableSlots.Contains(_currentShow.Slot))
        {
            availableSlots.Add(_currentShow.Slot);
            availableSlots.Sort();
        }

        cmbSlot.ItemsSource = availableSlots;
    }

    private void BtnUpdateShow_OnClick(object sender, RoutedEventArgs e)
    {
        _currentShow.FilmId = (int) cmbFilm.SelectedValue;
        _currentShow.Price = decimal.Parse(txtPrice.Text);
        _currentShow.Slot = (int) cmbSlot.SelectedValue;
        db.SaveChanges();
        
        MessageBox.Show("Updated successfully");
    }
}