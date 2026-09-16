using BL.Interfaces;
using DL;
using DL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Model.Common;
using Model.DTOs;
using Model.Entities;
using Model.Enums;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class LeaveRequestBL : ILeaveRequestBL
    {
        //inject
        private readonly ILeaveRequestDL _leaveRequestDL;
        public LeaveRequestBL(ILeaveRequestDL leaveRequestDL)
        {
            _leaveRequestDL = leaveRequestDL;
        }
        /// <summary>
        /// Nhân viên hủy đơn của mình khi còn chờ duyệt
        /// user: nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse<LeaveRequest> CancelPending(long id, long employeeId)
        {
            var request = _leaveRequestDL.GetById(id);
            if (request == null)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "NOT_FOUND", "Không tìm thấy đơn nghỉ phép");
            }
            if (request.EmployeeId != employeeId)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "FORBIDDEN", "Bạn không có quyền hủy đơn này");
            }
            if (request.Status != LeaveRequestStatus.Pending)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "INVALID_STATUS", "Chỉ được hủy đơn đang chờ duyệt");
            }
            // thực hiện hủy đơn
            bool success = _leaveRequestDL.CancelPending(id, employeeId);
            if (!success)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "CANCEL_FAILED", "Không thể hủy đơn, vui lòng tải lại danh sách");
            }
            // Lấy lại dữ liệu sau khi cập nhật để trả về
            var updatedRequest = _leaveRequestDL.GetById(id);
            if (updatedRequest == null)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "RELOAD_FAILED", "Đơn đã được hủy nhưng không tải lại được thông tin");
            }
            return ApiResponse<LeaveRequest>.SuccessResponse(
                updatedRequest, "Hủy đơn nghỉ phép thành công");
        }

        /// <summary>
        /// Lấy danh sách đơn của nhân viên theo các bộ lọc
        /// user: nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="requestCode"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ApiResponse<List<LeaveRequest>> GetByEmployee(long employeeId, string? requestCode = null, long? leaveTypeId = null, LeaveRequestStatus? status = null)
        {
            if (employeeId <= 0)
            {
                return ApiResponse<List<LeaveRequest>>.ErrorResponse(
                    "INVALID_EMPLOYEE", "Thông tin nhân viên không hợp lệ");
            }
            // Có truyền trạng thái và giá trị không thuộc enum thì mới báo lỗi
            if (status.HasValue && !System.Enum.IsDefined(typeof(LeaveRequestStatus), status.Value))
            {
                return ApiResponse<List<LeaveRequest>>.ErrorResponse(
                    "INVALID_STATUS",
                    "Trạng thái không hợp lệ");
            }
            var requests = _leaveRequestDL.GetByEmployee(employeeId, requestCode, leaveTypeId, status);
            return ApiResponse<List<LeaveRequest>>.SuccessResponse(
                requests, "Lấy danh sách đơn nghỉ phép thành công");
        }

        /// <summary>
        /// Lấy danh sách đơn trong phạm vi quản lý theo các bộ lọc
        /// user : quản lý
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="searchText"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>

        public ApiResponse<List<LeaveRequest>> GetForManager(long managerId, string? searchText = null, long? leaveTypeId = null, LeaveRequestStatus? status = null)
        {
            if (managerId <= 0)
            {
                return ApiResponse<List<LeaveRequest>>.ErrorResponse(
                    "INVALID_MANAGER", "Thông tin quản lý không hợp lệ");
            }
            if (status.HasValue)
            {
                return ApiResponse<List<LeaveRequest>>.ErrorResponse(
                    "INVALID_STATUS", "Trạng thái không hợp lệ");
            }
            var requests = _leaveRequestDL.GetForManager(managerId, searchText, leaveTypeId, status);
            return ApiResponse<List<LeaveRequest>>.SuccessResponse(
                requests, "Lấy danh sách đơn nghỉ phép thành công");
        }

        /// <summary>
        /// Lấy chi tiết đơn nghỉ phép theo ID và kiểm tra quyền xem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse<LeaveRequest> GetLeaveRequestById(long id, long userId, UserRole role)
        {
            var request = _leaveRequestDL.GetById(id);
            if (request == null)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "NOT_FOUND", "Không tìm thấy đơn nghỉ phép");
            }
            bool canView = false;
            if((int) role == 1)
            {
                // nhân viên chỉ xem đơn của mình
                canView = request.EmployeeId == userId;
            }
            else if ((int)role == 2)
            {
                // Kiểm tra đơn có thuộc phạm vi quản lý không
                var requests = _leaveRequestDL.GetForManager(userId);
                canView = requests.Any(item => item.Id == id);
            }
            if (!canView)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "FORBIDDEN", "Bạn không có quyền xem đơn nghỉ này");
            }
            return ApiResponse<LeaveRequest>.SuccessResponse(
                request, "Lấy chi tiết đơn nghỉ phép thành công");
        }
        /// <summary>
        /// thêm mới đơn nghỉ phép
        /// </summary>
        /// <param name="request"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse<long> Insert(CreateLeaveRequest request, long employeeId)
        {
            if (employeeId <= 0)
            {
                return ApiResponse<long>.ErrorResponse(
                    "INVALID_EMPLOYEE", "Thông tin nhân viên không hợp lệ");
            }
            if (request.LeaveTypeId <= 0)
            {
                return ApiResponse<long>.ErrorResponse(
                    "LEAVE_TYPE_REQUIRED", "Vui lòng chọn loại nghỉ");
            }
            if (request.StartDate == default || request.EndDate == default)
            {
                return ApiResponse<long>.ErrorResponse(
                    "DATE_REQUIRED", "Vui lòng chọn ngày bắt đầu và ngày kết thúc");
            }
            if (request.EndDate.Date < request.StartDate.Date)
            {
                return ApiResponse<long>.ErrorResponse(
                    "INVALID_DATE", "Ngày kết thúc không được trước ngày bắt đầu");
            }
            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                return ApiResponse<long>.ErrorResponse(
                    "REASON_REQUIRED", "Vui lòng nhập lý do nghỉ");
            }
            string reason = request.Reason.Trim();
            if (reason.Length > 1000)
            {
                return ApiResponse<long>.ErrorResponse(
                    "REASON_TOO_LONG", "Lý do không được vượt quá 1000 ký tự");
            }
            if (request.Note?.Length > 500)
            {
                return ApiResponse<long>.ErrorResponse(
                    "NOTE_TOO_LONG", "Ghi chú không được vượt quá 500 ký tự");
            }
            if (request.HandoverPerson?.Length > 100)
            {
                return ApiResponse<long>.ErrorResponse(
                    "HANDOVER_TOO_LONG", "Người bàn giao không được vượt quá 100 ký tự");
            }
            // tính tổng số ngày nghỉ
            decimal numberOfDays = (request.EndDate.Date - request.StartDate.Date).Days + 1;
            // tạo mã đơn nghỉ
            string requestCode = "LR" + Guid.NewGuid().ToString().Substring(0,18);
            // chuyển đổi thành entity
            var leaveRequest = new LeaveRequest
            {
                RequestCode = requestCode,
                EmployeeId = employeeId,
                LeaveTypeId = request.LeaveTypeId,
                StartDate = request.StartDate.Date,
                EndDate = request.EndDate.Date,
                NumberOfDays = numberOfDays,
                Reason = reason,
                Note = request.Note?.Trim(),
                HandoverPerson = request.HandoverPerson?.Trim(),
                Status = LeaveRequestStatus.Pending
            };
            // gọi hàm thêm mới đơn
            long id = _leaveRequestDL.Insert(leaveRequest);
            if (id <= 0)
            {
                return ApiResponse<long>.ErrorResponse(
                    "CREATE_FAILED", "Tạo đơn nghỉ phép thất bại");
            }
            return ApiResponse<long>.SuccessResponse(id, "Tạo đơn nghỉ phép thành công");

        }

        /// <summary>
        /// Quản lý duyệt hoặc từ chối đơn đang chờ duyệt
        /// user : quản lý
        /// </summary>
        /// <param name="id">ID đơn nghỉ</param>
        /// <param name="managerId">ID quản lý đang đăng nhập</param>
        /// <param name="request">Trạng thái và nội dung phản hồi</param>
        public ApiResponse<LeaveRequest> ProcessPending(long id, long managerId, ProcessLeaveRequest request)
        {
            if (request.Status != LeaveRequestStatus.Approved &&
                request.Status != LeaveRequestStatus.Rejected)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "INVALID_STATUS", "Chỉ được chọn duyệt hoặc từ chối");
            }
            // Kiểm tra đơn tồn tại
            var leaveRequest = _leaveRequestDL.GetById(id);
            if (leaveRequest == null)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "NOT_FOUND", "Không tìm thấy đơn nghỉ phép");
            }
            // Kiểm tra đơn thuộc phạm vi quản lý
            var requests = _leaveRequestDL.GetForManager(managerId);
            if (!requests.Any(item => item.Id == id))
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "FORBIDDEN", "Bạn không có quyền xử lý đơn này");
            }
            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "INVALID_STATUS", "Chỉ được xử lý đơn đang chờ duyệt");
            }
            // Lấy thông tin từ DTO và truyền xuống DL
            bool success = _leaveRequestDL.ProcessPending(id, managerId, request);
            if (!success)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "PROCESS_FAILED", "Không thể xử lý đơn, vui lòng tải lại danh sách");
            }
            // Lấy lại chi tiết đơn sau khi cập nhật
            var updatedRequest = _leaveRequestDL.GetById(id);
            if (updatedRequest == null)
            {
                return ApiResponse<LeaveRequest>.ErrorResponse(
                    "RELOAD_FAILED", "Đơn đã được xử lý nhưng không tải lại được thông tin");
            }
            string message = request.Status == LeaveRequestStatus.Approved? "Duyệt đơn thành công": "Từ chối đơn thành công";
            return ApiResponse<LeaveRequest>.SuccessResponse(updatedRequest, message);
        }
        /// <summary>
        /// Nhân viên sửa đơn của mình khi còn chờ duyệt
        /// </summary>
        /// <param name="id">ID đơn nghỉ</param>
        /// <param name="request">Nội dung cần cập nhật</param>
        /// <param name="employeeId">ID nhân viên đang đăng nhập</param>
        public ApiResponse<bool> UpdatePending(long id,UpdateLeaveRequest request,long employeeId)
        {
            // Lấy đơn cần sửa
            var leaveRequest = _leaveRequestDL.GetById(id);
            if (leaveRequest == null)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "NOT_FOUND", "Không tìm thấy đơn nghỉ phép");
            }
            // Chỉ được sửa đơn của mình
            if (leaveRequest.EmployeeId != employeeId)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "FORBIDDEN", "Bạn không có quyền sửa đơn này");
            }
            // Chỉ được sửa đơn đang chờ duyệt
            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "INVALID_STATUS", "Chỉ được sửa đơn đang chờ duyệt");
            }
            if (request.LeaveTypeId <= 0)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "LEAVE_TYPE_REQUIRED", "Vui lòng chọn loại nghỉ");
            }
            if (request.StartDate == default || request.EndDate == default)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "DATE_REQUIRED", "Vui lòng chọn ngày bắt đầu và ngày kết thúc");
            }
            if (request.EndDate.Date < request.StartDate.Date)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "INVALID_DATE", "Ngày kết thúc không được trước ngày bắt đầu");
            }
            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                return ApiResponse<bool>.ErrorResponse(
                    "REASON_REQUIRED", "Vui lòng nhập lý do nghỉ");
            }
            // Tính ngày liên tiếp, gồm cả ngày bắt đầu và kết thúc
            decimal numberOfDays = (request.EndDate.Date - request.StartDate.Date).Days + 1;

            // Gán các trường được phép sửa vào entity
            leaveRequest.LeaveTypeId = request.LeaveTypeId;
            leaveRequest.StartDate = request.StartDate.Date;
            leaveRequest.EndDate = request.EndDate.Date;
            leaveRequest.NumberOfDays = numberOfDays;
            leaveRequest.Reason = request.Reason;
            leaveRequest.Note = request.Note?.Trim();
            leaveRequest.HandoverPerson = request.HandoverPerson?.Trim();

            // DL cập nhật nếu đơn vẫn thuộc nhân viên và còn chờ duyệt
            bool success = _leaveRequestDL.UpdatePending(leaveRequest, employeeId);
            if (!success)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "UPDATE_FAILED", "Không thể cập nhật đơn, vui lòng tải lại danh sách");
            }
            return ApiResponse<bool>.SuccessResponse(true, "Cập nhật đơn thành công");
        }
    }
}
