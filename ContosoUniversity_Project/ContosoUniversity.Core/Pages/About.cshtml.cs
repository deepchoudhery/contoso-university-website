using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Data;

namespace ContosoUniversity.Core.Pages
{
    public class AboutModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public AboutModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<EnrollmentDateGroup> EnrollmentStats { get; set; } = new List<EnrollmentDateGroup>();

        public async Task OnGetAsync()
        {
            EnrollmentStats = await _context.Enrollments
                .GroupBy(e => e.Date)
                .Select(g => new EnrollmentDateGroup
                {
                    EnrollmentDate = g.Key,
                    StudentCount = g.Count()
                })
                .OrderBy(e => e.EnrollmentDate)
                .ToListAsync();
        }
    }

    public class EnrollmentDateGroup
    {
        public DateTime EnrollmentDate { get; set; }
        public int StudentCount { get; set; }
    }
}
