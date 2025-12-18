using System;
using System.Collections.Generic;

namespace Labb_3_SchoolDB.Models;

public partial class EmployeeRole
{
    public int EmployeeRoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
