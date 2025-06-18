﻿using System;
using System.Collections.Generic;

namespace System_Uznawania_Przychodow.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int IdRola { get; set; }

    public virtual Rola IdRolaNavigation { get; set; } = null!;
}
