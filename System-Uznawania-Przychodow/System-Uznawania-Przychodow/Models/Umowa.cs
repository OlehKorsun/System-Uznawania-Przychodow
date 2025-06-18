using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Umowa
{
    public int IdUmowa { get; set; }

    public DateOnly DataOd { get; set; }

    public DateOnly DataDo { get; set; }

    public int CzyOplacona { get; set; }

    public int CzyPodpisana { get; set; }

    public decimal Cena { get; set; }

    public int Sprzedawca { get; set; }

    public int Odbiorca { get; set; }

    public int IdOprogramowanie { get; set; }

    public int? IdZnizka { get; set; }

    public int? IdRata { get; set; }

    public virtual Oprogramowanie IdOprogramowanieNavigation { get; set; } = null!;

    public virtual Ratum? IdRataNavigation { get; set; }

    public virtual Znizka? IdZnizkaNavigation { get; set; }

    public virtual Klient OdbiorcaNavigation { get; set; } = null!;

    public virtual ICollection<Oplatum> Oplata { get; set; } = new List<Oplatum>();

    public virtual Klient SprzedawcaNavigation { get; set; } = null!;
}
