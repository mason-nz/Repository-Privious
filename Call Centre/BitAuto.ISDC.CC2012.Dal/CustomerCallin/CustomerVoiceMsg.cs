using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CustomerVoiceMsg : DataBase
    {
        public static CustomerVoiceMsg Instance = new CustomerVoiceMsg();
        public const string P_CUSTOMERVOICEMSG_SELECT = "p_CustomerVoiceMsg_Select";

        /// 未接来电数据批量插入留言表（来源：1 留言 2未接来电）
        /// <summary>
        /// 未接来电数据批量插入留言表（来源：1 留言 2未接来电）
        /// </summary>
        public int BCPCustomerVoiceMsgHolly()
        {
            //批量插入
            string sql = @"INSERT INTO CustomerVoiceMsg(Vender,CallNO,CalledNo,StartTime,EndTime,SGID,SessionID,SourceType,Status,CreateTime,ExclusiveUserID,ExclusiveAgentNum)
                                SELECT 1,ORIANI,ORIDNIS,ISNULL(InitiatedTime,EndTime),EndTime,SKILLID,SessionID,2,0,GETDATE(),
                                (SELECT Top 1 UserID FROM dbo.EmployeeAgent WHERE AgentNum=dbo.HollyDataTemp.VARAGENTIDZ),
                                VARAGENTIDZ
                                FROM dbo.HollyDataTemp
                                WHERE 1=1
                                AND ISNULL(TallTime,0)=0
                                AND CALLDIRECTION=1
                                AND SessionID NOT IN (SELECT SessionID FROM CustomerVoiceMsg)";
            int num = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);
            return num;
        }
        /// 根据联络ID获取数据
        /// <summary>
        /// 根据联络ID获取数据
        /// </summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public CustomerVoiceMsgInfo GetCustomerVoiceMsgInfo(string SessionID)
        {
            string sql = @"SELECT * FROM CustomerVoiceMsg WHERE SessionID='" + SqlFilter(SessionID) + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return new CustomerVoiceMsgInfo(dt.Rows[0]);
            }
            else return null;
        }
        /// 分页查询数据
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustomerVoiceMsgData(QueryCustomerVoiceMsg query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhere(query);
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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTOMERVOICEMSG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// 分页查询专属客服未接来电
        /// <summary>
        /// 分页查询专属客服未接来电
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetExclusiveMissedCallingsData(ExclusiveMissedCalls query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            string where = "";

            where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("f", "a", "BGID", "ExclusiveUserID", userid);

            if (!string.IsNullOrEmpty(query.ANI))
            {
                where += " and a.CallNO ='" + SqlFilter(query.ANI) + "'";
            }
            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                where += " and a.StartTime>='" + SqlFilter(query.BeginTime) + "'";
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                where += " and a.StartTime<='" + SqlFilter(query.EndTime) + "'";
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and a.ExclusiveAgentNum='" + SqlFilter(query.AgentNum) + "'";
            }
            if (!string.IsNullOrEmpty(query.AgentID))
            {
                where += " and a.ExclusiveUserID='" + SqlFilter(query.AgentID) + "'";
            }
            if (!string.IsNullOrEmpty(query.AgentGroup))
            {
                where += " and b.BGID = (" + Dal.Util.SqlFilterByInCondition(query.AgentGroup) + ")";
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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ExclusiveMissedCallings_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 构造查询条件
        /// <summary>
        /// 构造查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetWhere(QueryCustomerVoiceMsg query)
        {
            string where = "";
            if (!string.IsNullOrEmpty(query.ANI))
            {
                where += " and a.CallNO ='" + SqlFilter(query.ANI) + "'";
            }
            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                where += " and a.StartTime>='" + SqlFilter(query.BeginTime) + "'";
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                where += " and a.StartTime<='" + SqlFilter(query.EndTime) + "'";
            }
            if (!string.IsNullOrEmpty(query.selBusinessType) && query.selBusinessType != "-1")
            {
                where += " and b.TelMainNum='" + SqlFilter(query.selBusinessType.Substring(3)) + "'";
            }
            if (!string.IsNullOrEmpty(query.Agent))
            {
                where += " and d.TrueName like '%" + SqlFilter(query.Agent) + "%'";
            }
            if (!string.IsNullOrEmpty(query.PRBeginTime))
            {
                where += " and a.ProcessTime>='" + SqlFilter(query.PRBeginTime) + "'";
            }
            if (!string.IsNullOrEmpty(query.PREndTime))
            {
                where += " and a.ProcessTime<='" + SqlFilter(query.PREndTime) + "'";
            }
            if (!string.IsNullOrEmpty(query.prStatus))
            {
                where += " and a.Status IN (" + Dal.Util.SqlFilterByInCondition(query.prStatus) + ")";
            }
            if (!string.IsNullOrEmpty(query.IsExclusive))
            {
                if (query.IsExclusive == "0")
                {
                    where += " and g.UserID is null";
                }
                else
                {
                    where += " and g.UserID is not null";
                }
            }
            if (!string.IsNullOrEmpty(query.HasSkill))
            {
                if (query.HasSkill == "1")
                {
                    //有
                    where += " and ISNULL(a.SGID,0)>0";
                }
                if (query.HasSkill == "0")
                {
                    //无
                    where += " and ISNULL(a.SGID,0)=0";
                }
            }
            if (!string.IsNullOrEmpty(query.Hasaudio))
            {
                if (query.Hasaudio == "1")
                {
                    //有
                    where += " and a.SourceType=1";
                }
                if (query.Hasaudio == "0")
                {
                    //无
                    where += " and a.SourceType=2";
                }
            }
            return where;
        }
    }
}
