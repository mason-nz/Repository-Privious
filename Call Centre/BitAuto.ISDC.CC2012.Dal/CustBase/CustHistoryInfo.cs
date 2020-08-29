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
    /// 数据访问类CustHistoryInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustHistoryInfo : DataBase
    {
        public static readonly CustHistoryInfo Instance = new CustHistoryInfo();

        private const string P_CUSTHISTORYINFO_SELECT = "p_CustHistoryInfo_Select2";
        private const string P_CUSTHISTORYINFO_INSERT = "p_CustHistoryInfo_Insert";
        private const string P_CUSTHISTORYINFO_UPDATE = "p_CustHistoryInfo_Update";
        private const string P_CUSTHISTORYINFO_DELETE = "p_CustHistoryInfo_Delete";
        private const string P_CUSTHISTORYINFO_EXPORT = "p_CustHistoryInfo_Export";

        protected CustHistoryInfo()
        { }

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
        public DataTable GetCustHistoryInfo(QueryCustHistoryInfo query, string order, int currentPage, int pageSize, string fields, out int totalCount)
        {
            string where = GetWhereStr(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 40000),
					new SqlParameter("@order", SqlDbType.VarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
	                new SqlParameter("@fields", SqlDbType.VarChar, 8000),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 10)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Value = fields;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYINFO_SELECT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 导出用
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public DataTable GetCustHistoryInfoForExport(QueryCustHistoryInfo query, string fields)
        {
            string where = GetWhereStr(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 40000),
	                new SqlParameter("@fields", SqlDbType.VarChar, 8000)
					};
            parameters[0].Value = where;
            parameters[1].Value = fields;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYINFO_EXPORT, parameters);

            return ds.Tables[0];
        }
        /// <summary>
        /// 获取查询条件语句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string GetWhereStr(QueryCustHistoryInfo query)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND chi.RecID=" + query.RecID;
            }
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND chi.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.CallRecordID != Constant.INT_INVALID_VALUE)
            {
                where += " And chi.CallRecordID=" + query.CallRecordID.ToString();
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND chi.CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND chi.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:00'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND chi.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            //客户姓名
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND cbi.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            //问题性质 （枚举）
            if (query.QuestionQualityStr != Constant.STRING_INVALID_VALUE)
            {
                where += " AND chi.QuestionQuality in (" + Dal.Util.SqlFilterByInCondition(query.QuestionQualityStr.ToString()) + ")";
            }
            if (query.IsCompaintStr != Constant.STRING_INVALID_VALUE && query.IsCompaintStr != "")
            {
                where += " AND chi.IsComplaint in (" + Dal.Util.SqlFilterByInCondition(query.IsCompaintStr.ToString()) + ")";
            }
            //是否转发 刘学文 10.24
            if (query.IsForwardingStr != Constant.STRING_INVALID_VALUE && query.IsForwardingStr != "")
            {
                if (query.IsForwardingStr.Contains("0") && !query.IsForwardingStr.Contains("1"))
                {
                    where += " AND chi.TaskID not in (Select top 1 TaskID from CustHistoryTemplateMapping AS ctm Where ctm.TaskID=chi.TaskID)";
                }
                else if (!query.IsForwardingStr.Contains("0") && query.IsForwardingStr.Contains("1"))
                {
                    where += " AND chi.TaskID in (Select top 1 TaskID from CustHistoryTemplateMapping AS ctm Where ctm.TaskID=chi.TaskID)";
                }
            }

            //任务状态 （枚举）
            if (query.ProcessStatusStr != null && query.ProcessStatusStr != "")
            {
                if (query.ProcessStatusStr.Contains(((int)EnumTaskStatus.TaskStatusNow).ToString()))
                {
                    //是否包含“处理中”
                    if (query.Status != Constant.STRING_INVALID_VALUE)
                    {
                        string[] array_Status = Util.SqlFilterByInCondition(query.Status).Split(',');
                        string strStatus = string.Empty;
                        where += " AND chi.TaskID in (SELECT a.TaskID FROM (SELECT TOP 1 CustHistoryLog.TaskID,Status FROM CustHistoryLog WHERE CustHistoryLog.TaskID=chi.TaskID ORDER BY SolveTime DESC) a WHERE 1=1 ";
                        for (int i = 0; i < array_Status.Length; i++)
                        {
                            if (i == 0)
                            {
                                strStatus += " AND (";
                            }
                            else
                            {
                                strStatus += " OR ";
                            }
                            strStatus += " a.Status =" + array_Status[i];
                            if (i == array_Status.Length - 1)
                            {
                                strStatus += ")";
                            }
                        }
                        where += strStatus;
                        where += ")";
                    }
                }

                where += " AND chi.ProcessStatus in (" + Dal.Util.SqlFilterByInCondition(query.ProcessStatusStr.ToString()) + ")";
            }
            int judgeType = 0;    //判断咨询类型表的问题类别类型，1-int整型；2-string 字符型
            //咨询类型 （枚举）
            if (query.ConsultID != Constant.INT_INVALID_VALUE && query.ConsultID != 0 && query.ConsultID != null)
            {
                string ConsultTableName = string.Empty;
                switch (query.ConsultID)
                {
                    case (int)ConsultType.NewCar: ConsultTableName = "ConsultNewCar";
                        break;
                    case (int)ConsultType.SecondCar: ConsultTableName = "ConsultSecondCar";
                        judgeType = 1;
                        break;
                    case (int)ConsultType.PFeedback: ConsultTableName = "ConsultPFeedback";
                        judgeType = 2;
                        break;
                    //case (int)ConsultType.Activity: ConsultTableName = "ConsultActivity";
                    //    judgeType = 1;
                    //    break;
                    case (int)ConsultType.POther: ConsultTableName = "ConsultPOther";
                        break;
                    case (int)ConsultType.PUseCar: ConsultTableName = "ConsultPUseCar";
                        judgeType = 2;
                        break;
                    case (int)ConsultType.DCoop: ConsultTableName = "ConsultDCoop";
                        judgeType = 1;
                        //modify by qizq 2012-12-20
                        //query.ConsultDCoopType = 1;
                        break;
                    case (int)ConsultType.DCoopFeedback: ConsultTableName = "ConsultDFeedback";
                        judgeType = 1;
                        //modify by qizq 2012-12-20
                        //query.ConsultDCoopType = 2;
                        break;
                    case (int)ConsultType.DCoopOther: ConsultTableName = "ConsultDOther";
                        judgeType = 1;
                        //modify by qizq 2012-12-20
                        //query.ConsultDCoopType = 3;
                        break;
                }
                where += " AND chi.ConsultID=" + query.ConsultID.ToString() + " AND chi.ConsultDataID in (SELECT con.RecID FROM " + ConsultTableName + " AS con WHERE con.RecID=chi.ConsultDataID";
                //*modify by qizq 2012-12-20
                //如果是经销商合作表，则还要根据其Type类型字段作为条件查询
                //if (ConsultTableName == "ConsultDCoop" && query.ConsultDCoopType != Constant.INT_INVALID_VALUE)
                //{
                //    where += " AND con.Type in (" + Dal.Util.SqlFilterByInCondition(query.ConsultDCoopType.ToString()) + ")";
                //}
                //*
                //如果存在问题类别，加上此条件
                if (query.QuestionType != Constant.STRING_INVALID_VALUE && query.QuestionType != "")
                {
                    //1-int整型；2-string 字符型
                    //if (judgeType == 1)
                    //{
                    //    string[] array_type = query.QuestionType.Split(',');
                    //    string strType = string.Empty;
                    //    where += " AND con.QuestionType in (" + Dal.Util.SqlFilterByInCondition(query.QuestionType) + ")";
                    //}
                    //else if (judgeType == 2)
                    //{
                    string[] array_type = Util.SqlFilterByInCondition(query.QuestionType).Split(',');
                    string strType = string.Empty;
                    for (int i = 0; i < array_type.Length; i++)
                    {
                        if (i == 0)
                        {
                            strType += " AND (";
                        }
                        else
                        {
                            strType += " OR ";
                        }
                        strType += " con.QuestionType LIKE '%" + array_type[i] + "%'";
                        if (i == array_type.Length - 1)
                        {
                            strType += ")";
                        }
                    }
                    where += strType;

                }
                where += ")";
            }
            else
            {
                if (query.ConsultID == 0)
                {
                    where += " AND ConsultID!=60010 AND ConsultID!=60011 AND ConsultID!=-2";
                }
            }
            //当登陆者为本人权限时，加上条件，查询的结果是本人处理的记录
            if (query.RightStr != Constant.STRING_INVALID_VALUE)
            {
                where += " AND chi.TaskID in (SELECT tcs.TaskID FROM TaskCurrentSolveUser AS tcs WHERE tcs.TaskID=chi.TaskID AND tcs.CurrentSolveUserEID=" + query.RightStr + ")";
            }
            return where;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustHistoryInfo GetCustHistoryInfo(long RecID)
        {
            QueryCustHistoryInfo query = new QueryCustHistoryInfo();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustHistoryInfo(query, string.Empty, 1, 1, Entities.CustHistoryInfo.SelectFieldStr, out count);
            if (count > 0)
            {
                return LoadSingleCustHistoryInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据录音ID，查询客户历史记录
        /// </summary>
        public Entities.CustHistoryInfo GetCustHistoryInfoByCallRecordID(Int64 callRecordID)
        {
            QueryCustHistoryInfo query = new QueryCustHistoryInfo();
            query.CallRecordID = callRecordID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustHistoryInfo(query, string.Empty, 1, 1, Entities.CustHistoryInfo.SelectFieldStr, out count);
            if (count > 0)
            {
                return LoadSingleCustHistoryInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据任务ID查询历史信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Entities.CustHistoryInfo GetCustHistoryInfo(string taskId)
        {
            string sqlStr = " SELECT * FROM CustHistoryInfo WHERE TaskID=@TaskID";
            SqlParameter parameter = new SqlParameter("@TaskID", taskId);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCustHistoryInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据TaskID、BusinessType查找联系记录
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="businessType">业务类型：1工单，2团购订单，3客户核实，4其他任务</param>
        /// <returns></returns>
        public Entities.CustHistoryInfo GetCustHistoryInfo(string taskID, string custID, int businessType)
        {
            string sqlStr = "SELECT * FROM CustHistoryInfo WHERE CustID=@CustID and TaskID=@TaskID and BusinessType=@BusinessType"; SqlParameter[] parameters ={
                                           new SqlParameter("@TaskID",SqlDbType.VarChar,20),
                                           new SqlParameter("@CustID",SqlDbType.VarChar,20),
                                           new SqlParameter("@BusinessType",SqlDbType.Int)
                                      };
            parameters[0].Value = taskID;
            parameters[1].Value = custID;
            parameters[2].Value = businessType;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCustHistoryInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public Entities.CustHistoryInfo LoadSingleCustHistoryInfo(DataRow row)
        {
            Entities.CustHistoryInfo model = new Entities.CustHistoryInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.TaskID = row["TaskID"].ToString();
            if (row["CallRecordID"].ToString() != "")
            {
                model.CallRecordID = long.Parse(row["CallRecordID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            if (row["RecordType"].ToString() != "")
            {
                model.RecordType = int.Parse(row["RecordType"].ToString());
            }
            if (row["ConsultID"].ToString() != "")
            {
                model.ConsultID = int.Parse(row["ConsultID"].ToString());
            }
            if (row["ConsultDataID"].ToString() != "")
            {
                model.ConsultDataID = int.Parse(row["ConsultDataID"].ToString());
            }
            if (row["QuestionQuality"].ToString() != "")
            {
                model.QuestionQuality = int.Parse(row["QuestionQuality"].ToString());
            }
            if (row["LastTreatmentTime"].ToString() != "")
            {
                model.LastTreatmentTime = DateTime.Parse(row["LastTreatmentTime"].ToString());
            }
            if (row["IsComplaint"].ToString() != "")
            {
                if ((row["IsComplaint"].ToString() == "1") || (row["IsComplaint"].ToString().ToLower() == "true"))
                {
                    model.IsComplaint = true;
                }
                else
                {
                    model.IsComplaint = false;
                }
            }
            if (row["ProcessStatus"].ToString() != "")
            {
                model.ProcessStatus = int.Parse(row["ProcessStatus"].ToString());
            }
            if (row["IsSendEmail"].ToString() != "")
            {
                if ((row["IsSendEmail"].ToString() == "1") || (row["IsSendEmail"].ToString().ToLower() == "true"))
                {
                    model.IsSendEmail = true;
                }
                else
                {
                    model.IsSendEmail = false;
                }
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["BusinessType"].ToString() != "")
            {
                model.BusinessType = int.Parse(row["BusinessType"].ToString());
            }
            return model;
        }
        #endregion

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.CustHistoryInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@CallRecordID", SqlDbType.BigInt,8),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@RecordType", SqlDbType.Int,4),
					new SqlParameter("@ConsultID", SqlDbType.Int,4),
					new SqlParameter("@ConsultDataID", SqlDbType.Int,4),
					new SqlParameter("@QuestionQuality", SqlDbType.Int,4),
					new SqlParameter("@LastTreatmentTime", SqlDbType.DateTime),
					new SqlParameter("@IsComplaint", SqlDbType.Bit,1),
					new SqlParameter("@ProcessStatus", SqlDbType.Int,4),
					new SqlParameter("@IsSendEmail", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@BusinessType", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.CallRecordID;
            parameters[3].Value = model.CustID;
            parameters[4].Value = model.RecordType;
            parameters[5].Value = model.ConsultID;
            parameters[6].Value = model.ConsultDataID;
            parameters[7].Value = model.QuestionQuality;
            parameters[8].Value = model.LastTreatmentTime;
            parameters[9].Value = model.IsComplaint;
            parameters[10].Value = model.ProcessStatus;
            parameters[11].Value = model.IsSendEmail;
            parameters[12].Value = model.CreateTime;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.BusinessType;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.CustHistoryInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@CallRecordID", SqlDbType.BigInt,8),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@RecordType", SqlDbType.Int,4),
					new SqlParameter("@ConsultID", SqlDbType.Int,4),
					new SqlParameter("@ConsultDataID", SqlDbType.Int,4),
					new SqlParameter("@QuestionQuality", SqlDbType.Int,4),
					new SqlParameter("@LastTreatmentTime", SqlDbType.DateTime),
					new SqlParameter("@IsComplaint", SqlDbType.Bit,1),
					new SqlParameter("@ProcessStatus", SqlDbType.Int,4),
					new SqlParameter("@IsSendEmail", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                     new SqlParameter("@BusinessType", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.CallRecordID;
            parameters[3].Value = model.CustID;
            parameters[4].Value = model.RecordType;
            parameters[5].Value = model.ConsultID;
            parameters[6].Value = model.ConsultDataID;
            parameters[7].Value = model.QuestionQuality;
            parameters[8].Value = model.LastTreatmentTime;
            parameters[9].Value = model.IsComplaint;
            parameters[10].Value = model.ProcessStatus;
            parameters[11].Value = model.IsSendEmail;
            parameters[12].Value = model.CreateTime;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.BusinessType;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTHISTORYINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CustHistoryInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@CallRecordID", SqlDbType.BigInt,8),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@RecordType", SqlDbType.Int,4),
					new SqlParameter("@ConsultID", SqlDbType.Int,4),
					new SqlParameter("@ConsultDataID", SqlDbType.Int,4),
					new SqlParameter("@QuestionQuality", SqlDbType.Int,4),
					new SqlParameter("@LastTreatmentTime", SqlDbType.DateTime),
					new SqlParameter("@IsComplaint", SqlDbType.Bit,1),
					new SqlParameter("@ProcessStatus", SqlDbType.Int,4),
					new SqlParameter("@IsSendEmail", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@BusinessType", SqlDbType.Int,4)                    };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.CallRecordID;
            parameters[3].Value = model.CustID;
            parameters[4].Value = model.RecordType;
            parameters[5].Value = model.ConsultID;
            parameters[6].Value = model.ConsultDataID;
            parameters[7].Value = model.QuestionQuality;
            parameters[8].Value = model.LastTreatmentTime;
            parameters[9].Value = model.IsComplaint;
            parameters[10].Value = model.ProcessStatus;
            parameters[11].Value = model.IsSendEmail;
            parameters[12].Value = model.CreateTime;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.BusinessType;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYINFO_UPDATE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYINFO_DELETE, parameters);
        }

        public bool HavCustHistoryInfoByCustID(string custID)
        {
            string sqlStr = "SELECT count(*) FROM CustHistoryInfo WHERE CustID=@CustID";
            SqlParameter parameter = new SqlParameter("@CustID", custID);
            string objValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter).ToString();
            if (int.Parse(objValue) == 0)
                return false;
            else
                return true;
        }
    }
}

