/********************************************************
*创建人：lixiong
*创建时间：2017/8/23 13:59:58
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.BOP2017.BLL.GDT;

namespace XYAuto.BUOC.BOP2017.Test.GDT
{
    [TestClass]
    public class GdtImportDataTest
    {
        [Description("账户余额测试数据")]
        [TestMethod]
        public void GdtAccountBalance_InsertByGdtServer()
        {
            var list = new List<Entities.GDT.GdtAccountBalance>()
            {
            };
            GdtAccessToken.Instance.InsertByGdtServer(list, new Entities.GDT.GdtAccountBalance { });
        }
    }
}