using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类QS_Result。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_Result : DataBase
    {
        public static readonly QS_Result Instance = new QS_Result();
        private const string P_QS_RESULT_SELECT = "p_QS_Result_Select";
        private const string P_QS_RESULT_INSERT = "p_QS_Result_Insert";
        private const string P_QS_RESULT_UPDATE = "p_QS_Result_Update";
        private const string P_QS_RESULT_DELETE = "p_QS_Result_Delete";
        private const string P_QS_RESULT_SELECTAPPEALSTAT = "p_QS_Result_SelectAppealStat";
        private const string P_QS_RESULT_SELECTWORKLOADSTAT = "p_QS_Result_SelectWorkloadStat";
        private const string P_QS_RESULT_SELECTSCORESTAT = "p_QS_Result_SelectScoreStat";

        protected QS_Result()
        { }

        private string getWhereBySelect(Entities.QueryCallRecordInfo query)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("cob", "BGID", "CreateUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            #region 条件
            if (query.ANI != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.ANI like '%" + StringHelper.SqlFilter(query.ANI) + "%'";
            }
            //申诉起始时间和结束时间都不为空
            if (query.AppealBeginTime != Constant.STRING_INVALID_VALUE && query.AppealBeginTime != "" && query.AppealEndTime != Constant.STRING_INVALID_VALUE && query.AppealEndTime != "")
            {
                where += " and exists ( select RecID from QS_ApprovalHistory where   ApprovalType='30007' and QS_ApprovalHistory.QS_RID=q.QS_RID and CreateTime >='" + StringHelper.SqlFilter(query.AppealBeginTime) + " 0:0:0' and CreateTime <='" + StringHelper.SqlFilter(query.AppealEndTime) + " 23:59:59')";
            }
            //申诉起始时间不为空和结束时间为空
            if (query.AppealBeginTime != Constant.STRING_INVALID_VALUE && query.AppealBeginTime != "" && (query.AppealEndTime == Constant.STRING_INVALID_VALUE || query.AppealEndTime == ""))
            {
                where += " and exists ( select RecID from QS_ApprovalHistory where   ApprovalType='30007' and QS_ApprovalHistory.QS_RID=q.QS_RID and CreateTime >='" + StringHelper.SqlFilter(query.AppealBeginTime) + " 0:0:0')";
            }
            //申诉起始时间为空和结束时间不为空
            if ((query.AppealBeginTime == Constant.STRING_INVALID_VALUE || query.AppealBeginTime == "") && query.AppealEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and exists ( select RecID from QS_ApprovalHistory where   ApprovalType='30007' and QS_ApprovalHistory.QS_RID=q.QS_RID and CreateTime <='" + StringHelper.SqlFilter(query.AppealEndTime) + " 23:59:59')";
            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE)
            {
                //where += " and cob.CreateTime>='" + DateTime.Parse(query.BeginTime.ToString()).ToShortDateString() + " 0:0:0'";
                where += " and c.CreateTime>='" + DateTime.Parse(query.BeginTime.ToString()) + "'";
            }
            if (query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                //where += " and cob.CreateTime<='" + DateTime.Parse(query.EndTime.ToString()).ToShortDateString() + " 23:59:59'";
                where += " and c.CreateTime<='" + DateTime.Parse(query.EndTime.ToString()) + "'";
            }
            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " and cob.CallID=" + query.CallID;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE && query.SCID != -1)
            {
                where += " and cob.SCID=" + query.SCID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE && query.BGID != -1)
            {
                where += " and cob.BGID=" + query.BGID;
            }
            if (query.PhoneNum != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.PhoneNum like '%" + StringHelper.SqlFilter(query.PhoneNum) + "%'";
            }
            if (query.ScoreBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and q.CreateTime>='" + StringHelper.SqlFilter(query.ScoreBeginTime) + " 0:0:0'";
            }
            if (query.ScoreEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and q.CreateTime<='" + StringHelper.SqlFilter(query.ScoreEndTime) + " 23:59:59'";
            }
            if (query.QSResultStatus != Constant.STRING_INVALID_VALUE && CommonFunction.ObjectToInteger(query.QSResultStatus) > 0)
            {
                where += " and (q.Status in (" + Dal.Util.SqlFilterByInCondition(query.QSResultStatus) + ")";

                if (query.QSResultStatus.Contains("20001"))
                {
                    where += " or q.Status is null ";
                }

                where += ")";
            }
            if (query.BusinessID != Constant.STRING_INVALID_VALUE)
            {
                where += " and cob.BusinessID='" + StringHelper.SqlFilter(query.BusinessID) + "'";
            }
            if (query.CallType != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.CallStatus in (" + Dal.Util.SqlFilterByInCondition(query.CallType) + ")";
            }
            if (query.SpanTime1 != Constant.INT_INVALID_VALUE)
            {
                where += " and c.tallTime>=" + query.SpanTime1;
            }
            if (query.SpanTime2 != Constant.INT_INVALID_VALUE)
            {
                where += " and c.tallTime<=" + query.SpanTime2;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE && query.CreateUserID != -1)
            {
                where += " and cob.CreateUserID=" + query.CreateUserID;
            }
            if (query.ScoreCreater != Constant.INT_INVALID_VALUE && query.ScoreCreater != -1)
            {
                where += " and q.CreateUserID=" + query.ScoreCreater;
            }
            //申诉结果
            if (query.QSStateResult != Constant.STRING_INVALID_VALUE && query.QSStateResult != Constant.STRING_EMPTY_VALUE)
            {
                where += " and q.StateResult in (" + Dal.Util.SqlFilterByInCondition(query.QSStateResult) + ")";
            }
            //评分表
            if (query.ScoreTable != Constant.INT_INVALID_VALUE && query.ScoreTable != -1)
            {
                //将要用此评分表评分的
                where += " and ((QS_RulesRange.QS_RTID=" + query.ScoreTable + " And q.CallID is null)";
                //已经用此评分表评过分的
                where += " or (q.QS_RTID=" + query.ScoreTable + "))";
            }
            if (query.IsFilterNull != Constant.INT_INVALID_VALUE && query.IsFilterNull == 1)
            {
                where += " And (c.TallTime>0 and c.CallID>0 and cob.BusinessID is not null and cob.BusinessID <> '')";
            }
            //if (query.Qualified != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " and q.IsQualified in (" + Dal.Util.SqlFilterByInCondition(query.Qualified) + ")";
            //}
            if (query.QSResultScore != Constant.STRING_INVALID_VALUE)
            {
                where += " and isnull(ivr.Score,0) in (" + Dal.Util.SqlFilterByInCondition(query.QSResultScore) + ")";
            }
            //问题解决 第一级
            if (query.SelSolve != Constant.INT_INVALID_VALUE && query.SelSolve >= 0)
            {
                where += "  AND isnull(ivr.Score,0) like '" + query.SelSolve + "%'";
            }
            //满意度 第2级
            if (query.IVRScore != Constant.INT_INVALID_VALUE && query.IVRScore >= 0)
            {
                where += "  AND isnull(ivr.Score,0) like '[12]" + query.IVRScore + "'";
            }
            //话务坐席所属分组
            int CallAgentBGID = CommonFunction.ObjectToInteger(query.CallAgentBGID);
            if (CallAgentBGID > 0)
            {
                where += " AND cob.CreateUserID IN (SELECT UserID FROM dbo.EmployeeAgent WHERE BGID=" + CallAgentBGID + " )";
            }
            #endregion

            return where;
        }
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetQS_Result(QueryQS_Result query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RTID=" + query.QS_RTID;
            }
            if (query.QS_RID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RID=" + query.QS_RID;
            }
            if (query.CallReCordID != Constant.INT_INVALID_VALUE)
            {
                where += " And CallReCordID=" + query.CallReCordID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULT_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_Result GetQS_Result(int QS_RID)
        {
            QueryQS_Result query = new QueryQS_Result();
            query.QS_RID = QS_RID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_Result(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleQS_Result(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_Result GetQS_Result(long callid)
        {
            QueryQS_Result query = new QueryQS_Result();
            query.CallReCordID = callid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_Result(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleQS_Result(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.QS_Result LoadSingleQS_Result(DataRow row)
        {
            Entities.QS_Result model = new Entities.QS_Result();

            if (row["QS_RID"].ToString() != "")
            {
                model.QS_RID = int.Parse(row["QS_RID"].ToString());
            }
            if (row["CallReCordID"].ToString() != "")
            {
                model.CallReCordID = Int64.Parse(row["CallReCordID"].ToString());
            }
            if (row["QS_RTID"].ToString() != "")
            {
                model.QS_RTID = int.Parse(row["QS_RTID"].ToString());
            }
            model.SeatID = row["SeatID"].ToString();
            if (row["ScoreType"].ToString() != "")
            {
                model.ScoreType = int.Parse(row["ScoreType"].ToString());
            }
            if (row["Score"].ToString() != "")
            {
                model.Score = decimal.Parse(row["Score"].ToString());
            }
            if (row["IsQualified"].ToString() != "")
            {
                model.IsQualified = int.Parse(row["IsQualified"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["StateResult"].ToString() != "")
            {
                model.StateResult = int.Parse(row["StateResult"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            if (row["CallID"].ToString() != "")
            {
                model.CallID = Int64.Parse(row["CallID"].ToString());
            }
            model.QualityAppraisal = row["QualityAppraisal"].ToString();
            return model;
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.QS_Result model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@CallReCordID", SqlDbType.BigInt,8),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@SeatID", SqlDbType.VarChar,50),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@IsQualified", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@StateResult", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@QualityAppraisal", SqlDbType.VarChar,2000),new SqlParameter("@CallID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.QS_RID;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.QS_RTID;
            parameters[3].Value = model.SeatID;
            parameters[4].Value = model.ScoreType;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.IsQualified;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.StateResult;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.ModifyTime;
            parameters[12].Value = model.ModifyUserID;
            parameters[13].Value = model.QualityAppraisal;
            parameters[14].Value = model.CallID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULT_UPDATE, parameters);
        }
        public bool HasScored(string CallID)
        {
            string strSql = " SELECT COUNT(1) FROM QS_Result WHERE CallReCordID='" + StringHelper.SqlFilter(CallID) + "' AND CallID='" + StringHelper.SqlFilter(CallID) + "'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj != null)
            {
                int num = 0;
                int.TryParse(obj.ToString(), out num);
                return num > 0 ? true : false;

            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 合格型-导出质检标准信息
        /// </summary> 
        /// <param name="query">query</param>
        /// <returns>集合</returns>
        public DataTable GetStandardByQualifiedType(QueryQS_Result query)
        {
            string where = string.Empty;

            DataSet ds;
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And qsr.QS_RTID=" + query.QS_RTID;
            }

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar,1000)
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QS_Result_ExportStandardByQualifiedType", parameters);
            return ds.Tables[0];
        }
        public DataTable GetAnswerByQualifiedType_IM(string where)
        {
            string sql = "SELECT  a.* FROM   dbo.QS_ResultDetail a where 1=1 " + where;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        #region 分月查询
        ///  增加一条数据
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.QS_Result model, string tableEndName)
        {
            //复制话务数据到质检话务冗余表
            string sql1 = @"INSERT  INTO CallRecord_ORIG_QS
            ( [RecID] , [SessionID] , [CallID] , [ExtensionNum] , [PhoneNum] , [ANI] ,
              [CallStatus] , [SwitchINNum] , [OutBoundType] , [SkillGroup] , [InitiatedTime] , [RingingTime] ,
              [EstablishedTime] , [AgentReleaseTime] , [CustomerReleaseTime] , [AfterWorkBeginTime] , [AfterWorkTime] ,
              [ConsultTime] , [ReconnectCall] , [TallTime] , [AudioURL] , [CreateTime] , [CreateUserID] ,
              [TransferInTime] , [TransferOutTime]
			)
            SELECT [RecID] ,[SessionID] ,[CallID] ,[ExtensionNum] ,[PhoneNum] ,[ANI] ,
                    [CallStatus] ,[SwitchINNum] ,[OutBoundType] ,[SkillGroup] ,[InitiatedTime] ,[RingingTime] ,
                    [EstablishedTime] ,[AgentReleaseTime] ,[CustomerReleaseTime] ,[AfterWorkBeginTime] , [AfterWorkTime] ,
                    [ConsultTime] ,[ReconnectCall] ,[TallTime] ,[AudioURL] , [CreateTime] ,[CreateUserID] ,
                    [TransferInTime] ,[TransferOutTime]
            FROM CallRecord_ORIG" + tableEndName + @"  
            WHERE   CallID = '" + model.CallReCordID + @"'
            AND [RecID] NOT IN (SELECT RecID FROM CallRecord_ORIG_QS)";

            string sql2 = @" INSERT  INTO CallRecord_ORIG_Business_QS
            ( [RecID] ,[CallID] ,[BGID] ,[SCID] ,[BusinessID] ,[CreateUserID] ,[CreateTime] ,[BFTaskID])
            SELECT  [RecID] ,[CallID] ,[BGID] ,[SCID] ,[BusinessID] ,[CreateUserID] ,[CreateTime] ,[BFTaskID]
            FROM    CallRecord_ORIG_Business" + tableEndName + @"  
            WHERE   CallID = '" + model.CallReCordID + @"'
            AND [RecID] NOT IN (SELECT RecID FROM CallRecord_ORIG_Business_QS)";

            string sql3 = @"INSERT  INTO IVRSatisfaction_QS
            ([Oid] ,[CallID] ,[CallRecordID] ,[Score] ,[CreateTime])
            SELECT [Oid] ,[CallID] ,[CallRecordID] ,[Score] ,[CreateTime]
            FROM IVRSatisfaction" + tableEndName + @"  
            WHERE   CallID = '" + model.CallReCordID + @"'
            AND [Oid] NOT IN (SELECT Oid FROM IVRSatisfaction_QS)";

            int j = 0;
            j += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql1);
            j += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql2);
            j += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql3);

            SqlParameter[] parameters = {
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@CallReCordID", SqlDbType.BigInt,8),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@SeatID", SqlDbType.VarChar,50),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@IsQualified", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@StateResult", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@QualityAppraisal", SqlDbType.VarChar,2000),
                    new SqlParameter("@CallID", SqlDbType.BigInt,8),
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallReCordID;
            parameters[2].Value = model.QS_RTID;
            parameters[3].Value = model.SeatID;
            parameters[4].Value = model.ScoreType;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.IsQualified;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.StateResult;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.ModifyTime;
            parameters[12].Value = model.ModifyUserID;
            parameters[13].Value = model.QualityAppraisal;
            parameters[14].Value = model.CallID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULT_INSERT, parameters);
            int id = (int)parameters[0].Value;
            return id;
        }

        /// 查询录音质检数据
        /// <summary>
        /// 查询录音质检数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultList(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount, log4net.ILog log)
        {
            string where = string.Empty;
            where = getWhereBySelect(query);
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
                    new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            log.Info(string.Format("【调用脚本P_QS_RESULT_SELECTLIST】,开始：条件：{0}", where));

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_QS_RESULT_SELECTLIST", parameters);
            sw.Stop();
            totalCount = (int)(parameters[5].Value);
            log.Info(string.Format("【调用脚本P_QS_RESULT_SELECTLIST】,耗时：{2}毫秒，条件：{0}，查询总数：{1}", where, totalCount, sw.ElapsedMilliseconds));
            return ds.Tables[0];
        }
        /// 成绩统计查询
        /// <summary>
        /// 成绩统计查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultScoreStat(string where, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULT_SELECTSCORESTAT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// 工作量统计查询
        /// <summary>
        /// 工作量统计查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultWorkloadStat(string where, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULT_SELECTWORKLOADSTAT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// 合格型-导出答案信息
        /// <summary>
        /// 合格型-导出答案信息
        /// </summary>
        /// <param name="query">查询条件</param> 
        /// <returns></returns>
        public DataTable GetAnswerByQualifiedType(QueryQS_ResultDetail query, string tableEndName)
        {
            string where = string.Empty;
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And qsrd.QS_RTID=" + query.QS_RTID;
            }
            if (query.ScoreType != Constant.INT_INVALID_VALUE)
            {
                where += " And qsrd.ScoreType=" + query.ScoreType;
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And qsrd.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:0:0'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And qsrd.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE && query.CreateUserID != -1)
            {
                where += " And qsrd.CreateUserID=" + query.CreateUserID;
            }
            if (query.CallBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And cob.CreateTime>='" + StringHelper.SqlFilter(query.CallBeginTime) + "'";
            }
            if (query.CallEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And cob.CreateTime<='" + StringHelper.SqlFilter(query.CallEndTime) + "'";
            }
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QS_Result_ExportAnswerByDetails", parameters);
            return ds.Tables[0];
        }
        /// 合格型-导出基本信息
        /// <summary>
        /// 合格型-导出基本信息
        /// </summary>
        /// <param name="query">查询条件</param> 
        /// <returns></returns>
        public DataTable GetBaseInfoByQualifiedType(QueryCallRecordInfo query, string tableEndName)
        {
            string where = string.Empty;

            where = getWhereBySelect(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QS_Result_ExportBaseInfoByQualifiedType", parameters);
            return ds.Tables[0];
        }
        /// 评分型-成绩主表导出
        /// <summary>
        /// 评分型-成绩主表导出
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable getResultByExport(Entities.QueryCallRecordInfo query, string tableEndName)
        {
            DataSet ds = null;
            string where = string.Empty;
            where = getWhereBySelect(query);

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20)
					};
            parameters[0].Value = where;
            parameters[1].Value = tableEndName;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QS_Result_selectByExport", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            { return null; }
        }
        /// 申诉结果统计
        /// <summary>
        /// 申诉结果统计
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultAppealStat(string where, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULT_SELECTAPPEALSTAT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

       
        /// <summary>
///   部门成绩明细热线数据
/// </summary>
/// <param name="whereBGID"></param>
/// <param name="whereTime1"></param>
/// <param name="whereTime2"></param>
/// <param name="currentPage"></param>
/// <param name="pageSize"></param>
/// <param name="searchTable"></param>
/// <param name="totalCount"></param>
/// <param name="userid"></param>
/// <returns></returns>
        public DataTable GetQS_ResultGradeStatisticsHotLine(string whereBGID, string whereTime1, string whereTime2, int currentPage, int pageSize, string searchTable, out int totalCount, SqlConnection conn)
        {
            DataSet ds;
            string proc = "p_GradeStatisticsHotLine";

            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("YichePartID");
            string whereAgent = "  ";
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                whereAgent += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        whereAgent += " or ";
                    }
                    whereAgent += " urf.DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                whereAgent += " )";
            }


          
                try
                {                 
                    SqlParameter[] parameters = {
					new SqlParameter("@whereBGID", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@SearchTable", SqlDbType.NVarChar, 50),
                    new SqlParameter("@whereTime1", SqlDbType.NVarChar, 4000),
					new SqlParameter("@whereTime2", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@whereAgent", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

                    parameters[0].Value = whereBGID;
                    parameters[1].Value = searchTable;
                    parameters[2].Value = whereTime1; 
                    parameters[3].Value = whereTime2;
                    parameters[4].Value = whereAgent;
                    parameters[5].Value = "  T_Talk DESC ";
                    parameters[6].Value = pageSize;
                    parameters[7].Value = currentPage;
                    parameters[8].Direction = ParameterDirection.Output;

                    ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, proc, parameters);
                    totalCount = (int)(parameters[8].Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
              
      
            return ds.Tables[0];
        }

       /// <summary>
        /// 部门成绩在线明细
       /// </summary>
       /// <param name="whereOut"></param>
       /// <param name="order"></param>
       /// <param name="currentPage"></param>
       /// <param name="pageSize"></param>
       /// <param name="tableEndName"></param>
       /// <param name="totalCount"></param>
       /// <param name="userid"></param>
       /// <returns></returns>
        public DataTable GetQS_ResultGradeStatisticsOnLine(string whereOut, string order, int currentPage, int pageSize, string tableEndName, out int totalCount, int userid)
        {
            DataSet ds;
            string proc = "p_GradeStatisticsOnLine";

            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("YichePartID");
           string  whereIn = "SELECT em.UserID,em.BGID FROM EmployeeAgent em  INNER JOIN SysRightsManager.dbo.UserInfo urf   ON urf.UserID=em.UserID AND urf.Status=0  AND em.BGID IS NOT NULL  ";
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                whereIn += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        whereIn += " or ";
                    }
                    whereIn += " DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                whereIn += " )";
            }        
                order = " BGID DESC ";
                proc = "p_GradeStatisticsOnLine";
                whereOut += " And BGID in (SELECT BGID FROM UserGroupDataRigth WHERE USERID = " + userid + ") ";//数据权限


            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRINGS))
            {

                try
                {
                    SqlParameter[] parameters = {
					new SqlParameter("@whereOut", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@whereIn", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

                    parameters[0].Value = whereOut;
                    parameters[1].Value = whereIn;
                    parameters[2].Value = tableEndName;
                    parameters[3].Value = order;
                    parameters[4].Value = pageSize;
                    parameters[5].Value = currentPage;
                    parameters[6].Direction = ParameterDirection.Output;

                    ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, proc, parameters);
                    totalCount = (int)(parameters[6].Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return ds.Tables[0];
        }
        /// 抽检频次统计方法
        /// <summary>
        /// 抽检频次统计方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultFrequenyStatistics(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            string where = string.Empty;

            where = getWhereBySelect(query);

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@whereOut", SqlDbType.NVarChar, 4000),
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
                                        };
            parameters[0].Value = query.QSScoreCreaters;
            parameters[1].Value = where;
            parameters[2].Value = tableEndName;
            parameters[3].Value = order;
            parameters[4].Value = pageSize;
            parameters[5].Value = currentPage;
            parameters[6].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_FrequencyStatistics", parameters);
            totalCount = (int)(parameters[6].Value);
            return ds.Tables[0];
        }
        #endregion
        /// <summary>
        /// 根据评分表主键获取评分表类型
        /// </summary>
        /// <param name="qs_Rid"></param>
        /// <returns></returns>
        public string GetScoreTypeByRID(string qs_Rid)
        {
            string strSql = " SELECT TOP 1 ScoreType FROM dbo.QS_Result WHERE QS_RID= '" + StringHelper.SqlFilter(qs_Rid)  + "'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? "" : obj.ToString();
        }

        /// <summary>
        /// 根据RTID和CallId获取RTID
        /// </summary>
        /// <param name="qsRtid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetRidByCallidAndRtid(int qsRtid, Int64 callid)
        {
            string sql = @"SELECT QS_RID FROM dbo.QS_Result WHERE CallID=" + callid + " AND QS_RTID=" + qsRtid;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return CommonFunction.ObjectToInteger(obj,0);
        }
    }
}

