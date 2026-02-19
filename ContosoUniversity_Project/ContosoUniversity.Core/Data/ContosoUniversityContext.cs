using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Data
{
    public class ContosoUniversityContext : DbContext
    {
        public ContosoUniversityContext(DbContextOptions<ContosoUniversityContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names to match existing database
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
            modelBuilder.Entity<Instructor>().ToTable("Instructors");
            modelBuilder.Entity<Student>().ToTable("Students");

            // Configure relationships
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
