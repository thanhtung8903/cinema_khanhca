using System;
using System.Collections.Generic;

namespace SE1853_FirstWPF.Models;

public partial class Show
{
    public int ShowId { get; set; }

    public int RoomId { get; set; }

    public int FilmId { get; set; }

    public DateOnly ShowDate { get; set; }

    public decimal Price { get; set; }

    public bool? Status { get; set; }

    public int Slot { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Film Film { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
