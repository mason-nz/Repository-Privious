using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;
namespace BitAuto.ISDC.CC2012.Dal
{
    public class Personalization : DataBase
    {
        #region Instance
        public static readonly Personalization Instance = new Personalization();
        #endregion

        #region const
        private const string P_PersonalCollection_Select_Knowledge = "p_PersonalCollection_Select_Knowledge";
        private const string P_PersonalCollection_Select_FAQ = "p_PersonalCollection_Select_FAQ";
        private const string P_PersonalCollection_Delete = "p_PersonalCollection_Delete";
        private const string P_PersonalCollection_Insert = "p_PersonalCollection_Insert";

        private const string P_KLRaiseQuestions_Insert = "p_KLRaiseQuestions_Insert";
        private const string P_KLRaiseQuestions_Select_ForCollection = "p_KLRaiseQuestions_Select_ForCollection";
        private const string P_KLRaiseQuestions_Select = "p_KLRaiseQuestions_Select";
        private const string P_KLRaiseQuestions_Update_Tra = "p_KLRaiseQuestions_Update_Tra";
        private const string P_KLRaiseQuestions_Delete_Tra = "p_KLRaiseQuestions_Delete_Tra";

        private const string P_KLRaiseQuestionsLog_Select = "p_KLRaiseQuestionsLog_Select";
 
        #endregion


        #region Contructor
        protected Personalization()
        { }
        #endregion


        #region  个人收藏
        /// <summary>
        /// 查询收藏的知识点信息
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCollectedKnowledgeData(QueryKLFavorites query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where += getCommonWhere(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PersonalCollection_Select_Knowledge, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 查询收藏的FAQ信息
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCollectedFAQData(QueryKLFavorites query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where += getCommonWhere(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PersonalCollection_Select_FAQ, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public string getCommonWhere(QueryKLFavorites query)
        {
            string where = string.Empty;

            if (query.Id != Constant.INT_INVALID_VALUE)
            {
                where += " and Id=" + query.Id;
            }
            if (query.UserId != Constant.INT_INVALID_VALUE)
            {
                where += " and UserId=" + query.UserId;
            }
            if (query.KLRefId != Constant.INT_INVALID_VALUE)
            {
                where += " and KLRefId=" + query.KLRefId;
            }
            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " and Type=" + query.Type;
            }
            if (query.CreateTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and CONVERT(VARCHAR(10),CreateTime,120) ='" + query.CreateTime.ToString("yyyy-MM-dd") + "'";
            }
            return where;
        }


        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.QueryKLFavorites model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int),
					new SqlParameter("@UserId", SqlDbType.Int),
					new SqlParameter("@KLRefId", SqlDbType.Int),
					new SqlParameter("@Type", SqlDbType.Int)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.KLRefId;
            parameters[3].Value = model.Type;            

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PersonalCollection_Insert, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        /// 判断此本人是否已经收藏了该条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true：已经收藏；false：未收藏</returns>
        public bool IsCollected(Entities.QueryKLFavorites model)
        {
            string sqlStr = "SELECT COUNT(*) FROM dbo.KLFavorites WHERE UserId=@UserId AND KLRefId=@KLRefId AND Type=@Type";

            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int),
					new SqlParameter("@KLRefId", SqlDbType.Int),
					new SqlParameter("@Type", SqlDbType.Int),
            };
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.KLRefId;
            parameters[2].Value = model.Type;
            string objValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters).ToString();
            if (int.Parse(objValue) == 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 删除一条收藏数据，同事修改总收藏量
        /// </summary>
        ///<param name="Id">收藏数据ID</param>
        /// <returns>0:操作执行失败；1：操作执行成功</returns>
        public int Delete(int Id)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PersonalCollection_Delete, parameters);
             
        }

        #endregion

        #region  问题解答
        public DataTable GetKLRaiseQuestionModelDataById(int id)
        {
            string strSql = "SELECT * FROM KLRaiseQuestions where id='" + id + "'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 增加一条问题数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回值大于零表示执行成功</returns>
        public int InsertKLRaiseQuestion(Entities.KLRaiseQuestions model)
        {
            SqlParameter[] parameters = {
                  new SqlParameter("@Id", SqlDbType.Int),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@CONTENT", SqlDbType.NVarChar,4000),
					new SqlParameter("@KLCId", SqlDbType.Int,4),
					new SqlParameter("@KLRefId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@AnswerUser", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyDate", SqlDbType.DateTime),
					new SqlParameter("@LastModifyBy", SqlDbType.Int,4)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CreateUserId;
            parameters[2].Value = model.CreateDate;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.CONTENT;
            parameters[5].Value = model.KLCId;
            parameters[6].Value = model.KLRefId;
            parameters[7].Value = model.Type;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.AnswerUser;
            parameters[10].Value = model.BGID;
            parameters[11].Value = model.LastModifyDate;
            parameters[12].Value = model.LastModifyBy;



            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLRaiseQuestions_Insert, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        /// 修改一条问题数据，同时添加日志数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回值 1：执行成功；0：执行失败</returns>
        public int UpdateKLRaiseQuestion(Entities.KLRaiseQuestions model)
        {
            SqlParameter[] parameters = {
                                             new SqlParameter("@backDate", SqlDbType.Int),
                  new SqlParameter("@Id", SqlDbType.Int),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@CONTENT", SqlDbType.NVarChar,4000),
					new SqlParameter("@KLCId", SqlDbType.Int,4),
					new SqlParameter("@KLRefId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@AnswerUser", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyDate", SqlDbType.DateTime),
					new SqlParameter("@LastModifyBy", SqlDbType.Int,4)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Id;
            parameters[2].Value = model.CreateUserId;
            parameters[3].Value = model.CreateDate;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.CONTENT;
            parameters[6].Value = model.KLCId;
            parameters[7].Value = model.KLRefId;
            parameters[8].Value = model.Type;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.AnswerUser;
            parameters[11].Value = model.BGID;
            parameters[12].Value = model.LastModifyDate;
            parameters[13].Value = model.LastModifyBy;
 
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure,P_KLRaiseQuestions_Update_Tra, parameters);
        }
        /// <summary>
        /// 删除一条问题数据，同时删除日志数据
        /// </summary>
        /// <param name="Id">问题ID</param>
        /// <returns>返回值 1：执行成功；0：执行失败</returns>
        public int DeleteKLRaiseQuestionById(int Id)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLRaiseQuestions_Delete_Tra, parameters);
        }
        /// <summary>
        /// 通过条件查询收藏的KLRaiseQuestion数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKLRaiseQuestionData(KLRaiseQuestions query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where += GetKLRaiseQuestionWhere(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLRaiseQuestions_Select_ForCollection, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 通过条件查询提问的KLRaiseQuestion数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKLRaiseQuestionDataForManage(QueryKLRaiseQuestions query, string QuestionedUserName, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            string where = string.Empty;

            where += GetQueryKLRaiseQuestionWhere(query);

            where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("KLRaiseQuestions", "BGID", "CreateUserID", userid);
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@questionedUserName", SqlDbType.NVarChar, 200),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = QuestionedUserName;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLRaiseQuestions_Select, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

        public string GetKLRaiseQuestionWhere(KLRaiseQuestions query)
        {
            string where = string.Empty;

            if (query.Id != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.Id=" + query.Id;
            }
            if (query.CreateUserId != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.CreateUserId=" + query.CreateUserId;
            }
            if (query.CreateDate != Constant.DATE_INVALID_VALUE)
            {
                where += " and klr.CreateDate='" + query.CreateDate + "'";
            }
            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " and klr.Title='" + StringHelper.SqlFilter(query.Title) + "'";
            }
            if (query.CONTENT != Constant.STRING_INVALID_VALUE)
            {
                where += " and klr.CONTENT like '%" + StringHelper.SqlFilter(query.CONTENT) + "%'";
            }
            if (query.KLCId != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.KLCId=" + query.KLCId;
            }
            if (query.KLRefId != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.KLRefId=" + query.KLRefId;
            }
            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.Type=" + query.Type;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.Status=" + query.Status;
            }
            if (query.AnswerUser != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.AnswerUser=" + query.AnswerUser;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.BGID=" + query.BGID;
            }
            if (query.LastModifyDate != Constant.DATE_INVALID_VALUE)
            {
                where += " and CONVERT(VARCHAR(10),klr.LastModifyDate,120) ='" + query.LastModifyDate.ToString("yyyy-MM-dd") + "'";
            }
            if (query.LastModifyBy != Constant.INT_INVALID_VALUE)
            {
                where += " and klr.LastModifyBy=" + query.LastModifyBy;
            }
            return where;
        }
        public string GetQueryKLRaiseQuestionWhere(QueryKLRaiseQuestions query)
        {
            string where = string.Empty;

            if (query.Id != Constant.INT_INVALID_VALUE)
            {
                where += " and Id=" + query.Id;
            }
            if (query.CreateUserId != Constant.INT_INVALID_VALUE)
            {
                where += " and CreateUserId=" + query.CreateUserId;
            }
            if (query.CreateDate != Constant.DATE_INVALID_VALUE)
            {
                where += " And CreateDate<='" + query.CreateDate.ToString("yyyy-MM-dd") + " 23:59:59' And CreateDate>='" + query.CreateDate.ToString("yyyy-MM-dd") + " 00:00:00' ";
            }
            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " and Title like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (query.CONTENT != Constant.STRING_INVALID_VALUE)
            {
                where += " and CONTENT like '%" + StringHelper.SqlFilter(query.CONTENT) + "%'";
            }
            if (query.KLCId != Constant.INT_INVALID_VALUE && query.RegionID!=Constant.INT_INVALID_VALUE)
            {
                where += " and KLCId IN (SELECT KCID FROM dbo.TF_GetKnowledgeCategory(" + query.KLCId + ") )";
            }
            if (query.KLRefId != Constant.INT_INVALID_VALUE)
            {
                where += " and KLRefId=" + query.KLRefId;
            }
            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " and Type=" + query.Type;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Status=" + query.Status;
            }
            if (query.AnswerUser != Constant.INT_INVALID_VALUE)
            {
                where += " and AnswerUser=" + query.AnswerUser;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and BGID=" + query.BGID;
            }
            if (query.LastModifyDate != Constant.DATE_INVALID_VALUE)
            {
                where += " And LastModifyDate<='" + query.CreateDate.ToString("yyyy-MM-dd") + " 23:59:59' And LastModifyDate>='" + query.CreateDate.ToString("yyyy-MM-dd") + " 00:00:00' ";
           
            }
            if (query.LastModifyBy != Constant.INT_INVALID_VALUE)
            {
                where += " and LastModifyBy=" + query.LastModifyBy;
            }
            if (query.CreateUserName != Constant.STRING_INVALID_VALUE)
            {
                where += " and CONTENT like '%" + StringHelper.SqlFilter(query.CreateUserName) + "%'";
            }
            if (query.QueryStatuses != Constant.STRING_INVALID_VALUE)
            {
                where += " and Status in(" + Dal.Util.SqlFilterByInCondition(query.QueryStatuses) + ")";
            }

            if (query.CreateBeginTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And CreateDate >='" + query.CreateBeginTime.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (query.CreateEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And CreateDate<='" + query.CreateEndTime.ToString("yyyy-MM-dd") + " 23:59:59'";
            }
            if (query.AnswerBeginTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And LastModifyDate >='" + query.AnswerBeginTime.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (query.AnswerEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And LastModifyDate<='" + query.AnswerEndTime.ToString("yyyy-MM-dd") + " 23:59:59'";
            }
        
           
            //if (query.QueryBGIDs != Constant.STRING_INVALID_VALUE && query.QueryBGIDs != "")
            //{
            //    where += " And BGID in(" + Dal.Util.SqlFilterByInCondition(query.QueryBGIDs) + ")";
            //}
            return where;
        }
        /// <summary>
        /// 获取解答人信息列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnswerUserListData()
        {
            string strSql = "SELECT DISTINCT a.AnswerUser,b.TrueName FROM KLRaiseQuestions AS a INNER JOIN SysRightsManager.dbo.UserInfo AS b ON a.AnswerUser=b.UserID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS,CommandType.Text,strSql).Tables[0];
        }
        /// <summary>
        /// 根据提问id获取提问明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetQuestionDetailsById(string id)
        {
            string strSql = @"SELECT Id,Title,(SELECT Name FROM dbo.KnowledgeCategory WHERE KCID=KLCId) AS KCName,CONTENT,
                (SELECT TOP 1 Comments FROM KLRaiseQuestionsLog WHERE KLRQId = KLRaiseQuestions.id AND Action=2 ORDER BY KLRaiseQuestionsLog.CreateDate DESC) AS AnswerContent
                FROM dbo.KLRaiseQuestions 
                WHERE Id='" + StringHelper.SqlFilter(id) +"'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 更具提问id 获取提问操作明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetQuestionOperationLogById(string id)
        {
            string strSql = @" SELECT b.TrueName,CASE WHEN Action=1 THEN '提问' WHEN Action=2 THEN '解答' ELSE '' END AS ACTION
                 ,CASE WHEN a.Status=0 THEN '待解答' WHEN a.Status=1 THEN '已解答' ELSE '' END AS STATUS,
                 CASE WHEN a.CreateDate IS NULL THEN '' WHEN CONVERT(VARCHAR(10),a.CreateDate,120) = '1900-01-01' THEN '' ELSE CONVERT(VARCHAR(19),a.CreateDate,120) END AS CreateDate,
                 a.Comments
                 FROM dbo.KLRaiseQuestionsLog AS a LEFT JOIN SysRightsManager.dbo.UserInfo AS b ON a.CreateUserId=b.UserID 
                 WHERE a.KLRQId='" + StringHelper.SqlFilter(id) + @"' ORDER BY a.CreateDate DESC";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 通过KLRQId查询KLRaiseQuestionLog数据
        /// </summary>
        /// <param name="KLRQId"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKLRaiseQuestionLogDataByKLRQId(int KLRQId, string order, int currentPage, int pageSize, out int totalCount)
        {
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
		    };
            parameters[0].Value =  "and KLRQId=" + KLRQId;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLRaiseQuestionsLog_Select, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        
        #endregion


    }
}
