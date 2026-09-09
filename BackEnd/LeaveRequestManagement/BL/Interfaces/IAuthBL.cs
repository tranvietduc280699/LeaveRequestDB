using Model.DTOs;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IAuthBL
    {
        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        User Login(LoginRequest request);
        /// <summary>
        /// đăng ký
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        User Register(User request);


    }
}
