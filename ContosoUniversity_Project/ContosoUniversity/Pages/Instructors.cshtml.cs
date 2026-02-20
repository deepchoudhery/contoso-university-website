using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.BLL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages
{
    public class InstructorsModel : PageModel
    {
        private readonly Instructors_Logic _instructorsLogic;

        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        public string NextSortDirection { get; set; } = "asc";

        public InstructorsModel(Instructors_Logic instructorsLogic)
        {
            _instructorsLogic = instructorsLogic;
        }

        public void OnGet(string? sortExpression = null, string? sortDirection = null)
        {
            if (!string.IsNullOrEmpty(sortExpression))
            {
                string direction = sortDirection == "asc" ? "asc" : "desc";
                NextSortDirection = direction == "asc" ? "desc" : "asc";
                Instructors = _instructorsLogic.GetSortedInstrucors(sortExpression, direction);
            }
            else
            {
                Instructors = _instructorsLogic.getInstructors();
                NextSortDirection = "asc";
            }
        }
    }
}
