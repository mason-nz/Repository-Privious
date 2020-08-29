using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace BitAuto.DSC.APPReport2016.WebAPI.Common
{
    public class Common
    {
        /// 计算缩放范围
        /// <summary>
        /// 计算缩放范围
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] GetZoomIndex(int count)
        {
            if (count < 6)
            {
                return new int[2] { 0, count - 1 };
            }
            else if (count < 12)
            {
                return new int[2] { count - 6, count - 1 };
            }
            else
            {
                //数据至少有全年的数据
                if (DateTime.Today.Month <= 6)
                {
                    //上半年
                    return new int[2] { count - 12, count - 6 - 1 };
                }
                else
                {
                    //下半年
                    return new int[2] { count - 6, count - 1 };
                }
            }
        }
        /// 将object类型转换为decimal?
        /// <summary>
        /// 将object类型转换为decimal?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? Object2DecimalNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                return null;
            }
            else
            {
                return decimal.Parse(obj.ToString());
            }
        }
        /// 生成对比柱图的数据
        /// <summary>
        /// 生成对比柱图的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static CompareBarData CreateCompareBarData(DataTable dt)
        {
            CompareBarData amountBarData = new CompareBarData();
            if (dt.Rows.Count > 0)
            {
                int minDate = dt.AsEnumerable().Min(t => t.Field<int>("date"));
                int maxDate = dt.AsEnumerable().Max(t => t.Field<int>("date"));
                int minYear = int.Parse(minDate.ToString().Substring(0, 4));
                int maxYear = int.Parse(maxDate.ToString().Substring(0, 4));
                //两个柱状图和一个线图数据放到一个list中
                List<string> dataKey = new List<string>();
                List<List<decimal?>> dataVal = new List<List<decimal?>>();
                List<decimal?> listLastYear = new List<decimal?>();
                List<decimal?> listThisYear = new List<decimal?>();
                List<decimal?> listThisYearLine = new List<decimal?>();
                List<decimal?> listThisYearPredict = new List<decimal?>();
                //构造数据
                foreach (DataRow dr in dt.Rows)
                {
                    int year = int.Parse(dr["date"].ToString().Substring(0, 4));
                    //前一年数组数据
                    if ((minYear < maxYear) && (year >= minYear && year < maxYear))
                    {
                        //有两年以上的数据
                        listLastYear.Add(Common.Object2DecimalNullable(dr["amount"]));
                    }
                    else if (minYear == maxYear)
                    {
                        //只有一年的数据
                        listLastYear.Add(null);
                    }

                    //当年数组数据,线图数据和X轴数据
                    //有两年以上的数据 或者 只有一年的数据
                    if ((minYear < maxYear) && (year > minYear && year <= maxYear) || (minYear == maxYear))
                    {
                        int i_year = int.Parse(dr["date"].ToString().Substring(0, 4));
                        int i_month = int.Parse(dr["date"].ToString().Substring(4));
                        //判断是否是预估数据
                        if (i_year == DateTime.Now.Year && i_month > DateTime.Now.Month)
                        {
                            listThisYearPredict.Add(1);
                        }
                        else
                        {
                            listThisYearPredict.Add(0);
                        }
                        dataKey.Add(i_year + "-" + i_month.ToString("00"));
                        listThisYear.Add(Common.Object2DecimalNullable(dr["amount"]));
                        listThisYearLine.Add(Common.Object2DecimalNullable(dr["monthBasis"]));
                    }
                }
                dataVal.Add(listLastYear);
                dataVal.Add(listThisYear);
                dataVal.Add(listThisYearLine);
                dataVal.Add(listThisYearPredict);
                amountBarData.seriesname = new List<string>() { (maxYear - 1).ToString(), maxYear.ToString() };
                amountBarData.datakey = dataKey;
                amountBarData.dataval = dataVal;
                amountBarData.zoomindex = new List<int>(Common.GetZoomIndex(dataKey.Count));
                amountBarData.isonlyoneyear = minYear == maxYear ? 1 : 0;
            }
            return amountBarData;
        }
    }

    public class CompareBarData
    {
        public List<string> seriesname { get; set; }
        public List<string> datakey { get; set; }
        public List<List<decimal?>> dataval { get; set; }
        public List<int> zoomindex { get; set; }
        public int isonlyoneyear { get; set; }
    }
}