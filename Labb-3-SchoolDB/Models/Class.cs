using System;
using System.Collections.Generic;

namespace Labb_3_SchoolDB.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Employee? Teacher { get; set; }
}
