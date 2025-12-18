using System;
using System.Collections.Generic;

namespace Labb_3_SchoolDB.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<Employee> Teachers { get; set; } = new List<Employee>();
}
