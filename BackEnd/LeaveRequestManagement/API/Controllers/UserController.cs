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
    }
}