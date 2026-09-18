using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        /// <summary>
        /// Cập nhật họ tên, số điện thoại và địa chỉ
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <param name="request">Thông tin cần cập nhật</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPut]
        public IActionResult UpdateProfile([FromQuery]long id, [FromBody] UpdateProfileRequest request)
        {
            var result = _userBL.UpdateProfile(id, request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// lấy danh sách tên quản lý
        /// user: nhân viên (để phụ trách đơn xin nghỉ)
        /// </summary>
        /// <returns></returns>
        [HttpGet("managers")]
        public IActionResult GetManagers()
        {
            var result = _userBL.GetManagers();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Lấy danh sách nhân viên theo điều kiện lọc
        /// </summary>
        /// <param name="departmentId">Id phòng ban, không truyền thì lấy tất cả</param>
        /// <param name="keyword">Tìm theo tên, mã nhân viên hoặc email</param>
        /// <param name="status">1: đang hoạt động, 2: ngừng hoạt động</param>
        /// <returns></returns>
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] long? departmentId,[FromQuery] string? keyword,[FromQuery] int? status)
        {
            var result = await _userBL.GetEmployeesAsync(departmentId,keyword,status);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}