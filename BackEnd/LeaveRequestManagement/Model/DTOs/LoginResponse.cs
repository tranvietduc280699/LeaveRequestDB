using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs
{
    /// <summary>
    /// token và thông tin cơ bản của người dùng sau khi đăng nhập thành công
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public long Id { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
