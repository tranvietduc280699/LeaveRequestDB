using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enums
{
    /// <summary>
    /// định nghĩa trạng thái đơn nghỉ phép
    /// </summary>
    public enum LeaveRequestStatus
    {
        Pending = 1, // chờ duyệt
        Approved = 2, // được duyệt
        Rejected = 3, // từ chối
        Cancelled = 4  // được nhân viên hủy
    }
}
