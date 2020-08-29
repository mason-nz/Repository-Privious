using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class BrandAd
    {
        public static readonly BrandAd Instance = new BrandAd();

        /// .品牌广告合作文字信息（合作概况 + 合作排名）
        /// <summary>
        /// .品牌广告合作文字信息（合作概况 + 合作排名）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet GetBrandOverViewData()
        {
            return Dal.BrandAd.Instance.GetBrandOverViewData();
        }

        /// 获取品牌柱状图数据
        /// <summary>
        /// 获取品牌柱状图数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetBrandBarData()
        {
            return Dal.BrandAd.Instance.GetBrandBarData();
        }

        /// 获取品牌排名数据
        /// <summary>
        /// 获取品牌排名数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="orderType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBrandRankData(string orderBy, int pageIndex, int pageSize, out int totalCount)
        {
            string strOrder = "";
            if (!string.IsNullOrEmpty(orderBy))
            {
                strOrder = "a." + orderBy;
            }
            else
            {
                strOrder = "a.Amount DESC";  //默认：合作金额倒序排名
            }
            DataTable dt = Dal.BrandAd.Instance.GetBrandRankData(strOrder, pageIndex, pageSize, out totalCount);
            return dt;
        }
    }
}
