/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017New.Test.ActivityVerify
* 类 名 称 ：ActivityVerifyTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/13 10:13:20
********************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;

namespace XYAuto.ITSC.Chitunion2017New.Test.ActivityVerify
{
    [TestClass]
    public class ActivityVerifyTest
    {
        [TestMethod]
        public void IsNewUser()
        {
            ShareOrderInfo.Instance.AddOrderVerify(1739);
        }
    }
}
