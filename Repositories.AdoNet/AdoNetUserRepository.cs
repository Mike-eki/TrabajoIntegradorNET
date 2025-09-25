using Models;
using Models.Entities;
using Repositories;
using Repositories.Interfaces;
using System.Data.SqlClient;

namespace Repositories.AdoNet
{
    public class AdoNetUserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public AdoNetUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User? GetUserByUsername(string username)
        {
            throw new NotImplementedException();
            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //using var command = new SqlCommand(
            //    "SELECT Id, Username, Email, Name, PasswordHash, RoleId FROM Users WHERE Username = @Username",
            //    connection);
            //command.Parameters.AddWithValue("@Username", username);

            //using var reader = command.ExecuteReader();
            //return reader.Read() ? MapUserFromReader(reader) : null;
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
