using Model.Entities;

namespace BL.Interfaces
{
    public interface IDepartmentBL
    {
        /// <summary>
        /// lấy tất cả dữ liệu phòng ban
        /// </summary>
        /// <returns></returns>
        List<Department> GetAll();
    }
}