using System;
using System.Collections.Generic;

namespace MauiAppProject;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
