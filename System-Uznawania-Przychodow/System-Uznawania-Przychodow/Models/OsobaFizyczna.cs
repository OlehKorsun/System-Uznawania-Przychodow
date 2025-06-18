using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class OsobaFizyczna
{
    public int IdKlient { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string Pesel { get; set; } = null!;

    public int IsDeleted { get; set; }

    public virtual Klient IdKlientNavigation { get; set; } = null!;
}
