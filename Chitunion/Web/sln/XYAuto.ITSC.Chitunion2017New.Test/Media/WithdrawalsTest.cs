using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017New.Test.Media
{
    [TestClass]
    public class WithdrawalsTest
    {

        [TestMethod]
        public void GetWithdrawalsStatisticsList()
        {
            var queryList = WithdrawalsBll.Instance.GetWithdrawalsStatisticsList(new QueryWithdrawalsArgs { PageIndex = 1, PageSize = 20,OrderBy=1001 });


            string result = JsonConvert.SerializeObject(queryList);
        }
        [TestMethod]
        public void GetIncomeDetailModelList()
        {
            var queryList = WithdrawalsBll.Instance.GetIncomeDetailModelList(new QueryWithdrawalsArgs { PageIndex = 1, PageSize = 20,UserID=1422 });


            string result = JsonConvert.SerializeObject(queryList);
        }
    }
}
