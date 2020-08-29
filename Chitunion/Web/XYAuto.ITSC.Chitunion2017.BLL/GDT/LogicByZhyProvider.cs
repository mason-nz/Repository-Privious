/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 13:20:30
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.WebService.Common;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Fund;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Report;

namespace XYAuto.ITSC.Chitunion2017.BLL.GDT
{
    /// <summary>
    /// 封装对智慧云接口的封装
    /// </summary>
    public class LogicByZhyProvider : CurrentOperateBase
    {
        private readonly AppSettingsForZhy _appSettings = AppSettingsForZhy.Instance;
        protected readonly string Timestamp;

        public LogicByZhyProvider()
        {
            Timestamp = SignUtility.ConvertDateTimeInt(DateTime.Now).ToString();
        }

        /// <summary>
        /// 需求状态发生变化，通知智慧云
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public ReturnValue DemandStatusNote(ToDemandNotes notes)
        {
            var retValue = VerifyOfNecessaryParameters(notes);
            if (retValue.HasError)
                return retValue;

            var status = GetOrganizeAdsStatus(notes.AuditStatus);
            if (status < 0)
                return CreateFailMessage(retValue, "1001", "审核状态有问题");

            var dicParams = GetSignatureParams()
                .Insert("OrganizeID", notes.OrganizeId)
                .Insert("OrganizeAdsID", notes.DemandBillNo)
                .Insert("OrganizeAdsStatus", status)
                .Insert("Reject", notes.Reject);

            var par = GetSortDicValue(dicParams);

            var postData = $"OrganizeID={notes.OrganizeId}&OrganizeAdsID={notes.DemandBillNo}&OrganizeAdsStatus={status}&Reject={notes.Reject}";

            var sign = GetSignatureText(dicParams);
            var requestUrl = _appSettings.ZhyApiUrl + $"organize/status?appkey={_appSettings.ZhyApiAppKey}&signature={sign}&timestamp={Timestamp}";
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespZhyBaseDto<dynamic>>
               (s => HttpClient.PostByFrom(requestUrl, postData), WebService.Common.Loger.ZhyLogger.Info);

            if (result == null || !result.Success)
            {
                return CreateFailMessage(retValue, "1002", result == null ? "接口未知错误" : result.ErrorMessage);
            }
            return CreateSuccessMessage(retValue, "0", "通知成功");
        }

        /// <summary>
        /// 账户资金变动通知智慧云
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public ReturnValue AccountFundsNote(ToAccountFundNotes notes)
        {
            var retValue = VerifyOfNecessaryParameters(notes);
            if (retValue.HasError)
                return retValue;
            var dicParams = GetSignatureParams()
              .Insert("OrganizeID", notes.OrganizeId)
              .Insert("OrganizeAdsID", notes.DemandBillNo)
              .Insert("MoneyTpe", (int)notes.MoneyTpe)
              .Insert("TradeType", (int)notes.TradeType)
              .Insert("TradeMoney", notes.TradeMoney)
              .Insert("TradeNo", notes.TradeNo);

            var postData = $"OrganizeID={notes.OrganizeId}&OrganizeAdsID={notes.DemandBillNo}" +
                           $"&MoneyTpe={(int)notes.MoneyTpe}&TradeMoney={notes.TradeMoney}&TradeNo={notes.TradeNo}" +
                           $"&TradeType={(int)notes.TradeType}";

            var sign = GetSignatureText(dicParams);
            var requestUrl = _appSettings.ZhyApiUrl + $"organize/movement?appkey={_appSettings.ZhyApiAppKey}&signature={sign}&timestamp={Timestamp}";
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespZhyBaseDto<dynamic>>
               (s => HttpClient.PostByFrom(requestUrl, postData), WebService.Common.Loger.ZhyLogger.Info);

            if (result == null || !result.Success)
            {
                return CreateFailMessage(retValue, "1003", result == null ? "接口未知错误" : result.ErrorMessage);
            }
            return CreateSuccessMessage(retValue, "0", "通知成功");
        }

        private Dictionary<string, object> GetSignatureParams()
        {
            return new Dictionary<string, object>
            {
                {"appkey", _appSettings.ZhyApiAppKey},
                {"appsecret", _appSettings.ZhyApiAppSecret},
                {"timestamp", Timestamp},
            };
        }

        private string GetPostJson(Dictionary<string, object> dicParams)
        {
            var dic = dicParams.Where(x => !x.Key.Equals("appkey") && !x.Key.Equals("appsecret")
                                           && !x.Key.Equals("timestamp"));
            return JsonConvert.SerializeObject(dic);
        }

        private string GetSignatureText(Dictionary<string, object> dicParams)
        {
            var dicParm = GetSortDicValue(dicParams);
            Loger.ZhyLogger.Info($"加密串：{dicParm}");
            return HttpUtility.UrlEncode(SignUtility.GetSignature(dicParm), Encoding.UTF8);
        }

        private string GetSortDicValue(Dictionary<string, object> dicParams)
        {
            var dictSortedAsc = dicParams.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            string signature = string.Empty;
            foreach (var item in dictSortedAsc)
            {
                signature += item.Value + "";
            }
            return signature;
        }

        /// <summary>
        /// 投放状态：草稿 = 0, 审核中 = 1,审核通过 = 2, 审核未通过 = 3,执行中 = 4,已完成 = 5,已撤销 = 6, 终止 = 7
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private int GetOrganizeAdsStatus(DemandAuditStatusEnum status)
        {
            switch (status)
            {
                case DemandAuditStatusEnum.Rejected:
                    return 3;//已驳回-->审核未通过
                case DemandAuditStatusEnum.PendingPutIn:
                    return 2;//待投放（审核通过）-->审核通过
                case DemandAuditStatusEnum.Puting:
                    return 4;//投放中-->执行中
                case DemandAuditStatusEnum.IsOver:
                    return 5;//已结束-->已完成
                case DemandAuditStatusEnum.Terminated:
                    return 7;//已终止-->终止
            }
            return -1;
        }
    }

    public static class DictionaryExt
    {
        public static Dictionary<string, object> Insert(this Dictionary<string, object> dictionary,
            string key, object value)
        {
            dictionary.Add(key, value);
            return dictionary;
        }
    }

    /// <summary>
    /// 智慧云接口枚举（慎用）
    /// </summary>
    public class ZhyEnum
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public enum ZhyTradeTypeEnum
        {
            充值 = 1,
            消费 = 2,
            回划 = 3
        }

        public enum ZhyMoneyTpeEnum
        {
            现金 = 1,
        }
    }
}