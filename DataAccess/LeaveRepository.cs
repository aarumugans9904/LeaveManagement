using EmployeeLeaveManagementAPI.DataAccess;
using EmployeeLeaveManagementAPI.Models;
using Microsoft.Data.SqlClient;

namespace LeaveManagementAPI.DataAccess
{
    public class LeaveRepository
    {
        private readonly DbHelper _dbHelper;

        public LeaveRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Apply Leave
        public void ApplyLeave(LeaveRequest leave)
        {
            using SqlConnection con =
                _dbHelper.GetConnection();

            string query = @"INSERT INTO LeaveRequests
                            (EmployeeId, LeaveType,
                             FromDate, ToDate,
                             Reason, Status)
                             VALUES
                            (@EmployeeId, @LeaveType,
                             @FromDate, @ToDate,
                             @Reason, 'Pending')";

            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@EmployeeId",
                leave.EmployeeId);

            cmd.Parameters.AddWithValue("@LeaveType",
                leave.LeaveType);

            cmd.Parameters.AddWithValue("@FromDate",
                leave.FromDate);

            cmd.Parameters.AddWithValue("@ToDate",
                leave.ToDate);

            cmd.Parameters.AddWithValue("@Reason",
                leave.Reason);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // Get All Leaves
        public List<LeaveRequest> GetLeaves()
        {
            List<LeaveRequest> leaves = new();

            using SqlConnection con =
                _dbHelper.GetConnection();

            string query =
                "SELECT * FROM LeaveRequests";

            SqlCommand cmd =
                new(query, con);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                leaves.Add(new LeaveRequest
                {
                    LeaveId =
                        Convert.ToInt32(reader["LeaveId"]),

                    EmployeeId =
                        Convert.ToInt32(reader["EmployeeId"]),

                    LeaveType = reader["LeaveType"] == DBNull.Value ? null : reader["LeaveType"].ToString(),

                    FromDate =
                        Convert.ToDateTime(reader["FromDate"]),

                    ToDate =
                        Convert.ToDateTime(reader["ToDate"]),

                    Reason =
                        reader["Reason"].ToString(),

                    Status =
                        reader["Status"].ToString()
                });
            }

            return leaves;
        }

        // Get Leave By Id
        public LeaveRequest GetLeaveById(int id)
        {
            LeaveRequest leave = null;

            using SqlConnection con =
                _dbHelper.GetConnection();

            string query =
                "SELECT * FROM LeaveRequests WHERE LeaveId=@Id";

            SqlCommand cmd =
                new(query, con);

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            if (reader.Read())
            {
                leave = new LeaveRequest
                {
                    LeaveId =
                        Convert.ToInt32(reader["LeaveId"]),

                    EmployeeId =
                        Convert.ToInt32(reader["EmployeeId"]),

                    LeaveType = reader["LeaveType"] == DBNull.Value ? null : reader["LeaveType"].ToString(),

                    FromDate =
                        Convert.ToDateTime(reader["FromDate"]),

                    ToDate =
                        Convert.ToDateTime(reader["ToDate"]),

                    Reason =
                        reader["Reason"].ToString(),

                    Status =
                        reader["Status"].ToString()
                };
            }

            return leave;
        }

        // Approve Leave
        public void ApproveLeave(int id)
        {
            using SqlConnection con =
                _dbHelper.GetConnection();

            string query = @"UPDATE LeaveRequests
                             SET Status='Approved'
                             WHERE LeaveId=@Id";

            SqlCommand cmd =
                new(query, con);

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // Reject Leave
        public void RejectLeave(int id)
        {
            using SqlConnection con =
                _dbHelper.GetConnection();

            string query = @"UPDATE LeaveRequests
                             SET Status='Rejected'
                             WHERE LeaveId=@Id";

            SqlCommand cmd =
                new(query, con);

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // Get Leave History By Employee
        public List<LeaveRequest> GetLeaveHistoryByEmployee(int employeeId)
        {
            List<LeaveRequest> leaves = new();

            using SqlConnection con =
                _dbHelper.GetConnection();

            string query = @"SELECT *
                             FROM LeaveRequests
                             WHERE EmployeeId=@EmployeeId";

            SqlCommand cmd =
                new(query, con);

            cmd.Parameters.AddWithValue(
                "@EmployeeId",
                employeeId);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                leaves.Add(new LeaveRequest
                {
                    LeaveId =
                        Convert.ToInt32(reader["LeaveId"]),

                    EmployeeId =
                        Convert.ToInt32(reader["EmployeeId"]),

                    LeaveType = reader["LeaveType"] == DBNull.Value ? null : reader["LeaveType"].ToString(),

                    FromDate =
                        Convert.ToDateTime(reader["FromDate"]),

                    ToDate =
                        Convert.ToDateTime(reader["ToDate"]),

                    Reason =
                        reader["Reason"].ToString(),

                    Status =
                        reader["Status"].ToString()
                });
            }

            return leaves;
        }

        // Check Employee Exists
        public bool EmployeeExists(int employeeId)
        {
            using SqlConnection con =
                _dbHelper.GetConnection();

            string query = @"SELECT COUNT(*)
                             FROM Employees
                             WHERE EmployeeId=@Id";

            SqlCommand cmd =
                new(query, con);

            cmd.Parameters.AddWithValue(
                "@Id",
                employeeId);

            con.Open();

            int count =
                (int)cmd.ExecuteScalar();

            return count > 0;
        }
    }
}