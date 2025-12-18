using System;
using System.Collections.Generic;

namespace Labb_3_SchoolDB.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public DateOnly DateOfIssue { get; set; }

    public int? TeacherId { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public int? GradeScaleId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual GradeScale? GradeScale { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Employee? Teacher { get; set; }
}
