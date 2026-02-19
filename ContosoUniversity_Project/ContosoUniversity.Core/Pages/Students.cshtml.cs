using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Data;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Pages
{
    public class StudentsModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public StudentsModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get; set; } = new List<Student>();
        public string? SearchString { get; set; }

        public async Task OnGetAsync(string? searchString)
        {
            SearchString = searchString;

            IQueryable<Student> studentsQuery = _context.Students.Include(s => s.Enrollments);

            if (!string.IsNullOrEmpty(searchString))
            {
                string[] nameParts = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length >= 2)
                {
                    string firstName = nameParts[0];
                    string lastName = nameParts[1];
                    studentsQuery = studentsQuery.Where(s => 
                        s.FirstName.Contains(firstName) && s.LastName.Contains(lastName));
                }
                else if (nameParts.Length == 1)
                {
                    studentsQuery = studentsQuery.Where(s => 
                        s.FirstName.Contains(nameParts[0]) || s.LastName.Contains(nameParts[0]));
                }
            }

            Students = await studentsQuery.ToListAsync();
        }
    }
}
