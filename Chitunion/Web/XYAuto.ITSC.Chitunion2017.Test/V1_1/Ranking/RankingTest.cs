/********************************************************
*创建人：lixiong
*创建时间：2017/7/10 17:35:55
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Ranking;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1.Ranking
{
    [TestClass]
    public class RankingTest
    {
        [TestMethod]
        public void ranklist_query_test()
        {
            var userId = 1211;//ConfigurationUtil.GetAppSettingValue("RankListQueryByUserId").ToInt();

            var list = BLL.Ranking.StatWeixinRankList.Instance.GetRankList(new RankingQuery<RespStatWeixinRankListDto>()
            {
                //CategoryId = requestDto.CategoryId,
                CreateUserId = userId,
                PageSize = 100
                //KeyWord = requestDto.Keyword
            });
            Console.WriteLine(JsonConvert.SerializeObject(new
            {
                List = list.Item1,
                Info = list.Item2
            }));
        }

        [TestMethod]
        public void ranklist_query_kw_test()
        {
            var userId = 1211;//ConfigurationUtil.GetAppSettingValue("RankListQueryByUserId").ToInt();

            var list = BLL.Ranking.StatWeixinRankList.Instance.GetRankList(new RankingQuery<RespStatWeixinRankListDto>()
            {
                CategoryId = 52011,
                CreateUserId = userId,
                KeyWord = "erewt"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void ranklist_CategoryList_test()
        {
            var list = BLL.Ranking.StatWeixinRankList.Instance.GetRankingCategoryList(1121);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}