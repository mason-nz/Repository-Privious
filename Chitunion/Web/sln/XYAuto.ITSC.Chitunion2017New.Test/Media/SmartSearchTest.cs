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
    public class SmartSearchTest
    {
        /// <summary>
        /// 智能搜索添加
        /// </summary>
        [TestMethod]
        public void AddSmartSearchInfo()
        {
            int t = SmartSearchBll.Instance.AddSmartSearchInfo(new SmartSearchModel
            {
                Name = "测试名称",
                BeginTime = DateTime.Now,
                Demand = "今天是个好日子",
                BudgetPrice = 1900.12M,
                CreateTime = DateTime.Now,
                MaterialUrl = "http://www.baidu.com",
                EndTime = DateTime.Now,
                Purposes = 12,
                UserID = 3
            });
        }

        /// <summary>
        /// 获取智能搜索列表
        /// </summary>
        /// <param name="SmartSearchID"></param>
        [TestMethod]
        public void GetSmartSearchApp()
        {
            var query1 = SmartSearchBll.Instance.GetSmartSearchMediaList(73);


            string tem1 = JsonConvert.SerializeObject(query1);

            var query = SmartSearchBll.Instance.GetSmartSearchList(new QueryArgs { Name="哈"}, 1420);


            string tem = JsonConvert.SerializeObject(query);
        }

        [TestMethod]
        public void GetSmartSearchMediaList()
        {
            var query = SmartSearchBll.Instance.GetSmartSearchMediaList(2);


            string tem = JsonConvert.SerializeObject(query);
        }
        /// <summary>
        /// 获取智能匹配列表
        /// </summary>
        [TestMethod]
        public void GetSmartSearchList()
        {
            var query = SmartSearchBll.Instance.GetSmartSearchList(new QueryArgs(), 1420);


            string tem = JsonConvert.SerializeObject(query);
        }
        /// <summary>
        /// 获取推广计划总数
        /// </summary>
        [TestMethod]
        public void GetSmartSearchCount()
        {
            var query = SmartSearchBll.Instance.GetSmartSearchCount(0);


            string tem = JsonConvert.SerializeObject(query);
        }

    }
}
