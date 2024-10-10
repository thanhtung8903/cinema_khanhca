using System.Windows;
using System.Windows.Controls;
using SE1853_FirstWPF.Models;

namespace SE1853_FirstWPF;

public partial class Shows : Window
{
    public Shows()
    {
        InitializeComponent();
        loadData();
        loadRooms();
        loadFilms();
    }
    
    CinemaContext db = new CinemaContext();

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
        cmbFilm.ItemsSource = films;
        cmbFilm.DisplayMemberPath = "Title";
        cmbFilm.SelectedValuePath = "FilmId";
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
    
    private void btnAddShow_OnClick(object sender, RoutedEventArgs e)
    {
        int roomId = (int)cmbRoom.SelectedValue;
        int slot = int.Parse(cmbSlot.Text);
        DateOnly showDate = DateOnly.Parse(dpShowDate.Text);  // Parse the date from the text box
    
        // Extract the date part for comparison
        var existingShow = db.Shows
            .FirstOrDefault(s => s.RoomId == roomId 
                                 && s.Slot == slot 
                                 && s.ShowDate == showDate);

        if (existingShow != null)
        {
            MessageBox.Show("A show with the same Room, Slot, and Date already exists.");
            return;
        }

        bool? status = null;  // Nullable boolean to handle True, False, or Null

        if (cmbStatus.SelectedItem != null)
        {
            string selectedStatus = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString();
            if (selectedStatus == "True")
            {
                status = true;
            }
            else if (selectedStatus == "False")
            {
                status = false;
            }
            else
            {
                status = null;  // Handle "Null" status
            }
        }
        
        Show show = new Show
        {
            RoomId = roomId,
            FilmId = (int)cmbFilm.SelectedValue,
            ShowDate = showDate,  // Ensure this is a DateTime
            Price = decimal.Parse(txtPrice.Text),
            Status = status,
            Slot = slot
        };
        db.Shows.Add(show);
        db.SaveChanges();
        loadData();
    }

    private void btnUpdateShow_OnClick(object sender, RoutedEventArgs e)
    {
        int roomId = (int)cmbRoom.SelectedValue;
        int slot = int.Parse(cmbSlot.Text);
        DateOnly showDate = DateOnly.Parse(dpShowDate.Text);  // Parse the date from the text box
    
        // Extract the date part for comparison
        var existingShow = db.Shows
            .FirstOrDefault(s => s.RoomId == roomId 
                                 && s.Slot == slot 
                                 && s.ShowDate == showDate);

        if (existingShow != null && existingShow.ShowId != int.Parse(txtShowID.Text))
        {
            MessageBox.Show("A show with the same Room, Slot, and Date already exists.");
            return;
        }
        
        bool? status = null;  // Nullable boolean to handle True, False, or Null

// Check the selected status
        if (cmbStatus.SelectedItem != null)
        {
            string selectedStatus = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString();
            if (selectedStatus == "True")
            {
                status = true;
            }
            else if (selectedStatus == "False")
            {
                status = false;
            }
            else
            {
                status = null;  // Handle "Null" status
            }
        }
        
        int id = int.Parse(txtShowID.Text);
        var show = db.Shows.Find(id);
        show.RoomId = roomId;
        show.FilmId = (int)cmbFilm.SelectedValue;
        show.ShowDate = showDate;  // Ensure this is a DateTime
        show.Price = decimal.Parse(txtPrice.Text);
        show.Status = status;
        show.Slot = slot;
        db.SaveChanges();
        loadData();
    }

    private void btnSearchShow_OnClick(object sender, RoutedEventArgs e)
    {
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
        loadData();
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
            cmbFilm.Text = selectedShow.GetType().GetProperty("Film")?.GetValue(selectedShow, null)?.ToString();
            cmbRoom.Text = selectedShow.GetType().GetProperty("Room")?.GetValue(selectedShow, null)?.ToString();
            dpShowDate.Text = selectedShow.GetType().GetProperty("ShowDate")?.GetValue(selectedShow, null)?.ToString();
            txtPrice.Text = selectedShow.GetType().GetProperty("Price")?.GetValue(selectedShow, null)?.ToString();
            cmbStatus.Text = selectedShow.GetType().GetProperty("Status")?.GetValue(selectedShow, null)?.ToString();
            cmbSlot.Text = selectedShow.GetType().GetProperty("Slot")?.GetValue(selectedShow, null)?.ToString();
        }
    }
}