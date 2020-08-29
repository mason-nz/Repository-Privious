using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.BLL.SysRight
{
    public class UserRole
    {
        #region Instance
        public static readonly UserRole Instance = new UserRole();
        #endregion

        #region Contructor
        protected UserRole()
        {
        }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryUserRole">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>角色资源对应表集合</returns>
        public DataTable GetUserRole(string where, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SysRight.UserRole.Instance.GetUserRole(where, currentPage, pageSize, out  totalCount);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 添加详细
        /// </summary>
        /// <param name="UserRole">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertUserRole(Entities.SysRight.UserRole UserRole)
        {
            return Dal.SysRight.UserRole.Instance.InsertUserRole(UserRole);
        }
        #endregion

        #region Updata
        /// <summary>
        /// 更新详细
        /// </summary>
        /// <param name="UserRole">值对象</param>
        /// <returns>成功:1;失败:-1</returns>
        public int UpdataUserRole(Entities.SysRight.UserRole UserRole)
        {
            return Dal.SysRight.UserRole.Instance.UpdataUserRole(UserRole);
        }
        #endregion

        public void Delete(int RecID)
        {
            Dal.SysRight.UserRole.Instance.Delete(RecID);
        }
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="UserID"></param>
        public void DeleteByuserID(int UserID)
        {
            Dal.SysRight.UserRole.Instance.DeleteByuserID(UserID);
        }
        public bool IsExistByUserIDAndRoleIDAndSysID(int userID, string roleID, string sysID)
        {
            int count = 0;
            string str = "And UserRole.UserID=" + userID + " And UserRole.RoleID='" + roleID + "' And UserRole.SysID='" + sysID + "'";

            DataTable dt = new DataTable();
            dt = GetUserRole(str, 1, int.MaxValue, out count);
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
        /// 判断用户是否系统角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool IsSysRole(int UserID)
        {
            return Dal.SysRight.UserRole.Instance.IsSysRole(UserID);
        }

        /// <summary>
        /// 根据系统ID得到有本系统权限的人员
        /// </summary>
        public DataTable GetUsersBySysID(string SysID, int currentPage, int pageSize, out int totalCount)
        {
            return this.GetUserRole(string.Format(" and UserRole.SysID = '{0}'", SysID), currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 拼接数据权限
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="tablenameUserID">UserID所属的表名称，或表别名</param>
        /// <param name="UserIDFileName">UserID字段名称</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="msg"></param>
        /// <returns>返回Sql字符串</returns>
        public string GetSqlRightStr(Entities.EnumResourceType resourceType, string tablenameUserID, string UserIDFileName, int UserID,int PubID, out string msg)
        {
            return Dal.SysRight.UserRole.Instance.GetSqlRightStr(resourceType, tablenameUserID, UserIDFileName, UserID, PubID, out msg);            
        }
    }
}
