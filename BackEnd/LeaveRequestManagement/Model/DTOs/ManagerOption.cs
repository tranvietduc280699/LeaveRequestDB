namespace Model.DTOs
{
    /// <summary>
    /// DTO trả ra thông tin danh sách quản lý
    /// </summary>
    public class ManagerOption
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}