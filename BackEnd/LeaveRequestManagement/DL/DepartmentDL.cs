using DL.Database;
using DL.Interfaces;
using Model.Entities;
using Model.Enums;
using MySqlConnector;

namespace DL
{
    public class DepartmentDL : IDepartmentDL
    {
        //inject DatabaseConnection vào DepartmentDL 
        private readonly DatabaseConnection _databaseConnection;

        public DepartmentDL(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<Department> GetAll()
        {
            List<Department> departments = new List<Department>();

            using MySqlConnection connection = _databaseConnection.GetConnection();
            connection.Open();

            string sql = @"
                SELECT ID, CODE, NAME, STATUS, CREATED_AT, UPDATED_AT
                FROM DEPARTMENTS
                WHERE STATUS = 1
                ORDER BY NAME";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Department department = new Department
                {
                    Id = Convert.ToInt64(reader["ID"]),
                    Code = reader["CODE"].ToString()!,
                    Name = reader["NAME"].ToString()!,
                    Status = (UserStatus)Convert.ToInt32(reader["STATUS"]),
                    CreatedAt = Convert.ToDateTime(reader["CREATED_AT"]),
                    UpdatedAt = Convert.ToDateTime(reader["UPDATED_AT"])
                };

                departments.Add(department);
            }

            return departments;
        }
    }
}