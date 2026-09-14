using Model.Enums;
using System;

namespace Model.Entities
{
    public class Department
    {
        // id của phòng ban
        public long Id { get; set; }
        // mã phòng ban
        public string Code { get; set; } = string.Empty;
        // tên phòng ban

        public string Name { get; set; } = string.Empty;
        // trạng thái của phòng ban

        public UserStatus Status { get; set; }
        // ngày tạo phòng ban

        public DateTime CreatedAt { get; set; }
        // ngày cập nhật phòng ban

        public DateTime UpdatedAt { get; set; }
    }
}