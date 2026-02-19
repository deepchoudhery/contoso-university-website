using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ContosoUniversityContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if database is already seeded
            if (context.Students.Any())
            {
                return; // Database has been seeded
            }

            // Seed Departments
            var departments = new Department[]
            {
                new Department { DepartmentName = "Computer Science", BuildingNumber = 1, ManagingInstructorID = 1 },
                new Department { DepartmentName = "Mathematics", BuildingNumber = 2, ManagingInstructorID = 2 },
                new Department { DepartmentName = "Physics", BuildingNumber = 3, ManagingInstructorID = 3 }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

            // Seed Instructors
            var instructors = new Instructor[]
            {
                new Instructor { FirstName = "John", LastName = "Smith", BirthDate = new DateTime(1975, 3, 15), Email = "john.smith@contoso.edu" },
                new Instructor { FirstName = "Emily", LastName = "Johnson", BirthDate = new DateTime(1980, 7, 22), Email = "emily.johnson@contoso.edu" },
                new Instructor { FirstName = "Michael", LastName = "Brown", BirthDate = new DateTime(1978, 11, 8), Email = "michael.brown@contoso.edu" }
            };
            context.Instructors.AddRange(instructors);
            context.SaveChanges();

            // Seed Courses
            var courses = new Course[]
            {
                new Course { CourseName = "Introduction to Programming", StudentsMax = 30, DepartmentID = 1, InstructorID = 1 },
                new Course { CourseName = "Data Structures", StudentsMax = 25, DepartmentID = 1, InstructorID = 1 },
                new Course { CourseName = "Calculus I", StudentsMax = 40, DepartmentID = 2, InstructorID = 2 },
                new Course { CourseName = "Linear Algebra", StudentsMax = 35, DepartmentID = 2, InstructorID = 2 },
                new Course { CourseName = "Physics I", StudentsMax = 30, DepartmentID = 3, InstructorID = 3 }
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();

            // Seed Students
            var students = new Student[]
            {
                new Student { FirstName = "Alice", LastName = "Williams", BirthDate = new DateTime(2000, 5, 10), Email = "alice.williams@student.contoso.edu" },
                new Student { FirstName = "Bob", LastName = "Davis", BirthDate = new DateTime(1999, 8, 25), Email = "bob.davis@student.contoso.edu" },
                new Student { FirstName = "Charlie", LastName = "Miller", BirthDate = new DateTime(2001, 2, 14), Email = "charlie.miller@student.contoso.edu" },
                new Student { FirstName = "Diana", LastName = "Wilson", BirthDate = new DateTime(2000, 11, 30), Email = "diana.wilson@student.contoso.edu" },
                new Student { FirstName = "Eve", LastName = "Moore", BirthDate = new DateTime(1999, 6, 18), Email = "eve.moore@student.contoso.edu" }
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            // Seed Enrollments
            var enrollments = new Enrollment[]
            {
                new Enrollment { StudentID = 1, CourseID = 1, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 1, CourseID = 3, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 2, CourseID = 1, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 2, CourseID = 2, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 3, CourseID = 3, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 3, CourseID = 4, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 4, CourseID = 5, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 5, CourseID = 1, Date = new DateTime(2024, 9, 1) },
                new Enrollment { StudentID = 5, CourseID = 5, Date = new DateTime(2024, 9, 1) }
            };
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}
