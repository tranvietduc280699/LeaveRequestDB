using Model.Entities;

namespace DL.Interfaces
{
    public interface IDepartmentDL
    {
        /// <summary>
        /// lấy tất cả dữ liệu phòng ban
        /// </summary>
        /// <returns></returns>
        List<Department> GetAll();
    }
}