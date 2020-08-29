using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_6
{
    [TestClass]
    public class LsTest
    {
        [TestMethod]
        public void ConvertToChituUrl()
        {
            List<string> list = new List<string>();
            list.Add("http://f10.baidu.com/it/u=1216657413,1541355072&fm=76");
            list.Add("http://www.cnblogs.com/skins/BlackSun/images/BlackSunBottom.jpg");
            list.Add("http://common.cnblogs.com/images/icon_weibo_24.png");
            BLL.WxEditor.WxArticleGroup.Instance.ConvertToChituUrl("");
        }

        [TestMethod]
        public void FindSrc(){
        }
    }
}
