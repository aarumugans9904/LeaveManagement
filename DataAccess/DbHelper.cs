using Microsoft.Data.SqlClient;

namespace EmployeeLeaveManagementAPI.DataAccess
{
    public class DbHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}