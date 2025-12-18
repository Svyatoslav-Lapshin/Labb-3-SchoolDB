
using Labb_3_SchoolDB.Models;
using Microsoft.EntityFrameworkCore;
using School;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Labb_3_SchoolDB.Data;


namespace DatabaseSchool.Menu
{

    internal class MainMenu
    {
        private static readonly CultureInfo SwedishCulture = new CultureInfo("sv-SE");

        public static void DisplayMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            using SchoolInfoDbContext context = new SchoolInfoDbContext();
            while (true)
            {
                int userInput = InputValidation.ValidateIntInRange(
                    "1.Get all students\n" +
                    "2.Get all students in a specific class\n" +
                    "3.Adding new students\n" +
                    "4.Get staff\n" +
                    "5.Adding new staff\n" +
                    "Input your choice:", 1, 6);
                Console.Clear();
                switch (userInput)
                {
                    case 1:

                        int sortField = InputValidation.ValidateIntInRange(
                            "How would you like to sort the students?\n(1 - First name, 2 - Last name):", 1, 2);
                        int sortOrder = InputValidation.ValidateIntInRange(
                            "Sort order?\n(1 - Ascending, 2 - Descending):", 1, 2);
                        Console.Clear();
                        GetAllStudents(context, sortField, sortOrder);
                        break;

                    case 2:
                        Console.WriteLine();
                        GetStudentsInClass(context);
                        break;

                    case 3:
                        Console.WriteLine();
                        AddNewStudent(context);
                        break;

                    case 4:
                        int optionShowingChoice = InputValidation.ValidateIntInRange(
                           "What would you like to view?\n(1 - View all employees, 2 - View employees by category):", 1, 2);
                        ShowAllStaff(context, optionShowingChoice);

                        break;

                    case 5:

                        Console.WriteLine();
                        AddNewStaff(context);
                        break;

                    case 6:
                        Console.WriteLine("Goodbye");
                        return;
                }

            }
            static void GetAllStudents(SchoolInfoDbContext context, int sortField, int sortOrder)
            {
                var students = context.Students.ToList();
                switch (sortField)
                {
                    case 1: //First name
                        if (sortOrder == 1)
                        {
                            students = students.OrderBy(s => s.StudentName, StringComparer.Create(SwedishCulture, ignoreCase: true)).ToList();
                        }
                        else
                        {
                            students = students.OrderByDescending(s => s.StudentName, StringComparer.Create(SwedishCulture, ignoreCase: true)).ToList();
                        }
                        break;

                    case 2://Last name
                        if (sortOrder == 1)
                        {
                            students = students.OrderBy(s => s.StudentLastName, StringComparer.Create(SwedishCulture, ignoreCase: true)).ToList();
                        }
                        else
                        {
                            students = students.OrderByDescending(s => s.StudentLastName, StringComparer.Create(SwedishCulture, ignoreCase: true)).ToList();
                        }
                        break;
                }

                Console.Clear();
                Console.WriteLine("List of students:");
                foreach (var student in students)
                {
                    Console.WriteLine($"• {student.StudentName} {student.StudentLastName}");

                }

                ClearConsoleAfterKeyPress();
            }


            static void GetStudentsInClass(SchoolInfoDbContext context)
            {
                ShowAllClasees(context);
                string userInpputClass = InputValidation.ValidateStringInput("Enter the class name to get the students in that class:");
                while (!context.Classes.Any(c => c.ClassName.Equals(userInpputClass)))
                {
                    Console.WriteLine("Invalid class name. Please try again.");
                    userInpputClass = InputValidation.ValidateStringInput("Enter the class name to get the students in that class:");
                }

                var result = context.Students
                    .Include(с => с.Class)
                    .Where(s => s.Class != null && s.Class.ClassName == userInpputClass)
                    .ToList();

                var studentsClass = result.ToList();

                Console.WriteLine("Students from specific class:");
                foreach (var student in studentsClass)
                {
                    Console.WriteLine($"• {student.StudentName} {student.StudentLastName} Class: {student.Class?.ClassName}");
                }

                ClearConsoleAfterKeyPress();
            }


            static void AddNewStudent(SchoolInfoDbContext context)
            {
                ShowAllClasees(context);

                int classId = InputValidation.ValidateIntInput("Enter the class ID for the student:");
                while (!context.Classes.Any(c => c.ClassId == classId))
                {
                    Console.WriteLine("Invalid class ID. Please try again.");
                    classId = InputValidation.ValidateIntInput("Enter the class ID for the student:");
                }

                var newStudent = new Student
                {
                    StudentName = InputValidation.ValidateStringInput("Enter the student's first name:"),
                    StudentLastName = InputValidation.ValidateStringInput("Enter the student's last name:"),
                    Email = InputValidation.ValidateStringInput("Enter the student's email:"),
                    PersonNumber = InputValidation.ValidateStringInput("Enter the student's person number:"),
                    ClassId = classId
                };
                context.Students.Add(newStudent);
                context.SaveChanges();

                ClearConsoleAfterKeyPress();
            }

            static void ShowAllStaff(SchoolInfoDbContext context, int choice)
            {

                switch (choice)
                {
                    case 1:
                        var employees = context.Employees.Include(e => e.EmployeeRoles).ToList();
                        Console.WriteLine("Available staff:");
                        foreach (var employee in employees)
                        {
                            Console.WriteLine($"• {employee.EmployeeId} --> {employee.EmployeeName} {employee.EmployeeLastName}");
                        }

                        break;
                    case 2:
                        ShowAllRoles(context);
                        int roleId = InputValidation.ValidateIntInput("Enter the role ID to filter employees by role:");
                        while (!context.EmployeeRoles.Any(r => r.EmployeeRoleId == roleId))
                        {
                            Console.WriteLine("Invalid role ID. Please try again.");
                            roleId = InputValidation.ValidateIntInput("Enter the role ID to filter employees by role:");
                        }

                        var employeeRole = context.EmployeeRoles
                            .Include(e => e.Employees)
                            .Where(e => e.EmployeeRoleId == roleId).ToList();

                        foreach (var employee in employeeRole)
                        {
                            Console.WriteLine($"{employee.RoleName} Role");
                            foreach (var emp in employee.Employees)
                            {
                                Console.WriteLine($"•{emp.EmployeeName} {emp.EmployeeLastName}");
                            }

                        }

                        break;


                }
                ClearConsoleAfterKeyPress();

            }


            static void AddNewStaff(SchoolInfoDbContext context)
            {
                var newEmployee = new Employee
                {
                    EmployeeName = InputValidation.ValidateStringInput("Enter the employee's first name:"),
                    EmployeeLastName = InputValidation.ValidateStringInput("Enter the employee's last name:"),

                };
                context.Employees.Add(newEmployee);
                context.SaveChanges();

                ShowAllRoles(context);
                int roleId = InputValidation.ValidateIntInput("Enter the role ID to filter employees by role:");
                while (!context.EmployeeRoles.Any(r => r.EmployeeRoleId == roleId))
                {
                    Console.WriteLine("Invalid role ID. Please try again.");
                    roleId = InputValidation.ValidateIntInput("Enter the role ID to filter employees by role:");
                }

                //Assign role to the new employee
                var role = context.EmployeeRoles.Find(roleId);
                if (role != null)
                {
                    newEmployee.EmployeeRoles.Add(role);
                }

                //If role is teacher, add teacher-specific data
                if (roleId == 1)
                {
                    AddTeacherData(context, newEmployee.EmployeeId);
                }
                ClearConsoleAfterKeyPress();

            }


            static void AddTeacherData(SchoolInfoDbContext context, int employeeId)
            {

                int amountOfCourse = InputValidation.ValidateIntInput("Enter the number of courses to assign to the teacher: ");
                int counter = 0;
                int courseId = 0;
                while (counter < amountOfCourse)
                {
                    ShowAllCourses(context);
                    //Additional logic to add teacher-specific data
                    courseId = InputValidation.ValidateIntInput("Enter the course ID to assign to the teacher:");
                    while (!context.Courses.Any(c => c.CourseId == courseId))
                    {
                        Console.WriteLine("Invalid course ID. Please try again.");
                        courseId = InputValidation.ValidateIntInput("Enter the course ID to assign to the teacher:");
                    }
                    counter++;
                    Console.Clear();
                }


                ShowAllClasees(context);
                int classId = InputValidation.ValidateIntInput("Enter the class ID to assign to the teacher:");
                while (!context.Classes.Any(c => c.ClassId == classId))
                {
                    Console.WriteLine("Invalid class ID. Please try again.");
                    classId = InputValidation.ValidateIntInput("Enter the class ID to assign to the teacher:");
                }

                //Add course and class assignment to the teacher
                var course = context.Courses.Find(courseId);
                var employee = context.Employees.Find(employeeId);
                var classEntity = context.Classes.Find(classId);

                if (classEntity != null)
                {
                    employee?.Classes.Add(classEntity);

                }
                if (course != null)
                {
                    employee?.Courses.Add(course);
                }


                context.SaveChanges();

            }

            //Helpter methods to show data from the database
            static void ShowAllCourses(SchoolInfoDbContext context)
            {
                var courses = context.Courses.ToList();
                Console.WriteLine("Available courses:");
                foreach (var course in courses)
                {
                    Console.WriteLine($"{course.CourseId} -->{course.CourseName}");
                }
            }

            static void ShowAllRoles(SchoolInfoDbContext context)
            {
                var roles = context.EmployeeRoles.ToList();
                Console.WriteLine("Available roles:");
                foreach (var role in roles)
                {
                    Console.WriteLine($"• {role.EmployeeRoleId} -->{role.RoleName}");
                }
            }

            static void ShowAllClasees(SchoolInfoDbContext context)
            {

                var classes = context.Classes.ToList();
                Console.WriteLine("Available classes:");
                foreach (var className in classes)
                {
                    Console.WriteLine($"• {className.ClassId} -->{className.ClassName}");
                }
            }

            //Helper method to clear console after key press
            static void ClearConsoleAfterKeyPress()
            {
                Console.WriteLine("\nPress any button to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}