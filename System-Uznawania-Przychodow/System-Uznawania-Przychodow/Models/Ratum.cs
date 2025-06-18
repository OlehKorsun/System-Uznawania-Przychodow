using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Ratum
{
    public int IdRata { get; set; }

    public int Wartosc { get; set; }

    public int DzienMiesiaca { get; set; }

    public virtual ICollection<Umowa> Umowas { get; set; } = new List<Umowa>();
}
