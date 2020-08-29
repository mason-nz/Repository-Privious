﻿/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 17:20:12
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.Entities.GDT;
using XYAuto.ITSC.Chitunion2017.Entities.Query.GDT;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_2
{
    [TestClass]
    public class LogicByZhyProviderTest
    {
        private readonly LogicByZhyProvider _logicByZhyProvider;

        public LogicByZhyProviderTest()
        {
            _logicByZhyProvider = new LogicByZhyProvider();
        }

        [TestMethod]
        public void DemandStatusNote()
        {
            var retValue = _logicByZhyProvider.DemandStatusNote(new ToDemandNotes()
            {
                OrganizeId = 64,
                AuditStatus = DemandAuditStatusEnum.Rejected,
                DemandBillNo = 100105,
                Reject = "100105 第一下次驳回，第一次驳回，驳回原因：没写促销政策"
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void AccountFundsNoteTest()
        {
            var retValue = _logicByZhyProvider.AccountFundsNote(new ToAccountFundNotes()
            {
                DemandBillNo = 100106,
                MoneyTpe = ZhyEnum.ZhyMoneyTpeEnum.现金,
                OrganizeId = 64,
                TradeType = ZhyEnum.ZhyTradeTypeEnum.回划,
                TradeMoney = 300,
                TradeNo = "100106ZHTHH1"
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }
    }
}