using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.Test.Logging;

namespace XYAuto.BUOC.ChiTuData2017.Test
{
    [TestClass]
    public class HttpClientTest
    {
        [TestMethod]
        public void Log4NetTest()
        {
            //Infrastruction.Loger.Log4Net.Info($"this is log test");

            Infrastruction.Loger.Log4Net.Error($"this is Error test");
            //Infrastruction.Loger.ZhyLogger.Info($"this is ZhyLogger test");

            string subject = "ChiTuData2017_WebApi——报错通知";
            BLL.EmailHelper.Instance.SendErrorMail("测试发送邮件~", subject, new string[] { "lix@xingyuanauto.com" });

            //Infrastruction.Loger.ZhyLogger.Error($"this is ZhyLogger Error test");
            // new LogHelper().Info("this is LogHelper");
            //var str = "频繁加班视力变差，不怪电脑都怪它 | 美好测评.zip";

            //Console.WriteLine(str.Replace(" ", ""));
            //Console.WriteLine(str.ToSqlFilter());
            //string rPath = @"D:\GitRoot\A5信息系统研发\销售业务管理平台\Chitunion\XYAuto.ITSC.Chitunion2017.Test\V1_1\UploadFiles\Materiel\Temp\20170915182143\频繁加班视力变差，不怪电脑都怪它|美好测评.zip";

            //StringBuilder rBuilder = new StringBuilder(rPath);
            //foreach (char rInvalidChar in Path.GetInvalidPathChars())
            //{
            //    rBuilder.Replace(rInvalidChar.ToString(), string.Empty);
            //}
            //Console.WriteLine(rBuilder.ToString());
        }

        private string GetReplacePath(string rPath)
        {
            var rBuilder = new StringBuilder(rPath);
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                rBuilder.Replace(rInvalidChar.ToString(), string.Empty);
            }
            return rBuilder.ToString();
        }

        [TestMethod]
        public void Get()
        {
            var client = new DoHttpClient();
            //var content = client.Get("http://www.kuaidi100.com/query?type=yuantong&postid=11111111111");
            //Console.WriteLine(content.Result);

            var str = "[{\"field\":\"system_status\",\"operator\":\"EQUALS\",\"values\":[\"AD_STATUS_NORMAL\"]}]";

            Console.WriteLine(str);
            for (int i = 0; i < 10; i++)
            {
                var startTime = DateTime.Now;

                var content = client.Get("https://sandbox-api.e.qq.com/v1.0/adgroups/get?access_token=39d1314ff788dce84f7b533f23d39b0b&timestamp=1508302228&nonce=1aab2d1d40944fd697d8bfaeefde59b3" +
                      "&account_id=100000606&page=1&page_size=20" +
                      "&filtering=[{\"field\":\"system_status\",\"operator\":\"EQUALS\",\"values\":[\"AD_STATUS_NORMAL\"]}]");
                Console.WriteLine(content.Result);
                Console.WriteLine($"第{i}次请求，花费时间:{(DateTime.Now - startTime).TotalMilliseconds} ms");
            }
        }

        [TestMethod]
        public void PostByFormTest()
        {
            var client = new DoHttpClient();
            var client1 = new DoHttpClient(new System.Net.Http.HttpClient());
            //var url = "http://api.xingyuanauto.com/organize/status?appkey=chituads&signature=MqKDKjmRkmzzuVOX2t%2fsChxzbuA%3d&timestamp=1504609123";
            //var postData = "OrganizeID=64&OrganizeAdsID=100160&OrganizeAdsStatus=7&Reject=无";
            //var content = client.PostByForm(url, postData);
            //Console.WriteLine(content.Result);
        }

        [TestMethod]
        public void TestLog()
        {
            for (int i = 0; i < 100; i++)
            {
                Infrastruction.Loger.Log4Net.Info($"this is log test.this is log testthis is log testthis is log test"
                                                 );
                Infrastruction.Loger.Log4Net.Error($"this is Error"
                                                );
                Infrastruction.Loger.ZhyLogger.Info($"this is log test.this is log testthis is log testthis is log test");
                Infrastruction.Loger.ZhyLogger.Error($"this is log test-------------------.this Error");
            }
        }
    }
}