/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 16:53:20
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Linq;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Distribute
{
    public class DistributeProvider : CurrentOperateBase
    {
        public DistributeProvider()
        {
        }

        #region 获取详情相关

        /// <summary>
        /// 获取物料详情信息
        /// </summary>
        /// <param name="materielId"></param>
        /// <param name="distributeType"></param>
        /// <returns></returns>
        public RespMaterielInfoDto GetMaterielInfo(int materielId, int distributeType)
        {
            var info = Dal.Distribute.MaterielDistributeDetailed.Instance.GetMaterielInfo(materielId, distributeType);

            if (info != null)
            {
                var tp = DistributeProfile.GetDistributeUser(info, distributeType);

                var resp = AutoMapper.Mapper.Map<Entities.Distribute.MaterielInfo, RespMaterielInfoDto>(info);
                resp.DistributeTime = tp.Item2;
                resp.DistributeUser = tp.Item1;
                resp.DistributeTypeName = ((DistributeTypeEnum)distributeType).GetEnumDesc();

                return resp;
            }
            return null;
        }

        #endregion 获取详情相关

        #region 渠道相关

        /// <summary>
        /// 获取渠道列表
        /// </summary>
        /// <param name="distributeId">分发id</param>
        /// <param name="distributeIds"></param>
        /// <returns></returns>
        public List<MaterielChannelDetailed> GetChannelDetaileds(int distributeId, List<int> distributeIds)
        {
            return Dal.Distribute.MaterielChannelDetailed.Instance.GetList(distributeId, distributeIds, 0);
        }

        #endregion 渠道相关

        public List<Entities.Distribute.MaterielTemp> GetArticleInfo(XyAttrTypeEnum xyAttrType, string date,
            int topPageSize = 100, int startId = 0)
        {
            return Dal.Distribute.MaterielInfo.Instance.GetArticleInfo(xyAttrType, date, topPageSize, startId);
        }

        public List<Entities.Distribute.MaterielTemp> GetArticleInfo(DistributeQuery<Entities.Distribute.MaterielTemp> query)
        {
            return Dal.Distribute.MaterielInfo.Instance.GetArticleInfo(query);
        }

        public MaterielDistributeTotal GetDistributeTotals(string startDate, string endDate, string sqlWhere)
        {
            var resp = new MaterielDistributeTotal();
            var totalList = Dal.Distribute.MaterielDistributeDetailed.Instance.GetDistributeTotals(startDate, endDate, sqlWhere);
            resp.TotalInquiryNumber = totalList.Sum(s => s.TotalInquiryNumber);
            resp.TotalTelConnectNumber = totalList.Sum(s => s.TotalTelConnectNumber);
            resp.TotalSessionNumber = totalList.Sum(s => s.TotalSessionNumber);
            resp.TotalForwardNumber = totalList.Sum(s => s.TotalForwardNumber);
            resp.TotalMateriel = totalList.Sum(s => s.TotalMateriel);
            resp.TotalDistribute = totalList.Sum(s => s.TotalDistribute);
            resp.TotalClue = (resp.TotalInquiryNumber + resp.TotalTelConnectNumber + resp.TotalSessionNumber);
            return resp;
        }

        public Entities.Distribute.MaterielExtend GetMaterielInfoByBodyArticleId(int bodyArticleId)
        {
            return Dal.Distribute.MaterielInfo.Instance.GetMaterielInfoByBodyArticleId(bodyArticleId);
        }
    }
}