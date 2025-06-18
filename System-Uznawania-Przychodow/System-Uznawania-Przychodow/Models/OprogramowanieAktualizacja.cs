using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class OprogramowanieAktualizacja
{
    public int IdOprogramowanie { get; set; }

    public int IdAktualizacja { get; set; }

    public DateOnly Date { get; set; }

    public virtual Aktualizacja IdAktualizacjaNavigation { get; set; } = null!;

    public virtual Oprogramowanie IdOprogramowanieNavigation { get; set; } = null!;
}
