using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using Entities = BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    public class AgentStatusDentail : DataBase
    {
        #region Instance
        public static readonly AgentStatusDentail Instance = new AgentStatusDentail();
        #endregion

        #region const
        //private const string P_EMPLOYEEAGENT_SELECT = "p_EmployeeAgent_Select";

        #endregion

        #region Contructor
        protected AgentStatusDentail()
        { }
        #endregion

        /// <summary>
        /// 根据RecID,把坐席最新状态的结束时间更新为当前时间,同时更新状态保持时长（s）。
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int UpdateAgentLastStatus(int nRecID)
        {
            //            string strSql = @"UPDATE    [dbo].[AgentStatusDetail]
            //                            SET       EndTime = '" + DateTime.Now.ToString() + @"',TimeLong =DATEDIFF(ss,StartTime,'" + DateTime.Now.ToString() + @"')
            //                            WHERE     RecID = ( SELECT TOP 1
            //                                                        ISNULL(RecID, -1)
            //                                                FROM      [dbo].[AgentStatusDetail]
            //                                                WHERE UserID=" + UserId + @" 
            //                                                ORDER BY  RecID DESC
            //                                            )";
            string strSql =
                " UPDATE dbo.AgentStatusDetail SET EndTime=GETDATE(),TimeLong=DATEDIFF(s,StartTime,GETDATE()) WHERE RecID=" + nRecID.ToString();

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// <summary>
        /// 把坐席当前库中最新一条状态数据的endtime更新为当前时间，同时插入一条新的状态数据，开始时间为当前时间，status为传入的状态值
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Status"></param>
        /// <returns>返回值0：返回插入新数据的主键值（RecID）</returns>
        public int AddAgentStatusData(int UserId, int Status, int RecID)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@RecID", SqlDbType.Int,4),
                new SqlParameter("@UserId", SqlDbType.Int,4),
                new SqlParameter("@Status", SqlDbType.Int,4),                
                new SqlParameter("@NewRecID", SqlDbType.Int,4)
            };

            parameters[0].Value = RecID;
            parameters[1].Value = UserId;
            parameters[2].Value = Status;
            parameters[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_AddAgentStatusData", parameters);
            return (int)parameters[3].Value;
        }

        public DataTable GetSourceTypeByBGIDS(string bgids)
        {
            string strSql = @"SELECT DISTINCT LineID 
                              FROM [CC2012].[dbo].[BusinessGroupLineMapping] 
                              WHERE Status=0 AND LineID!='' AND BGID IN (" + StringHelper.SqlFilter(bgids) + ")";
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, strSql).Tables[0];
        }



        /// <summary>
        /// 查询客服统计明细
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable Get(Entities.QueryAgentStatusDetail query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where += " and Detail.StartTime>='" + query.Starttime.ToString("yyyy-MM-dd") + "'";
            where += " and Detail.StartTime<'" + query.Starttime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            where += " and Detail.UserID=" + query.UserID;

            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Detail.Status=" + query.Status;
            }

            if (query.AgentNum != Constant.STRING_EMPTY_VALUE)
            {
                where += " and Detail.UserID=(select top 1 UserId from v_AgentInfo where AgentNum=" + StringHelper.SqlFilter(query.AgentNum) + ") ";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_AgentStatusDetail_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
    }
}
