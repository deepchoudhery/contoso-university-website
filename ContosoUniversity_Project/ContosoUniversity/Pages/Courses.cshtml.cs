using System.Collections.Generic;
using System.Linq;
using ContosoUniversity.BLL;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages
{
    public class CoursesModel : PageModel
    {
        public List<string> Departments { get; set; } = new List<string>();
        public string SelectedDepartment { get; set; }
        public List<Cours> CoursesByDepartment { get; set; } = new List<Cours>();
        public string CourseSearchName { get; set; }
        public Cours CourseByName { get; set; }

        public void OnGet()
        {
            LoadDepartments();
        }

        public IActionResult OnPostSearchByDepartment(string department)
        {
            LoadDepartments();
            SelectedDepartment = department;
            CoursesByDepartment = new Courses_Logic().GetCourses(department);
            return Page();
        }

        public IActionResult OnPostSearchByName(string courseName)
        {
            LoadDepartments();
            CourseSearchName = courseName;
            var results = new Courses_Logic().GetCourse(courseName);
            CourseByName = results.FirstOrDefault();
            return Page();
        }

        private void LoadDepartments()
        {
            using var db = new ContosoUniversityEntities();
            Departments = db.Departments.Select(d => d.DepartmentName).ToList();
        }
    }
}
