using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class Leads
    {
        public static readonly Leads Instance = new Leads();
        /// <summary>
        /// 获取平台/业务线维度Leads饼图数据
        /// </summary>
        /// <param name="searchDate"></param>
        /// <param name="searchType">1:平台维度；2：业务线维度</param> 
        public DataTable GetLeadsPieChartData(DateTime searchDate, int searchType,out DateTime maxDate)
        {
            maxDate = DateTime.Now;
            SqlParameter[] parameters = {
					new SqlParameter("@searchDate", SqlDbType.DateTime), 
					new SqlParameter("@searchType", SqlDbType.Int,4),
                    new SqlParameter("@maxDate",SqlDbType.DateTime)};
            parameters[0].Value = searchDate;
            parameters[1].Value = searchType;
            parameters[2].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetLeadsPieChartData", parameters);
            if (parameters[2].Value !=null && parameters[2].Value != DBNull.Value)
            {
                maxDate = (DateTime)parameters[2].Value;
            }
            return ds.Tables[0];
        }
         
        /// <summary>
        /// 获取平台/业务线维度Leads数趋势和导向构成总览+详细数据
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="searchType">1:平台维度；2：业务线维度</param> 
        public DataSet GetLeadsOverView_Data(DateTime searchDate, int searchType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@searchDate", SqlDbType.DateTime), 
					new SqlParameter("@searchType", SqlDbType.Int,4)};
            parameters[0].Value = searchDate;
            parameters[1].Value = searchType; 
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetLeadsOverView_Data", parameters);
        }
        /// <summary>
        /// 获取平台/业务线维度Leads导向构成环形图数据。siteId>0时：按平台维度查；lineId>0时：按业务线维度查
        /// </summary>
        /// <param name="searchDate"></param>
        /// <param name="siteId"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public DataTable GetLeadsCircleChartData(DateTime searchDate, int siteId, int lineId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@searchDate", SqlDbType.DateTime), 
                    new SqlParameter("@siteId", SqlDbType.Int,4), 
					new SqlParameter("@lineId", SqlDbType.Int,4)};
            parameters[0].Value = searchDate;
            parameters[1].Value = siteId;
            parameters[2].Value = lineId; 
            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetLeadsCircleChartData", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 获取平台/业务线维度Leads数据趋势折线图数据。siteId>0时：按平台维度查；lineId>0时：按业务线维度查
        /// </summary>
        /// <param name="searchDate"></param>
        /// <param name="siteId"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public DataTable GetLeadsLineChartData(DateTime searchDate, int siteId, int lineId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@searchDate", SqlDbType.DateTime), 
                    new SqlParameter("@siteId", SqlDbType.Int,4), 
					new SqlParameter("@lineId", SqlDbType.Int,4)};
            parameters[0].Value = searchDate;
            parameters[1].Value = siteId;
            parameters[2].Value = lineId;
            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetLeadsLineChartData", parameters);
            return ds.Tables[0];
        }
    }
}
