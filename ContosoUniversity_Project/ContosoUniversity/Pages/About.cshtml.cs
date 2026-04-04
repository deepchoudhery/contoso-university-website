using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Bll;

namespace ContosoUniversity.Pages
{
    public class AboutModel : PageModel
    {
        private readonly Enrollmet_Logic _enrollmetLogic;

        public Dictionary<string, int> EnrollmentStats { get; set; } = new Dictionary<string, int>();

        public AboutModel(Enrollmet_Logic enrollmetLogic)
        {
            _enrollmetLogic = enrollmetLogic;
        }

        public void OnGet()
        {
            EnrollmentStats = _enrollmetLogic.Get_Enrollment_ByDate();
        }
    }
}
