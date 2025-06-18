using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Oprogramowanie
{
    public int IdOprogramowanie { get; set; }

    public string Nazwa { get; set; } = null!;

    public string Opis { get; set; } = null!;

    public string Wersja { get; set; } = null!;

    public int IdKategoria { get; set; }

    public virtual Kategorium IdKategoriaNavigation { get; set; } = null!;

    public virtual ICollection<OprogramowanieAktualizacja> OprogramowanieAktualizacjas { get; set; } = new List<OprogramowanieAktualizacja>();

    public virtual ICollection<Umowa> Umowas { get; set; } = new List<Umowa>();
}
