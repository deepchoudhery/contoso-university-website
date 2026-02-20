using System;
using System.Collections.Generic;
using System.Linq;
using ContosoUniversity.Bll;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages
{
    public class StudentsModel : PageModel
    {
        public List<object> StudentsData { get; set; } = new List<object>();
        public List<string> Courses { get; set; } = new List<string>();
        public List<object> SearchResults { get; set; } = new List<object>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPostDelete(int id)
        {
            new StudentsListLogic().DeleteStudent(id);
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int id, string name, string email)
        {
            new StudentsListLogic().UpdateStudentData(id, name, email);
            return RedirectToPage();
        }

        public IActionResult OnPostAdd(string firstName, string lastName, string birthDate, string course, string email)
        {
            try
            {
                DateTime birth = DateTime.Parse(birthDate);
                new StudentsListLogic().InsertNewEntry(firstName, lastName, birth, course, email ?? "Has not specified");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            LoadData();
            return Page();
        }

        public IActionResult OnPostSearch(string searchName)
        {
            LoadData();
            SearchResults = new StudentsListLogic().GetStudents(searchName);
            return Page();
        }

        private void LoadData()
        {
            StudentsData = new StudentsListLogic().GetJoinedTableData();
            using var db = new ContosoUniversityEntities();
            Courses = db.Courses.Select(c => c.CourseName).ToList();
        }
    }
}
