using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Common;
using Model.DTOs;
using Model.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        //injec
        private readonly IAuthBL _authBL;

        public AuthController(IAuthBL authBL)
        {
            _authBL = authBL;
        }
        /// <summary>
        /// đăng nhập 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var result = _authBL.Login(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Đăng nhập thành công"));
        }
        /// <summary>
        /// đăng ký
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            var result = _authBL.Register(user);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Đăng ký thành công"));
        }
    }
}