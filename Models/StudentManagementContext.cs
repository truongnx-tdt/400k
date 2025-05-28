using System.Data.Entity;
using StudentManagement.Models;

public class StudentManagementContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<AcademicRecord> AcademicRecords { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    public StudentManagementContext() : base("name=StudentManagementConnectionString") { }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicRecord>()
            .HasKey(a => a.RecordId); // Xác định khóa chính
        modelBuilder.Entity<Department>()
            .HasKey(d => d.DepartmentId); // Xác định khóa chính
        modelBuilder.Entity<Class>()
            .HasKey(c => c.ClassId); // Xác định khóa chính
        modelBuilder.Entity<Subject>()
            .HasKey(s => s.SubjectId); // Xác định khóa chính
        modelBuilder.Entity<Student>()
            .HasKey(s => s.StudentId); // Xác định khóa chính
        modelBuilder.Entity<Role>()
            .HasKey(r => r.RoleId); // Xác định khóa chính
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId); // Xác định khóa chính
        base.OnModelCreating(modelBuilder);
    }

}