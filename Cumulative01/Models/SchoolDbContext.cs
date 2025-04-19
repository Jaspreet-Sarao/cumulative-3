using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace Cumulative01.Models
{
    public class SchoolDbContext
    {
        //Here will set our credentials to connect to the database

        private static string User { get { return "Jaspreet"; } }
        private static string Password { get { return "1234"; } }
        private static string Database { get { return "School"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        // This method will return the connection string to connect to the schoolDb

        protected static string ConnectionString
        {
            get
            {
                return "server=" + Server
                    + ";user=" + User
                    + ";database=" + Database
                    + ";port=" + Port
                    + ";password=" + Password + "; convert zero datetime = True";
            }
        }
        /// <summary>
        /// Creates and returns a new MySQL connection instance
        /// </summary>
        /// <returns>An initialized MySqlConnection object</returns>
        /// <example>
        /// <code>
        /// using(var connection = dbContext.GetConnection())
        /// {
        ///     connection.Open();
        ///     // Database operations
        /// }
        /// </code>
        /// </example>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
      

    }

}
    