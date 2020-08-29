using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ���ݷ�����ConverSationDetail��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ConverSationDetail : DataBase
    {
        #region Instance
        public static readonly ConverSationDetail Instance = new ConverSationDetail();
        #endregion

        #region const
        private const string P_CONVERSATIONDETAIL_SELECT = "p_ConverSationDetail_Select";
        private const string P_CONVERSATIONDETAIL_INSERT = "p_ConverSationDetail_Insert";
        private const string P_CONVERSATIONDETAIL_UPDATE = "p_ConverSationDetail_Update";
        private const string P_CONVERSATIONDETAIL_DELETE = "p_ConverSationDetail_Delete";
        #endregion

        #region Contructor
        protected ConverSationDetail()
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
        public DataTable GetConverSationDetail(QueryConverSationDetail query, string order, int currentPage, int pageSize, out int totalCount, string tableName = "")
        {
            if (tableName == "")
            {
                DateTime date = DateTime.Now;
                tableName = "ConverSationDetail_" + date.ToString("yyyyMM");
            }

            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@TableName", SqlDbType.VarChar, 30),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = tableName;
            parameters[1].Value = BuildWhere(query);
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// ������ѯ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string BuildWhere(QueryConverSationDetail query)
        {
            string strWhere = string.Empty;
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                strWhere += " and RecID=" + query.RecID;
            }
            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                strWhere += " and CSID=" + query.CSID;
            }
            if (query.Sender != Constant.INT_INVALID_VALUE)
            {
                strWhere += " and Sender=" + query.Sender;
            }
            if (query.Content != Constant.STRING_INVALID_VALUE)
            {
                strWhere += " and Content like '%" + SqlFilter(query.Content) + "%'"; ;
            }
            return strWhere;
        }

        /// <summary>
        /// ��ȡ�Ự��ϸ�ı���
        /// </summary>
        /// <returns>���ر���</returns>
        public string GetSationDetailName()
        {
            DateTime date = DateTime.Now;
            string tableName = "ConverSationDetail_" + date.ToString("yyyyMM");
            string strSql = @"declare @sql nvarchar(1000)
                             if not exists (select 1 from sysobjects where type in ('U') and name=@TableName)
                             begin
                             set @sql='
                             CREATE TABLE '+@TableName+'(
                             	[RecID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
                             	[CSID] [int] NULL,
                             	[Sender] [tinyint] NULL,
                             	[Content] [varchar](500) NULL,
                             	[Type] [tinyint] NULL,
                             	[Status] [tinyint] NULL,
                             	[CreateTime] [datetime] NULL default(getdate())
                             	)'
                             execute(@sql)
                             end
                             select @TableName";
            SqlParameter[] parameters = {
					new SqlParameter("@TableName", SqlDbType.VarChar)};
            parameters[0].Value = tableName;
            return SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters).ToString();
        }


        /// <summary>
        ///  ���ݻỰID��ѯ��ϸ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetDetailByCSID(int CSID, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var CSInfo = Conversations.Instance.GetConversations(CSID);
            if (CSInfo != null)
            {
                string tableName = "ConverSationDetail_" + CSInfo.CreateTime.Value.ToString("yyyyMM");
                return GetConverSationDetail(new QueryConverSationDetail() { CSID = CSID }, order, currentPage, pageSize, out totalCount, tableName);
            }
            return null;
        }


        /// <summary>
        ///  ģ����ѯ��Ա��ʷ��¼
        /// </summary>
        /// <param name="date">���¸�ʽ��yyyyMM)</param>
        /// <param name="content">����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��</param>
        /// <param name="pageSize">ҳ��</param>
        /// <param name="totalCount">����</param>
        /// <returns></returns>
        public DataTable GetDetailByContent(DateTime date, string content, string order, int currentPage, int pageSize, out int totalCount)
        {
            string tableName = "ConverSationDetail_" + date.ToString("yyyyMM");
            return GetConverSationDetail(new QueryConverSationDetail() { Content = content }, order, currentPage, pageSize, out totalCount, tableName);
        }
        #endregion

        public object GetSourceTypeValue(string VisitID)
        {
            string strSql = "SELECT SourceType FROM dbo.UserVisitLog WHERE VisitID='" + VisitID + "'";
            return SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
    }
}

