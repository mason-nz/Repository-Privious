using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Leads
    {
        public static readonly Leads Instance = new Leads();
        /// <summary>
        /// 获取平台/业务线维度Leads饼图数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itemId">1:平台维度；2：业务线维度</param> 
        public DataTable GetLeadsPieChartData(DateTime searchDate, int searchType,out  DateTime maxDate)
        {
            return Dal.Leads.Instance.GetLeadsPieChartData(searchDate,searchType,out maxDate);
        }

        /// <summary>
        /// 获取平台/业务线维度Leads数趋势和导向构成总览+详细数据
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="searchType">1:平台维度；2：业务线维度</param> 
        public DataSet GetLeadsOverView_Data(DateTime searchDate, int searchType)
        {
            return Dal.Leads.Instance.GetLeadsOverView_Data(searchDate, searchType);
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
            return Dal.Leads.Instance.GetLeadsCircleChartData(searchDate, siteId, lineId);
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
            return Dal.Leads.Instance.GetLeadsLineChartData(searchDate, siteId, lineId);
        }
    }
}
