using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Common;
using Model.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/leaveTypes")]
    public class LeaveTypeController: ControllerBase
    {
        // injec
        private readonly ILeaveTypeBL _leaveTypeBL;
        public LeaveTypeController(ILeaveTypeBL leaveTypeBL)
        {
            _leaveTypeBL = leaveTypeBL;
        }
        /// <summary>
        /// lấy thông tin loại nghỉ phép theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetLeaveTypeById(int id)
        {
            var leaveType =  _leaveTypeBL.GetLeaveTypeById(id);
            if (leaveType == null)
            {
                return NotFound(ApiResponse<LeaveType>.ErrorResponse("LEAVE_TYPE_NOT_FOUND", "Không tìm thấy loại nghỉ phép"));
            }
            return Ok(ApiResponse<List<LeaveType>>.SuccessResponse( leaveType, "Lấy loại nghỉ phép thành công"));
        }
    }
}
