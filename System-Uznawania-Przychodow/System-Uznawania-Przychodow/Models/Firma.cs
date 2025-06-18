using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Firma
{
    public int IdKlient { get; set; }

    public string Nazwa { get; set; } = null!;

    public string Krs { get; set; } = null!;

    public virtual Klient IdKlientNavigation { get; set; } = null!;
}
