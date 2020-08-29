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
    /// ���ݷ�����UserGroupDataRigth��
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
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
        /// ��ѯ�û��µ��û���
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
        /// �õ�һ������ʵ��
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
        /// ��ȡ������������ַ���
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
        ///  ����һ������
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
        ///  ����һ������
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
        ///  ����һ������
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
        ///  ����һ������
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
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERGROUPDATARIGTH_DELETE, parameters);
        }

        /// <summary>
        /// ͨ���û�IDɾ������
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

        #region ����Ȩ��_sqlƴ��
        /// <summary>
        /// ���ݱ����ƣ������ֶ����ƣ���ǰ���ֶ����ƣ���ǰ��¼��id��ƴ������Ȩ������
        /// </summary>
        /// <param name="tablename">�����ƣ�������</param>
        /// <param name="BgIDFileName">�����ֶ�����</param>
        /// <param name="UserIDFileName">����Ȩ���ֶ�����</param>
        /// <param name="UserID">��ǰ��id</param>
        /// <returns>����Sql�ַ���</returns>
        public string GetSqlRightstr(string tablename, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return GetSqlRightstr(tablename, tablename, BgIDFileName, UserIDFileName, UserID);
        }

        /// <summary>
        /// ƴ������Ȩ�ޣ���BGID��UserID��������ʱ
        /// </summary>
        /// <param name="tablenameBgID">BGID�����ı����ƣ�������</param>
        /// <param name="tablenameUserID">UserID�����ı����ƣ�������</param>
        /// <param name="BgIDFileName">BGID�ֶ�����</param>
        /// <param name="UserIDFileName">UserID�ֶ�����</param>
        /// <param name="UserID">��ϯID</param>
        /// <returns>����Sql�ַ���</returns>
        public string GetSqlRightstr(string tablenameBgID, string tablenameUserID, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return GetSqlRightstr(tablenameBgID, tablenameUserID, BgIDFileName, UserIDFileName, UserID, string.Empty);
        }

        /// <summary>
        /// �����б�ƴ������Ȩ�ޣ�������Ȩ���жϣ����˵��ж��⻹��������������������紦����Ҳ���Բ鿴����һ���ַ��� add lxw 13.10.12
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
            //ȡ����Ȩ��
            where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'");
            //ȡ��ǰ������Ӧ������Ȩ����
            where.Append(" OR " + tablenameBgID + "." + BgIDFileName + " IN (SELECT BGID FROM UserGroupDataRigth WHERE USERID = " + UserID + ")");
            if (!string.IsNullOrEmpty(whereStr))
            {
                //�������������
                where.Append(" OR " + whereStr);
            }
            where.Append(") ");
            return where.ToString();
        }

        /// <summary>
        /// ������ȡ������Ӧ���鴮
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

        /// ɾ����Ա�ͷ�������Բ��ϵĴ�������
        /// <summary>
        /// ɾ����Ա�ͷ�������Բ��ϵĴ�������
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

