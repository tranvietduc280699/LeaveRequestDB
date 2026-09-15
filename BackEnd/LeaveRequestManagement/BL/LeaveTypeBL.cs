using BL.Interfaces;
using DL;
using DL.Interfaces;
using Model.Common;
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
        public ApiResponse<List<LeaveType>> GetLeaveType()
        {
            var leaveTypes = _leaveTypeDL.GetLeaveType() ?? new List<LeaveType>();
            if (leaveTypes == null)
            {
                return ApiResponse<List<LeaveType>>.ErrorResponse("LEAVE_TYPE_NOT_FOUND", "Không tìm thấy loại nghỉ phép");
            }
            return ApiResponse<List<LeaveType>>.SuccessResponse(
                leaveTypes, "Lấy danh sách loại nghỉ phép thành công");
        }
    }
}
