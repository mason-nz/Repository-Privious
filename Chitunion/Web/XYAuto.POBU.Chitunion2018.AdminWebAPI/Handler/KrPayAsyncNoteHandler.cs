using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Xy.ToolBox.Config;
using XY.Framework.Messaging.Bus;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Config;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.KrProxy;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Kr;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Handler
{
    public class KrPayAsyncNoteHandler : ISubscribeHandler
    {
        private readonly KrFundsConfigSection _configSection = SectionInvoke<KrFundsConfigSection>.GetConfig(KrFundsConfigSection.SectionName);
        public void Handle(MessageData message)
        {
            Loger.Log4Net.Info($"库容支付转账接口异步回调通知");
            var dataJson = JsonConvert.SerializeObject(message.Data);
            Loger.ZhyLogger.Info($"库容支付转账接口异步回调通知-1：{JsonConvert.SerializeObject(message)}");
            Loger.ZhyLogger.Info($"库容支付转账接口异步回调通知-2，Data：{dataJson}");

            var data = JsonConvert.DeserializeObject<RespDisbursementStatusMessage>(dataJson);
            if (data == null || data.Data == null)
            {
                Loger.ZhyLogger.Info($"库容支付转账接口异步回调通知-3:返回信息错误，JsonConvert.DeserializeObject<RespDisbursementStatusMessage> 失败");
                throw new Exception("message.Data 返回信息错误 data == null");
            }
            var retValue = new ReturnValue();
            var payStatus = data.Data.Status == DisbursementStatus.结算成功
                ? WithdrawalsStatusEnum.已支付
                : WithdrawalsStatusEnum.支付失败;
            if (_configSection.IsTestPayCallBack)
            {
                payStatus = WithdrawalsStatusEnum.支付失败;
            }
            var remark = new KrErrorMessageProvider().GetKrBaseDto(data.Data.Remark).ErrorMessage;
            retValue = new WithdrawalsProvider(new ConfigEntity(), new ReqWithdrawalsDto())
                .AuditPayResult(retValue, data.Data.DisbursementNo, remark, dataJson, payStatus);

            if (retValue.HasError)
            {
                Loger.ZhyLogger.Info($"库容支付转账接口异步回调通知-4:{JsonConvert.SerializeObject(retValue)}");
            }
        }

        public string[] SubscribeTags { get; } = { "fund.disbursementstatus.tag" };
    }
}