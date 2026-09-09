using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Common;

namespace API.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentController : ControllerBase
    {
        // inject
        private readonly IDepartmentBL _departmentBL;
        public DepartmentController(IDepartmentBL departmentBL)
        {
            _departmentBL = departmentBL;
        }
        /// <summary>
        /// Lấy danh sách tất cả phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _departmentBL.GetAll();
            return Ok(ApiResponse<object>.SuccessResponse(result));
        }
    }
}