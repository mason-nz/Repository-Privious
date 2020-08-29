/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 17:58:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    public class GdtAccessToken

    {
        #region Instance

        public static readonly GdtAccessToken Instance = new GdtAccessToken();

        #endregion Instance

        public Entities.GDT.GdtAccessToken GetInfo(int relationType, int clientId)
        {
            return Dal.GDT.GdtAccessToken.Instance.GetInfo(relationType, clientId);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtAccountBalance> list,
            Entities.GDT.GdtAccountBalance entityWhere)
        {
            return Dal.GDT.GdtAccountBalance.Instance.InsertByGdtServer(list, entityWhere);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtAdGroup> list,
            Entities.GDT.GdtAdGroup entityWhere, int pageIndex)
        {
            return Dal.GDT.GdtAdGroup.Instance.InsertByGdtServer(list, entityWhere, pageIndex);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtCampaign> list,
            Entities.GDT.GdtCampaign entityWhere, int pageIndex)
        {
            return Dal.GDT.GdtCampaign.Instance.InsertByGdtServer(list, entityWhere, pageIndex);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtDailyRrport> list,
            Entities.GDT.GdtDailyRrport entityWhere, int pageIndex)
        {
            return Dal.GDT.GdtDailyRrport.Instance.InsertByGdtServer(list, entityWhere, pageIndex);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtHourlyRrport> list,
            Entities.GDT.GdtHourlyRrport entityWhere, int pageIndex)
        {
            return Dal.GDT.GdtHourlyRrport.Instance.InsertByGdtServer(list, entityWhere, pageIndex);
        }

        public int InsertByGdtServer(Entities.GDT.GdtRealtimeCost entity)
        {
            return Dal.GDT.GdtRealtimeCost.Instance.InsertByGdtServer(entity);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtStatementDaily> list,
            Entities.GDT.GdtStatementDaily entityWhere, int pageIndex)
        {
            return Dal.GDT.GdtStatementDaily.Instance.InsertByGdtServer(list, entityWhere, pageIndex);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtStatementsDetailed> list,
            Entities.GDT.GdtStatementsDetailed entityWhere, int pageIndex)
        {
            return Dal.GDT.GdtStatementsDetailed.Instance.InsertByGdtServer(list, entityWhere, pageIndex);
        }
    }
}