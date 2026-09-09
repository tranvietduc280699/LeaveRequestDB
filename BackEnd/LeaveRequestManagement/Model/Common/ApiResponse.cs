using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    /// <summary>
    /// hàm trả về thành công hoặc thất bại
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(
            T data,
            string message = "Thành công",
            string code = "SUCCESS")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Code = code,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(
            string code,
            string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Code = code,
                Message = message,
                Data = default
            };
        }
    }
}
