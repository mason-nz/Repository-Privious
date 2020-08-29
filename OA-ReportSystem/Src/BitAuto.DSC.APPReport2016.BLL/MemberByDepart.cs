using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class MemberByDepart
    {
        public static MemberByDepart Instance = new MemberByDepart();

        /// 获取最大日期
        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public int GetMaxDate(string ItemIds)
        {
            return Dal.MemberByDepart.Instance.GetMaxDate(ItemIds);
        }
        /// 获取新车会员按区域统计数据
        /// <summary>
        /// 获取新车会员按区域统计数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="yearMonth"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetData(string itemId, string yearMonth, string orderBy, int pageIndex, int pageSize, out int totalCount)
        {
            DataTable dt = Dal.MemberByDepart.Instance.GetData(itemId, yearMonth, orderBy, pageIndex, pageSize, out totalCount);
            return dt;
        }


        /// <summary>
        /// 获取某段时间内的会员数量
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="ItemIds">会员类型</param>
        /// <returns></returns>
        public DataTable GetDtFGL(int startDate, int endDate, string ItemIds)
        {
            return Dal.MemberByDepart.Instance.GetDtFGL(startDate, endDate, ItemIds);

        }
    }
}
