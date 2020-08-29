using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.APPReport2016.Dal;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Amount
    {
        public static Amount Instance = new Amount();

        /// <summary>
        /// 获取最新年份
        /// </summary>
        /// <returns></returns>
        public int GetLatestYear()
        {
            return Dal.Amount.Instance.GetLatestYear();
        }

        /// <summary>
        /// 根据年份获取数据（获取业务收入饼图）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<object> GetDataByYear(int year)
        {
            DataTable dt = Dal.Amount.Instance.GetDataByYear(year);

            if (dt == null)
            {
                return null;
            }
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new { Name = Convert.ToString(dr["DictName"]), Count = CommonFunction.ObjectToDecimal(dr["Amount"]), DataType = Convert.ToInt32(dr["ItemId"]) });
            }

            return list;

        }

        /// <summary>
        /// 检查上一年是否存在数据
        /// </summary>
        /// <param name="year"></param>
        public bool CheckHasDataPreYearByYear(int year)
        {
            int preYear = year - 1;
            return Dal.Amount.Instance.CheckHasDataByYear(preYear);
        }

        /// <summary>
        /// 检查下一年是否存在数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool CheckHasDataNextYearByYear(int year)
        {
            if (year == DateTime.Now.Year)//如果是当前年份，则直接返回false
            {
                return false;
            }
            int nextYear = year + 1;
            return Dal.Amount.Instance.CheckHasDataByYear(nextYear);

        }
        /// <summary>
        /// 获取总统计数（获取业务收入饼图）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public decimal GetTotalAmountByYear(int year)
        {
            return Dal.Amount.Instance.GetTotalAmountByYear(year);
        }

        /// 获取业务收入柱状图数据
        /// <summary>
        /// 获取业务收入柱状图数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public DataTable GetAmountBarData(int year, int itemId)
        {
            return Dal.Amount.Instance.GetAmountBarData(year, itemId);
        }

    }
}
