using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类UserGroupDataRigth。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-14 11:25:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserGroupDataRigth : DataBase
    {
        #region Instance
        public static readonly UserGroupDataRigth Instance = new UserGroupDataRigth();
        #endregion

        #region const
        private const string P_USERGROUPDATARIGTH_SELECT = "p_UserGroupDataRigth_Select";
        private const string P_USERGROUPDATARIGTH_INSERT = "p_UserGroupDataRigth_Insert";
        private const string P_USERGROUPDATARIGTH_UPDATE = "p_UserGroupDataRigth_Update";
        private const string P_USERGROUPDATARIGTH_DELETE = "p_UserGroupDataRigth_Delete";
        private const string P_USERGROUPDATARIGTH_GETUSERRIGHTNAMESTR = "p_UserGroupDataRigth_GetUserRightNameStr";
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
            string where = string.Empty;
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and  RecID=" + query.RecID + "";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and UserID=" + query.UserID + "";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and BGID=" + query.BGID + "";
            }
            if (query.RightType != Constant.INT_INVALID_VALUE)
            {
                where += " and RightType=" + query.RightType + "";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 查询用户下的用户组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId, string where)
        {
            string sqlStr = @"SELECT ugdr.*,Name FROM UserGroupDataRigth AS ugdr JOIN BusinessGroup AS bg ON ugdr.BGID=bg.BGID 
                                        WHERE UserID=@UserID ";
            if (!string.IsNullOrEmpty(where))
            {
                sqlStr += " " + where;
            }
            sqlStr += " Order by bg.Name";
            SqlParameter parameter = new SqlParameter("UserID", userId);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserGroupDataRigth GetUserGroupDataRigth(int RecID)
        {
            QueryUserGroupDataRigth query = new QueryUserGroupDataRigth();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserGroupDataRigth(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleUserGroupDataRigth(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取负责分组名字字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userId)
        {
            string str = string.Empty;
            SqlParameter parameter = new SqlParameter("@UserID", userId);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_GETUSERRIGHTNAMESTR, parameter);
            if (obj != null)
            {
                str = CommonFunction.ObjectToString(obj).TrimEnd(',');
            }
            return str;
        }
        private Entities.UserGroupDataRigth LoadSingleUserGroupDataRigth(DataRow row)
        {
            Entities.UserGroupDataRigth model = new Entities.UserGroupDataRigth();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["UserID"].ToString() != "")
            {
                model.UserID = int.Parse(row["UserID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["RightType"].ToString() != "")
            {
                model.RightType = int.Parse(row["RightType"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.UserGroupDataRigth model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@RightType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.RightType;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserGroupDataRigth model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@RightType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.RightType;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.UserGroupDataRigth model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@RightType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.RightType;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UserGroupDataRigth model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@RightType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.RightType;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_DELETE, parameters);
        }

        /// <summary>
        /// 通过用户ID删除分组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteByUserID(int userId)
        {
            string sqlStr = "DELETE FROM UserGroupDataRigth WHERE UserID=@UserID";
            SqlParameter parameter = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
        }
        #endregion

        #region 数据权限_sql拼接
        /// <summary>
        /// 根据表名称，分组字段名称，当前人字段名称，当前登录人id，拼接数据权限条件
        /// </summary>
        /// <param name="tablename">表名称，或表别名</param>
        /// <param name="BgIDFileName">分组字段名称</param>
        /// <param name="UserIDFileName">个人权限字段名称</param>
        /// <param name="UserID">当前人id</param>
        /// <returns>返回Sql字符串</returns>
        public string GetSqlRightstr(string tablename, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return GetSqlRightstr(tablename, tablename, BgIDFileName, UserIDFileName, UserID);
        }

        /// <summary>
        /// 拼接数据权限，当BGID、UserID属于两表时
        /// </summary>
        /// <param name="tablenameBgID">BGID所属的表名称，或表别名</param>
        /// <param name="tablenameUserID">UserID所属的表名称，或表别名</param>
        /// <param name="BgIDFileName">BGID字段名称</param>
        /// <param name="UserIDFileName">UserID字段名称</param>
        /// <param name="UserID">坐席ID</param>
        /// <returns>返回Sql字符串</returns>
        public string GetSqlRightstr(string tablenameBgID, string tablenameUserID, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return GetSqlRightstr(tablenameBgID, tablenameUserID, BgIDFileName, UserIDFileName, UserID, string.Empty);
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
            return GetSqlRightstr(tablename, tablename, BgIDFileName, UserIDFileName, UserID, whereStr);
        }

        public string GetSqlRightstr(string tablenameBgID, string tablenameUserID, string BgIDFileName, string UserIDFileName, int UserID, string whereStr)
        {
            StringBuilder where = new StringBuilder();
            //取本人权限
            where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'");
            //取当前人所对应的数据权限组
            where.Append(" OR " + tablenameBgID + "." + BgIDFileName + " IN (SELECT BGID FROM UserGroupDataRigth WHERE USERID = " + UserID + ")");
            if (!string.IsNullOrEmpty(whereStr))
            {
                //其他情况的条件
                where.Append(" OR " + whereStr);
            }
            where.Append(") ");
            return where.ToString();
        }

        /// <summary>
        /// 根据人取人所对应分组串
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetGroupStr(int userid)
        {
            string groupstr = string.Empty;
            string sqlstr = "SELECT distinct groupstr=ISNULL(STUFF((SELECT ',' + RTRIM(UserGroupDataRigth.BGID) FROM UserGroupDataRigth where [dbo].UserGroupDataRigth.userid = f.userid FOR XML PATH('')), 1, 1, ''), '') FROM dbo.UserGroupDataRigth f WHERE UserID=" + userid;

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                groupstr = dt.Rows[0]["groupstr"].ToString();
            }
            else
            {
                groupstr = "-99";
            }
            return groupstr;
        }
        #endregion

        /// 删除人员和分组区域对不上的错误数据
        /// <summary>
        /// 删除人员和分组区域对不上的错误数据
        /// </summary>
        /// <returns></returns>
        public int DeleteErrorData(int userid)
        {
            string sql = @"DELETE  FROM UserGroupDataRigth
                                        WHERE   RecID IN ( SELECT   a.RecID
                                                           FROM     UserGroupDataRigth a ,
                                                                    dbo.EmployeeAgent b ,
                                                                    dbo.BusinessGroup c
                                                           WHERE    a.UserID = b.UserID
                                                                    AND a.BGID = c.BGID
                                                                    AND b.RegionID != c.RegionID )
                                                        AND UserID = " + userid;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

    }
}

