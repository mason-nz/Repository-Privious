/********************************************************
*创建人：lixiong
*创建时间：2017/9/14 14:11:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Security;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.Test.ExternalApi.Zhy
{
    [TestClass]
    public class LogicByMaterielStatisticsTest
    {
        private readonly MaterielStatisticsProvider _provider;
        private readonly LogicByMaterielStatistics _logicByMateriel;

        public LogicByMaterielStatisticsTest()
        {
            BLL.AutoMapperConfig.AutoMapperConfig.Configure();
            var client = new DoHttpClient(new System.Net.Http.HttpClient());
            _provider = new MaterielStatisticsProvider(client);
            _logicByMateriel = new LogicByMaterielStatistics(client, new PullDataConfig());
        }

        [TestMethod]
        public void MaterielStatisticsProvider_Test()
        {
            var startDate = DateTime.Now.AddDays(-15);
            var articleId = new List<int>() { 232126, 522826, 207249 };

            articleId.ForEach(s =>
            {
                for (int i = 0; i < 1; i++)
                {
                    var info = _provider.PullStatistics(new PullStatisticsDto()
                    {
                        HeadArticleId = s,
                        MaterielId = s,
                        DateTime = startDate.AddDays(i).ToString("yyyy-MM-dd")
                    });
                    // Console.WriteLine(JsonConvert.SerializeObject(info));
                }
                startDate = DateTime.Now.AddDays(-15);
            });

            //Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void JosnTest()
        {
            var json = "{\"Success\":true,\"Data\":{\"MaterielID\":21257,\"Url\":\"http://dealer.h5.qichedaquan.com/301480/KonwledgeDetail/46728/0\",\"PV\":24850,\"UV\":1,\"JumpChance\":1,\"Clue\":{\"PhoneClueCount\":0,\"InquiryCount\":0,\"ConversationCount\":0},\"HeadMaterial\":{\"PV\":24850,\"UV\":1,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":1,\"Share\":{\"WeChatFriend\":1,\"WeChatFriends\":0,\"QQ\":0},\"AgreeCount\":1},\"WaistMaterial\":[{\"MaterielID\":96032,\"Url\":\"http://dealer.h5.qichedaquan.com/301480/KonwledgeDetail/46728/46845\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":100528,\"Url\":\"http://dealer.h5.qichedaquan.com/301480/KonwledgeDetail/46728/46844\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":108944,\"Url\":\"http://dealer.h5.qichedaquan.com/301480/KonwledgeDetail/46728/46841\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":109031,\"Url\":\"http://dealer.h5.qichedaquan.com/301480/KonwledgeDetail/46728/46842\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}}]},\"ErrorCode\":0,\"ErrorMessage\":\"请求成功\",\"ErrorDetail\":null}";

            json = "{\"Success\":true,\"Data\":{\"MaterielID\":21257,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/0\",\"PV\":24850,\"UV\":1,\"JumpChance\":1.0,\"Clue\":{\"PhoneClueCount\":0,\"InquiryCount\":0,\"ConversationCount\":0},\"HeadMaterial\":{\"PV\":24850,\"UV\":1,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":1,\"Share\":{\"WeChatFriend\":1,\"WeChatFriends\":0,\"QQ\":0},\"AgreeCount\":1},\"WaistMaterial\":[{\"MaterielID\":96032,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46845\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":120,\"Phone\":40,\"Inquiry\":10,\"QA\":50,\"HeadPortrait\":440},\"ClikePV\":{\"Applet\"780,\"Phone\":98,\"Inquiry\":23,\"QA\":21,\"HeadPortrait\":20},\"ShareTotal\":653,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":90,\"QQ\":430}},{\"MaterielID\":100528,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46844\",\"PV\":44340,\"UV\":6540,\"WaistClikePV\":330,\"WaistClikeUV\":420,\"ClikeUV\":{\"Applet\":440,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":9870},\"ClikePV\":{\"Applet\":540,\"Phone\":310,\"Inquiry\":4320,\"QA\":4320,\"HeadPortrait\":31320},\"ShareTotal\":566780,\"Share\":{\"WeChatFriend\":430,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":108944,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46841\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":109031,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46842\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}}]},\"ErrorCode\":0,\"ErrorMessage\":\"请求成功\",\"ErrorDetail\":null}";
            var dto = JsonConvert.DeserializeObject<RespZhyBaseDto<RespMaterielDto>>(json);

            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }

        [TestMethod]
        public void AddToMaterielInfo_Test()
        {
            var logic = new LogicByMaterielStatistics(new DoHttpClient(), new PullDataConfig());
            var json = "{\"Success\":true,\"Data\":{\"MaterielID\":21257,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/0\",\"PV\":24850,\"UV\":1,\"JumpChance\":1.0,\"Clue\":{\"PhoneClueCount\":0,\"InquiryCount\":0,\"ConversationCount\":0},\"HeadMaterial\":{\"PV\":24850,\"UV\":1,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":1,\"Share\":{\"WeChatFriend\":1,\"WeChatFriends\":0,\"QQ\":0},\"AgreeCount\":1},\"WaistMaterial\":[{\"MaterielID\":96032,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46845\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":100528,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46844\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":108944,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46841\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":109031,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46842\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}}]},\"ErrorCode\":0,\"ErrorMessage\":\"请求成功\",\"ErrorDetail\":null}";

            json = "{\"Success\":true,\"Data\":{\"MaterielID\":537,\"Url\":\"http://dealer.h5.qichedaquan.com/25238/KonwledgeDetail/912386/0\",\"PV\":0,\"UV\":0,\"JumpChance\":0,\"Clue\":{\"PhoneClueCount\":0,\"InquiryCount\":0,\"ConversationCount\":0},\"HeadMaterial\":{\"PV\":0,\"UV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0},\"AgreeCount\":0},\"WaistMaterial\":[{\"MaterielID\":117704,\"Url\":\"http://dealer.h5.qichedaquan.com/25238/KonwledgeDetail/912386/912465\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":106974,\"Url\":\"http://dealer.h5.qichedaquan.com/25238/KonwledgeDetail/912386/912467\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}}]},\"ErrorCode\":0,\"ErrorMessage\":\"请求成功\",\"ErrorDetail\":null}";
            var dto = JsonConvert.DeserializeObject<RespZhyBaseDto<RespMaterielDto>>(json);

            logic.AddToMaterielInfo(dto.Data);
        }

        [TestMethod]
        public void DoInsertBefore_Test()
        {
            var json = "{\"Success\":true,\"Data\":{\"MaterielID\":21257,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/0\",\"PV\":24850,\"UV\":1,\"JumpChance\":1.0,\"Clue\":{\"PhoneClueCount\":0,\"InquiryCount\":0,\"ConversationCount\":0},\"HeadMaterial\":{\"PV\":24850,\"UV\":1,\"ClikeUV\":{\"Applet\":10,\"Phone\":70,\"Inquiry\":90,\"QA\":50,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":21,\"Phone\":22,\"Inquiry\":32,\"QA\":221,\"HeadPortrait\":234},\"ShareTotal\":1,\"Share\":{\"WeChatFriend\":1,\"WeChatFriends\":3,\"QQ\":2},\"AgreeCount\":1},\"WaistMaterial\":[{\"MaterielID\":96032,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46845\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":55,\"Phone\":56,\"Inquiry\":45,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":88,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":100528,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46844\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":108944,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46841\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":109031,\"Url\":\"http://dealer.h5.qichedaquan.com/300020/KonwledgeDetail/46728/46842\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}}]},\"ErrorCode\":0,\"ErrorMessage\":\"请求成功\",\"ErrorDetail\":null}";

            json = "{\"Success\":true,\"Data\":{\"MaterielID\":537,\"Url\":\"http://dealer.h5.qichedaquan.com/25238/KonwledgeDetail/912386/0\",\"PV\":0,\"UV\":0,\"JumpChance\":0,\"Clue\":{\"PhoneClueCount\":0,\"InquiryCount\":0,\"ConversationCount\":0},\"HeadMaterial\":{\"PV\":0,\"UV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0},\"AgreeCount\":0},\"WaistMaterial\":[{\"MaterielID\":117704,\"Url\":\"http://dealer.h5.qichedaquan.com/25238/KonwledgeDetail/912386/912465\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}},{\"MaterielID\":106974,\"Url\":\"http://dealer.h5.qichedaquan.com/25238/KonwledgeDetail/912386/912467\",\"PV\":0,\"UV\":0,\"WaistClikePV\":0,\"WaistClikeUV\":0,\"ClikeUV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ClikePV\":{\"Applet\":0,\"Phone\":0,\"Inquiry\":0,\"QA\":0,\"HeadPortrait\":0},\"ShareTotal\":0,\"Share\":{\"WeChatFriend\":0,\"WeChatFriends\":0,\"QQ\":0}}]},\"ErrorCode\":0,\"ErrorMessage\":\"请求成功\",\"ErrorDetail\":null}";

            var dto = JsonConvert.DeserializeObject<RespZhyBaseDto<RespMaterielDto>>(json);
            var retValue = new ReturnValue();

            dto.Data.MaterielId = 77;

            retValue = _logicByMateriel.DoInsert(retValue, dto.Data, DateTime.Now.ToString("yyyy-MM-dd"));

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void Enum_Test()
        {
            var dicMaterielClickDetailsEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielClickDetailsEnum));

            //foreach (var item in dicMaterielClickDetailsEnum)
            //{
            //    Console.WriteLine(item.Key + "--" + item.Value);
            //}

            dicMaterielClickDetailsEnum.ToList().ForEach(s =>
            {
                if (s.Key == "3003")
                    return;
                Console.WriteLine(s.Key + "--" + s.Value);
            });

            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd HH:mm:ss" };

            Console.WriteLine(DateTime.Now.ToString("T"));
            Console.WriteLine(Convert.ToDateTime("2017-09-25", dtFormat));
            Console.WriteLine(DateTime.Parse("2017-09-25").AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute));
        }

        [TestMethod]
        public void PullStatistics_Test()
        {
            XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.MaterielStatisticsProvider provider =
                new MaterielStatisticsProvider(new DoHttpClient());
            var resp = provider.PullStatistics(new PullStatisticsDto()
            {
                HeadArticleId = 641139,
                MaterielId = 12716,
                DateTime = "2017-11-21"
            });
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        [TestMethod]
        public void GetMaterielInfoByBodyArticleId_Test()
        {
            var info = new DistributeProvider().GetMaterielInfoByBodyArticleId(216459);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("拉取分发详情明细")]
        [TestMethod]
        public void PullMaterielDetails_Test()
        {
            var date = DateTime.Now.AddDays(-21);
            for (int i = 0; i < 10; i++)
            {
                var resp = _provider.PullMaterielDetails(DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"));
            }

            //var resp = _provider.PullMaterielDetails("2017-10-22");
            //Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        [Description("拉取分发详情明细")]
        [TestMethod]
        public void PullDistribute_Test()
        {
            //_logicByMateriel.PullDistribute("2017-10-13");
            //var date = DateTime.Now.AddDays(-21);
            //for (int i = 0; i < 10; i++)
            //{
            //    _logicByMateriel.PullDistribute(date.AddDays(i).ToString("yyyy-MM-dd"));
            //}

            var dt = DateTime.Now;
            var cut = Convert.ToDateTime("2017-11-02 17:20:33.637");
            Console.WriteLine((dt - cut).TotalHours);
            cut = Convert.ToDateTime(cut.ToString("yyyy-MM-dd"));
            //Console.WriteLine(Convert.ToDateTime(cut.ToString("yyyy-MM-dd")));

            Console.WriteLine((dt - cut).Days);
            Console.WriteLine((dt - cut.AddDays(1)).Days);
            Console.WriteLine((dt - DateTime.Now).Days);
            Console.WriteLine((dt - DateTime.Now.AddDays(-1)).Days);
            Console.WriteLine((dt - DateTime.Now.AddDays(-2)).Days);
        }

        [TestMethod]
        public void LoopPullStatistics_Test()
        {
            var provider = new LogicByMaterielStatistics(new DoHttpClient(new System.Net.Http.HttpClient()), new PullDataConfig()
            {
                DateOffset = -21,
                PullDataQueryDateOffset = 10
            });
            provider.LoopPullStatisticsTask();
        }

        [TestMethod]
        public void TaskTest()
        {
            //var cts = new CancellationTokenSource();
            //var ct = cts.Token;
            var postProvider = new MaterielStatisticsProvider(new DoHttpClient(new System.Net.Http.HttpClient()));
            var request = new PullStatisticsDto
            {
                MaterielId = 7391,
                HeadArticleId = 592958,
                DateTime = "2017-11-01"
            };

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    Task.Run(() =>
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            postProvider.PullStatistics(request);
                        }
                    });
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        [TestMethod]
        public void PullCarConsultant()
        {
            string appkey = "chitunion";
            string appsecret = "6DE30E30-CF16-4C39-AD03-EDB2D4867C84";

            var timestamp = SignUtility.ConvertDateTimeInt(DateTime.Now).ToString();
            var dicParams = new Dictionary<string, object>
            {
                {"appkey", appkey},
                {"appsecret", appsecret},
                {"timestamp",timestamp},
            };

            var postData = $"";

            var sign = ZhySignatureProvider.GetSignatureText(dicParams);
            var requestUrl = $"http://api.xingyuanauto.com/chitunion/RecordDealerSerial?appkey={appkey}&signature={sign}&timestamp={timestamp}";

            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespZhyBaseDto<dynamic>>
               (s => new Infrastruction.Http.DoHttpClient().PostByForm(requestUrl, postData).Result, Loger.ZhyLogger.Info);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}