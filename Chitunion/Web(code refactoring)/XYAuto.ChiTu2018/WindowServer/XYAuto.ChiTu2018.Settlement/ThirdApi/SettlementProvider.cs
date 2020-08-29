using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundsmanagementPublisher.Client;
using FundsmanagementPublisher.Models;
using XYAuto.ChiTu2018.Infrastructure.Extensions;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Settlement.Extensions;
using XYAuto.ChiTu2018.Settlement.ThirdApi.Config;
using XYAuto.CTUtils.Image.FastDFS.ToolBox.Config;

namespace XYAuto.ChiTu2018.Settlement.ThirdApi
{
    /// <summary>
    /// 注释：SettlementProvider
    /// 作者：lix
    /// 日期：2018/5/22 11:34:05
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class SettlementProvider : VerifyOperateBase
    {
        private readonly KrFundsConfigSection _configSection;
        public SettlementProvider()
        {
            _configSection = SectionInvoke<KrFundsConfigSection>.GetConfig(KrFundsConfigSection.SectionName);
        }

        public decimal Settlement(string startDate, string endDate, int pageSize)
        {
            var query = new SettlementQueryPageInput()
            {
                AccessAppId = _configSection.AppId,
                BeginCreateTime = Convert.ToDateTime(startDate),
                EndCreateTime = Convert.ToDateTime(endDate),
                PageIndex = 1,
                PageSize = pageSize,
                PaymentChannel = PaymentChannel.支付宝,
                PaymentType = PaymentType.线上,
                Status = SettlementStatus.结算成功
            };

            $"Settlement pageIndex {query.PageIndex} start".LogInfo();

            var respTp = new SettlementClient().GetPage(query, XYAuto.CTUtils.Log.Log4NetHelper.Default().Info);

            return GetSettlementNextPage(query, respTp.Data.Item1, respTp.Data.Item2);
        }

        private decimal GetSettlementNextPage(SettlementQueryPageInput query, int totalCount, List<SettlementDto> list)
        {
            var sumPrice = new List<decimal>();
            sumPrice.Add(list.Sum(s => s.ActualAmount));
            var page = query.PageIndex;
            var offsetCount = GetOffsetPageCount(totalCount, query.PageSize);
            if (query.PageIndex == offsetCount)
            {
                return sumPrice.Sum();
            }
            $"Settlement GetSettlementNextPage offsetCount is {offsetCount}".LogInfo();
            var client = new SettlementClient();
            for (var i = page + 1; i <= offsetCount; i++)
            {
                query.PageIndex = i;
                $"Settlement GetSettlementNextPage pageIndex {query.PageIndex} start".LogInfo();
                var respTp = client.GetPage(query, XYAuto.CTUtils.Log.Log4NetHelper.Default().Info);
                if (!respTp.Success || respTp.Data.Item1 == 0)
                {
                    break;
                }
                sumPrice.Add(respTp.Data.Item2.Sum(s => s.ActualAmount));
            }
            return sumPrice.Sum();
        }

    }
}
