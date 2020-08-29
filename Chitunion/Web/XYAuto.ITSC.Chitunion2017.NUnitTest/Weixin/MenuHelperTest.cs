using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Senparc.Weixin.MP.Containers;
using XYAuto.ITSC.Chitunion2017.NUnitTest.Utils;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.Weixin
{
    /// <summary>
    /// 注释：MenuHelperTest
    /// 作者：masj
    /// 日期：2018/5/29 9:14:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestFixture]
    public class MenuHelperTest
    {
        private string appid;//开发环境
        private string weixinAppSecret;//开发环境

        [SetUp]
        public void MenuHelper()
        {
            appid = "wxf0eea6fec2756b45";//开发环境
            weixinAppSecret = "2e35c3d7acb3219b6d77415267d06975";
        }

        [Test]
        public void GetMenuTest()
        {
            string WeChatMenuClickDataPath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath", true);

            //bool status = false;

            ////连接共享文件夹  
            //status = FileShareHelper.connectState(WeChatMenuClickDataPath, "FileSharer", "123.abc");
            //if (status)
            //{
            //    string fileContent = FileShareHelper.ReadFiles(WeChatMenuClickDataPath + "\\WxMenuData.config");

            //    var list =
            //        XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetWxMenuConfigData(WeChatMenuClickDataPath);

            //}
            var list =
                    XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetWxMenuConfigData(WeChatMenuClickDataPath);
            AccessTokenContainer.Register(appid, weixinAppSecret);
            var menu = BLL.WeChat.MenuHelper.Instance.GetMenu(appid);

            string jsonStr = JsonConvert.SerializeObject(menu.menu);

            Assert.AreNotEqual(menu, null);
        }
    }
}
