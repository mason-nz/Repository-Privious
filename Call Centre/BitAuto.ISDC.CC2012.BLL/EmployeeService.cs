using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;

namespace BitAuto.ISDC.CC2012.BLL
{
    public static class EmployeeService
    {
        private static IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                            ConfigurationManager.AppSettings["OrganizationService"]);

        public static IList<Employee> SearchByName(string name, int pageIndex, int pageSize, out int count)
        {
            IList<Employee> list = service.QueryEmployeeByCnName(name);
            count = list.Count;
            int index = 0;
            if (list.Count > pageIndex * pageSize)
            {
                index = pageIndex * pageSize;
            }
            else
            {
                index = list.Count;
            }
            if (list.Count < pageSize)
            {
                return list;
            }
            IList<Employee> result = new List<Employee>();
            for (int i = (pageIndex - 1) * pageSize; i < index; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }
    }
}
