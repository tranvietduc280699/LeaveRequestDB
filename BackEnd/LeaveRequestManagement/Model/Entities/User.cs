using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class User
    {
        // ID
        public long Id { get; set; }

        // EMPLOYEE_CODE
        public string EmployeeCode { get; set; } = string.Empty;

        // FULL_NAME
        public string FullName { get; set; } = string.Empty;

        //  EMAIL
        public string Email { get; set; } = string.Empty;

        // PASSWORD
        public string Password { get; set; } = string.Empty;

        // PHONE
        public string? Phone { get; set; }

        //  ADDRESS
        public string? Address { get; set; }

        //  POSITION
        public string? Position { get; set; }

        // DEPARTMENT_ID
        public long? DepartmentId { get; set; }

        //  ROLE
        public UserRole Role { get; set; }

        //  STATUS
        public UserStatus Status { get; set; }

        // CREATED_AT
        public DateTime CreatedAt { get; set; }

        // UPDATED_AT
        public DateTime UpdatedAt { get; set; }

    }
}
