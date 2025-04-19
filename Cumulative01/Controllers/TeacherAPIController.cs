
using Microsoft.AspNetCore.Mvc;
using Cumulative01.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Cumulative01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves a list of all teachers in the system
        /// </summary>
        /// <example>
        /// GET api/TeacherAPI/Teacher
        /// </example>
        /// <returns>A list of Teacher objects containing all teacher details</returns>
        [HttpGet(template: "Teacher")]
        public List<Teacher> ListTeacherNames()
        {
            List<Teacher> teachers = new List<Teacher>();

            MySqlConnection Connection = _context.GetConnection();

            Connection.Open();


            string SQLQuery = "SELECT * FROM teachers";

            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = SQLQuery;

            MySqlDataReader DataReader = Command.ExecuteReader();



            while (DataReader.Read())
            {

                int TeacherId = Convert.ToInt32(DataReader["teacherid"]);
                string TeacherFname = DataReader["teacherfname"].ToString();
                string TeacherLname = DataReader["teacherlname"].ToString();
                string EmployeeID = DataReader["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(DataReader["hiredate"]);
                double Salary = Convert.ToDouble(DataReader["salary"]);

                Teacher newTeacher = new Teacher();
                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherFname = TeacherFname;
                newTeacher.TeacherLname = TeacherLname;
                newTeacher.EmployeeID = EmployeeID;
                newTeacher.HireDate = HireDate;
                newTeacher.Salary = Salary;

                teachers.Add(newTeacher);


            }

            Connection.Close();



            return teachers;
        }
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        /// <summary>
        /// Retrieves a specific teacher by their ID
        /// </summary>
        /// <example>
        /// GET api/TeacherAPI/FindTeacher/5
        /// </example>
        /// <param name="id">The ID of the teacher to retrieve</param>
        /// <returns>A Teacher object containing the teacher's details</returns>
        public Teacher FindTeacher(int id)
        {
            Teacher teacher = new Teacher();

            MySqlConnection Connection = _context.GetConnection();
            Connection.Open();

            string SQL = "Select * FROM teachers Where teacherid = " + id.ToString();

            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = SQL;

            MySqlDataReader DataReader = Command.ExecuteReader();


            while (DataReader.Read())
            {
                int TeacherId = Convert.ToInt32(DataReader["teacherid"]);
                string TeacherFname = DataReader["teacherfname"].ToString();
                string TeacherLname = DataReader["teacherlname"].ToString();
                string EmployeeID = DataReader["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(DataReader["hiredate"]);
                double Salary = Convert.ToDouble(DataReader["salary"]);

                teacher.TeacherId = TeacherId;
                teacher.TeacherFname = TeacherFname;
                teacher.TeacherLname = TeacherLname;

                teacher.EmployeeID = EmployeeID;
                teacher.HireDate = HireDate;
                teacher.Salary = Salary;
            }

            Connection.Close();


            return teacher;
        }




        /// <summary>  
        /// Inserts a new Teacher record into the database  
        /// </summary>  
        /// <param name="TeacherData">The Teacher object containing the details to be added</param>  
        /// <example>  
        /// Example POST request:  
        /// POST: api/TeacherAPI/AddTeacher  
        /// Headers:  
        ///   Content-Type: application/json  
        /// Body:  
        /// {  
        ///     "TeacherFirstName": "Jane",  
        ///     "TeacherLastName": "Smith",  
        ///     "EmployeeID": "T67890",  
        ///     "HireDate": "2024-01-15",  
        ///     "Salary": 80000  
        /// }  
        /// </example>  
        /// <returns>  
        /// The generated Teacher ID if successful; returns 0 if the operation fails.  
        /// </returns>  
        [HttpPost("AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)
        {
            using (MySqlConnection Connection = _context.GetConnection())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = @"INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) 
                                VALUES (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";


                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFname);

                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLname);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeID);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);

                Command.ExecuteNonQuery();


                return Convert.ToInt32(Command.LastInsertedId);
            }


            return 0;
        }



        /// <summary>
        /// Removes a Teacher record from the database based on the specified ID
        /// </summary>
        /// <param name="id">The unique identifier of the teacher to be removed</param>
        /// <example>
        /// Example request:
        /// DELETE: api/TeacherAPI/DeleteTeacher/5
        /// </example>
        /// <returns>
        /// Returns the count of affected database rows after the deletion
        /// </returns>
        [HttpDelete("DeleteTeacher/{id}")]
        public int DeleteTeacher(int id)
        {
            using (MySqlConnection Connection = _context.GetConnection())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "DELETE FROM teachers WHERE teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                return Command.ExecuteNonQuery();
            }


            return 0;
        }
        /// <summary>
        /// Updates an existing Teacher in the database
        /// </summary>
        /// <param name="TeacherId">Primary key of the teacher to update</param>
        /// <param name="TeacherData">Teacher object with updated information</param>
        /// <example>
        /// PUT: api/TeacherAPI/UpdateTeacher/3
        /// BODY: { "TeacherFirstName": "John", "TeacherLastName": "Doe", "EmployeeID": "T123", "HireDate": "2021-09-01", "Salary": 60000 }
        /// </example>
        /// <returns>
        /// Nothing (void). Updates the teacher record in the database.
        /// </returns>

        [HttpPut("UpdateTeacher/{TeacherId}")]
        public void UpdateTeacher(int TeacherId, [FromBody] Teacher TeacherData)
        {
            using (MySqlConnection Connection = _context.GetConnection())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = @"UPDATE teachers 
                                SET teacherfname = @teacherfname, 
                                    teacherlname = @teacherlname, 
                                    employeenumber = @employeenumber, 
                                    hiredate = @hiredate, 
                                    salary = @salary 
                                WHERE teacherid = @id";

                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFname);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLname);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeID);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);
                Command.Parameters.AddWithValue("@id", TeacherId);

                Command.ExecuteNonQuery();
            }
        }

    }
}


   
    


