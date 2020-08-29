using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class MemberDaily
    {
        public static MemberDaily Instance = new MemberDaily();

        /// 查询天表-MemberDaily，获取当前时间的合作数和当年最大的合作数
        /// <summary>
        /// 查询天表-MemberDaily，获取当前时间的合作数和当年最大的合作数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataSet GetMemberHZForDay(int itemId)
        {
            return Dal.MemberDaily.Instance.GetMemberHZForDay(itemId);
        }
        /// 查询月表Member，某些会员和date时间之前的全部合作数据
        /// <summary>
        /// 查询月表Member，某些会员和date时间之前的全部合作数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="date"></param>
        public DataTable GetMemberHZForMonth(DateTime date, string itemids)
        {
            return Dal.MemberDaily.Instance.GetMemberHZForMonth(date, itemids);
        }
    }
}
