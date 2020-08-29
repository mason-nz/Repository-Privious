using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundsmanagementPublisher.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using FundsmanagementPublisher.Models;
using Newtonsoft.Json;
using XY.Web.ApiProxy.Common;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.KrProxy;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Kr;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using DisbursementStatus = XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.KrProxy.DisbursementStatus;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class WithdrawalsProviderTest
    {
        //public void ZhySignatureProvider

        public WithdrawalsProviderTest()
        {

            MediaMapperConfig.Configure();
        }

        [TestMethod]
        public void DisbursementTest()
        {
            //计算secret = MD5({appkey}{appsecret}{yyyy-MM-dd});


            var retValue = new KrFundsProvider(new ConfigEntity()
            {
                IdentityNo = "",
                LoginUser = new XYAuto.ITSC.Chitunion2017.Common.LoginUser
                {
                    Type = (int)CardType.对公,
                    Mobile = "15001180260",
                    UserID = 43
                }
            }).Disbursement(new LeWithdrawalsDetail()
            {
                WithdrawalsPrice = 0.01m,
                PayeeAccount = "15001180260",
                RecID = 10,

            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [Description("个税计算器")]
        [TestMethod]
        public void PriceCalcTest()
        {
            var request = new ReqWithdrawalsDto()
            {
                WithdrawalsPrice = 6000
            };
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = 43,
            }, request).PriceCalc();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [Description("提现申请")]
        [TestMethod]
        public void WithdrawalsApplyTest()
        {
            var request = new ReqWithdrawalsDto()
            {
                WithdrawalsPrice = 51,
                Mobile = "13611111111",
                MsgCode = "666666"
            };
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = 5,//申请人
                UserType = UserTypeEnum.个人
            }, request).Withdrawals();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("提现申请-审核")]
        [TestMethod]
        public void AuditTest()
        {
            var request = new ReqWithdrawalsAuditDto()
            {
                WithdrawalsId = 4
            };
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = 1295,
            }, new ReqWithdrawalsAgainDto()).Audit(request);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("提现申请-审核-回调")]
        [TestMethod]
        public void AuditPayResultTest()
        {
            var disbursementNo = "2018011819225081811504";
            var status = WithdrawalsStatusEnum.已支付;
            var retValue = new ReturnValue();
            retValue = new WithdrawalsProvider(new ConfigEntity(), new ReqWithdrawalsAgainDto())
                .AuditPayResult(retValue, disbursementNo, "", "", status);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }
        [TestMethod]
        public void KrPayAsyncNoteHandlerTest()
        {
            var dataJson = "{\"Data\":{\"DisbursementNo\":\"2018011714170871372583\",\"AppId\":\"chitulianmeng\",\"AppName\":\"赤兔联盟\",\"BizDisbursementNo\":\"19\",\"BizNo\":null,\"Status\":12,\"StatusText\":\"结算成功\",\"Remark\":\"提现申请\"},\"Sign\":\"AB68D147A7633993E8FB3FD9B96B3AB1\"}";
            var data = JsonConvert.DeserializeObject<RespDisbursementStatusMessage>(dataJson);
            var retValue = new ReturnValue();
            retValue = new WithdrawalsProvider(new ConfigEntity(), new ReqWithdrawalsDto())
                .AuditPayResult(retValue, data.Data.DisbursementNo, "", dataJson
                , data.Data.Status == DisbursementStatus.结算成功 ? WithdrawalsStatusEnum.已支付 : WithdrawalsStatusEnum.支付失败);

            if (retValue.HasError)
            {
                Loger.ZhyLogger.Info($"库容支付转账接口异步回调通知-4:{JsonConvert.SerializeObject(retValue)}");
            }
        }


        [TestMethod]
        public void LogTest()
        {
            //    Loger.Log4Net.Info($" this is Log4Net info");
            //    Loger.ZhyLogger.Info($" this is ZhyLogger info");
            //    Loger.ZhyLogger.Error($" this is ZhyLogger Error");
            //Console.WriteLine(AppClient.Current.GetProjectBaseUrl("FundsmanagementPublisher"));

            //Trace.TraceError($"时间：{DateTime.Now}日志编号：");
            
            
        }




        [Description("收入管理-提现操作-校验点击按钮")]
        [TestMethod]
        public void VerifyWithdrawalsClickTest()
        {
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = 43,
            }, new ReqWithdrawalsDto()).VerifyWithdrawalsClick();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void GetIndividualTaxPeiceTest()
        {
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = 43,
            }, new ReqWithdrawalsDto()).GetIndividualTaxPeice(70000, 0);
            Console.WriteLine(retValue);
        }
        [TestMethod]
        public void VerifyWithdrawalsAgainTest()
        {
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                //CreateUserId = 43,
            }, new ReqWithdrawalsAgainDto()
            {
                WithdrawalsId = 61
            }).VerifyWithdrawalsAgain();
            Console.WriteLine(retValue);
        }

        [TestMethod]
        public void GetAuditDetailsTest()
        {
            var info = new WithdrawalsProvider(new ConfigEntity(), new ReqWithdrawalsAgainDto()).GetAuditDetails(40);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void GetKrBaseDtoTest()
        {


            var message = "转账失败:{\"Success\":false,\"Data\":null,\"ErrorCode\":\"10000\",\"ErrorMessage\":\"该手机号对应多个支付宝账户，请传入收款方姓名确定正确的收款账号\",\"ErrorDetail\":null}";

            Console.WriteLine(message.IndexOf(':'));

            //message = message.Remove(0, message.IndexOf(':') + 1);
            //Console.WriteLine(message);
            var info = new KrErrorMessageProvider().GetKrBaseDto(message);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }
    }


}
