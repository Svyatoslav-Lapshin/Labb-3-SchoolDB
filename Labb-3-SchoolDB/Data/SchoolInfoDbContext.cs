using System;
using System.Collections.Generic;
using Labb_3_SchoolDB.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb_3_SchoolDB.Data;

public partial class SchoolInfoDbContext : DbContext
{
    public SchoolInfoDbContext()
    {
    }

    public SchoolInfoDbContext(DbContextOptions<SchoolInfoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<GradeScale> GradeScales { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=.;Database=SchoolInfoDb;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927C02FAFF745");

            entity.ToTable("Class");

            entity.Property(e => e.ClassName).HasMaxLength(100);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Class_Teacher");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A7E6E246B8");

            entity.ToTable("Course");

            entity.Property(e => e.CourseName).HasMaxLength(150);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11DD6E54B3");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeLastName).HasMaxLength(150);
            entity.Property(e => e.EmployeeName).HasMaxLength(150);

            entity.HasMany(d => d.Courses).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeacherCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_COURSE_TEACHER"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Teacher_Course"),
                    j =>
                    {
                        j.HasKey("TeacherId", "CourseId").HasName("PK_TeacherCourseId");
                        j.ToTable("TeacherCourse");
                    });

            entity.HasMany(d => d.EmployeeRoles).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeRoleAssignment",
                    r => r.HasOne<EmployeeRole>().WithMany()
                        .HasForeignKey("EmployeeRoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Employee_Role"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Employee_RoleAssign"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "EmployeeRoleId").HasName("PK_Employee_Role");
                        j.ToTable("EmployeeRoleAssignment");
                    });
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.EmployeeRoleId).HasName("PK__Employee__346186E682E868A7");

            entity.ToTable("EmployeeRole");

            entity.Property(e => e.RoleName).HasMaxLength(150);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade__54F87A57AB8FB54D");

            entity.ToTable("Grade");

            entity.HasOne(d => d.Course).WithMany(p => p.Grades)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Grade_Course");

            entity.HasOne(d => d.GradeScale).WithMany(p => p.Grades)
                .HasForeignKey(d => d.GradeScaleId)
                .HasConstraintName("FK_Grade_GradeScale");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Grade_Student");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Grade_Teacher");
        });

        modelBuilder.Entity<GradeScale>(entity =>
        {
            entity.HasKey(e => e.GradeScaleId).HasName("PK__GradeSca__B5AD3A67F75B1DEF");

            entity.ToTable("GradeScale");

            entity.Property(e => e.Letter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Value).HasColumnType("decimal(5, 1)");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B99B8356C26");

            entity.ToTable("Student");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.PersonNumber).HasMaxLength(20);
            entity.Property(e => e.StudentLastName).HasMaxLength(150);
            entity.Property(e => e.StudentName).HasMaxLength(150);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Student_Class");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
