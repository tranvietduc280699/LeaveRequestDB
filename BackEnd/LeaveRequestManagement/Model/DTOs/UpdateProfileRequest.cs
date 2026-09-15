using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs
{
   /// <summary>
   /// dữ liệu cập nhật thông tin cá nhân
   /// </summary>
    public class UpdateProfileRequest
    {
        // Họ và tên
        public string FullName { get; set; } = string.Empty;

        // Số điện thoại
        public string Phone { get; set; }

        // Địa chỉ
        public string? Address { get; set; }
    }
}
