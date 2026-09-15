using DL.Database;
using DL.Interfaces;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class LeaveTypeDL: ILeaveTypeDL
    {
        public readonly DatabaseConnection _connectionString;
        public LeaveTypeDL(DatabaseConnection connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// lấy thông tin loại nghỉ phép theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<LeaveType>? GetLeaveType()
        {
            
            string sql = $"SELECT * FROM LEAVE_TYPES";

            // thực thi connection string để lấy dữ liệu từ cơ sở dữ liệu
            using var connection = _connectionString.GetConnection();
            connection.Open();

            var command = new MySqlConnector.MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            var leaveTypes = new List<LeaveType>();

            // Đọc lần lượt tất cả bản ghi
            while (reader.Read())
            {
                leaveTypes.Add(new LeaveType
                {
                    Id = reader.GetInt64("ID"),
                    Code = reader.GetString("CODE"),
                    Name = reader.GetString("NAME"),
                    Description = reader.IsDBNull(reader.GetOrdinal("DESCRIPTION"))? null: reader.GetString("DESCRIPTION"),
                    Status = reader.GetInt32("STATUS"),
                    CreatedAt = reader.GetDateTime("CREATED_AT"),
                    UpdatedAt = reader.GetDateTime("UPDATED_AT")
                });
            }

            // Không có dữ liệu thì trả về danh sách rỗng
            return leaveTypes;

        }
    }
}
