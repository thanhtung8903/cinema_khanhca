using System.Windows;
using System.Windows.Controls;
using SE1853_FirstWPF.Models;

namespace SE1853_FirstWPF;

public partial class Shows : Window
{
    public Shows()
    {
        InitializeComponent();
        // loadData();
        loadRooms();
        loadFilms();
        loadToday();
    }
    
    CinemaContext db = new CinemaContext();

    private void loadToday()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        dpShowDate.Text = today.ToString();
    }
    
    private void loadRooms()
    {
        var rooms = db.Rooms.ToList();
        cmbRoom.ItemsSource = rooms;
        cmbRoom.DisplayMemberPath = "Name";
        cmbRoom.SelectedValuePath = "RoomId";
    }
    
    private void loadFilms()
    {
        var films = db.Films.ToList();
    }
    
    public void loadData()
    {
        var dt = from show in db.Shows
            join film in db.Films on show.FilmId equals film.FilmId
            join room in db.Rooms on show.RoomId equals room.RoomId
            select new
            {
                ID = show.ShowId,
                Film = film.Title,
                Room = room.Name,
                ShowDate = show.ShowDate,
                Price = show.Price,
                Status = show.Status,
                Slot = show.Slot
            };

        var data2 = dt.ToList();

        dgShows.ItemsSource = data2;
    }
    


    private void btnSearchShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (cmbRoom.SelectedValue == null || string.IsNullOrEmpty(dpShowDate.Text))
        {
            MessageBox.Show("Please select a room and date");
            return;
        }
        
        
        //search by Show Date and the selected Room
        int roomId = (int)cmbRoom.SelectedValue;
        DateOnly showDate = DateOnly.Parse(dpShowDate.Text);  // Parse the date from the text box
        
        var dt = from show in db.Shows
            join film in db.Films on show.FilmId equals film.FilmId
            join room in db.Rooms on show.RoomId equals room.RoomId
            where show.ShowDate == showDate && show.RoomId == roomId
            select new
            {
                ID = show.ShowId,
                Film = film.Title,
                Room = room.Name,
                ShowDate = show.ShowDate,
                Price = show.Price,
                Status = show.Status,
                Slot = show.Slot
            };
        
        var data2 = dt.ToList();
        dgShows.ItemsSource = data2;
        
    }

    private void btnDeleteShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(txtShowID.Text))
        {
            MessageBox.Show("Please select a show to delete");
            return;
        }

        int id = int.Parse(txtShowID.Text);
        var show = db.Shows.Find(id);
        db.Shows.Remove(show);
        db.SaveChanges();
    }

    private void dgShows_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (dgShows.SelectedItem == null)
        {
            return;
        }

        var selectedShow = dgShows.SelectedItem;

        // Lấy giá trị từ đối tượng kiểu ẩn danh bằng reflection
        if (selectedShow != null)
        {
            txtShowID.Text = selectedShow.GetType().GetProperty("ID")?.GetValue(selectedShow, null)?.ToString();
            cmbRoom.Text = selectedShow.GetType().GetProperty("Room")?.GetValue(selectedShow, null)?.ToString();
            dpShowDate.Text = selectedShow.GetType().GetProperty("ShowDate")?.GetValue(selectedShow, null)?.ToString();
        }
    }

    private void BtnAddShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (cmbRoom.SelectedValue == null || string.IsNullOrEmpty(dpShowDate.Text))
        {
            MessageBox.Show("Please select a room and date");
            return;
        }

        int roomId = (int)cmbRoom.SelectedValue;
        DateOnly showDate = DateOnly.Parse(dpShowDate.Text);

        //open the add show window and pass the selected room and date
        AddShows addShow = new AddShows(roomId, showDate);
        addShow.ShowDialog();
    }

    private void BtnUpdateShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (cmbRoom.SelectedValue == null || string.IsNullOrEmpty(dpShowDate.Text))
        {
            MessageBox.Show("Please select a room and date");
            return;
        }

        int roomId = (int)cmbRoom.SelectedValue;
        DateOnly showDate = DateOnly.Parse(dpShowDate.Text);
        int showId = int.Parse(txtShowID.Text);

        //open the add show window and pass the selected room and date
        UpdateShows addShow = new UpdateShows(showId, roomId, showDate);
        addShow.ShowDialog();
    }
}