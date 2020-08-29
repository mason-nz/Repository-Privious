using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Member
    {
        public static Member Instance = new Member();

        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public DateTime GetMaxDate(string ItemIds = "")
        {
            string maxDate = Dal.Member.Instance.GetMaxDate(ItemIds);
            DateTime date;
            try
            {
                //验证日期格式
                //格式为yyyyMM ，长度为6
                maxDate = maxDate.Trim();
                maxDate = maxDate.Substring(0, 4) + "-" + maxDate.Substring(4, 2);
                date = DateTime.Parse(maxDate);

                //最大时间为今天
                if (date > DateTime.Now)
                {
                    date = DateTime.Now;
                }
            }
            catch
            {
                date = DateTime.Now;
            }
            return date;

        }
        /// <summary>
        /// 获取某段时间内的会员数量
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="itemIds">会员类型</param>
        /// <returns></returns>
        public DataTable GetData(int startDate, int endDate, string itemIds)
        {
            return Dal.Member.Instance.GetData(startDate, endDate, itemIds);
        }
        /// <summary>
        /// 平均贡献值
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public string GetAvgAmount(int startDate, int endDate, string itemIds = "")
        {
            return Dal.Member.Instance.GetAvgAmount(startDate, endDate, itemIds);
        }

         /// <summary>
        /// 年平均贡献值
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public string GetAvgAmount(int year, string itemIds)
        {
            return Dal.Member.Instance.GetAvgAmount(year,itemIds);
        }
    }
}
