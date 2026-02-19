namespace ContosoUniversity.Core.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int BuildingNumber { get; set; }
        public int ManagingInstructorID { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
