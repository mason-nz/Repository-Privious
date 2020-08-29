using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class EmployeeSuper
    {
        public static readonly EmployeeSuper Instance = new EmployeeSuper();


        /// <summary>
        /// 分页获取员工（多条件搜索）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetEmployeeSuper(QueryEmployeeSuper query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeSuper.Instance.GetEmployeeSuper(query, order, currentPage, pageSize, out totalCount);
        }
    }
}
