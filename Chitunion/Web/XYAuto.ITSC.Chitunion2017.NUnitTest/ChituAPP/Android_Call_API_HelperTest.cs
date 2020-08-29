using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers;
using XYAuto.ITSC.Chitunion2017.BLL.AppAuth;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.ChituAPP
{
    [TestFixture]
    public class Android_Call_API_HelperTest
    {
        [SetUp]
        public void Android_Call_API_Helper()
        {

        }

        /// <summary>
        /// 验证DES加解密
        /// </summary>
        [Test]
        public void VerifyDesEncrypt()
        {
            string originStr = "appidDF5605CC-B4F2-4989-8AAE-7D9BD1F55179para11234para2Falsetimestamp1524798972453version1.0DF5605CC-B4F2-4989-8AAE-7D9BD1F55179";
            string secretStr = "chituapp";
            string desEncryptStr = DESHelper.ToDESEncrypt(originStr, secretStr);
            string desDecryptStr = DESHelper.ToDESDecrypt(desEncryptStr, secretStr);
            Assert.AreEqual(desDecryptStr, originStr);
        }

        /// <summary>
        /// 验证url参数
        /// </summary>
        [Test]
        public void VerifySign()
        {
            //string dd2 = "appidDF5605CC-B4F2-4989-8AAE-7D9BD1F55179mobile13691486115timestamp1524902375782version1.0DF5605CC-B4F2-4989-8AAE-7D9BD1F55179";

            //string jiami=  DESHelper.ToDESEncrypt(dd2, "chituapp");
            //string jiemi = DESHelper.ToDESDecrypt("4EXfVrw4G7EYQrj3DX0lj35C6oWr0Gxx4QVVZzF2Tx3TwastIjnaHuX8thk6 J6MYJTIYCtXwGKY589uLgCDEF8WTg2XV5EUjyty9pi7X/xDIOzc+CbADh7Jd aO4jfFBvoGo1oecsLiY4I9mf0BPuBtgunz7HR6/+f4HhTYyXyrA=", "chituapp");

            EnumAppAuthRequestVerify dd = new EnumAppAuthRequestVerify();
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("appid", "DF5605CC-B4F2-4989-8AAE-7D9BD1F55179");
            //para.Add("timestamp", BLL.Util.ConvertDateTimeInt(DateTime.Now).ToString());
            para.Add("timestamp", "1524902375782");
            para.Add("version", "1.0");
            para.Add("mobile", "13691486115");
            //para.Add("para1", "abc");
            para.Add("sign", AppAuthHelper.Instance.GetSign(para, ref dd));

            bool flag = AppAuthHelper.Instance.VerifySign(para, ref dd);
            Assert.AreEqual(true, flag);
        }


        /// <summary>
        /// 验证url请求
        /// </summary>
        /// <param name="url"></param>
        [Test]
        public void UrlSignTest([Values("http://app1.chitunion.com/api/Test/TestUrlSign")] string url)
        {
            EnumAppAuthRequestVerify dd = new EnumAppAuthRequestVerify();
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("appid", "DF5605CC-B4F2-4989-8AAE-7D9BD1F55179");
            para.Add("timestamp", BLL.Util.ConvertDateTimeInt(DateTime.Now).ToString());
            para.Add("version", "1.0");
            para.Add("para2", "1234");
            para.Add("para1", "abc");
            para.Add("sign", AppAuthHelper.Instance.GetSign(para, ref dd));


            Dictionary<string, string> para2 = new Dictionary<string, string>();
            para2.Add("appid", para["appid"].ToString());
            para2.Add("timestamp", para["timestamp"].ToString());
            para2.Add("version", para["version"].ToString());
            para2.Add("para2", para["para2"].ToString());
            para2.Add("para1", para["para1"].ToString());
            para2.Add("sign", para["sign"].ToString());

            //HttpWebResponse hwr = XYAuto.ITSC.Chitunion2017.WebService.Common.HttpHelper.CreateGetHttpResponse(url);
            HttpWebResponse hwr = XYAuto.ITSC.Chitunion2017.WebService.Common.HttpHelper.CreatePostHttpResponse(url, para2, null, 1);
            string msg = XYAuto.ITSC.Chitunion2017.WebService.Common.HttpHelper.GetResponseString(hwr);
            JsonResult jr = JsonConvert.DeserializeObject<JsonResult>(msg);
            Assert.AreEqual("OK", jr.Message);
        }



    }
}
