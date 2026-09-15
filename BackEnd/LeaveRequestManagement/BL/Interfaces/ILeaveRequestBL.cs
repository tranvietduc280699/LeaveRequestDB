using Model.Common;
using Model.DTOs;
using Model.Entities;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface ILeaveRequestBL
    {
       /// <summary>
       /// Thêm mới 1 yêu cầu nghỉ phép
       /// </summary>
       /// <param name="request"></param>
       /// <param name="employeeId"></param>
       /// <returns></returns>
        ApiResponse<long> Insert(CreateLeaveRequest request, long employeeId);
       /// <summary>
       /// xem chi tiết đơn và kiểm tra người xem
       /// </summary>
       /// <param name="id"></param>
       /// <param name="userId"></param>
       /// <param name="role"></param>
       /// <returns></returns>
        ApiResponse<LeaveRequest> GetLeaveRequestById(long id,long userId,UserRole role);
        /// <summary>
        /// lấy danh sách yêu cầu nghỉ phép của nhân viên
        /// user: nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="requestCode"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ApiResponse<List<LeaveRequest>> GetByEmployee(
            long employeeId,
            string? requestCode = null,
            long? leaveTypeId = null,
            LeaveRequestStatus? status = null);
      /// <summary>
      /// cập nhật đơn nghỉ phép có trạng thái đang chờ duyệt
      /// user: nhân viên
      /// </summary>
      /// <param name="id"></param>
      /// <param name="request"></param>
      /// <param name="employeeId"></param>
      /// <returns></returns>
        ApiResponse<bool> UpdatePending(long id,UpdateLeaveRequest request,long employeeId);

        /// <summary>
        /// Hủy đơn nghỉ phép còn chờ duyệt
        /// user: nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ApiResponse<LeaveRequest> CancelPending(long id, long employeeId);

        /// <summary>
        /// lấy danh sách đơn nghỉ phép của nhân viên theo id
        /// user: Quản lý
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="searchText"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>

        ApiResponse<List<LeaveRequest>> GetForManager(
            long managerId,
            string? searchText = null,
            long? leaveTypeId = null,
            LeaveRequestStatus? status = null);
        /// <summary>
        /// Quản lý duyệt hoặc từ chối đơn đang chờ duyệt
        /// user: quản lý
        /// </summary>
        /// <param name="id"></param>
        /// <param name="managerId"></param>
        /// <param name="status"></param>
        /// <param name="responseContent"></param>
        /// <returns></returns>
        ApiResponse<LeaveRequest>  ProcessPending(
            long id,
            long managerId,
            ProcessLeaveRequest request);
    }
}
