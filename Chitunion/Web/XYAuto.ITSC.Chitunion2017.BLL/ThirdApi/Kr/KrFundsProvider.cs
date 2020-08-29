using System;
using FundsmanagementPublisher.Models;
using Newtonsoft.Json;
using Xy.ToolBox.Config;
using XY.Web.ApiProxy.Common;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Config;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Request.Kr;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Exceptions;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Kr
{
    /// <summary>
    /// 库容-资金管理的接口-代理类
    /// </summary>
    public class KrFundsProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly KrFundsConfigSection _configSection;

        public KrFundsProvider(ConfigEntity configEntity)
        {
            _configEntity = configEntity;
            _configSection = SectionInvoke<KrFundsConfigSection>.GetConfig(KrFundsConfigSection.SectionName);
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="withdrawalsDetail"></param>
        /// <returns></returns>
        public ReturnValue Disbursement(
            Entities.LETask.LeWithdrawalsDetail withdrawalsDetail)
        {
            var disbursementInput = GetDisbursementInput(withdrawalsDetail);
            var retValue = VerifyOfNecessaryParameters(disbursementInput);
            if (retValue.HasError)
                return retValue;
            if (disbursementInput.CustomerAccount.CardType == CardType.对私 &&
                string.IsNullOrWhiteSpace(disbursementInput.CustomerAccount.IdNo))
            {
                return CreateFailMessage(retValue, "50002", "对私类型的业务，必须填好证件号");
            }
            if (_configSection.IsTestPay)
            {
                disbursementInput.Amount = 0.1m;
                disbursementInput.TaxAmount = 0m;
                disbursementInput.ActualAmount = 0.1m;
            }
            try
            {
                var resp = new FundsmanagementPublisher.Client.DisbursementClient().Create(disbursementInput);
                //修改同步接口返回
                var respSyncResult = JsonConvert.SerializeObject(resp);

                if (!resp.Success)
                {
                    Loger.ZhyLogger.Error($"库容转账接口错误Disbursement:{respSyncResult}");
                    return CreateFailMessage(retValue, "50001", $"转账接口同步返回错误:{resp.ErrorMessage}");
                }
                //todo:现在耦合度太强，LE_DisbursementPay 插入一条关联数据，为了记录
                if (resp.Data != null)
                {
                    Dal.LETask.LeDisbursementPay.Instance.Insert(new LeDisbursementPay()
                    {
                        WithdrawalsId = withdrawalsDetail.RecID,
                        BizNo = resp.Data.BizNo,
                        BizDisbursementNo = resp.Data.BizDisbursementNo,
                        ContractNo = resp.Data.ContractNo,
                        CreateTime = DateTime.Now,
                        DisbursementNo = resp.Data.DisbursementNo,
                        Remark = resp.Data.Remark,
                        Status = 0
                    }, respSyncResult);
                }
                retValue.ReturnObject = respSyncResult;
            }
            catch (Exception exception)
            {
                Loger.ZhyLogger.Error($"库容转账接口错误:{exception.Message}{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}");
                //todo:应该锁定该申请单
                throw new KrPayCreateDisbursementException("创建结算订单接口异常");
                //return CreateFailMessage(retValue, "50002", $"创建结算订单接口异常");
            }

            return retValue;
        }

        private DisbursementInput GetDisbursementInput(
            Entities.LETask.LeWithdrawalsDetail withdrawalsDetail)
        {
            var disb = new DisbursementInput()
            {
                AppId = _configSection.AppId,
                Amount = withdrawalsDetail.WithdrawalsPrice,//付款金额
                ActualAmount = withdrawalsDetail.PracticalPrice,//实际付款
                TaxAmount = withdrawalsDetail.IndividualTaxPeice,//个税
                BizDisbursementNo = withdrawalsDetail.RecID.ToString(),
                //BizNo = withdrawalsDetail.OrderID.ToString(),
                ContractNo = withdrawalsDetail.RecID.ToString(),
                TradeClassificationCode = _configSection.TradeClassificationCode,
                Remark = _configSection.PayRemark,
                CustomerAccount = new FundsmanagementPublisher.Models.CustomerAccountInput()
                {
                    BizCustomerNo = _configEntity.LoginUser.UserID.ToString(),
                    CustomerName = withdrawalsDetail.PayeeAccount,//客户名称 应与支付宝名称一样 否则付款不成功
                    CardNo = withdrawalsDetail.PayeeAccount,
                    AccountType = AccountType.支付宝,
                    IdType = IdType.身份证号,
                    IdNo = _configEntity.IdentityNo,
                    CardType = _configEntity.LoginUser.Type == (int)UserTypeEnum.企业
                                    ? CardType.对公 : CardType.对私,
                    Mobile = _configEntity.LoginUser.Mobile,

                }
            };

            return disb;
        }

    }
}
