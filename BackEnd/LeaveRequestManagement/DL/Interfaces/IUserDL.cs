using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Interfaces
{
    public interface IUserDL
    {
        /// <summary>
        /// lấy thông tin theo email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User? GetByEmail(string email);
        /// <summary>
        /// Kiểm tra email tồn tại hay ko
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        bool CheckEmailExists(string email);
        /// <summary>
        /// Kiểm tra mã nhân viên
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>

        bool CheckEmployeeCodeExists(string employeeCode);
        /// <summary>
        /// thêm mới tài khoản
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        long Insert(User user);
    }
}
