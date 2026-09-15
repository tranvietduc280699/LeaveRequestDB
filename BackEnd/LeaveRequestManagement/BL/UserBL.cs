using BL.Interfaces;
using DL.Interfaces;
using Model.Common;
using Model.DTOs;


namespace BL
{
    public class UserBL: IUserBL
    {
        private readonly IUserDL _userDL;
        public UserBL(IUserDL userDl)
        {
            _userDL = userDl;
        }
        /// <summary>
        /// Cập nhật họ tên, số điện thoại và địa chỉ
        /// </summary>
        /// <param name="id">ID người dùng đang đăng nhập</param>
        /// <param name="request">Thông tin cần cập nhật</param>
        /// <returns>Kết quả cập nhật thông tin cá nhân</returns>
        public ApiResponse<bool> UpdateProfile(long id, UpdateProfileRequest request)
        {
            // Kiểm tra ID người dùng
            if (id <= 0)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "INVALID_USER",
                    "Người dùng không hợp lệ");
            }

            // Họ tên bắt buộc nhập
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return ApiResponse<bool>.ErrorResponse(
                    "FULL_NAME_REQUIRED",
                    "Vui lòng nhập họ và tên");
            }

            // Số điện thoại bắt buộc nhập
            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                return ApiResponse<bool>.ErrorResponse(
                    "PHONE_REQUIRED",
                    "Vui lòng nhập số điện thoại");
            }

            // Xóa khoảng trắng ở đầu và cuối
            string fullName = request.FullName.Trim();
            string phone = request.Phone.Trim();
            string? address = request.Address?.Trim();

            if (phone.Length > 20)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "PHONE_TOO_LONG",
                    "Số điện thoại không được vượt quá 20 ký tự");
            }

            // Gán dữ liệu đã xử lý vào DTO
            request.FullName = fullName;
            request.Phone = phone;
            request.Address = string.IsNullOrWhiteSpace(address) ? null : address;

            // Gọi DL cập nhật database
            bool success = _userDL.UpdateProfile(id, request);

            if (!success)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "UPDATE_FAILED",
                    "Không thể cập nhật thông tin cá nhân");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Cập nhật thông tin cá nhân thành công");
        }
        /// <summary>
        /// Lấy danh sách tên quản lý
        /// </summary>
        /// <returns></returns>
        public ApiResponse<List<ManagerOption>> GetManagers()
        {
            var managers = _userDL.GetManagers();

            return ApiResponse<List<ManagerOption>>.SuccessResponse(
                managers,
                "Lấy danh sách người quản lý thành công");
        }
    }
}
