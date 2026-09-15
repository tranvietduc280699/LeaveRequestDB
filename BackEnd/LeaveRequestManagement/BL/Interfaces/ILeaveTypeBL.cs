using Model.Common;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface ILeaveTypeBL
    {
        /// <summary>
        /// lấy thông tin loại nghỉ phép theo id
        /// </summary>
        /// <returns></returns>
        ApiResponse<List<LeaveType>>? GetLeaveType();
    }
}
