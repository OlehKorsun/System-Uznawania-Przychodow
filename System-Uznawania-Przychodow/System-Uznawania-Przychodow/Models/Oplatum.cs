using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Oplatum
{
    public int IdOplata { get; set; }

    public DateTime Date { get; set; }

    public int IdKlient { get; set; }

    public int IdUmowa { get; set; }

    public decimal Wartosc { get; set; }

    public int CzyZwrocone { get; set; }

    public virtual Klient IdKlientNavigation { get; set; } = null!;

    public virtual Umowa IdUmowaNavigation { get; set; } = null!;
}
