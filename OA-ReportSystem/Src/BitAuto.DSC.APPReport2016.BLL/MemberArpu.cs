using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class MemberArpu
    {
        public static MemberArpu Instance = new MemberArpu();

        private MemberArpu()
        { }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public DataTable GetData(int year, int quarter, string itemIds)
        {
            return Dal.MemberArpu.Instance.GetData(year, quarter, itemIds);
        }
    }
}
