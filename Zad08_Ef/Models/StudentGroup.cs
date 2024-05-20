using System;
using System.Collections.Generic;

namespace Zad08_Ef.Models;

public partial class StudentGroup
{
    public int IdStudent { get; set; }

    public int IdGroup { get; set; }

    public DateTime RegisteredAt { get; set; }

    public virtual Group IdGroupNavigation { get; set; } = null!;

    public virtual Student IdStudentNavigation { get; set; } = null!;
}
