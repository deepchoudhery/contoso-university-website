using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using ContosoUniversity.Models;

namespace ContosoUniversity.BLL
{
    public class Instructors_Logic
    {
        private readonly ContosoUniversityEntities _context;
        private readonly IConfiguration _configuration;

        public Instructors_Logic(ContosoUniversityEntities context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Get Instructors List
        public List<Instructor> getInstructors()
        {
            var instructors = (from instructor in _context.Instructors
                               select instructor).ToList<Instructor>();

            return instructors;
        }
        #endregion

        #region Get Sorted Instructors List
        public List<Instructor> GetSortedInstrucors(string expression, string direction)
        {
            List<Instructor> list = new List<Instructor>();
            string query = "select * from dbo.[Instructors]";
            string connectionStr = _configuration.GetConnectionString("ContosoUniversity")
                ?? throw new InvalidOperationException("Connection string 'ContosoUniversity' not found.");

            if (!String.IsNullOrEmpty(expression))
            {
                // Whitelist valid column names to prevent SQL injection
                var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "InstructorID", "FirstName", "LastName", "BirthDate", "Email"
                };
                string safeDirection = string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase) ? "asc" : "desc";
                if (!allowedColumns.Contains(expression))
                {
                    throw new ArgumentException($"Invalid sort expression: {expression}");
                }
                query += " order by " + expression + " " + safeDirection;
            }

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Instructor instr = new Instructor();

                            instr.InstructorID = Convert.ToInt32(dr["InstructorID"]);
                            instr.FirstName = dr["FirstName"].ToString()!;
                            instr.LastName = dr["LastName"].ToString()!;
                            instr.BirthDate = DateTime.Parse(dr["BirthDate"].ToString()!);
                            instr.Email = dr["Email"].ToString()!;

                            list.Add(instr);
                        }
                        dr.Close();
                    }
                }
                con.Close();
            }
            return list;
        }
        #endregion
    }
}