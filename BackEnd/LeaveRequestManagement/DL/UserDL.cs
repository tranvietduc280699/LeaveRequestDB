using DL.Database;
using DL.Interfaces;
using Model.DTOs;
using Model.Entities;
using Model.Enums;
using MySqlConnector;
using MySqlX.XDevAPI.Common;

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
            // Thực hiện truy vấn và kiểm tra số lượng bản ghi bị ảnh hưởng
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
            // thực hiện truy vấn và đọc dữ liệu
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
        /// <summary>
        /// đăng ký người dùng mới  
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

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
            // Thực hiện truy vấn 
            command.ExecuteNonQuery();

            return command.LastInsertedId;
        }
        /// <summary>
        /// Cập nhật họ tên, số điện thoại và địa chỉ
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <param name="request">Thông tin cần cập nhật</param>
        /// <returns>Cập nhật thành công hay không</returns>
        public bool UpdateProfile(long id, UpdateProfileRequest request)
        {
            string sql = @"
                UPDATE USERS
                SET FULL_NAME = @FullName,
                    PHONE = @Phone,
                    ADDRESS = @Address,
                    UPDATED_AT = NOW()
                WHERE ID = @Id;
            ";

            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@FullName", request.FullName);
            command.Parameters.AddWithValue("@Phone", request.Phone);
            command.Parameters.AddWithValue("@Address",(object?)request.Address ?? DBNull.Value);

            // Thực hiện truy vấn và kiểm tra số lượng bản ghi bị ảnh hưởng
            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Lấy danh sách quản lý
        /// user: nhân viên (để chọn người phụ trách đơn)
        /// </summary>
        /// <returns></returns>
        public List<ManagerOption> GetManagers()
        {
            string sql = @"
                SELECT ID, FULL_NAME
                FROM USERS
                WHERE ROLE = 2
                  AND STATUS = 1
                ORDER BY FULL_NAME;
            ";

            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            //Thực hiện truy vấn
            using var reader = command.ExecuteReader();

            var managers = new List<ManagerOption>();
            // thêm vào list
            while (reader.Read())
            {
                managers.Add(new ManagerOption
                {
                    Id = reader.GetInt64("ID"),
                    FullName = reader.GetString("FULL_NAME")
                });
            }

            return managers;
        }
        /// <summary>
        /// lấy danh sách nhân viên
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<User>> GetEmployeesAsync(long? departmentId,string? keyword,int? status)
        {
            var sql = @"
                SELECT
                    U.ID AS Id,
                    U.EMPLOYEE_CODE AS EmployeeCode,
                    U.FULL_NAME AS FullName,
                    U.EMAIL AS Email,
                    U.PHONE AS Phone,
                    U.ADDRESS AS Address,
                    U.POSITION AS Position,
                    U.DEPARTMENT_ID AS DepartmentId,
                    D.NAME AS DepartmentName,
                    U.ROLE AS Role,
                    U.STATUS AS Status,
                    U.CREATED_AT AS CreatedAt,
                    U.UPDATED_AT AS UpdatedAt
                FROM USERS U
                LEFT JOIN DEPARTMENTS D
                    ON U.DEPARTMENT_ID = D.ID
                WHERE U.ROLE = 1
                  AND (
                        @DepartmentId IS NULL
                        OR U.DEPARTMENT_ID = @DepartmentId
                  )
                  AND (
                        @Keyword IS NULL
                        OR @Keyword = ''
                        OR U.FULL_NAME LIKE CONCAT('%', @Keyword, '%')
                        OR U.EMPLOYEE_CODE LIKE CONCAT('%', @Keyword, '%')
                        OR U.EMAIL LIKE CONCAT('%', @Keyword, '%')
                  )
                  AND (
                        @Status IS NULL
                        OR U.STATUS = @Status
                  )
                ORDER BY U.FULL_NAME;
            ";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();

            using var command = new MySqlCommand(sql , connection);
            // truyền param
            command.Parameters.AddWithValue("@DDepartmentId", departmentId.HasValue ? departmentId.Value : DBNull.Value);
            command.Parameters.AddWithValue("@keyword", string.IsNullOrEmpty(keyword) ? DBNull.Value : keyword);
            command.Parameters.AddWithValue("@status", status.HasValue ? status.Value : DBNull.Value);
            // thực hiện truy vấn;
            using var reader = await command.ExecuteReaderAsync();

            // khởi tạo list
            var users = new List<User>();

            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = Convert.ToInt64(reader["Id"]),
                    EmployeeCode = reader["EmployeeCode"].ToString()!,
                    FullName = reader["FullName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                    Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                    Position = reader["Position"] == DBNull.Value ? null : reader["Position"].ToString(),
                    DepartmentId = reader["DepartmentId"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(reader["DepartmentId"]),
                                DepartmentName = reader["DepartmentName"] == DBNull.Value
                    ? null
                    : reader["DepartmentName"].ToString(),
                    Role = (UserRole)Convert.ToInt32(reader["Role"]),
                    Status = (UserStatus)Convert.ToInt32(reader["Status"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };

                users.Add(user);
            }

            return users;
        }
    }
}
