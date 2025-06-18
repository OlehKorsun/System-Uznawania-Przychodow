using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Klient
{
    public int IdKlient { get; set; }

    public string Email { get; set; } = null!;

    public string Adres { get; set; } = null!;

    public string NrTelefonu { get; set; } = null!;

    public virtual Firma? Firma { get; set; }

    public virtual ICollection<Oplatum> Oplata { get; set; } = new List<Oplatum>();

    public virtual OsobaFizyczna? OsobaFizyczna { get; set; }

    public virtual ICollection<Umowa> UmowaOdbiorcaNavigations { get; set; } = new List<Umowa>();

    public virtual ICollection<Umowa> UmowaSprzedawcaNavigations { get; set; } = new List<Umowa>();
}
