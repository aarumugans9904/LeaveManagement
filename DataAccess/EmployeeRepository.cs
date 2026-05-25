using EmployeeLeaveManagementAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeLeaveManagementAPI.DataAccess
{
    public class EmployeeRepository
    {
        private readonly DbHelper _dbHelper;

        public EmployeeRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void AddEmployee(Employee employee)
        {
            using SqlConnection con = _dbHelper.GetConnection();

            string query = @"INSERT INTO Employees
                           (EmployeeName,Email,Department)
                           VALUES
                           (@EmployeeName,@Email,@Department)";

            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
            {
                throw new Exception("An employee with this email already exists.");
            }

        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new();

            using SqlConnection con = _dbHelper.GetConnection();

            string query = "SELECT * FROM Employees";

            SqlCommand cmd = new(query, con);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    EmployeeName = reader["EmployeeName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString()
                });
            }

            return employees;
        }

        public Employee GetEmployeeById(int id)
        {
            Employee employee = null;

            using SqlConnection con = _dbHelper.GetConnection();

            string query =
                "SELECT * FROM Employees WHERE EmployeeId=@Id";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                employee = new Employee
                {
                    EmployeeId =
                        Convert.ToInt32(reader["EmployeeId"]),
                    EmployeeName =
                        reader["EmployeeName"].ToString(),
                    Email =
                        reader["Email"].ToString(),
                    Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString()
                };
            }

            return employee;
        }

        public void UpdateEmployee(int id, Employee employee)
        {
            using SqlConnection con = _dbHelper.GetConnection();

            string query = @"UPDATE Employees
                            SET EmployeeName=@EmployeeName,
                            Email=@Email,
                            Department=@Department
                            WHERE EmployeeId=@Id";

            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@EmployeeName",
                employee.EmployeeName);
            cmd.Parameters.AddWithValue("@Email",
                employee.Email);
            cmd.Parameters.AddWithValue("@Department",
                employee.Department);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteEmployee(int id)
        {
            using SqlConnection con = _dbHelper.GetConnection();

            string query =
                "DELETE FROM Employees WHERE EmployeeId=@Id";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}