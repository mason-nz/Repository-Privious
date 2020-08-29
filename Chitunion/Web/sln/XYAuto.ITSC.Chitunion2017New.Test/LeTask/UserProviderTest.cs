using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.User;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.App;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class UserProviderTest
    {
        public UserProviderTest()
        {

            MediaMapperConfig.Configure();//配置autpMapper
        }

        [TestMethod]
        public void ModifyAttestation()
        {
            var retValue = new UserProvider(new ConfigEntity()
            {

                CreateUserId = 48,
                LoginUser = new LoginUser() { Category = 29002 }
            }, new ReqUserDto()
            {
                Mobile = "15369302962",
                MobileCode = "6481"
            }).ModifyAttestation();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void ReportTest()
        {
            var reqReportApp = new ReqReportAppDto()
            {
                //UserId = 1,
                //ActivationTime = DateTime.Now,
                AllowLocationInfo = "北京",
                AllowNoticeInfo = "允许通知",
                AndroidId = "55678",
                AppVersion = "v1.3",
                Carrier = "联通",
                Channel = "app",
                IMEI = "34556787654",
                IMSI = "34556787654",
                PhoneModel = "iphone x",
                ScreenResolution = "1080x2080",
                SystemVersion = "ios 11",
                Network = "wifi"
            };
            var retValue = new ReportAppInfoProvider(reqReportApp).Report();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }
        [TestMethod]
        public void AddProfit()
        {
            var isSuccess = Chitunion2017.BLL.Profit.Profit.Instance.AddProfit(1435, ProfitTypeEnum.提现申请海报奖励, $"提现申请成功", 1, DateTime.Now, 1);
            if (!isSuccess)
            {
                Console.WriteLine("提现申请奖励错误");
            }
        }

        [TestMethod]
        public void GetUserInfoByToken()
        {
            Chitunion2017.Entities.UserManage.LoginUserInfo lu = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByToken("8349eab3717b4610bc056fc62d85281a");

        }
        [TestMethod]
        public void LoginForWeChat()
        {
            string errorMsg = string.Empty;
            var dto = new ReqLoginDTO
            {
                openid = "okG5Y1CYE_eLkFa8_-1AfIOl-dhs",
                unionid = "o_J_G06UAzkmcf08fPW7iv5Wt8IY",
                nickname = "12345678901234567890123456789012",
                sex = "0",
                language = "zh_CN",

            };
            var resp = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.LoginForWeChat(dto, ref errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        [TestMethod]
        public void GetMsgPushConfig()
        {
            var request = new ReqAppPushSwitchDto()
            {
                DeviceId = "34556787654"
            };
            var resp = new AppPushMsgSwitchLogProvider(request).GetPushConfig();
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }
        [TestMethod]
        public void SetPushConfig()
        {
            var request = new ReqAppPushSwitchDto()
            {
                DeviceId = "34556787654"
            };
            var resp = new AppPushMsgSwitchLogProvider(request).SetPushConfig();
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }
        [TestMethod]
        public void UpdateClosed()
        {
            var request = new ReqAppPushSwitchDto()
            {
                DeviceId = "34556787654"
            };
            var resp = new AppPushMsgSwitchLogProvider(request).ClosedPushTips();
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }
    }
}
