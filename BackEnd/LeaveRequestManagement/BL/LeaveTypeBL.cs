using BL.Interfaces;
using DL.Interfaces;
using Model.Entities;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class LeaveTypeBL : ILeaveTypeBL
    {
        //inject
        private readonly ILeaveTypeDL _leaveTypeDL;
        public LeaveTypeBL(ILeaveTypeDL leaveTypeDL)
        {
            _leaveTypeDL = leaveTypeDL;
        }
        /// <summary>
        /// lấy thông tin loại nghỉ phép theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<LeaveType>? GetLeaveTypeById(int id)
        {
            
            return _leaveTypeDL.GetLeaveTypeById(id);
        }
    }
}
