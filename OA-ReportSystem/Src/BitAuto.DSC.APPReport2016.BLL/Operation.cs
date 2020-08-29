using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Operation
    {
        public static Operation Instance = new Operation();

        /// 获取最新日期
        /// <summary>
        /// 获取最新日期
        /// </summary>
        /// <returns></returns>
        public DateTime GetLatestDate()
        {
            return Dal.Operation.Instance.GetLatestDate();
        }
        private DataTable GetOperationDataByDate(DateTime date)
        {
            return Dal.Operation.Instance.GetOperationDataByDate(date);
        }
        /// 查询最近一天的运行日报情况
        /// <summary>
        /// 查询最近一天的运行日报情况
        /// </summary>
        /// <returns></returns>
        public object GetReturnObjectData()
        {
            DateTime dtime = GetLatestDate();

            var RetDate = new { RetDateVal = dtime.ToString("yyyy-MM-dd"), WeekDay = Util.GetWeekNameByDate(dtime) };

            DataTable dt = GetOperationDataByDate(dtime);
            if (dt == null)
            {
                return null;
            }

            object Traffic = null;
            object Leads = null;
            object BuyUser = null;
            object Trades = null;
            foreach (DataRow dr in dt.Rows)
            {
                int typeID = Convert.ToInt32(dr["ItemId"]);
                string count = Convert.ToString(dr["Count"]);//bigint
                decimal weekBasis = CommonFunction.ObjectToDecimal(dr["WeekBasis"]);
                decimal dayBasis = CommonFunction.ObjectToDecimal(dr["DayBasis"]);
                string dictName = Convert.ToString(dr["DictName"]);
                if (typeID == 70001) //总覆盖用户数
                {
                    Traffic = new { Name = dictName, Count = count, WeekBasis = weekBasis, DayBasis = dayBasis };
                }
                else if (typeID == 70002) //总Leads数
                {
                    Leads = new { Name = dictName, Count = count, WeekBasis = weekBasis, DayBasis = dayBasis };
                }
                else if (typeID == 70003) //总下单用户数
                {
                    BuyUser = new { Name = dictName, Count = count, WeekBasis = weekBasis, DayBasis = dayBasis };
                }
                else if (typeID == 70004) //总交易量
                {
                    Trades = new { Name = dictName, Count = count, WeekBasis = weekBasis, DayBasis = dayBasis };
                }
            }
            var dataList = new { RetDate = RetDate, Traffic = Traffic, Leads = Leads, BuyUser = BuyUser, Trades = Trades };
            return dataList;
        }
        /// 获取线图信息
        /// <summary>
        /// 获取线图信息
        /// </summary>
        /// <returns></returns>
        public object GetDataTrendByDate()
        {
            DateTime dtime = GetLatestDate();

            DataTable dt = Dal.Operation.Instance.GetDataTrendByDate(dtime);
            if (dt == null)
            {
                return null;
            }

            List<string> seriesname = new List<string>();
            seriesname.Add(Dal.Operation.Instance.GetDictNameByDictID(70001));//总覆盖用户数
            seriesname.Add(Dal.Operation.Instance.GetDictNameByDictID(70002));///总Leads数
            seriesname.Add(Dal.Operation.Instance.GetDictNameByDictID(70003));///总下单用户数

            List<string> xAxisdata = new List<string>();//riqi
            List<List<string>> seriesdata = new List<List<string>>();

            List<string> seriesdataTraffic = new List<string>();//总覆盖用户数
            List<string> seriesdataLeads = new List<string>();//leads
            List<string> seriesdataBuyUser = new List<string>();//总下单用户数

            foreach (DataRow dr in dt.Rows)
            {
                int itemId = Convert.ToInt32(dr["ItemId"]);//bigint 太长 传递前台只能用string
                string count = Convert.ToString(dr["Count"]);
                string oprationDate = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
                if (itemId == 70001) //总覆盖用户数
                {
                    seriesdataTraffic.Add(count);
                }
                else if (itemId == 70002) //总Leads数
                {
                    seriesdataLeads.Add(count);
                }
                else if (itemId == 70003) //总下单用户数
                {
                    seriesdataBuyUser.Add(count);
                }
                if (!xAxisdata.Contains(oprationDate))//riqi
                {
                    xAxisdata.Add(oprationDate);
                }
            }
            seriesdata.Add(seriesdataTraffic);
            seriesdata.Add(seriesdataLeads);
            seriesdata.Add(seriesdataBuyUser);
            var dataList = new { seriesname = seriesname, xAxisdata = xAxisdata, seriesdata = seriesdata };
            return dataList;
        }
    }
}
