using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.BLL.SysRight
{
    public class RoleInfo
    {
        #region Instance
        public static readonly RoleInfo Instance = new RoleInfo();
        #endregion

        #region Contructor
        protected RoleInfo()
        {
        }
        #endregion

        #region Select
        ///// <summary>
        ///// 根据角色ID查询
        ///// </summary>
        ///// <param name="RoleID">角色ID</param>
        ///// <returns></returns>
        //public DataTable GetRoleInfoByRoleID(string RoleID)
        //{
        //    return Dal.SysRight.RoleInfo.Instance.GetRoleInfoByRoleID(RoleID);
        //}
        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryRoleInfo">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>用户表集合</returns>
        public DataTable GetRoleInfo(QueryRoleInfo queryRoleInfo, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SysRight.RoleInfo.Instance.GetRoleInfo(queryRoleInfo, currentPage, pageSize, out  totalCount);
        }
        /// <summary>
        /// 判断选择的角色是否有用户使用
        /// </summary>
        /// <param name="roleIds">角色id</param>        
        /// <returns>用户表集合</returns>
        public int UserRoleIsUse(string roleIds)
        {
            return Dal.SysRight.RoleInfo.Instance.UserRoleIsUse(roleIds);
        }
        ///// <summary>
        ///// 得到全部
        ///// </summary>
        ///// <returns>集合</returns>
        //public DataTable GetRoleInfoAll()
        //{
        //    return Dal.RoleInfo.Instance.GetRoleInfoAll();
        //}
        public DataTable GetRoleInfoBySysID(string sysID)
        {
            return Dal.SysRight.RoleInfo.Instance.GetRoleInfoBySysID(sysID);
        }

        /// <summary>
        /// 通过角色id获取拥有该角色的用户信息
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户信息</returns>
        public DataTable GetUserInfoByRoleId(string roleID)
        {
            return Dal.SysRight.RoleInfo.Instance.GetUserInfoByRoleId(roleID);
        }
        #endregion

        //#region SelectByID
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="id">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.SysRight.RoleInfo GetRoleInfo(string roleID)
        {
            return Dal.SysRight.RoleInfo.Instance.GetRoleInfo(roleID);
        }
        //#endregion
        #region Insert
        /// <summary>
        /// 添加详细
        /// </summary>
        /// <param name="RoleInfo">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertRoleInfo(Entities.SysRight.RoleInfo RoleInfo)
        {
            //Bll.SysOperationLog.Instance.Insert("添加角色:" + RoleInfo.RoleName);
            return Dal.SysRight.RoleInfo.Instance.InsertRoleInfo(RoleInfo);
        }
        #endregion

        #region Updata
        /// <summary>
        /// 更新详细
        /// </summary>
        /// <param name="RoleInfo">值对象</param>
        /// <returns>成功:1;失败:-1</returns>
        public int UpdataRoleInfo(Entities.SysRight.RoleInfo RoleInfo)
        {
            //Bll.SysOperationLog.Instance.Insert("编辑角色:" + RoleInfo.RoleName);
            return Dal.SysRight.RoleInfo.Instance.UpdataRoleInfo(RoleInfo);
        }
        #endregion

        //public bool IsExistBySysID(string sysID)
        //{
        //    int count = 0;
        //    QueryRoleInfo roleInfo = new QueryRoleInfo();
        //    roleInfo.SysID = sysID;
        //    DataTable dt = new DataTable();
        //    dt = GetRoleInfo(roleInfo, 1, int.MaxValue, out count);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public bool IsExistBySysIDAndRoleName(string sysID, string roleName)
        //{
        //    int count = 0;
        //    QueryRoleInfo roleInfo = new QueryRoleInfo();
        //    roleInfo.SysID = sysID;
        //    roleInfo.RoleName = roleName;
        //    DataTable dt = new DataTable();
        //    dt = GetRoleInfo(roleInfo, 1, int.MaxValue, out count);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public Entities.RoleInfo GetRoleInfoByRecID(int addRoleID)
        //{
        //    return Dal.RoleInfo.Instance.GetRoleInfoByRecID(addRoleID);
        //}
    }
}
