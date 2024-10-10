using System;
using System.Collections.Generic;

namespace SE1853_FirstWPF.Models;

public partial class Film
{
    public int FilmId { get; set; }

    public int GenreId { get; set; }

    public string Title { get; set; } = null!;

    public int Year { get; set; }

    public string CountryCode { get; set; } = null!;

    public string? FilmUrl { get; set; }

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
}
