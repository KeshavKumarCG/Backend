using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString = "Data Source=IN-ANUSHKA-SONI\\SQLEXPRESS;Initial Catalog=CarParkingSystemDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context; // Correctly assign the context from DI
        }

      [HttpDelete("{carId}")]
public async Task<IActionResult> DeleteUserAndArchive(string carId)
{
    // Begin a transaction to ensure atomicity
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // Fetch the car record based on the provided CarID
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == carId);
        if (car == null)
        {
            return NotFound("Car not found");
        }

        // Fetch the user associated with the car's ID (Users.CarID matches Cars.ID)
        var user = await _context.Users.FirstOrDefaultAsync(u => u.CarID == carId);
        if (user == null)
        {
            return NotFound("User not found for the given CarID");
        }

        // Prepare archive data
        var archive = new Archive
        {
            Name = user.Name,
            Phone = user.PhoneNumber,
            Email = user.Email,
            Role = "User", // You can modify this role assignment if needed
            CarNumber = car.CarNumber,
            CarModel = car.CarModel,
            ArchivedAt = DateTime.UtcNow
        };

        // Add the record to the Archives table
        await _context.Archives.AddAsync(archive);

        // Remove the user and car records from the respective tables
        _context.Users.Remove(user);
        _context.Cars.Remove(car);

        // Save all changes
        await _context.SaveChangesAsync();

        // Commit the transaction
        await transaction.CommitAsync();

        return Ok("User and associated car data archived and deleted successfully.");
    }
    catch (Exception ex)
    {
        // Roll back the transaction on any error
        await transaction.RollbackAsync();

        return BadRequest(new
        {
            message = "Error archiving user and deleting records",
            error = ex.Message,
            innerException = ex.InnerException?.Message
        });
    }
}

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

        [HttpPut("UpdateUser/{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            // Update Cars
                            string carUpdateQuery = @"
                                UPDATE Cars
                                SET CarNumber = @CarNumber, CarModel = @CarModel
                                WHERE OwnerID = @UserID";
                            using (var carCmd = new SqlCommand(carUpdateQuery, connection, transaction))
                            {
                                carCmd.Parameters.AddWithValue("@CarNumber", model.CarNumber);
                                carCmd.Parameters.AddWithValue("@CarModel", model.CarModel);
                                carCmd.Parameters.AddWithValue("@UserID", userId);
                                carCmd.ExecuteNonQuery();
                            }

                            // Update Users
                            string userUpdateQuery = @"
                                UPDATE Users
                                SET Name = @Name, PhoneNumber = @PhoneNumber, Email = @Email
                                WHERE UserID = @UserID";
                            using (var userCmd = new SqlCommand(userUpdateQuery, connection, transaction))
                            {
                                userCmd.Parameters.AddWithValue("@Name", model.Name);
                                userCmd.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                                userCmd.Parameters.AddWithValue("@Email", model.Email);
                                userCmd.Parameters.AddWithValue("@UserID", userId);
                                userCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return Ok(new { message = "User and Car updated successfully!" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { message = ex.Message });
                    }
                }
            }

            return BadRequest(ModelState);
        }

        // Helper method to generate a new CarID
        private string GenerateNewCarID(SqlConnection connection, SqlTransaction transaction)
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
