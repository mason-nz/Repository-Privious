/********************************************************
*创建人：lixiong
*创建时间：2017/8/25 11:48:46
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData.Provider
{
    /// <summary>
    /// 需求过期状态通知
    /// </summary>
    internal class DemandStatusNotesSchduler
    {
        private static readonly LogicByZhyProvider LogicByZhyProvider = new LogicByZhyProvider();

        /// <summary>
        /// 需求过期处理
        /// </summary>
        public class DemandOverdueNotesSchduler : IJob
        {
            public void Execute()
            {
                BLL.Loger.GdtLogger.Info($" DemandStatusNotesSchduler DemandOverdueNotesSchduler 需求过期处理 DemandOverdue start..");
                var demandBillNoList = BLL.GDT.GdtAccountInfo.Instance.GetDemandStatusNoteses(DemandStatusNotesTypeEnum.DemandOverdue);
                if (demandBillNoList.Any())
                {
                    //循环调用通知
                    var request = new ToDemandNotes()
                    {
                    };
                    demandBillNoList.ForEach(item =>
                    {
                        request.OrganizeId = item.OrganizeId;
                        request.DemandBillNo = item.DemandBillNo;
                        request.AuditStatus = item.AuditStatus;
                        var retValue = LogicByZhyProvider.DemandStatusNote(request);
                        if (retValue.HasError)
                        {
                            BLL.Loger.ZhyLogger.Error($" DemandStatusNotesSchduler DemandOverdueNotesSchduler  需求过期处理 DemandOverdue is error:{retValue.Message}");
                        }
                        else
                        {
                            BLL.Loger.GdtLogger.Info($" DemandStatusNotesSchduler DemandOverdueNotesSchduler 需求过期处理 DemandOverdue 推送成功：{ JsonConvert.SerializeObject(item)}..");
                        }
                    });
                }

                BLL.Loger.GdtLogger.Info($" DemandStatusNotesSchduler DemandOverdueNotesSchduler 需求过期处理 DemandOverdue complete..");
            }
        }

        /// <summary>
        /// 广告组状态发生变化处理
        /// </summary>
        public class DemandAdGroupNotesSchduler : IJob
        {
            public void Execute()
            {
                BLL.Loger.GdtLogger.Info($" DemandStatusNotesSchduler DemandAdGroupNotesSchduler 广告组状态发生变化处理 start..");

                var adGroupList = BLL.GDT.GdtAccountInfo.Instance.GetAdGroupList((int)ConfiguredStatusEnum.暂停);
                var request = new ToDemandNotes()
                {
                    AuditStatus = DemandAuditStatusEnum.PendingPutIn,
                };
                adGroupList.ForEach(item =>
                {
                    var demandBillNoList = BLL.GDT.GdtAccountInfo.Instance.GetDemandStatusNotesesByAdGroup(item.AdgroupId);

                    demandBillNoList.ForEach(s =>
                    {
                        request.OrganizeId = s.OrganizeId;
                        request.DemandBillNo = s.DemandBillNo;
                        request.AuditStatus = s.AuditStatus;

                        var retValue = LogicByZhyProvider.DemandStatusNote(request);
                        if (retValue.HasError)
                        {
                            BLL.Loger.ZhyLogger.Error($" DemandStatusNotesSchduler DemandAdGroupNotesSchduler 广告组状态发生变化处理 is error:{retValue.Message}");
                        }
                        else
                        {
                            BLL.Loger.GdtLogger.Info($" DemandStatusNotesSchduler DemandAdGroupNotesSchduler 广告组状态发生变化处理 推送成功：{ JsonConvert.SerializeObject(item)}..");
                        }
                    });
                });
                BLL.Loger.GdtLogger.Info($" DemandStatusNotesSchduler DemandAdGroupNotesSchduler 广告组状态发生变化处理 DemandOverdue complete..");
            }
        }
    }
}