using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Models;
using ContosoUniversity.Bll;

namespace ContosoUniversity.Pages
{
    public class StudentsModel : PageModel
    {
        private readonly StudentsListLogic _studLogic;
        private readonly ContosoUniversityEntities _context;

        public List<dynamic> Students { get; set; } = new List<dynamic>();
        public List<string> Courses { get; set; } = new List<string>();
        public List<StudentInfo> SearchResults { get; set; } = new List<StudentInfo>();
        public string SearchName { get; set; } = string.Empty;

        public StudentsModel(StudentsListLogic studLogic, ContosoUniversityEntities context)
        {
            _studLogic = studLogic;
            _context = context;
        }

        public void OnGet()
        {
            LoadData();
        }

        public void OnGetSearch(string searchName)
        {
            SearchName = searchName ?? string.Empty;
            if (!string.IsNullOrEmpty(searchName))
            {
                SearchResults = _studLogic.GetStudents(searchName);
            }
            LoadData();
        }

        public IActionResult OnPostDelete(int id)
        {
            _studLogic.DeleteStudent(id);
            return RedirectToPage();
        }

        public IActionResult OnPostInsert(string firstName, string lastName, string birthDate, string email, string courseName)
        {
            if (DateTime.TryParse(birthDate, out DateTime birth))
            {
                _studLogic.InsertNewEntry(firstName, lastName, birth, courseName, email ?? "Has not specified");
            }
            return RedirectToPage();
        }

        private void LoadData()
        {
            Students = _studLogic.GetJoinedTableData().Cast<dynamic>().ToList();
            Courses = _context.Courses.Select(c => c.CourseName).ToList();
        }
    }

    public class StudentInfo
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public int StudentID { get; set; }
    }
}
