namespace ContosoUniversity.Core.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int StudentsMax { get; set; }
        public int DepartmentID { get; set; }
        public int InstructorID { get; set; }

        public Department? Department { get; set; }
        public Instructor? Instructor { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
