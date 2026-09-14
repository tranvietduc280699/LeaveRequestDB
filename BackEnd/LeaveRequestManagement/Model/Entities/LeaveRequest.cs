using Model.Enums;
using System;
namespace Model.Entities
{
    public class LeaveRequest
    {
        // id của yêu cầu nghỉ phép
        public long Id { get; set; }
        // mã yêu cầu nghỉ phép
        public string RequestCode { get; set; } = string.Empty;
        // id của nhân viên yêu cầu nghỉ phép 
        public long EmployeeId { get; set; }
        // id của loại nghỉ phép
        public long LeaveTypeId { get; set; }
        // ngày bắt đầu của yêu cầu nghỉ phép
        public DateTime StartDate { get; set; }
        // ngày kết thúc của yêu cầu nghỉ phép
        public DateTime EndDate { get; set; }
        // số ngày nghỉ phép
        public decimal NumberOfDays { get; set; }
        // lý do nghỉ phép
        public string Reason { get; set; } = string.Empty;
        // ngày tạo yêu cầu nghỉ phép
        public string? Note { get; set; }
        // người bàn giao công việc
        public string? HandoverPerson { get; set; }
        // trạng thái của yêu cầu nghỉ phép
        public LeaveRequestStatus Status { get; set; }
        // ngày duyệt yêu cầu nghỉ phép
        public long? ManagerId { get; set; }
        // phản hồi của người duyệt yêu cầu nghỉ phép
        public string? ManagerResponse { get; set; }

        // ngày duyệt yêu cầu nghỉ phép
        public DateTime? ProcessedAt { get; set; }
        // ngày tạo yêu cầu nghỉ phép
        public DateTime CreatedAt { get; set; }
        // ngày cập nhật yêu cầu nghỉ phép
        public DateTime UpdatedAt { get; set; }
    }
}