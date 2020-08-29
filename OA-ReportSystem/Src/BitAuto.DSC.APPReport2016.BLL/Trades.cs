using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Trades
    {
        public static Trades Instance = new Trades();

        /// <summary>
        /// 获取最新日期的交易量片图
        /// </summary>
        /// <returns></returns>
        public object GetRetrunObjectData()
        {

            DateTime date = Dal.Trades.Instance.GetLatestDate();

            var RetDate = new { RetDateVal = date.ToString("yyyy-MM-dd"), WeekDay = Util.GetWeekNameByDate(date) };

            DataTable dt = Dal.Trades.Instance.GetDataByDate(date);
            if (dt == null)
            {
                return null;
            }

            object huiMaiChe = null;
            object yiXin = null;

            foreach (DataRow dr in dt.Rows)
            {
                int typeID = CommonFunction.ObjectToInteger(dr["LineId"]);
                int count = CommonFunction.ObjectToInteger(dr["Count"]);//int
                decimal weekBasis = CommonFunction.ObjectToDecimal(dr["WeekBasis"]);
                decimal dayBasis = CommonFunction.ObjectToDecimal(dr["DayBasis"]);
                string dictName = Convert.ToString(dr["DictName"]);

                if (typeID == 20003)
                {
                    huiMaiChe = new { Name = dictName, Count = count, WeekBasis = weekBasis, DayBasis = dayBasis };
                }
                else if (typeID == 20004) //总Leads数
                {
                    yiXin = new { Name = dictName, Count = count, WeekBasis = weekBasis, DayBasis = dayBasis };
                }

            }

            var dataList = new { RetDate = RetDate, huiMaiChe = huiMaiChe, yiXin = yiXin };
            return dataList;
        }

        /// <summary>
        /// 获取交易量数据的线图
        /// </summary>
        /// <returns></returns>
        public object GetDataTrendByDate()
        {
            DateTime dtime = Dal.Trades.Instance.GetLatestDate();

            DataTable dt = Dal.Trades.Instance.GetDataTrendByDate(dtime);
            if (dt == null)
            {
                return null;
            }

            List<string> seriesname = new List<string>();
            seriesname.Add(Dal.Trades.Instance.GetDictNameByDictID(20003));//惠买车
            seriesname.Add(Dal.Trades.Instance.GetDictNameByDictID(20004));///易鑫

            List<string> xAxisdata = new List<string>();//日期
            List<List<int?>> seriesdata = new List<List<int?>>(); //数据

            List<int?> seriesdataHuiMaiChe = new List<int?>();//惠买车
            List<int?> seriesdataYiXin = new List<int?>();//易鑫

            //取数据中最小日期和最大日期
            DateTime st = CommonFunction.ObjectToDateTime(dt.Select("", "Date asc")[0]["Date"]);
            DateTime et = CommonFunction.ObjectToDateTime(dt.Select("", "Date desc")[0]["Date"]);

            //循环日期取数
            for (DateTime date = st; date <= et; date = date.AddDays(1))
            {
                string datestr = date.ToString("yyyy-MM-dd");
                xAxisdata.Add(datestr);

                //20003
                DataRow[] drs1 = dt.Select("LineId=20003 and Date='" + datestr + "'");
                if (drs1.Length > 0)
                {
                    seriesdataHuiMaiChe.Add(CommonFunction.ObjectToInteger(drs1[0]["Count"]));
                }
                else
                {
                    seriesdataHuiMaiChe.Add(null);
                }

                //20004
                DataRow[] drs2 = dt.Select("LineId=20004 and Date='" + datestr + "'");
                if (drs2.Length > 0)
                {
                    seriesdataYiXin.Add(CommonFunction.ObjectToInteger(drs2[0]["Count"]));
                }
                else
                {
                    seriesdataYiXin.Add(null);
                }
            }
            seriesdata.Add(seriesdataHuiMaiChe);
            seriesdata.Add(seriesdataYiXin);
            var dataList = new { seriesname = seriesname, xAxisdata = xAxisdata, seriesdata = seriesdata };
            return dataList;
        }
    }
}
