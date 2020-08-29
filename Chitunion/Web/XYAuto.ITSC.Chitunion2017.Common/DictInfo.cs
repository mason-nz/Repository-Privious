using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Common
{
    public class DictInfo
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeID(int typeID)
        {
            return Dal.DictInfo.Instance.GetDictInfoByTypeID(typeID);
        }

        /// <summary>
        /// 根据APP广告位的数量情况来查询分类信息，按照广告位数量倒序排列，若没有APP广告位，则不展现此分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetDictInfoByAPP()
        {
            return Dal.DictInfo.Instance.GetDictInfoByAPP();
        }
    }
}
