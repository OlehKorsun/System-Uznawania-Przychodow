using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Aktualizacja
{
    public int IdAktualizacja { get; set; }

    public string Opis { get; set; } = null!;

    public virtual ICollection<OprogramowanieAktualizacja> OprogramowanieAktualizacjas { get; set; } = new List<OprogramowanieAktualizacja>();
}
