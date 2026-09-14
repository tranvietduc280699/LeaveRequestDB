using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs
{
    /// <summary>
    /// DTO dùng để tạo yêu cầu nghỉ phép
    /// </summary>
    public class CreateLeaveRequest
    {
        // id của loại nghỉ phép
        public long LeaveTypeId { get; set; }
        // ngày bắt đầu nghỉ phép
        public DateTime StartDate { get; set; }
        // ngày kết thúc nghỉ phép
        public DateTime EndDate { get; set; }
        // số ngày nghỉ phép
        public decimal NumberOfDays { get; set; }
        // lý do nghỉ phép
        public string Reason { get; set; } = string.Empty; //""
        // ghi chú của yêu cầu nghỉ phép
        public string? Note { get; set; }
        // người bàn giao công việc
        public string? HandoverPerson { get; set; }
    }
}
