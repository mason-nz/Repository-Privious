/********************************
* 项目名称 ：XYAuto.ChiTu2018.API.Tests.LE
* 类 名 称 ：WithdrawalsTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/15 15:00:41
********************************/
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.API.Models.Enum;
using XYAuto.ChiTu2018.API.Models.Withdrawals;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.LE.Provider;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.LE.Query.Dto.Request;

namespace XYAuto.ChiTu2018.API.Tests.LE
{
    [TestClass]
    public class WithdrawalsTest
    {
        public WithdrawalsTest()
        {

        }

        /// <summary>
        /// 查询收入管理-详情
        /// </summary>
        [TestMethod]
        public void GetIncomeInfoMethod()
        {
            var info = Service.LE.LeWithdrawalsStatisticsService.Instance.GetIncomeInfo(1613);
            var JsonText = JsonConvert.SerializeObject(info);
        }
        /// <summary>
        /// 提现详情
        /// </summary>
        [TestMethod]
        public void GetInfoMethod()
        {
            EntityFrameworkProfiler.Initialize();
            Dictionary<string, object> dic = XYAuto.ChiTu2018.Service.LE.LeWithdrawalsDetailService.Instance.GetWithdrawalsInfo(27, 1613);
            var JsonText = JsonConvert.SerializeObject(dic);
            Console.WriteLine(JsonText);
        }
        /// <summary>
        /// 提现操作
        /// </summary>
        [TestMethod]
        public void WithdrawalsOptMethod()
        {
            var retValue = new WxWithdrawalsProvider(new ConfigEntity()
            {
                UserId = 1552
            }, new ReqWithdrawalsDto { WithdrawalsPrice = 50.23m, Ip = "192.238.23.23" }).Withdrawals();

            var query = JsonConvert.SerializeObject(retValue);
        }
        /// <summary>
        ///  收入管理-提现明细列表
        ///  
        /// </summary>
        /// <param name="requeryArgs"></param>
        /// <returns></returns>
        [TestMethod]
        public void GetWithdrawalsListMethod()
        {
            var list = Service.LE.LeWithdrawalsDetailService.Instance.GetIncomeWithdrawalsQuery(
                new ReqInComeDto
                {
                    UserId = 1552,
                    PageIndex = 1,
                    PageSize = 20
                });

            var query = JsonConvert.SerializeObject(list);
        }

        [TestMethod]
        public void GetRedirectUrlTest()
        {
            var url = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Domin") +
                  $"{GetRedirectUrl(WxTemplateRedirectEnum.提现成功)}?WithdrawalsId=" + 1;
            Console.WriteLine(url);
        }

        /// <summary>
        /// 获取wx模版配置的跳转地址
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetRedirectUrl(WxTemplateRedirectEnum type)
        {
            var redirectList = ConfigurationManager.AppSettings["WxNoticeTemplateUrl"];
            var list = JsonConvert.DeserializeObject<List<WxTemplateRedirectDto>>(redirectList);
            var info = list.FirstOrDefault(s => s.Type.Equals((int)type));
            return info != null ? info.RedirectUrl : string.Empty;
        }

       
    }
}
