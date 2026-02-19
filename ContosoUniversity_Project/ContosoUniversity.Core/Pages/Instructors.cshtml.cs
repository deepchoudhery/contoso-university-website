using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Data;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Pages
{
    public class InstructorsModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public InstructorsModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<Instructor> Instructors { get; set; } = new List<Instructor>();

        public async Task OnGetAsync()
        {
            Instructors = await _context.Instructors
                .Include(i => i.Courses)
                .ToListAsync();
        }
    }
}
