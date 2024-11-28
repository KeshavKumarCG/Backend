using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString = "YourDatabaseConnectionString";

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Start a transaction
                        using (var transaction = connection.BeginTransaction())
                        {
                            // Fetch the next CarID for the Cars table
                            string newCarID = GetNextCarID(connection, transaction);

                            // Insert into Cars table
                            string carInsertQuery = @"
                                INSERT INTO Cars (ID, CarNumber, CarModel, StatusID, CreatedBy, OwnerID, ValetID)
                                VALUES (@ID, @CarNumber, @CarModel, @StatusID, @CreatedBy, NULL, NULL)";
                            using (var carCmd = new SqlCommand(carInsertQuery, connection, transaction))
                            {
                                carCmd.Parameters.AddWithValue("@ID", newCarID);
                                carCmd.Parameters.AddWithValue("@CarNumber", model.CarNumber);
                                carCmd.Parameters.AddWithValue("@CarModel", model.CarModel);
                                carCmd.Parameters.AddWithValue("@StatusID", model.StatusID);
                                carCmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                                carCmd.ExecuteNonQuery();
                            }

                            // Insert into Users table
                            string userInsertQuery = @"
                                INSERT INTO Users (CYGID, Name, PhoneNumber, Email, Password, RoleID, CreatedBy, CarID)
                                VALUES (@CYGID, @Name, @PhoneNumber, @Email, @Password, @RoleID, @CreatedBy, @CarID)";
                            using (var userCmd = new SqlCommand(userInsertQuery, connection, transaction))
                            {
                                userCmd.Parameters.AddWithValue("@CYGID", model.CYGID);
                                userCmd.Parameters.AddWithValue("@Name", model.Name);
                                userCmd.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                                userCmd.Parameters.AddWithValue("@Email", model.Email);
                                userCmd.Parameters.AddWithValue("@Password", model.Password);
                                userCmd.Parameters.AddWithValue("@RoleID", model.RoleID);
                                userCmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                                userCmd.Parameters.AddWithValue("@CarID", newCarID);
                                userCmd.ExecuteNonQuery();
                            }

                            // Commit transaction
                            transaction.Commit();
                        }

                        return Ok(new { message = "User and Car added successfully!" });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { message = ex.Message });
                    }
                }
            }

            return BadRequest(ModelState);
        }

        private string GetNextCarID(SqlConnection connection, SqlTransaction transaction)
        {
            // Query to get the max CarID
            string query = "SELECT MAX(ID) FROM Cars";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    string lastCarID = result.ToString();
                    int numericPart = int.Parse(lastCarID.Substring(3)); // Extract numeric part
                    return $"CAR{(numericPart + 1).ToString("D3")}"; // Increment and format
                }
                else
                {
                    return "CAR001"; // Default start
                }
            }
        }
    }
}
