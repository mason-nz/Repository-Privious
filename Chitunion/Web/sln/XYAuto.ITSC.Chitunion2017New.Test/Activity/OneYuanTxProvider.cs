using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;

namespace XYAuto.ITSC.Chitunion2017New.Test.Activity
{
    /// <summary>
    /// 注释：OneYuanTxProvider
    /// 作者：lix
    /// 日期：2018/6/13 9:53:46
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class OneYuanTxProvider
    {
        [TestMethod]
        public void PostWithdrawas()
        {
            var retValue = new Chitunion2017.BLL.Activity.OneYuanTxProvider(new ConfigEntity()
            {
                CreateUserId = 1
            }, new ReqWithdrawalsDto
            {
                WithdrawalsPrice = 2,
            }).PostWithdrawas();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }
    }
}
