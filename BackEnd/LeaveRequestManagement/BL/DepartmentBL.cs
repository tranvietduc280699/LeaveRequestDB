using BL.Interfaces;
using DL.Interfaces;
using Model.Entities;

namespace BL
{
    public class DepartmentBL : IDepartmentBL
    {
        // injection dependency
        private readonly IDepartmentDL _departmentDL;

        public DepartmentBL(IDepartmentDL departmentDL)
        {
            _departmentDL = departmentDL;
        }
        /// <summary>
        /// lấy tất cả các phòng ban
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAll()
        {
            return _departmentDL.GetAll();
        }
    }
}