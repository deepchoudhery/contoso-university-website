using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.BLL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages
{
    public class CoursesModel : PageModel
    {
        private readonly Courses_Logic _coursLogic;
        private readonly ContosoUniversityEntities _context;

        public List<string> Departments { get; set; } = new List<string>();
        public List<Cours> CoursesByDepartment { get; set; } = new List<Cours>();
        public List<Cours> CoursesByName { get; set; } = new List<Cours>();
        public string? SearchedDepartment { get; set; }
        public string? SearchedCourseName { get; set; }

        public CoursesModel(Courses_Logic coursLogic, ContosoUniversityEntities context)
        {
            _coursLogic = coursLogic;
            _context = context;
        }

        public void OnGet()
        {
            LoadDepartments();
        }

        public void OnGetSearchByName(string courseName)
        {
            SearchedCourseName = courseName;
            if (!string.IsNullOrEmpty(courseName))
            {
                CoursesByName = _coursLogic.GetCourse(courseName);
            }
            LoadDepartments();
        }

        public IActionResult OnPostSearchByDepartment(string department)
        {
            SearchedDepartment = department;
            if (!string.IsNullOrEmpty(department))
            {
                CoursesByDepartment = _coursLogic.GetCourses(department);
            }
            LoadDepartments();
            return Page();
        }

        private void LoadDepartments()
        {
            Departments = _context.Departments.Select(d => d.DepartmentName).ToList();
        }
    }
}
