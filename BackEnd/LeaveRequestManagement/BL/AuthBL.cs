using BL.Interfaces;
using DL.Interfaces;
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

        public AuthBL(IUserDL userDL)
        {
            _userDL = userDL;
        }
        /// <summary>
        /// Kiểm tra thông tin đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public User Login(LoginRequest request)
        {
            User user = _userDL.GetByEmail(request.Email);
            if (user == null)
            {
                throw new Exception("Email không tồn tại");
            }
            if (user.Password != request.Password)
            {
                throw new Exception("Email hoặc mật khẩu không chính xác");
            }
            if (user.Status != UserStatus.Inactive)
            {
                throw new Exception("Tài khoản đã ngừng hoạt động");
            }
            return user;
        }
        /// <summary>
        /// Kiểm tra thông tin đăng ký
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
            // thêm mới
            long id = _userDL.Insert(request);
            request.Id = id; 

            return request;
        }
    }
}
