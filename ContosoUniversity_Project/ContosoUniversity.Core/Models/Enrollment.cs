namespace ContosoUniversity.Core.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public DateTime Date { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }

        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }
}
