using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Interfaces
{
    public interface ILeaveTypeDL
    {
        /// <summary>
        /// lấy thông tin loại nghỉ phép theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<LeaveType>? GetLeaveTypeById(int id);
    }
}
