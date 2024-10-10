﻿using System;
using System.Collections.Generic;

namespace SE1853_FirstWPF.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
