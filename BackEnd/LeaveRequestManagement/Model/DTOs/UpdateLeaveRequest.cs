using System;
namespace Model.DTOs
{
    /// <summary>
    /// Dữ liệu cập nhật nội dung đơn nghỉ phép
    /// </summary>
    public class UpdateLeaveRequest
    {
        // ID loại nghỉ được chọn, liên kết với bảng LEAVE_TYPES
        public long LeaveTypeId { get; set; }
        // Ngày bắt đầu nghỉ
        public DateTime StartDate { get; set; }
        // Ngày kết thúc nghỉ
        public DateTime EndDate { get; set; }
        // Lý do nghỉ, bắt buộc nhập
        public string Reason { get; set; } = string.Empty;
        // Ghi chú bổ sung, có thể để trống
        public string? Note { get; set; }
        // Tên người nhận bàn giao công việc, có thể để trống
        public string? HandoverPerson { get; set; }
    }
}