/********************************************************
*创建人：lixiong
*创建时间：2017/8/25 11:48:46
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Linq;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Provider
{
    /// <summary>
    /// 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云
    /// </summary>
    internal class DemandStatusNotesSchduler
    {
        private static readonly LogicByZhyProvider LogicByZhyProvider = new LogicByZhyProvider();

        /// <summary>
        /// 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云
        /// </summary>
        public class DemandOverdueNotesSchduler : IJob
        {
            public void Execute()
            {
                Loger.ZhyLogger.Info($" DemandStatusNotesSchduler 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云 start..");
                var demandBillNoList = BLL.GDT.GdtAccountInfo.Instance.GetDemandStatusNoteses();
                if (demandBillNoList.Any())
                {
                    //循环调用通知
                    var request = new ToDemandNotes();
                    demandBillNoList.ForEach(item =>
                    {
                        request.OrganizeId = item.OrganizeId;
                        request.DemandBillNo = item.DemandBillNo;
                        request.AuditStatus = item.AuditStatus;
                        var retValue = LogicByZhyProvider.DemandStatusNote(request);
                        if (retValue.HasError)
                        {
                            Loger.Log4Net.Error($" DemandStatusNotesSchduler 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云 is error:{retValue.Message}");
                            Loger.ZhyLogger.Info($" DemandStatusNotesSchduler 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云 is error:{retValue.Message}");
                        }
                        else
                        {
                            Loger.ZhyLogger.Info($" DemandStatusNotesSchduler需求达到开始时间，自动设置为【投放中】状态，并通知智慧云... 推送成功：{ JsonConvert.SerializeObject(item)}..");
                        }
                    });
                }

                Loger.ZhyLogger.Info($" DemandStatusNotesSchduler 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云 complete..");
            }
        }
    }
}