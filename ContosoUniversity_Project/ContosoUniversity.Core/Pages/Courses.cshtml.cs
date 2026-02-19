using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Data;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Pages
{
    public class CoursesModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public CoursesModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; } = new List<Course>();

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ToListAsync();
        }
    }
}
