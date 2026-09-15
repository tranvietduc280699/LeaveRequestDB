using Model.DTOs;
using Model.Entities;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Interfaces
{
    public interface ILeaveRequestDL
    {
        /// <summary>
        /// thêm mới đơn nghỉ phép
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        long Insert(LeaveRequest request);

        /// <summary>
        /// lấy thông tin đơn nghỉ phép theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LeaveRequest? GetById(long id);

        /// <summary>
        /// lấy danh sách đơn nghỉ phép của nhân viên theo id nhân viên và các bộ lọc
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="keyword"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        List<LeaveRequest> GetByEmployee(
            long employeeId,
            string? requestCode = null,
            long? leaveTypeId = null,
            LeaveRequestStatus? status = null);

        /// <summary>
        /// cập nhật đơn nghỉ phép còn chờ duyệt
        /// </summary>
        /// <param name="request"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        bool UpdatePending(LeaveRequest request, long employeeId);

        /// <summary>
        /// Hủy đơn nghỉ phép còn chờ duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        bool CancelPending(long id, long employeeId);

        /// <summary>
        /// lấy danh sách đơn nghỉ phép của nhân viên theo id người quản lý và các bộ lọc
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="searchText"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        List<LeaveRequest> GetForManager(
            long managerId,
            string? searchText = null,
            long? leaveTypeId = null,
            LeaveRequestStatus? status = null);

        /// <summary>
        /// Quản lý duyệt hoặc từ chối đơn đang chờ duyệt
        /// </summary>
        /// <param name="id">ID đơn nghỉ</param>
        /// <param name="managerId">ID quản lý xử lý</param>
        /// <param name="status">Trạng thái duyệt hoặc từ chối</param>
        /// <param name="responseContent">Nội dung phản hồi</param>
        /// <returns>Cập nhật thành công hay không</returns>
        bool ProcessPending(
            long id,
            long managerId,
            ProcessLeaveRequest request);
    }
}
