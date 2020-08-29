using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.BLL.SysRight
{
    public class RoleModule
    {
        #region Instance
        public static readonly RoleModule Instance = new RoleModule();
        #endregion

        #region Contructor
        protected RoleModule()
        {
        }
        #endregion


        #region Select
        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryRoleModule">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>角色资源对应表集合</returns>
        public DataTable GetRoleModule(QueryRoleModule queryRoleModule, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SysRight.RoleModule.Instance.GetRoleModule(queryRoleModule, currentPage, pageSize, out  totalCount);
        }
        #endregion

        public bool IsExistByModuleID(string moduleID)
        {
            int count = 0;
            QueryRoleModule roleModule = new QueryRoleModule();
            roleModule.ModuleID = moduleID;

            DataTable dt = new DataTable();
            dt = GetRoleModule(roleModule, 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int InsertRoleModuleAll(string roleId, string moduleIds, string sysID)
        {
            return Dal.SysRight.RoleModule.Instance.InsertRoleModuleAll(roleId, moduleIds, sysID);
        }
    }
}
