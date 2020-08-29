/********************************************************
*创建人：lixiong
*创建时间：2017/8/23 13:59:58
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_2
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