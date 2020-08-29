using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using NUnit.Framework;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.WebService.ITSupport;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.Weixin
{
    /// <summary>
    /// 注释：SendTempMsgTest
    /// 作者：masj
    /// 日期：2018/5/17 20:38:56
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestFixture]
    public class SendTempMsgTest
    {

        [SetUp]
        public void SendTempMsg()
        {
        }

        [Test]
        public void SendTempMsgT()
        {
            string appid = "wxf0eea6fec2756b45";//开发环境
            string weixinAppSecret = "2e35c3d7acb3219b6d77415267d06975";
            string openid = "ohvZzwIpMW_ROfDf7N9IvbdNrpHg";
            string tempid = "xcYZULiFuEkq5BXtY_EjLSiBXonOeVDhYmben6ub2QE";
            string url = "http://www.baidu.com";
            //Dictionary<string,string> para=new Dictionary<string, string>();
            var para = new
            {
                first = new TemplateDataItem("你发起的活动太火，报名踊跃，活动已确定成行，您可以到活动页面去设置哪些人获得“早到奖”了。玩的开心！\r\n", "#173177"),
                keyword1 = new TemplateDataItem("深圳的中秋活动", "#173177"),
                keyword2 = new TemplateDataItem("9", "#173177"),
                remark = new TemplateDataItem("感谢使用，查看活动详情>>", "#173177")
            };
            AccessTokenContainer.Register(appid, weixinAppSecret);
            bool flag = BLL.WeChat.TempHelper.Instance.SendTempMsg(appid,openid, tempid, url, para);
                
            Assert.AreEqual(true, flag);
        }
    }
}
