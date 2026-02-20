using System.Collections.Generic;
using ContosoUniversity.BLL;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages
{
    public class InstructorsModel : PageModel
    {
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        public string NextSortDirection { get; set; } = "asc";

        public void OnGet(string sortExpression = null, string sortDirection = "desc")
        {
            var logic = new Instructors_Logic();
            if (!string.IsNullOrEmpty(sortExpression))
            {
                Instructors = logic.GetSortedInstrucors(sortExpression, sortDirection);
                NextSortDirection = sortDirection == "asc" ? "desc" : "asc";
            }
            else
            {
                Instructors = logic.getInstructors();
            }
        }
    }
}
