using BL.Interfaces;
using DL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.Common;
using Model.DTOs;
using Model.Entities;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AuthBL : IAuthBL
    {
        //Inject
        private readonly IUserDL _userDL;
        // khởi tạo biến mã hóa mật khẩu
        private readonly PasswordHasher<User> _passwordHasher = new () ;

        public AuthBL(IUserDL userDL)
        {
            _userDL = userDL;
        }

        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ApiResponse<User> Login(LoginRequest request)
        {
            User? user = _userDL.GetByEmail(request.Email);
            if (user == null)
            {
                return ApiResponse<User>.ErrorResponse(
                    "EMAIL_NOT_FOUND",
                    "Email không tồn tại"
                );
            }
            // Kiểm tra mật khẩu nhập vào với chuỗi băm trong DB
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ApiResponse<User>.ErrorResponse(
                    "INVALID_PASSWORD",
                    "Email hoặc mật khẩu không chính xác"
                );
            }

            if (user.Status == UserStatus.Inactive)
            {
                return ApiResponse<User>.ErrorResponse(
                    "ACCOUNT_INACTIVE",
                    "Tài khoản đã ngừng hoạt động"
                );
            }
            // Không trả mật khẩu về frontend
            user.Password = string.Empty;
            return ApiResponse<User>.SuccessResponse(
                user,
                "Đăng nhập thành công"
            );
        }
        /// <summary>
        ///  đăng ký
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public User Register(User request)
        {
            if (_userDL.CheckEmailExists(request.Email))
            {
                throw new Exception("Email đã tồn tại");
            }

            if (_userDL.CheckEmployeeCodeExists(request.EmployeeCode))
            {
                throw new Exception("Mã nhân viên đã tồn tại");
            }
            // mã hóa mật khẩu(băm)
            request.Password = _passwordHasher.HashPassword(request, request.Password);

            // thêm mới
            long id = _userDL.Insert(request);

            // xóa mật khẩu trước khi trả về
            request.Password = string.Empty;

            request.Id = id; 

            return request;
        }
    }
}
