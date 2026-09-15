using Model.Enums;
namespace Model.DTOs
{
    /// <summary>
    /// Dữ liệu quản lý gửi để duyệt hoặc từ chối đơn nghỉ phép
    /// </summary>
    public class ProcessLeaveRequest
    {
        // Kết quả xử lý: Approved = 2 hoặc Rejected = 3
        public LeaveRequestStatus Status { get; set; }
        // Nội dung phản hồi của quản lý
        public string? ResponseContent { get; set; }
    }
}