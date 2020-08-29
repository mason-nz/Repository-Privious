using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.ThirdApi;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.ITSC.Chitunion2017.WebService.Common;
using XYAuto.Utils.Config;
using Loger = XYAuto.ITSC.Chitunion2017.BLL.Loger;
using Util = XYAuto.ITSC.Chitunion2017.BLL.Util;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class TaskProviderTest
    {
        public TaskProviderTest()
        {
            MediaMapperConfig.Configure();
        }

        [TestMethod]
        public void DrawStringByImageTest()
        {
            var text = "test_1";
            var fileOriginPath = $@"D:\1.png";
            var fileGenPath = $@"D:\{DateTime.Now.ToString("yyyy-MM-dd")}.png";
            Util.DrawStringByImage(text, fileOriginPath, fileGenPath);
        }

        [TestMethod]
        public void GenerateByFile_test()
        {
            var logoUrl = "/UploadFiles/2017/7/24/14/买车要砍价.png";

            var uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            var fileName = uploadFilePath + logoUrl.Replace(@"/", "\\");

            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = "https://www.baidu.com/",
                FileName = fileName,
                Width = 150,
                Height = 150,
                SaveFileName = @"D:\test-2.jpg"
            });

            provider.GenerateQrIntoImage(@"D:\test-2-1.jpg", 575, 75);
        }

        [TestMethod]
        public void GenerateImageTest()
        {
            var imagePath =
                new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto())
                .GenerateImage("https://www.baidu.com/",
                "/UploadFiles/Task/QRImage/2017/12/25/17/新年更优惠.png");
            Console.WriteLine(imagePath);


        }
        [TestMethod]
        public void CleanImgTest()
        {
            var logoUrl = "/UploadFiles/2017/7/24/14/买车要砍价.png";

            var uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            var fileName = uploadFilePath + logoUrl.Replace(@"/", "\\");
            var imageUrl = XYAuto.ITSC.Chitunion2017.BLL.Util.CleanImg(System.Drawing.Image.FromFile(fileName));
            var cleanImgUrlPrefix = ConfigurationUtil.GetAppSettingValue("CleanImgURLPrefix");//文章中替换图片url后的域名

            Console.WriteLine(imageUrl);
            Console.WriteLine(cleanImgUrlPrefix + imageUrl);
            //Console.WriteLine($"1===" + Util.CleanImg());
        }
        [TestMethod]
        public void JsonTest()
        {
            var taskPriceConfig = ConfigurationUtil.GetAppSettingValue("TaskPriceConfig", false);
            Console.WriteLine(taskPriceConfig);
            var configInfo1 = JsonConvert.DeserializeObject<TaskConfigEntity>(taskPriceConfig) ?? new TaskConfigEntity();
            var json = "{'DateRange':20,'RuleCount':20000,'CPCPrice':'2-8','CPLPrice':10,'TaskAmount':'800-1000','CPCLimitPrice':10,'CPLLimitPrice':100}";
            var configInfo = JsonConvert.DeserializeObject<TaskConfigEntity>(json) ?? new TaskConfigEntity();
            Console.WriteLine(JsonConvert.SerializeObject(configInfo));
        }

        [Description("外部接口-任务入库")]
        [TestMethod]
        public void ThirdApiTaskStorageTest()
        {
            var retValue = new TaskProvider(new ConfigEntity()
            {
                CreateUserId = 1992
            }, new ReqTaskReceiveDto())
                .ThirdApiTaskStorage(new ReqTaskStorageDto()
                {
                    TaskType = (int)LeTaskTypeEnum.ContentDistribute,
                    CategoryId = 10,
                    ImgUrl = "http://192.168.3.71/group1/M00/08/7E/oYYBAFo6BbCAHruYAAAV_MRigCQ017.JPG",
                    MaterialId = 180,
                    MaterialUrl = "http://news1.chitu.qichedaquan.com/materiel/chitunion/mobile/20171220/194.html",
                    Synopsis = "冰沙感、凉爽程度完全在线！",
                    TaskName = "不用刨冰机，只要一把叉子就能在家做芋圆红豆冰沙！"//物料名称
                });

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("外部接口-订单入库")]
        [TestMethod]
        public void ReceiveByDistributeUserIdentityTest()
        {
            var request = new ReqOrderStorageDto()
            {
                TaskId = 37,
                UserIdentity = "test",
                ChannelId = 101002
            };
            var retValue = new TaskProvider(new ConfigEntity(), request).ThirdApiOrderStorage();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("任务-内容分发-领取任务")]
        [TestMethod]
        public void TaskReceive()
        {
            var request = new ReqTaskReceiveDto
            {
                TaskType = (int)LeTaskTypeEnum.ContentDistribute,

                TaskId = 1508

            };
            var retValue = new TaskProvider(new ConfigEntity()
            {

                CreateUserId = 217
            }, request).Receive();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }
        [Description("任务-贴片-领取任务")]
        [TestMethod]
        public void TaskReceiveCoverIamge()
        {
            var request = new ReqTaskReceiveDto
            {
                TaskType = (int)LeTaskTypeEnum.CoverImage,
                TaskId = 8
            };
            var retValue = new TaskProvider(new ConfigEntity()
            {
                CreateUserId = 1409
            }, request).Receive();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        //图片叠加
        [TestMethod]
        public void btn_WaterMark_Click()
        {
            string path = "/UploadFiles/2017/7/24/14/timg.jpg";
            var uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            path = uploadFilePath + path.Replace(@"/", "\\");
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(path);

            var waterPath = @"/UploadFiles/2017/7/24/14/logo$831363d6-92ce-4680-a715-97f359f895d3.png";
            waterPath = uploadFilePath + waterPath.Replace(@"/", "\\");
            System.Drawing.Image imgWarter = System.Drawing.Image.FromFile(waterPath);

            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgWarter, new Rectangle(imgSrc.Width - imgWarter.Width,
                                                 imgSrc.Height - imgWarter.Height,
                                                 imgWarter.Width,
                                                 imgWarter.Height),
                        0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
            }

            string newpath = @"D:\test-12.jpg";
            imgSrc.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);

        }

        [Description("任务配置-测试")]
        [TestMethod]
        public void SetRuleTest()
        {
            var taskInfo = new LeTaskInfo() { };
            var provider = new TaskProvider(new ConfigEntity()
            {
                CreateUserId = 1409
            }, new ReqOrderStorageDto());
            provider.SetRule(taskInfo, 0.9m);

            Console.WriteLine(JsonConvert.SerializeObject(taskInfo));
        }

        [Description("获取字符串中的参数")]
        [TestMethod]
        public void GetUrlParamsTest()
        {
            var provider = new TaskProvider(new ConfigEntity()
            {
                CreateUserId = 1409
            }, new ReqOrderStorageDto());
            var url = "http://news1.chitu.qichedaquan.com/ct_m/20180116/9681.html?utm_source=chitu&utm_term=mpIoxVehcy";

            url = null;
            var args = provider.GetUrlParams(url, "utm_source");
            Console.WriteLine(args);
        }

        [TestMethod]
        public void DateTimeTest()
        {
            var dtFirst = DateTime.Now.FirstDayOfMonth();
            Console.WriteLine(dtFirst.ToString());
            var dtLast = DateTime.Now.LastDayOfMonth();
            Console.WriteLine(dtLast.ToString().ToSqlFilter());

            var num = Math.Floor(3 / 0.16);
            Console.WriteLine(num);
            Console.WriteLine(num * 0.16);
        }

        [TestMethod]
        public void HttpClientTest()
        {
            var result = new DoHttpClient().Get(
                 "http://www1.chitunion.com/api/task/GetDistrbuteList?PageIndex=1&PageSize=20&r=0.6491011156540758");
            Console.WriteLine(result.Result);
        }
        [TestMethod]
        public void GetProfitList()
        {
            XYAuto.ITSC.Chitunion2017.BLL.Profit.Profit.Instance.GetProfitList(10, 0,true);
        }
        [TestMethod]
        public void ReceiveByWeiXinVerifyUserId()
        {
            var retValue = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()
            {
                UserId = 5
            }).ReceiveByWeiXinVerifyUserId(5);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void ReceiveByWx()
        {
            var retValue = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()
            {
                UserId = 184,
                ChannelId = 101003,
                OrderUrl = "",
                TaskId = 1,
                TaskType = 192001,
                IP = "202.3.32.3",
                PromotionChannelId = 777777777777779997
            }).ReceiveByWeiXin();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void ReceiveByWxHttpPost()
        {
            //var postdate =
            //   "TaskId=102604&TaskType=192001&UserId=45898&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180327%2f175133.html%3futm_source%3dchitu%26utm_term%3duqljx8xfk3&IP=183.197.61.199&PromotionChannelID=1010030101";
            //var postDataList = new List<string>
            //{
            //    "postdateShareType=202001&TaskType=192001&TaskId=81144&UserId=37580&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180314%2f153624.html%3futm_source%3dchitu%26utm_term%3dDvDrPIUpZG",
            //    "postdateShareType=202001&TaskType=192001&TaskId=81144&UserId=35492&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180314%2f153624.html%3futm_source%3dchitu%26utm_term%3d9ZwfD5fvOj",
            //    "postdateShareType=202001&TaskType=192001&TaskId=81144&UserId=38070&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180314%2f153624.html%3futm_source%3dchitu%26utm_term%3dwFEgu9q6ak",
            //    "postdateShareType=202001&TaskType=192001&TaskId=81144&UserId=37901&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180314%2f153624.html%3futm_source%3dchitu%26utm_term%3dejocKmj8ol",
            //    "postdateShareType=202001&TaskType=192001&TaskId=81144&UserId=37779&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180314%2f153624.html%3futm_source%3dchitu%26utm_term%3dg2tcj4JwOi",
            //    "postdateShareType=202001&TaskType=192001&TaskId=81144&UserId=38021&ChannelId=101003&OrderUrl=http%3a%2f%2fnewscdn.chitunion.com%2fct_m%2f20180314%2f153624.html%3futm_source%3dchitu%26utm_term%3dRflaPQNSSD"
            //};

            var postDataList = File.ReadAllLines(@"D:\导入数据\orderLog.txt").ToList();
            // Console.WriteLine(text[0]);


            var rquestUrl = "http://www.chitunion.com/api/ThirdBusiness/ReceiveByWx";

            postDataList.ForEach(s =>
            {
                s += "&TaskType=192001";
                string geturl = ShareOrderInfo.Instance.PostWebRequest(rquestUrl, s);

                Console.WriteLine(JsonConvert.SerializeObject(geturl));
            });

        }

        [TestMethod]
        public void ReceiveByWeiXinVerifyUtmtermCodeTest()
        {
            var retValue = new ReturnValue();
            var orderUrl = "http://wx-ct.qichedaquan.com/NewsFolder/ct_m/20180412/11069.html?utm_source=chitu&utm_term=q80mk79wge";
            retValue = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()
            {
                UserId = 1337,
                ChannelId = 101003,
                OrderUrl = orderUrl,
            }).ReceiveByWeiXinVerifyUtmtermCode(retValue, orderUrl);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void PushAuditMessage()
        {
            new WithdrawalsProvider(new ConfigEntity(), new ReqWithdrawalsDto()).PushAuditMessage(new LeWithdrawalsDetail()
            {
                PayeeID = 163,
                WithdrawalsPrice = 100,
                RecID = 136
            });

            //PushAuditMessage(new LeWithdrawalsDetail()
            //{
            //    PayeeID = disbursementPayInfo.PayeeID,
            //    RecID = disbursementPayInfo.WithdrawalsId,
            //    WithdrawalsPrice = disbursementPayInfo.WithdrawalsPrice,
            //    PayDate = payTime
            //});
        }
        [TestMethod]
        public void PushAuditMessageHttp()
        {
            var requestUrl = "http://wxs.chitunion.com";
            var opendId = "oSuvAwQRAfxWcJis_ryG8b69j3yo";
            var price = 100.75;
            var recId = 22;
            requestUrl += $"/api/WeChatToChiTu/WithdrawalsNotice?openId={opendId}&dt={DateTime.Now}&price={price}&txid={recId}";
            var doHttpClient = new DoHttpClient(new System.Net.Http.HttpClient());
            var result = new DoPostApiLogClient(requestUrl, string.Empty)
                .GetPostResult<bool>(s => doHttpClient.Get(requestUrl).Result, Loger.Log4Net.Info);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void PostCreateShareOrder()
        {
            //todo:生成订单,调用接口：ReceiveByWeiXin
            var requestUrl = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost");
            var postData = "{\"TaskType\":192001,\"TaskId\":14328,\"MediaId\":0,\"IP\":\"::1\",\"ChannelId\":101005,\"OrderUrl\":\"http://newscdn.chitunion.com/ct_m/20180110/51565.html?utm_source=chitu&utm_term=mfq3cber0h\",\"UserId\":1,\"ShareType\":202004,\"PromotionChannelId\":0}";
            var doHttpClient = new DoHttpClient(new System.Net.Http.HttpClient());
            var result = new DoPostApiLogClient(requestUrl, postData)
                .GetPostResult<RespBaseChituDto<RespPostReceiceDto>>(
                    s => doHttpClient.PostByJson(requestUrl, postData).Result, Loger.Log4Net.Info);
            if (result == null || result.Status != 0)
            {
                Loger.Log4Net.Error($"CreateShareOrder http post ReceiveByWx fail." +
                                    (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                Console.WriteLine("CreateShareOrder http post ReceiveByWx fail");
                return;
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i);
            }
 
            Console.WriteLine("success");
        }

        [TestMethod]
        public void VerifyTaskArticleId()
        {
            var count = XYAuto.ITSC.Chitunion2017.BLL.LETask.LeWeixinOAuth.Instance.VerifyTaskArticleId(9782,
                  (int)LeTaskTypeEnum.ContentDistribute);
            Console.WriteLine(count);
        }
        [TestMethod]
        public void CreateShareTempQr()
        {
            var path = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()
            {
                UserId = 184,
                ChannelId = 101003,
                OrderUrl = "",
                TaskId = 1,
                TaskType = 192001
            }).CreateShareTempQr("http://192.168.3.76/group4/M00/01/E4/pIYBAFqVHhCAed3HAAFexG7RHAM994.png");
            Console.WriteLine(path);
        }

        public void Map()
        {
        }
    }
}
