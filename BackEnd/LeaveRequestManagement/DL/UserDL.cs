using DL.Database;
using DL.Interfaces;
using Model.Entities;
using Model.Enums;
using MySqlConnector;

namespace DL
{
    public class UserDL : IUserDL
    {
        private readonly DatabaseConnection _databaseConnection;

        public UserDL(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        /// <summary>
        /// Kiểm tra email có tồn tại không
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        public bool CheckEmailExists(string email)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM USERS
                WHERE EMAIL = @Email;
            ";

            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            return Convert.ToInt64(command.ExecuteScalar()) > 0;
        }
        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại không
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckEmployeeCodeExists(string employeeCode)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM USERS
                WHERE EMPLOYEE_CODE = @EmployeeCode;
            ";

            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeCode", employeeCode);

            return Convert.ToInt64(command.ExecuteScalar()) > 0;
        }
        /// <summary>
        /// đăng nhập theo mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User? GetByEmail(string email)
        {
            string sql = @"
                SELECT *
                FROM USERS
                WHERE EMAIL = @Email
                LIMIT 1;
            ";

            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new User
            {
                Id = Convert.ToInt64(reader["ID"]),
                EmployeeCode = reader["EMPLOYEE_CODE"].ToString()!,
                FullName = reader["FULL_NAME"].ToString()!,
                Email = reader["EMAIL"].ToString()!,
                Password = reader["PASSWORD"].ToString()!,
                Phone = reader["PHONE"] == DBNull.Value ? null : reader["PHONE"].ToString(),
                Address = reader["ADDRESS"] == DBNull.Value ? null : reader["ADDRESS"].ToString(),
                Position = reader["POSITION"] == DBNull.Value ? null : reader["POSITION"].ToString(),
                DepartmentId = reader["DEPARTMENT_ID"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(reader["DEPARTMENT_ID"]),
                Role = (UserRole)Convert.ToInt32(reader["ROLE"]),
                Status = (UserStatus)Convert.ToInt32(reader["STATUS"]),
                CreatedAt = Convert.ToDateTime(reader["CREATED_AT"]),
                UpdatedAt = Convert.ToDateTime(reader["UPDATED_AT"])
            };
        }

        public long Insert(User user)
        {
            string sql = @"
                INSERT INTO USERS
                (
                    EMPLOYEE_CODE,
                    FULL_NAME,
                    EMAIL,
                    PASSWORD,
                    PHONE,
                    ADDRESS,
                    POSITION,
                    DEPARTMENT_ID,
                    ROLE,
                    STATUS
                )
                VALUES
                (
                    @EmployeeCode,
                    @FullName,
                    @Email,
                    @Password,
                    @Phone,
                    @Address,
                    @Position,
                    @DepartmentId,
                    @Role,
                    @Status
                );
            ";

            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@EmployeeCode", user.EmployeeCode);
            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Phone", user.Phone);
            command.Parameters.AddWithValue("@Address", user.Address);
            command.Parameters.AddWithValue("@Position", user.Position);
            command.Parameters.AddWithValue("@DepartmentId", user.DepartmentId);
            command.Parameters.AddWithValue("@Role", user.Role);
            command.Parameters.AddWithValue("@Status", user.Status);

            command.ExecuteNonQuery();

            return command.LastInsertedId;
        }
    }
}
