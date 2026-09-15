using BL.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Model.Common;
using Model.DTOs;
using Model.Entities;
using Model.Enums;

namespace API.Controllers
{
    [Controller]
    [Route("api/managerRequest")]
    public class ManagerLeaveRequestController: Controller
    {
        private readonly ILeaveRequestBL _leaveRequestBL;
        public ManagerLeaveRequestController(ILeaveRequestBL leaveRequestBL)
        {
            _leaveRequestBL = leaveRequestBL;
        }
        /// <summary>
        /// lấy danh sách đơn của nhân viên theo các bộ lọc
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="searchText"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetForManager(
            [FromQuery] long managerId,
            [FromQuery] string? searchText = null,
            [FromQuery] long? leaveTypeId = null,
            [FromQuery] LeaveRequestStatus? status = null)
        {
            var result = _leaveRequestBL.GetForManager(managerId, searchText, leaveTypeId, status);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// quản lý duyệt hoặc từ chối đơn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="managerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/process")]
        public IActionResult ProcessPending(long id,[FromQuery] long managerId,[FromBody] ProcessLeaveRequest request)
        {
            var result = _leaveRequestBL.ProcessPending(id, managerId, request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
       /// <summary>
       /// Xem chi tiết đơn hàng của 1 nhân viên
       /// </summary>
       /// <param name="id"></param>
       /// <param name="managerId"></param>
       /// <returns></returns>
        [HttpGet("{id:long}")]
        public IActionResult GetById([FromRoute] long id,[FromQuery] long managerId)
        {
            var result = _leaveRequestBL.GetLeaveRequestById(id, managerId, (UserRole)2);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
