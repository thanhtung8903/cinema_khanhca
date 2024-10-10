using System;
using System.Collections.Generic;

namespace SE1853_FirstWPF.Models;

public partial class Country
{
    public string CountryCode { get; set; } = null!;

    public string? CountryName { get; set; }

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
