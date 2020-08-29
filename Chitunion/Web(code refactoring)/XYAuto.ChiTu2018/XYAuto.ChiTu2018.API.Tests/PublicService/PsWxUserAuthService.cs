using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Extend.User;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.User;

namespace XYAuto.ChiTu2018.API.Tests.PublicService
{
    /// <summary>
    /// 注释：PsWxUserAuthService
    /// 作者：lix
    /// 日期：2018/6/8 14:57:50
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class PsWxUserAuthService : BaseTest
    {
        [Description("微信授权用户处理")]
        [TestMethod]
        public void WeiXinUserOperation()
        {
            var retValue = Service.App.PublicService.PsWxUserAuthService.Instance.WeiXinUserOperation(new PsReqPostWxUserOperationDto()
            {
                unionid = "o_J_G08-jxrTDS78Zxyb8yM8HVTk-test",
                country = "CN",
                openid = "okG5Y1BogLhldxust7XRi_oTsn2s-test2",
                nickname = "灿灿-2-update",
                city = "Beijing",
                province = "Beijing",
                language = "zh_CN",
                headimgurl = "http://thirdwx.qlogo.cn/mmopen/vi_32/ZLhicyPutHBNXkfRAiaNaflH1n1y2fE4r9E5j0heQGGS3TpfU0xrsc1DO0ibuXjBZXpw6Uqpd99hCSoITUYyibzRWQ/132",
                sex = 2,
                LastUpdateTime = DateTime.Now,
                CreateTime = DateTime.Now,
                AuthorizeTime = DateTime.Now,
                subscribe_time = DateTime.Now,
                Source = 3008,//微信表的来源
                RegisterIp = "1.1.1.1",
                RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号,
                RegisterType = (int)RegisterTypeEnum.微信,
                PromotionChannelId = 10010
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
       
        }
    }
}
