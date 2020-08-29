using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Services.Organization.Remoting;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.BLL
{
    public static class DepartMentService
    {
        private static IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                            ConfigurationManager.AppSettings["OrganizationService"]);

        public static string GetDepartmentFullPath(string employeeNumber)
        {
            Department dept = service.GetDeptByEmployeeNumber(employeeNumber);
            if (dept != null)
            {
                return dept.FullPath;
            }
            else
            {
                return string.Empty;
            }
        }

        public static IList<BitAuto.Services.Organization.Remoting.Department> GetDepartmentByParentID(int departmentId)
        {
            IList<BitAuto.Services.Organization.Remoting.Department> list = service.QuerySubDept(departmentId, false);
            return list;
        }

        /// <summary>
        /// 获取部门的上级部门，即上级部门的上级部门也会返回
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public static IList<BitAuto.Services.Organization.Remoting.Department> GetSuperDepartmentByParentID(int departmentId)
        {
            IList<BitAuto.Services.Organization.Remoting.Department> list = service.QuerySuperDept(departmentId, true);
             return list;
        }

        public static Department Get(int departmentID)
        {
            Department department = service.GetDeptById(departmentID);
            if (department == null)
                return null;
            return department;
        }
    }
}
