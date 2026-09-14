using DL.Database;
using DL.Interfaces;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<LeaveType>? GetLeaveTypeById(int id)
        {
            
            string sql = $"SELECT * FROM LeaveTypes WHERE Id = {id}";

            // thực thi connection string để lấy dữ liệu từ cơ sở dữ liệu
            using var connection = _connectionString.GetConnection();
            connection.Open();

            var command = new MySqlConnector.MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            var result = command.ExecuteReader();
            if (!result.Read())
            {
                return null;
            }
            return new List<LeaveType>
            {
                new LeaveType
                {
                    Id = result.GetInt32("Id"),
                    Code = result.GetString("Code"),
                    Name = result.GetString("Name"),
                    Description = result.GetString("Description"),
                    Status = result.GetInt32("Status"),
                    CreatedAt = result.GetDateTime("CreatedAt"),
                    UpdatedAt = result.GetDateTime("UpdatedAt")
                }
            };

        }
    }
}
