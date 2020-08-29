using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.BLL.SysRight
{
    public class ModuleInfo
    {
        #region Instance
        public static readonly ModuleInfo Instance = new ModuleInfo();
        #endregion

        #region Contructor
        protected ModuleInfo()
        {
        }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryModuleInfo">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>用户表集合</returns>
        public DataTable GetModuleInfo(QueryModuleInfo queryModuleInfo, string orderStr, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SysRight.ModuleInfo.Instance.GetModuleInfo(queryModuleInfo, orderStr, currentPage, pageSize, out  totalCount);
        }
        public DataTable GetModuleInfoByRoleId(string roleId, string sysID)
        {
            return Dal.SysRight.ModuleInfo.Instance.GetModuleInfoByRoleId(roleId, sysID);
        }
        /// <summary>
        /// 按照userid查询
        /// </summary>
        /// <param name="userid">userid</param>
        /// <returns>集合</returns>
        public DataTable GetParentModuleInfoByUserID(int userid)
        {
            return Dal.SysRight.ModuleInfo.Instance.GetParentModuleInfoByUserID(userid);
        }

        #endregion

        #region Insert
        /// <summary>
        /// 添加详细
        /// </summary>
        /// <param name="ModuleInfo">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertModuleInfo(Entities.SysRight.ModuleInfo ModuleInfo)
        {
            //Bll.SysOperationLog.Instance.Insert("添加模块编号：" + ModuleInfo.ModuleID + "名称：" + ModuleInfo.ModuleName);
            return Dal.SysRight.ModuleInfo.Instance.InsertModuleInfo(ModuleInfo);

        }
        #endregion

        #region Updata
        /// <summary>
        /// 更新详细
        /// </summary>
        /// <param name="ModuleInfo">值对象</param>
        /// <returns>成功:1;失败:-1</returns>
        public int UpdataModuleInfo(Entities.SysRight.ModuleInfo ModuleInfo)
        {
            //Bll.SysOperationLog.Instance.Insert("编辑模块编号：" + ModuleInfo.ModuleID + "名称：" + ModuleInfo.ModuleName);
            return Dal.SysRight.ModuleInfo.Instance.UpdataModuleInfo(ModuleInfo);
        }
        #endregion

        public bool IsExistBySysID(string sysID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.SysID = sysID;
            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetModuleIDByRecID(int recID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.RecID = recID;
            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, string.Empty, 1, 99999, out count);
            if (count > 0)
            {
                return dt.Rows[0]["ModuleID"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public Entities.SysRight.ModuleInfo GetModuleInfo(string moduleid)
        {
            return Dal.SysRight.ModuleInfo.Instance.GetModuleInfo(moduleid);
        }

        public Entities.SysRight.ModuleInfo GetModuleInfo(int id)
        {
            return Dal.SysRight.ModuleInfo.Instance.GetModuleInfo(id);
        }

        public DataTable GetModuleNameRelation(QueryModuleInfo queryModuleInfo)
        {
            return Dal.SysRight.ModuleInfo.Instance.GetModuleNameRelation(queryModuleInfo);
        }

        public bool IsExistByModuleName(string moduleName)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ExistModuleName = moduleName;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataTable GetSysInfoByMaxRecID()
        {
            return Dal.SysRight.ModuleInfo.Instance.GetSysInfoByMaxRecID();
        }
        public string GenModuleID(string sysID)
        {
            string moduleID = "MOD0001";
            DataTable dt = new DataTable();
            dt = GetSysInfoByMaxRecID();
            if (dt != null && dt.Rows.Count > 0)
            {
                moduleID = dt.Rows[0]["ModuleID"].ToString().Trim();
                int id = 0;
                int.TryParse(moduleID.Substring(9, 4), out id);
                id++;
                moduleID = "MOD" + id.ToString().PadLeft(4, '0');
            }
            return sysID + moduleID;
        }

        public string GetPIDByModuleID(string moduleID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ModuleID = moduleID;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, string.Empty, 1, 99999, out count);
            if (count > 0)
            {
                return dt.Rows[0]["PID"].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public int GetLevelByModuleID(string moduleID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ModuleID = moduleID;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, string.Empty, 1, 99999, out count);
            if (count > 0)
            {
                int.TryParse(dt.Rows[0]["Level"].ToString().Trim(), out count);
            }
            return count;
        }

        public bool IsExistByModuleName(string moduleName, string moduleID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ExistModuleName = moduleName;
            moduleInfo.ExceptModuleID = moduleID;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, string.Empty, 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetRootModuleNameListBySysID(string sysID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.SysID = sysID;
            moduleInfo.PID = string.Empty;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 99999, out count);
            if (count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public bool IsExistByModuleNameAndSysID(string moduleName, string sysID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ExistModuleName = moduleName;
            moduleInfo.SysID = sysID;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExistByModuleNameAndSysID(string moduleName, string sysID, string moduleID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ExistModuleName = moduleName;
            moduleInfo.ExceptModuleID = moduleID;
            moduleInfo.SysID = sysID;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsExistByModuleNameAndSysID(string moduleName, string sysID, string moduleID, string pid)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ExistModuleName = moduleName;
            moduleInfo.ExceptModuleID = moduleID;
            moduleInfo.SysID = sysID;
            moduleInfo.PID = pid;
            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 999999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsExistByModuleNameAndSysIDAndPid(string moduleName, string sysID, string PID)
        {
            int count = 0;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ExistModuleName = moduleName;
            moduleInfo.PID = PID;
            moduleInfo.SysID = sysID;

            DataTable dt = new DataTable();
            dt = GetModuleInfo(moduleInfo, "OrderNum", 1, 99999, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断一个地址是否已存在于某个模块中
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool IsExist(string link, string sysid)
        {
            return Dal.SysRight.ModuleInfo.Instance.IsExist(link, sysid);
        }
    }
}
