using DL.Database;
using DL.Interfaces;
using Model.Entities;
using Model.Enums;
using MySqlConnector;
using System;
using System.Collections.Generic;
namespace DL
{
    public class LeaveRequestDL : ILeaveRequestDL
    {
        /// <summary>
        /// Kết nối cơ sở dữ liệu, được tiêm từ bên ngoài
        /// </summary>
        private readonly DatabaseConnection _databaseConnection;
        public LeaveRequestDL(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        /// <summary>
        /// Thêm đơn nghỉ phép mới, trạng thái mặc định là đang chờ duyệt
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public long Insert(LeaveRequest request)
        {
            const string sql = @"
                INSERT INTO LEAVE_REQUESTS
                (
                    REQUEST_CODE, EMPLOYEE_ID, LEAVE_TYPE_ID,
                    START_DATE, END_DATE, NUMBER_OF_DAYS,
                    REASON, NOTE, HANDOVER_PERSON, STATUS
                )
                VALUES
                (
                    @RequestCode, @EmployeeId, @LeaveTypeId,
                    @StartDate, @EndDate, @NumberOfDays,
                    @Reason, @Note, @HandoverPerson, @Status
                );";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@RequestCode", request.RequestCode);
            command.Parameters.AddWithValue("@EmployeeId", request.EmployeeId);
            AddContentParameters(command, request);
            command.Parameters.AddWithValue("@Status", (int)LeaveRequestStatus.Pending);
            command.ExecuteNonQuery();
            return command.LastInsertedId;
        }

        /// <summary>
        /// Cập nhật trạng thái đơn nghỉ phép còn chờ duyệt thành đã hủy, chỉ nhân viên mới được hủy đơn của mình
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public bool CancelPending(long id, long employeeId)
        {
            const string sql = @"
                UPDATE LEAVE_REQUESTS
                SET STATUS = @Cancelled,
                    UPDATED_AT = NOW()
                WHERE ID = @Id
                  AND EMPLOYEE_ID = @EmployeeId
                  AND STATUS = @Pending;";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            command.Parameters.AddWithValue("@Pending", (int)LeaveRequestStatus.Pending);
            command.Parameters.AddWithValue("@Cancelled", (int)LeaveRequestStatus.Cancelled);
            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Lấy danh sách đơn nghỉ phép của nhân viên theo id người quản lý và các bộ lọc
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="requestCode"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<LeaveRequest> GetByEmployee(
             long employeeId,
             string? requestCode = null,
             long? leaveTypeId = null,
             LeaveRequestStatus? status = null)
        {
            string sql = @"
        SELECT *
        FROM LEAVE_REQUESTS
        WHERE EMPLOYEE_ID = @EmployeeId";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            if (!string.IsNullOrWhiteSpace(requestCode))
            {
                command.CommandText += " AND REQUEST_CODE LIKE @RequestCode";
                command.Parameters.AddWithValue("@RequestCode", "%" + requestCode.Trim() + "%");
            }
            if (leaveTypeId.HasValue)
            {
                command.CommandText += " AND LEAVE_TYPE_ID = @LeaveTypeId";
                command.Parameters.AddWithValue("@LeaveTypeId", leaveTypeId.Value);
            }
            if (status.HasValue)
            {
                command.CommandText += " AND STATUS = @Status";
                command.Parameters.AddWithValue("@Status", (int)status.Value);
            }
            command.CommandText += " ORDER BY CREATED_AT DESC, ID DESC;";
            var requests = new List<LeaveRequest>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                requests.Add(MapLeaveRequest(reader));
            }
            return requests;
        }
        /// <summary>
        /// Lấy chi tiết đơn theo ID; BL phải kiểm tra quyền xem
        /// </summary>
        public LeaveRequest? GetById(long id)
        {
            const string sql = @"
                SELECT *
                FROM LEAVE_REQUESTS
                WHERE ID = @Id;";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapLeaveRequest(reader) : null;
        }
        /// <summary>
        /// Quản lý xem đơn của nhân viên cùng phòng ban
        /// </summary>
        public List<LeaveRequest> GetForManager(
            long managerId,
            string? searchText = null,
            long? leaveTypeId = null,
            LeaveRequestStatus? status = null)
        {
            string sql = @"
                SELECT LR.*
                FROM LEAVE_REQUESTS LR
                INNER JOIN USERS E ON E.ID = LR.EMPLOYEE_ID
                INNER JOIN USERS M ON M.DEPARTMENT_ID = E.DEPARTMENT_ID
                WHERE M.ID = @ManagerId
                  AND M.ROLE = 2
                  AND M.STATUS = 1
                  AND E.ROLE = 1";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand();
            command.Connection = connection;
            command.Parameters.AddWithValue("@ManagerId", managerId);
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                sql += @"
                    AND (E.FULL_NAME LIKE @SearchText
                         OR LR.REQUEST_CODE LIKE @SearchText)";
                command.Parameters.AddWithValue("@SearchText", "%" + searchText.Trim() + "%");
            }
            if (leaveTypeId.HasValue)
            {
                sql += " AND LR.LEAVE_TYPE_ID = @LeaveTypeId";
                command.Parameters.AddWithValue("@LeaveTypeId", leaveTypeId.Value);
            }
            if (status.HasValue)
            {
                sql += " AND LR.STATUS = @Status";
                command.Parameters.AddWithValue("@Status", (int)status.Value);
            }
            sql += " ORDER BY LR.CREATED_AT DESC, LR.ID DESC;";
            command.CommandText = sql;
            var requests = new List<LeaveRequest>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                requests.Add(MapLeaveRequest(reader));
            }
            return requests;
        }
        /// <summary>
        /// Quản lý duyệt hoặc từ chối đơn còn chờ duyệt
        /// </summary>
        public bool ProcessPending(
            long id,
            long managerId,
            LeaveRequestStatus status,
            string? responseContent)
        {
            if (status != LeaveRequestStatus.Approved &&
                status != LeaveRequestStatus.Rejected)
            {
                return false;
            }
            const string sql = @"
                UPDATE LEAVE_REQUESTS LR
                INNER JOIN USERS E ON E.ID = LR.EMPLOYEE_ID
                INNER JOIN USERS M ON M.DEPARTMENT_ID = E.DEPARTMENT_ID
                SET LR.STATUS = @Status,
                    LR.MANAGER_ID = @ManagerId,
                    LR.MANAGER_RESPONSE = @ResponseContent,
                    LR.PROCESSED_AT = NOW(),
                    LR.UPDATED_AT = NOW()
                WHERE LR.ID = @Id
                  AND LR.STATUS = @Pending
                  AND M.ID = @ManagerId
                  AND M.ROLE = 2
                  AND M.STATUS = 1
                  AND E.ROLE = 1;";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@ManagerId", managerId);
            command.Parameters.AddWithValue("@Status", (int)status);
            command.Parameters.AddWithValue("@Pending", (int)LeaveRequestStatus.Pending);
            command.Parameters.AddWithValue("@ResponseContent", (object?)responseContent ?? DBNull.Value);
            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// Sửa nội dung đơn của nhân viên khi còn chờ duyệt
        /// </summary>
        public bool UpdatePending(LeaveRequest request, long employeeId)
        {
            const string sql = @"
                UPDATE LEAVE_REQUESTS
                SET LEAVE_TYPE_ID = @LeaveTypeId,
                    START_DATE = @StartDate,
                    END_DATE = @EndDate,
                    NUMBER_OF_DAYS = @NumberOfDays,
                    REASON = @Reason,
                    NOTE = @Note,
                    HANDOVER_PERSON = @HandoverPerson,
                    UPDATED_AT = NOW()
                WHERE ID = @Id
                  AND EMPLOYEE_ID = @EmployeeId
                  AND STATUS = @Pending;";
            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", request.Id);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            command.Parameters.AddWithValue("@Pending", (int)LeaveRequestStatus.Pending);
            AddContentParameters(command, request);
            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// Các tham số dùng chung khi tạo và sửa đơn
        /// </summary>
        private static void AddContentParameters(MySqlCommand command, LeaveRequest request)
        {
            command.Parameters.AddWithValue("@LeaveTypeId", request.LeaveTypeId);
            command.Parameters.AddWithValue("@StartDate", request.StartDate.Date);
            command.Parameters.AddWithValue("@EndDate", request.EndDate.Date);
            command.Parameters.AddWithValue("@NumberOfDays", request.NumberOfDays);
            command.Parameters.AddWithValue("@Reason", request.Reason);
            command.Parameters.AddWithValue("@Note", (object?)request.Note ?? DBNull.Value);
            command.Parameters.AddWithValue("@HandoverPerson", (object?)request.HandoverPerson ?? DBNull.Value);
        }
        /// <summary>
        /// Chuyển một dòng dữ liệu DB thành entity LeaveRequest
        /// </summary>
        private static LeaveRequest MapLeaveRequest(MySqlDataReader reader)
        {
            return new LeaveRequest
            {
                Id = Convert.ToInt64(reader["ID"]),
                RequestCode = reader["REQUEST_CODE"].ToString()!,
                EmployeeId = Convert.ToInt64(reader["EMPLOYEE_ID"]),
                LeaveTypeId = Convert.ToInt64(reader["LEAVE_TYPE_ID"]),
                StartDate = Convert.ToDateTime(reader["START_DATE"]),
                EndDate = Convert.ToDateTime(reader["END_DATE"]),
                NumberOfDays = Convert.ToDecimal(reader["NUMBER_OF_DAYS"]),
                Reason = reader["REASON"].ToString()!,
                Note = reader["NOTE"] == DBNull.Value
                    ? null : reader["NOTE"].ToString(),
                HandoverPerson = reader["HANDOVER_PERSON"] == DBNull.Value
                    ? null : reader["HANDOVER_PERSON"].ToString(),
                Status = (LeaveRequestStatus)Convert.ToInt32(reader["STATUS"]),
                ManagerId = reader["MANAGER_ID"] == DBNull.Value
                    ? null : Convert.ToInt64(reader["MANAGER_ID"]),
                ManagerResponse = reader["MANAGER_RESPONSE"] == DBNull.Value
                    ? null : reader["MANAGER_RESPONSE"].ToString(),
                ProcessedAt = reader["PROCESSED_AT"] == DBNull.Value
                    ? null : Convert.ToDateTime(reader["PROCESSED_AT"]),
                CreatedAt = Convert.ToDateTime(reader["CREATED_AT"]),
                UpdatedAt = Convert.ToDateTime(reader["UPDATED_AT"])
            };
        }
    }
}