using System.Collections.Generic;
using ContosoUniversity.Bll;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages
{
    public class AboutModel : PageModel
    {
        public Dictionary<string, int> Enrollments { get; set; } = new Dictionary<string, int>();

        public void OnGet()
        {
            Enrollments = new Enrollmet_Logic().Get_Enrollment_ByDate();
        }
    }
}
