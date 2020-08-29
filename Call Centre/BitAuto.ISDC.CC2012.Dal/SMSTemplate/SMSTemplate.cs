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
    /// 数据访问类SMSTemplate。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-12-23 06:17:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SMSTemplate : DataBase
    {
        #region Instance
        public static readonly SMSTemplate Instance = new SMSTemplate();
        #endregion

        #region const
        private const string P_SMSTEMPLATE_SELECT = "p_SMSTemplate_Select";
        private const string P_SMSTEMPLATE_INSERT = "p_SMSTemplate_Insert";
        private const string P_SMSTEMPLATE_UPDATE = "p_SMSTemplate_Update";
        private const string P_SMSTEMPLATE_DELETE = "p_SMSTemplate_Delete";
        #endregion

        #region Contructor
        protected SMSTemplate()
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
        public DataTable GetSMSTemplate(QuerySMSTemplate query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", (Int32)query.LoginID);

                where += whereDataRight;
            }
            #endregion
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.BGID=" + query.BGID;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.SCID=" + query.SCID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CreateUserID=" + query.CreateUserID;
            }
            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.title like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.Status=" + query.Status;
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.RecID=" + query.RecID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSTEMPLATE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }



        /// <summary>
        /// 根据当前人取当前人数据权限下的模板创建人
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetCreateUserID(int userid)
        {
            DataTable dt = null;
            //string sqlstr = "select distinct createUserID from SMSTemplate where createUserID!=-1 and createUserID!=-2 and bgid in(select distinct BGID from dbo.UserGroupDataRigth where userid=" + userid + " union select bgid from dbo.EmployeeAgent where userid=" + userid + ")";
            string sqlstr = "select distinct createUserID from SMSTemplate where createUserID!=-1 and createUserID!=-2 and bgid in(select distinct BGID from dbo.UserGroupDataRigth where userid=" + userid + ")";
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr).Tables[0];
            return dt;
        }


        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SMSTemplate GetSMSTemplate(int RecID)
        {
            string sqlStr = "SELECT * FROM SMSTemplate WHERE RecID=@RecID";
            SqlParameter parameter = new SqlParameter("@RecID", RecID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleSMSTemplate(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SMSTemplate LoadSingleSMSTemplate(DataRow row)
        {
            Entities.SMSTemplate model = new Entities.SMSTemplate();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Title = row["Title"].ToString();
            model.Content = row["Content"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
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
        public void Insert(Entities.SMSTemplate model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSTEMPLATE_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.SMSTemplate model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SMSTEMPLATE_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SMSTemplate model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSTEMPLATE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SMSTemplate model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SMSTEMPLATE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSTEMPLATE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SMSTEMPLATE_DELETE, parameters);
        }
        #endregion

    }
}

