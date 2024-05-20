using System;
using System.Collections.Generic;

namespace Zad08_Ef.Models;

public partial class Student
{
    public int IdStudent { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}
