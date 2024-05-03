using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Services
{
    public class LogService: ILogService
    {
        private readonly string _connectionString;

        public LogService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void WriteToDatabase(string severity, string message, string context)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                string query = "INSERT INTO logs (Severity, Message, Context, DateAndTime) " +
                               "VALUES (@Severity, @Message, @Context, @DateAndTime)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Severity", severity); //Ex: Error, Warning, Success
                    command.Parameters.AddWithValue("@Message", message); //Ex: Home page did not load.
                    command.Parameters.AddWithValue("@Context", context); //Extra info, Ex: HomeController.cs
                    command.Parameters.AddWithValue("@DateAndTime", DateTime.Now); //Date and time gets defaulted at the time the log is created.

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        throw new Exception("Failed to insert log into database.");
                    }
                }
            }
        }
    }
}
