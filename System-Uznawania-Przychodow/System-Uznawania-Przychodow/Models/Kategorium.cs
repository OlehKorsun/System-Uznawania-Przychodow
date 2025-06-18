using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Kategorium
{
    public int IdKategoria { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Oprogramowanie> Oprogramowanies { get; set; } = new List<Oprogramowanie>();
}
