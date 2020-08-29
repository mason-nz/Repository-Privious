using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类UserGroupDataRigth 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-14 11:25:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserGroupDataRigth
    {
        #region Instance
        public static readonly UserGroupDataRigth Instance = new UserGroupDataRigth();
        #endregion

        #region Contructor
        protected UserGroupDataRigth()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetUserGroupDataRigth(QueryUserGroupDataRigth query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 查询用户下的用户组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId, null);
        }
        /// <summary>
        /// 查询用户下的用户组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId, string where)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId, where);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(new QueryUserGroupDataRigth(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserGroupDataRigth GetUserGroupDataRigth(int RecID)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(RecID);
        }
        /// <summary>
        /// 获取用户组名字字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userId)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupNamesStr(userId);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryUserGroupDataRigth query = new QueryUserGroupDataRigth();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserGroupDataRigth(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.UserGroupDataRigth.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.UserGroupDataRigth.Instance.Delete(sqltran, RecID);
        }
        /// <summary>
        /// 通过用户ID删除分组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteByUserID(int userId)
        {
            return Dal.UserGroupDataRigth.Instance.DeleteByUserID(userId);
        }
        #endregion

        /// <summary>
        /// 根据表名称，分组字段名称，当前人字段名称，当前登录人id，拼接数据权限条件
        /// </summary>
        /// <param name="tablename">表名称，或表别名</param>
        /// <param name="BgIDFileName">分组字段名称</param>
        /// <param name="UserIDFileName">个人权限字段</param>
        /// <param name="UserID">当前人id</param>
        /// <returns></returns>
        public string GetSqlRightstr(string tablename, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return Dal.UserGroupDataRigth.Instance.GetSqlRightstr(tablename, BgIDFileName, UserIDFileName, UserID);
        }

        /// <summary>
        /// 拼接数据权限，当BGID、UserID属于两表时
        /// </summary>
        /// <param name="tablenameBgID"></param>
        /// <param name="tablenameUserID"></param>
        /// <param name="BgIDFileName"></param>
        /// <param name="UserIDFileName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetSqlRightstr(string tablenameBgID, string tablenameUserID, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return Dal.UserGroupDataRigth.Instance.GetSqlRightstr(tablenameBgID, tablenameUserID, BgIDFileName, UserIDFileName, UserID);
        }

        /// <summary>
        /// 工单列表拼接数据权限，除了组权限判断，本人的判断外还有其他情况的条件，比如处理人也可以查看，传一段字符串 add lxw 13.10.12
        /// </summary>
        /// <param name="tablenameBgID"></param>
        /// <param name="tablenameUserID"></param>
        /// <param name="BgIDFileName"></param>
        /// <param name="UserIDFileName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetSqlRightstrByOrderWhere(string tablename, string BgIDFileName, string UserIDFileName, int UserID, string whereStr)
        {
            return Dal.UserGroupDataRigth.Instance.GetSqlRightstrByOrderWhere(tablename, BgIDFileName, UserIDFileName, UserID, whereStr);
        }


        public string GetGroupStr(int userid)
        {
            return Dal.UserGroupDataRigth.Instance.GetGroupStr(userid);
        }

        /// 删除人员和分组区域对不上的错误数据
        /// <summary>
        /// 删除人员和分组区域对不上的错误数据
        /// </summary>
        /// <returns></returns>
        public int DeleteErrorData(int userid)
        {
            return Dal.UserGroupDataRigth.Instance.DeleteErrorData(userid);
        }
    }
}

