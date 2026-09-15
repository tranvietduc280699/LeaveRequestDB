using Model.Common;
using Model.DTOs;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IUserBL
    {
        /// <summary>
        /// Cập nhập thông tin cá nhân
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiResponse<bool> UpdateProfile(long id, UpdateProfileRequest request);

        /// <summary>
        /// Lấy danh sách tên quản lý
        /// </summary>
        /// <returns></returns>
        ApiResponse<List<ManagerOption>> GetManagers();
    }
}
