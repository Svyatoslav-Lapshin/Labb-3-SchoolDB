using System;
using System.Collections.Generic;

namespace Labb_3_SchoolDB.Models;

public partial class GradeScale
{
    public int GradeScaleId { get; set; }

    public string Letter { get; set; } = null!;

    public decimal Value { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
