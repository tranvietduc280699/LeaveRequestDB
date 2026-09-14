using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class LeaveType
    {
        // id loại nghỉ phép
        public int Id { get; set; }
        // mã nghỉ phép
        public string Code { get; set; }
        // tên loại nghỉ phép
        public string Name { get; set; }
        // mô tả loại nghỉ phép
        public string Description { get; set; }
        // trạng thái loại nghỉ phép
        public int Status { get; set; }
        // ngày tạo loại nghỉ phép
        public DateTime CreatedAt { get; set; }
        // ngày cập nhật loại nghỉ phép
        public DateTime UpdatedAt { get; set; }

    }
}
