using System;
using System.Collections.Generic;

namespace Labb_3_SchoolDB.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public string StudentLastName { get; set; } = null!;

    public string PersonNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
