/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 19:48:36
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.Entities.Channel;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8
{
    public class ChannelInfoProvider : CurrentOperateBase
    {
        public ChannelInfoProvider()
        {
        }

        public dynamic Query(RequestGetChannelDto contextGet)
        {
            var retValue = VerifyOfNecessaryParameters(contextGet);
            if (retValue.HasError)
                return retValue;

            var list = BLL.Media.TwoBarCodeHistory.Instance.GetChannelList(new ChannelQuery<RespChannelListDto>()
            {
                //ChannelId = contextGet.ChannelId,
                MediaId = contextGet.MediaId,
                AdPosition1 = contextGet.AdPosition1,
                AdPosition2 = contextGet.AdPosition2,
                AdPosition3 = contextGet.AdPosition3,
                CooperateDate = contextGet.CooperateDate
            });

            //缓存
            var dicCache = new Dictionary<int, IEnumerable<PolicyInfo>>();

            list.ForEach(item =>
            {
                item.FinalCostPrice = GetPolicyList(item.ChannelID, item.CostPriceReference, item.AlreadyPayMoney, dicCache);
            });

            //用完即清空
            dicCache.Clear();

            return new { List = list };
        }

        private IEnumerable<PolicyInfo> CachePolicy(Dictionary<int, IEnumerable<PolicyInfo>> dicCache, int channelKey)
        {
            if (dicCache.ContainsKey(channelKey))
                return dicCache[channelKey];
            var policyList = Dal.Channel.Instance.GetList(channelKey);
            if (policyList.Count == 0)
            {
                return null;
            }
            dicCache.Add(channelKey, policyList);
            return policyList;
        }

        /// <summary>
        /// 根据渠道ChannelId,三个纬度查找成本金额，这里默认只能查找到一个金额,这里的总金额是取的订单表已成交的总金额
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public decimal GetPolicyList(ChannelQuery<RespChannelListDto> query)
        {
            var policyList = Dal.Media.TwoBarCodeHistory.Instance.GetChannelListForChannelId(query).FirstOrDefault();
            if (policyList == null)
                return 0;//没有找到相关的信息
            //缓存
            var dicCache = new Dictionary<int, IEnumerable<PolicyInfo>>();
            var constPrice = GetPolicyList(policyList.ChannelID, policyList.CostPriceReference, policyList.AlreadyPayMoney, dicCache);
            //用完即清空
            dicCache.Clear();
            return constPrice;
        }

        public decimal GetPolicyList(int channelId, decimal constPrice, decimal totlePrice, Dictionary<int, IEnumerable<PolicyInfo>> dicCache)
        {
            var policyList = CachePolicy(dicCache, channelId);

            if (policyList == null)
                return constPrice;

            //todo:先找到政策：和constPrice作比较, <= 注意：现在的数据格式不是一对多存储，是平行式存储
            var policyListItem = policyList.Where(s => s.Quota < totlePrice).OrderByDescending(s => s.Quota);

            if (!policyListItem.Any())
            {
                Loger.Log4Net.InfoFormat($"没有找到政策,channelId:{channelId}&constPrice:{constPrice}");
                return constPrice;
            }
            //todo:再找对应的规则，折扣
            //判断单个金额的满足的折扣

            //单个帐号金额 < 15000 (constPrice 现在需要和成本金额比较)
            var daYuPolicy = policyListItem.Where(s => s.SingleAccountSum < constPrice && s.SingleAccountSumType == (int)SingleAccountSumTypeEnum.大于)
                .OrderByDescending(s => s.SingleAccountSum);
            if (daYuPolicy.Any())
            {
                //满足存在大于，折扣的线路
                return GetSalePriceProxy(daYuPolicy, constPrice);
            }
            else
            {
                //继续找小于()
                var xiaoYuPolicy = policyListItem.Where(s => s.SingleAccountSum >= constPrice && s.SingleAccountSumType == (int)SingleAccountSumTypeEnum.小于等于)
                   .OrderByDescending(s => s.SingleAccountSum);
                //var xiaoYuPolicy = policyListItem.Where(s => s.SingleAccountSum <= constPrice && s.SingleAccountSumType == (int)SingleAccountSumTypeEnum.小于等于)
                //    .OrderByDescending(s => s.SingleAccountSum);
                if (!xiaoYuPolicy.Any())
                {
                    //不满足，退出,依然要计算，政策的折扣
                    Loger.Log4Net.InfoFormat($"GetPolicyList..【xiaoYuPolicy】 没有");
                    var sale = xiaoYuPolicy.FirstOrDefault();
                    if (sale != null) return constPrice - constPrice * (sale.PurchaseDiscount * 0.01m);
                    return constPrice;
                }
                return GetSalePriceProxy(xiaoYuPolicy, constPrice);
            }
        }

        private decimal GetSalePriceProxy(IEnumerable<PolicyInfo> prInfos, decimal constPrice)
        {
            // 当前这一步，不管后面的条件满足与否，都要乘以采购折扣
            var enumerable = prInfos as PolicyInfo[] ?? prInfos.ToArray();
            var itemPrice = (enumerable.Select(s => s.PurchaseDiscount).FirstOrDefault() * 0.01m) * constPrice;
            //todo:继续找返点类型+返点时间
            var rebateType1 = enumerable.Where(s => s.RebateType1 == (int)RebateType1Enum.返现);
            //todo:返货，无 不处理，只处理返现
            var policyInfos = rebateType1 as PolicyInfo[] ?? rebateType1.ToArray();
            if (!policyInfos.Any())
            {
                Loger.Log4Net.InfoFormat($"GetSalePriceProxy..【继续找返点类型+返点时间-->返货，无 不处理，只处理返现】 没有");
                //var sale = policyInfos.FirstOrDefault();
                //if (sale != null) return constPrice - constPrice * sale.PurchaseDiscount * 0.01m;
                return itemPrice;
            }
            //返现+比例
            var rebateType2Scale = policyInfos.Where(s => s.RebateType2 == (int)RebateType2Enum.比例
                                                         && s.RebateDateType == (int)RebateDateTypeEnum.立返);
            var infos = rebateType2Scale as PolicyInfo[] ?? rebateType2Scale.ToArray();
            if (infos.Any())
            {
                //按照比例，立即返回成本价的比例金额
                Loger.Log4Net.InfoFormat($"GetSalePriceProxy..【返现+比例】 开始计算");

                var sale = infos.FirstOrDefault();
                if (sale != null) return itemPrice - itemPrice * sale.RebateValue;
                return itemPrice;
            }
            else
            {
                //返现+金额
                var rebateType2Amount = policyInfos.Where(s => s.RebateType2 == (int)RebateType2Enum.金额
                                                             && s.RebateDateType == (int)RebateDateTypeEnum.立返);
                var type2Amount = rebateType2Amount as PolicyInfo[] ?? rebateType2Amount.ToArray();
                if (!type2Amount.Any())
                {
                    //有，接着计算,返回金额
                    Loger.Log4Net.InfoFormat($"GetSalePriceProxy..【返现+金额】 没有");
                    //var sale = type2Amount.FirstOrDefault();
                    //if (sale != null) return itemPrice - itemPrice * sale.PurchaseDiscount + sale.RebateValue;
                    return itemPrice;
                }
                //最后计算：itemPrice - itemPrice * PurchaseDiscount + RebateValue
                var sale = type2Amount.FirstOrDefault();
                if (sale != null) return itemPrice - sale.RebateValue;
                return itemPrice;
            }
        }
    }
}