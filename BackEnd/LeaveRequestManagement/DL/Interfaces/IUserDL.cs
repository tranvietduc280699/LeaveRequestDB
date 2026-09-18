using Model.DTOs;
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
        /// <summary>
        /// Cập nhật họ tên, số điện thoại và địa chỉ của người dùng
        /// </summary>
        /// <param name="id">ID người dùng cần cập nhật</param>
        /// <param name="request">Thông tin cần cập nhật</param>
        /// <returns>Cập nhật thành công hay không</returns>
        bool UpdateProfile(long id, UpdateProfileRequest request);

        /// <summary>
        /// Lấy danh sách tên những người quản lý 
        /// </summary>
        /// <returns></returns>
        List<ManagerOption> GetManagers();

        /// <summary>
        /// lấy tất cả danh sách người dùng 
        /// user: quản lý
        /// </summary>
        /// <returns></returns>

        Task<List<User>> GetEmployeesAsync(
            long? departmentId,
            string? keyword,
            int? status
        );
    }
}
