using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Traffic
    {
        public static Traffic Instance = new Traffic();

        /// <summary>
        /// 获取最新日期
        /// </summary>
        /// <returns></returns>
        public DateTime GetLatestDate()
        {
            return Dal.Traffic.Instance.GetLatestDate();
        }

        /// <summary>
        /// 获取平台覆盖数据饼图
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<object> GetDataByDate(DateTime date)
        {
            DataTable dt = Dal.Traffic.Instance.GetDataByDate(date);
            if (dt == null)
            {
                return null;
            }

            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows)
            {
                long uv = Convert.ToInt64(dr["UV"]);
                string name = Convert.ToString(dr["DictName"]);
                list.Add(new { Name = name, Count = uv.ToString() });
            }


            return list;
        }


        /// <summary>
        /// 获取平台覆盖数据片图前的总计数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public object GetWholeSiteByDate(DateTime date)
        {
            DataTable dt= Dal.Traffic.Instance.GetWholeSiteByDate(date);
            if (dt == null||dt.Rows.Count==0)
            {
                return null;
            }
            else
	      {
              return new { Count = Convert.ToString(dt.Rows[0]["UV"]), WeekBasis = CommonFunction.ObjectToDecimal(dt.Rows[0]["WeekBasis"]), DayBasis = CommonFunction.ObjectToDecimal(dt.Rows[0]["DayBasis"]) };
	      }

        }


        /// <summary>
        ///  获取平台覆盖数据片图
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<object> GetDataDetails(DateTime date)
        { 
           DataTable dt = Dal.Traffic.Instance.GetDataByDate(date);
            if (dt == null)
            {
                return null;
            }

            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows)
            {
                long uv = Convert.ToInt64(dr["UV"]);
                string name = Convert.ToString(dr["DictName"]);
                int siteId = Convert.ToInt32(dr["SiteId"]);
                decimal weekBasis = CommonFunction.ObjectToDecimal(dr["WeekBasis"]);
                decimal dayBasis = CommonFunction.ObjectToDecimal(dr["DayBasis"]);
                list.Add(new { Name = name, Count = uv.ToString(), WeekBasis = weekBasis, DayBasis = dayBasis, TypeID = siteId });
            }


            return list;
        }



        /// <summary>
        /// 获取单个平台线图
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public object GetDataTrendBySiteIdAndDate(int siteId, DateTime date)
        {
            DataTable dt = Dal.Traffic.Instance.GetDataTrendBySiteIdAndDate(siteId, date);
           if (dt==null)
           {
               return null;
           }

           List<string> seriesname = new List<string>();
           seriesname.Add("页面浏览量");//PV xian pv
           seriesname.Add("覆盖用户数");//UV

           List<string> xAxisdata = new List<string>();//riqi      

           List<List<string>> seriesdata = new List<List<string>>();

           List<string> seriesdataUV = new List<string>();
           List<string> seriesdataPV = new List<string>();

        

           foreach (DataRow dr in dt.Rows)
           {
               string uv = Convert.ToString(dr["UV"]);//bigint 太长 传递前台只能用string  not null
               string pv = Convert.ToString(dr["PV"]);
               string trafficDate = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
               seriesdataPV.Add(pv);//xian pv
               seriesdataUV.Add(uv);            
               xAxisdata.Add(trafficDate);

           }
           seriesdata.Add(seriesdataUV);
           seriesdata.Add(seriesdataPV);
           var dataList = new { seriesname = seriesname, xAxisdata = xAxisdata, seriesdata = seriesdata };

           return dataList;

        }
    }
}
