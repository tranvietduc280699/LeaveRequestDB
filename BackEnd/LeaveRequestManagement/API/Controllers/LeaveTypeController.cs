using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Common;
using Model.Entities;
using MySqlX.XDevAPI.Common;

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
        /// lấy thông tin loại nghỉ phép
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLeaveType()
        {
            var leaveType =  _leaveTypeBL.GetLeaveType();
            if (!leaveType.Success)
            {
                return BadRequest(leaveType);
            }
            return Ok(leaveType);
        }
    }
}
