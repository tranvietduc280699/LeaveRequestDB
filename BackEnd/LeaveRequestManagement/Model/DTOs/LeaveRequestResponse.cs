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
        public long Id { get; set; }
        public string RequestCode { get; set; } = string.Empty; // ""
        public string LeaveTypeName { get; set; } = string.Empty; //""
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NumberOfDays { get; set; }
        public LeaveRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
