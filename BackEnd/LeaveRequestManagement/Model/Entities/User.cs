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
        // id
        public long Id { get; set; }

        // mã người dùng
        public string EmployeeCode { get; set; } = string.Empty;

        // tên người dùng
        public string FullName { get; set; } = string.Empty;

        //  email
        public string Email { get; set; } = string.Empty;

        // mật khẩu người dùng
        public string Password { get; set; } = string.Empty;

        // số điện thoại
        public string? Phone { get; set; }

        //  địa chỉ
        public string? Address { get; set; }

        //  chức vụ
        public string? Position { get; set; }

        // id phòng ban
        public long? DepartmentId { get; set; }
        // tên phòng ban
        public string? DepartmentName { get; set; }

        //  vai trò
        public UserRole Role { get; set; }

        //  trạng thái
        public UserStatus Status { get; set; }

        // CREATED_AT
        public DateTime CreatedAt { get; set; }

        // UPDATED_AT
        public DateTime UpdatedAt { get; set; }

    }
}
