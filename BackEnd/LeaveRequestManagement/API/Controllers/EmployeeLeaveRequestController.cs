using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.Enums;
namespace API.Controllers
{
    [ApiController]
    [Route("api/employeeRequest")]
    public class EmployeeLeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestBL _leaveRequestBL;
        public EmployeeLeaveRequestController(ILeaveRequestBL leaveRequestBL)
        {
            _leaveRequestBL = leaveRequestBL;
        }
        /// <summary>
        /// Tạo đơn nghỉ phép
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert([FromQuery] long employeeId,[FromBody] CreateLeaveRequest request)
        {
            var result = _leaveRequestBL.Insert(request, employeeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Lấy danh sách đơn nhân viên teo bộ lọc
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="requestCode"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByEmployee(
            [FromQuery] long employeeId,
            [FromQuery] string? requestCode = null,
            [FromQuery] long? leaveTypeId = null,
            [FromQuery] LeaveRequestStatus? status = null)
        {
            var result = _leaveRequestBL.GetByEmployee(
                employeeId, requestCode, leaveTypeId, status);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
       /// <summary>
       /// xem chi tiết đơn của nhân viên
       /// </summary>
       /// <param name="id"></param>
       /// <param name="employeeId"></param>
       /// <returns></returns>
        [HttpGet("{id:long}")]
        public IActionResult GetById([FromRoute] long id,[FromQuery] long employeeId)
        {
            var result = _leaveRequestBL.GetLeaveRequestById(id, employeeId, (UserRole)1);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Sửa đơn đang chờ duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        public IActionResult UpdatePending([FromRoute] long id,[FromQuery] long employeeId,[FromBody] UpdateLeaveRequest request)
        {
            var result = _leaveRequestBL.UpdatePending(id, request, employeeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
       /// <summary>
       /// Hủy đơn đang chờ duyệt
       /// </summary>
       /// <param name="id"></param>
       /// <param name="employeeId"></param>
       /// <returns></returns>
        [HttpPut("{id:long}/cancel")]
        public IActionResult CancelPending([FromRoute] long id,[FromQuery] long employeeId)
        {
            var result = _leaveRequestBL.CancelPending(id, employeeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}