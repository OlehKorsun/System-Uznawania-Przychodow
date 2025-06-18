using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class Rola
{
    public int IdRola { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
