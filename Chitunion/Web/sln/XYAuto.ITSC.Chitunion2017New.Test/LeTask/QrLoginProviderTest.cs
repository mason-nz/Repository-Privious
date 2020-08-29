using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.Containers;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.WebService.Common;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class QrLoginProviderTest
    {
        static QrLoginProviderTest()
        {
            AccessTokenContainer.Register(
               ConfigurationManager.AppSettings["WeixinAppId"],
               ConfigurationManager.AppSettings["WeixinAppSecret"],
               "【ChiTu】公众号");
        }

        [TestMethod]
        public void EncryptTest()
        {
            var valType = UserCategoryEnum.广告主;
            var provider = new QrLoginProvider();
            var code = ITSC.Chitunion2017.Common.Util.GenerateRandomCode(5);

            var sign = provider.Encrypt(code);
            Console.WriteLine(sign);

            var reqKeyValue = new ReqKeyValueDto()
            {
                t = ReqMessageType.Pc端登录,
                v = $"{(int)valType}|{sign}"
            };

            var sceneStr = JsonConvert.SerializeObject(reqKeyValue);

            Console.WriteLine(sceneStr);

            Console.WriteLine(sceneStr.Length);
        }
        [TestMethod]
        public void PostCreateQr()
        {
            var appId = "wx3b4d1cdb1de3d00c";
            var appSecret = "2326e662d87034cab3ac09abfcdb12c1";
            var accessToken = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetAccessToken(appId, appSecret);
            var postUrl = $"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accessToken}";
            var postData =
                "{\"expire_seconds\":604800,\"action_name\":\"QR_STR_SCENE\",\"action_info\":{\"scene\":{\"scene_str\":\"test\"}}}";
            var result = new DoHttpClient().PostByJson(postUrl, postData);
            Console.WriteLine(result.Result);

            var qrCodeResult = JsonConvert.DeserializeObject<CreateQrCodeResult>(result.Result);

            var url = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);

            Console.WriteLine(url);
        }
        [TestMethod]
        public void CreateQrImage()
        {
            var provider = new QrLoginProvider();
            var url = provider.CreateQrImage("rtyui|6789");

            Console.WriteLine(url);
        }

        [TestMethod]
        public void GetLoginQrTest()
        {
            var provider = new QrLoginProvider();
            var url = provider.GetLoginQr(new ReqPostLoginQrDto()
            {
                LoginType = LoginType.媒体主
            });

            Console.WriteLine(JsonConvert.SerializeObject(url));
        }
        [TestMethod]
        public void VerifyQrLoginTest()
        {
            var provider = new QrLoginProvider();
            var retValue = provider.VerifyQrLogin(new ReqVerifyQrLoginDto()
            {
                Ticket = "TSpsEe6CboQ="
            });

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void DesEncryptTest()
        {
            var provider = new QrLoginProvider();
            var code = "00+hMUD8j+w=";
            Console.WriteLine("code1:" + code);
            var ticket = provider.Encrypt(code);
            Console.WriteLine("ticket:" + ticket);

            code = provider.DesEncrypt(ticket);
            Console.WriteLine("code2:" + code);

        }

        [TestMethod]
        public void PushUserLoginTest()
        {
            var provider = new QrLoginProvider();
            provider.InsertRole(97, 29002);
            //Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void SimulationLoginTest()
        {
            var provider = new QrLoginProvider();
            var retValue = provider.SimulationLogin(new ReqSimulationLoginDto()
            {
                EventKey = "{\"t\":2,\"v\":\"29001|trlPmgrvVz4=\"}",
                Ticket = "29001|trlPmgrvVz4=",
                WeixinOpendId = "oab7i0mmtJnZAFDR97sjy_F5K1Qg",
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }
    }
}
