using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs
{
    /// <summary>
    /// Response trả về thông tin của yêu cầu nghỉ phép
    /// </summary>
    public class LeaveRequestResponse
    {
        // id của yêu cầu nghỉ phép
        public long Id { get; set; }
        // mã yêu cầu nghỉ phép
        public string RequestCode { get; set; } = string.Empty; // ""
        // tên loại nghỉ phép
        public string LeaveTypeName { get; set; } = string.Empty; //""
        // ngày bắt đầu nghỉ phép
        public DateTime StartDate { get; set; }
        // ngày kết thúc nghỉ phép
        public DateTime EndDate { get; set; }
        // số ngày nghỉ phép
        public decimal NumberOfDays { get; set; }
        // trạng thái của yêu cầu nghỉ phép
        public LeaveRequestStatus Status { get; set; }
        // ngày tạo yêu cầu nghỉ phép
        public DateTime CreatedAt { get; set; }
    }
}
