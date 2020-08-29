/********************************************************
*创建人：lixiong
*创建时间：2017/8/23 11:21:31
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Web;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Infrastruction.Cache;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    public class GdtAccountInfo
    {
        #region Instance

        public static readonly GdtAccountInfo Instance = new GdtAccountInfo();

        #endregion Instance

        public List<Entities.GDT.GdtAccountInfo> GetList(int accountId)
        {
            return Dal.GDT.GdtAccountInfo.Instance.GetList(accountId);
        }

        public List<Entities.GDT.GdtAccountInfo> GetList()
        {
            return Dal.GDT.GdtAccountInfo.Instance.GetList(0);
            //return CacheHelper<List<Entities.GDT.GdtAccountInfo>>.Get(HttpRuntime.Cache,
            //   () => "xy.chitu.gdt.accountinfo",
            //   () => Dal.GDT.GdtAccountInfo.Instance.GetList(0), null, 30);
        }

        public void RemoveCacheKey(string cacheKey = "xy.chitu.gdt.accountinfo")
        {
            CacheHelper<dynamic>.RemoveAllCache(cacheKey);
        }

        public List<Entities.GDT.GdtDemandRelation> GetAccountId(int demandBillNo)
        {
            return Dal.GDT.GdtDemandRelation.Instance.GetAccountId(demandBillNo);
        }

        /// <summary>
        /// 广点通-每天定时修改需求过期状态，：需求过期通知，
        /// </summary>
        /// <returns></returns>
        public List<Entities.GDT.GdtDemandStatusNotes> GetDemandStatusNoteses()
        {
            return Dal.GDT.GdtDemand.Instance.GetDemandStatusNoteses();
        }

        /// <summary>
        /// 获取广告组列表
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="configuredStatus">客户设置状态</param>
        /// <param name="systemStatus">系统状态</param>
        /// <returns></returns>
        public List<Entities.GDT.GdtAdGroup> GetAdGroupList(int accountId, int configuredStatus, int systemStatus)
        {
            return Dal.GDT.GdtAdGroup.Instance.GetList(accountId, configuredStatus, systemStatus);
        }
    }
}