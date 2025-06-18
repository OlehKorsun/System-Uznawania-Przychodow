using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Znizka
{
    public int IdZnizka { get; set; }

    public string Nazwa { get; set; } = null!;

    public DateTime OkresOd { get; set; }

    public DateTime OkresDo { get; set; }

    public decimal Wartosc { get; set; }

    public virtual ICollection<Umowa> Umowas { get; set; } = new List<Umowa>();
}
