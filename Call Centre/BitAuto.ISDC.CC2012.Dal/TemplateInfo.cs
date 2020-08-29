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
    /// 数据访问类TemplateInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:23 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TemplateInfo : DataBase
    {
        #region Instance
        public static readonly TemplateInfo Instance = new TemplateInfo();
        #endregion

        #region const
        private const string P_TEMPLATEINFO_SELECT = "p_TemplateInfo_Select";
        private const string P_TEMPLATEINFOOne_SELECT = "p_TemplateInfoOne_Select";
        private const string P_TEMPLATEINFO_INSERT = "p_TemplateInfo_Insert";
        private const string P_TEMPLATEINFO_UPDATE = "p_TemplateInfo_Update";
        private const string P_TEMPLATEINFO_DELETE = "p_TemplateInfo_Delete";
        private const string P_TEMPLATEINFO_ClearUser = "p_TemplateInfo_ClearUser";
        private const string P_TEMPLATEINFO_getEmailServers = "p_TemplateInfo_getEmailServers";

        #endregion

        #region Contructor
        protected TemplateInfo()
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
        public DataTable GetTemplateInfo(QueryTemplateInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TCID != Constant.INT_INVALID_VALUE)
            {
                where += " And TCID in(" + query.TCID + ")";
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and TemplateInfo.RecID=" + query.RecID;
            }
            if (query.Content != Constant.STRING_INVALID_VALUE)
            {
                where += " And TCID in(" + Dal.Util.SqlFilterByInCondition(query.Content) + ")";
            }
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 2000),
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        public DataTable GetTemplateInfoOne(string RecID)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int, 4)
					};
            parameters[0].Value = RecID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFOOne_SELECT, parameters);

            return ds.Tables[0];
        }

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TemplateInfo GetTemplateInfo(int RecID)
        {
            QueryTemplateInfo query = new QueryTemplateInfo();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTemplateInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleTemplateInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.TemplateInfo LoadSingleTemplateInfo(DataRow row)
        {
            Entities.TemplateInfo model = new Entities.TemplateInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["TCID"].ToString() != "")
            {
                model.TCID = int.Parse(row["TCID"].ToString());
            }
            model.Title = row["Title"].ToString();
            model.Content = row["Content"].ToString();
            model.ReplaceTag = row["ReplaceTag"].ToString();
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
        public int Insert(Entities.TemplateInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TCID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReplaceTag", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TCID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.ReplaceTag;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.TemplateInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TCID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TCID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TemplateID", SqlDbType.Int)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFO_DELETE, parameters);
        }
        #endregion

        #region IsExist
        public bool IsExist(int tcId, string title)
        {
            bool result = false;
            string sqlStr = "SELECT * FROM TemplateInfo WHERE Title=@Title And TCID=@TCID";
            SqlParameter[] parameters ={
                                           new SqlParameter("@Title",StringHelper.SqlFilter(title)),
                                           new SqlParameter("@TCID",tcId)
                                      };

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                result = true;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="tcId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsExistNotThisRecID(int recId, int tcId, string title)
        {
            bool result = false;
            string sqlStr = "SELECT * FROM TemplateInfo WHERE Title=@Title And TCID=@TCID And RecID<>@RecID";
            SqlParameter[] parameters ={
                                           new SqlParameter("@Title",StringHelper.SqlFilter(title)),
                                           new SqlParameter("@TCID",tcId),
                                           new SqlParameter("@RecID",recId)
                                      };

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                result = true;
            }

            return result;
        }
        #endregion

        #region ClearUser
        /// <summary>
        /// 清理一个模板的接收人
        /// </summary>
        public int ClearUser(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TemplateID", SqlDbType.Int)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFO_ClearUser, parameters);
        }
        #endregion

        #region getEmailServers(int)
        /// <summary>
        /// 获取接受邮件的用户
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public DataTable getEmailServers(int TemplateID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@TemplateID", SqlDbType.NVarChar, 2000)};
            parameters[0].Value = TemplateID;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATEINFO_getEmailServers, parameters);
            return ds.Tables[0];
        }
        #endregion
    }
}

