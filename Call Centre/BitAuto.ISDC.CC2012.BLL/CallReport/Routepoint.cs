using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class Routepoint
    {
        private Routepoint()
        {
        }

        private static Routepoint instance = null;

        public static Routepoint Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Routepoint();
                }
                return instance;
            }
        }

        #region 热线数据报表
        /// 同步小时路由数据
        /// <summary>
        /// 同步小时路由数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SyncRoutepointHourData(DataTable dt, out string msg)
        {
            List<SqlBulkCopyColumnMapping> list = CreateSqlBulkCopyColumnMapping(dt);
            Util.BulkCopyToDB(dt, Dal.Routepoint.Instance.GetConnectionstrings(), TableName.REPORT_ROUTEPOINT_HOUR.ToString(), 1000, list, out msg);
            if (msg == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// 同步15分钟路由数据
        /// <summary>
        /// 同步15分钟路由数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SyncRoutepoint15MinData(DataTable dt, out string msg)
        {
            List<SqlBulkCopyColumnMapping> list = CreateSqlBulkCopyColumnMapping(dt);
            Util.BulkCopyToDB(dt, Dal.Routepoint.Instance.GetConnectionstrings(), TableName.REPORT_ROUTEPOINT_15MINUTES.ToString(), 1000, list, out msg);
            if (msg == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// 拷贝数据
        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <param name="olddt"></param>
        /// <param name="newdt"></param>
        /// <returns></returns>
        private List<SqlBulkCopyColumnMapping> CreateSqlBulkCopyColumnMapping(DataTable dt)
        {
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            AddSqlBulkCopyColumnMapping(dt, list, "BEGIN_TIME", "BeginTime");
            AddSqlBulkCopyColumnMapping(dt, list, "OBJECT_NAME", "ObjectName");
            AddSqlBulkCopyColumnMapping(dt, list, "OBJECT_TYPE", "ObjectType");
            AddSqlBulkCopyColumnMapping(dt, list, "AV_T_ABANDONED", "AV_T_ABANDONED");
            AddSqlBulkCopyColumnMapping(dt, list, "AV_T_ANSWERED", "AV_T_ANSWERED");
            AddSqlBulkCopyColumnMapping(dt, list, "AV_T_DISTRIBUTED", "AV_T_DISTRIBUTED");
            AddSqlBulkCopyColumnMapping(dt, list, "N_ABANDONED", "N_ABANDONED");
            AddSqlBulkCopyColumnMapping(dt, list, "N_ANSWERED", "N_ANSWERED");
            AddSqlBulkCopyColumnMapping(dt, list, "N_DISTRIBUTED", "N_DISTRIBUTED");
            AddSqlBulkCopyColumnMapping(dt, list, "N_ENTERED", "N_ENTERED");
            AddSqlBulkCopyColumnMapping(dt, list, "N_ABANDONED_IN_TR", "N_ABANDONED_IN_TR");
            AddSqlBulkCopyColumnMapping(dt, list, "N_DISTRIB_IN_TR", "N_DISTRIB_IN_TR");
            AddSqlBulkCopyColumnMapping(dt, list, "T_ABANDONED", "T_ABANDONED");
            AddSqlBulkCopyColumnMapping(dt, list, "T_ANSWERED", "T_ANSWERED");
            AddSqlBulkCopyColumnMapping(dt, list, "T_DISTRIBUTED", "T_DISTRIBUTED");
            //新增指标 2015-3-4 强斐
            AddSqlBulkCopyColumnMapping(dt, list, "N_ENTERED_OUT", "N_ENTERED_OUT");
            return list;
        }
        private void AddSqlBulkCopyColumnMapping(DataTable dt, List<SqlBulkCopyColumnMapping> list, string sou, string dec)
        {
            if (dt.Columns.Contains(sou))
            {
                list.Add(new SqlBulkCopyColumnMapping(sou, dec));
            }
        }

        /// 获取最大时间从表中
        /// <summary>
        /// 获取最大时间从表中
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="datecol"></param>
        /// <returns></returns>
        public DateTime GetMaxDateTimeFromTable(TableName tablename, Vender vendor)
        {
            string where = GetVendorWhere(vendor);
            return Dal.Routepoint.Instance.GetMaxDateTimeFromTable(tablename.ToString(), "BeginTime", where);
        }
        /// 清楚数据
        /// <summary>
        /// 清楚数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public int ClearDataFormBeginToEnd(TableName tablename, Vender vendor, DateTime st, DateTime et)
        {
            string where = GetVendorWhere(vendor);
            return Dal.Routepoint.Instance.ClearDataFormBeginToEnd(tablename.ToString(), st, et, where);
        }
        /// 获取热点路由数据
        /// <summary>
        /// 获取热点路由数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <param name="hzdt">汇总数据</param>
        /// <returns></returns>
        public DataTable GetRoutepointData(QueryRoutepoint query, int pageindex, int pagesize, out int total, out DataTable hzdt)
        {
            return Dal.Routepoint.Instance.GetRoutepointData(query, pageindex, pagesize, out total, out hzdt);
        }
        /// 获取厂家类型条件
        /// <summary>
        /// 获取厂家类型条件
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        private static string GetVendorWhere(Vender vendor)
        {
            string where = "";
            if (vendor == Vender.Genesys)
            {
                //Genesys类型
                where = " AND ObjectType >=001 AND ObjectType<=099";
            }
            else if (vendor == Vender.Holly)
            {
                //Holly类型
                where = " AND ObjectType >=101 AND ObjectType<=200";
            }
            return where;
        }
        #endregion

        #region 大屏报表
        /// 获取实时队列数据
        /// <summary>
        /// 获取实时队列数据
        /// </summary>
        /// <param name="type">1：企业；2：个人</param>
        /// <returns></returns>
        public DataTable GetRealTimeQueueData(int type, string bgid)
        {
            return Dal.Routepoint.Instance.GetRealTimeQueueData(type, bgid);
        }
        /// 获取指标完成数据
        /// <summary>
        /// 获取指标完成数据
        /// </summary>
        /// <param name="type">1：企业；2：个人</param>
        /// <returns></returns>
        public DataTable GetIndicatorsComData(int type, string bgid, string date)
        {
            return Dal.Routepoint.Instance.GetIndicatorsComData(type, bgid, date);
        }
        /// 获取实时监控数据
        /// <summary>
        /// 获取实时监控数据
        /// </summary>
        /// <param name="bgid">组id</param>
        /// <returns></returns>
        public DataTable GetRealTimeMonitoring(string bgid)
        {
            return Dal.Routepoint.Instance.GetRealTimeMonitoring(bgid);
        }
        /// 获取呼出业务接通情况实时监控
        /// <summary>
        /// 获取呼出业务接通情况实时监控
        /// </summary>
        /// <param name="SelectedDate">查询的日期</param>
        /// <param name="BGID">要查询的组</param>
        /// <returns></returns>
        public DataTable GetCallOutConnectedInfoData(string SelectedDate, string BGID)
        {
            return Dal.Routepoint.Instance.GetCallOutConnectedInfoData(SelectedDate, BGID);
        }
        /// 获取达人排名数据
        /// <summary>
        /// 获取达人排名数据
        /// </summary>
        /// <param name="BGID"></param>
        /// <returns></returns>
        public DataTable GetFourScreenData(string bgid, string date)
        {
            return Dal.Routepoint.Instance.GetFourScreenData(bgid, date);
        }

        /// 获取实时热线数据-合力
        /// <summary>
        /// 获取实时热线数据-合力
        /// </summary>
        /// <param name="hoteLine"></param>
        /// <returns></returns>
        public DataTable GetHotLineRealInfo(string hoteLine)
        {
            return Dal.Routepoint.Instance.GetHotLineRealInfo(hoteLine);
        }
        /// 获取实时热线数据-合力北京测试热线
        /// <summary>
        /// 获取实时热线数据-合力北京测试热线
        /// </summary>
        /// <param name="maxExtensionNum"></param>
        /// <param name="minExtensionNum"></param>
        /// <returns></returns>
        public DataTable GetHotLineRealInfo_BJTest(int maxExtensionNum, int minExtensionNum)
        {
            return Dal.Routepoint.Instance.GetHotLineRealInfo_BJTest(maxExtensionNum, minExtensionNum);
        }
        /// 获取热线指标完成数据-合力
        /// <summary>
        /// 获取热线指标完成数据-合力
        /// </summary>
        /// <param name="hoteLine"></param>
        /// <returns></returns>
        public DataTable GetHotLineStateInfo(string hoteLine)
        {
            return Dal.Routepoint.Instance.GetHotLineStateInfo(hoteLine);
        }
        #endregion

        /// 相关表
        /// <summary>
        /// 相关表
        /// </summary>
        public enum TableName
        {
            REPORT_ROUTEPOINT_HOUR,
            REPORT_ROUTEPOINT_15MINUTES
        }
    }
}
