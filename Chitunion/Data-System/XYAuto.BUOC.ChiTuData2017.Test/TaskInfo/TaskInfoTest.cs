/********************************************************
*创建人：hant
*创建时间：2017/12/18 17:28:01 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request;

using XYAuto.BUOC.ChiTuData2017.TaskAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.TaskInfo
{
    [TestClass]
    public class TaskInfoTest
    {

        [TestMethod]
        public void GetGetTaskList()
        {
            var resp = new RequestGetTaskList();
            resp.appId = "2";
            resp.appkey = "EFE3B2B2-D3E4-4B57-9BCC-A24AF0682C88";
            resp.page_index = "1";
            resp.sign = "bf98b6cbbe5eb09d84468c12421764d0";//7d350b3fa41024c9d9f51a31044245e2
            resp.tasktime = "2017-12-25 10:14:19";
            resp.timestamp = "1111111";
            resp.version = "1.0";
            string code = string.Empty;
            string msg = string.Empty;
            JsonResult J = Util.GetJsonDataByResult(BLL.TaskInfo.TaskInfo.Instance.GetTaskList(resp, ref code, ref msg), code, msg);
            string tt = Newtonsoft.Json.JsonConvert.SerializeObject(J);
        }


        [TestMethod]
        public void GetAppKey()
        {
            var resp = new RequestBase();
            resp.appId = "1";
            resp.appkey = "38B5BB53-0719-4691-9BB8-48224EB27A80";
            resp.sign = "6cfd1867b4b1b1cc83857d6c8a9ace0f";//a429d4177c19c1401f8110a534fba8f6
            resp.timestamp = "20171218173109";
            resp.version = "1.0";
            string code = string.Empty;
            string msg = string.Empty;
            string tt = BLL.TaskInfo.TaskInfo.Instance.GetAppKey(resp, ref code, ref msg);
        }


        [TestMethod]
        public void GetTaskStatus()
        {
            var resp = new RequestGetTaskStatus();
            resp.appId = "1";
            resp.appkey = "38B5BB53-0719-4691-9BB8-48224EB27A80";
            resp.sign = "5ea1254599661dcc9fbf556b9d0b23b0";//a429d4177c19c1401f8110a534fba8f6    5ea1254599661dcc9fbf556b9d0b23b0
            resp.timestamp = "20171218173109";
            resp.version = "1.0";
            resp.taskid = "1";
            string code = string.Empty;
            string msg = string.Empty;
            string tt = BLL.TaskInfo.TaskInfo.Instance.GetTaskStatus(resp, ref code, ref msg);
        }

        [TestMethod]
        public void GetTaskMaterialList()
        {
            var resp = new RequestTaskMaterialList();
            resp.appId = "1";
            resp.appkey = "38B5BB53-0719-4691-9BB8-48224EB27A80";
            resp.sign = "5ea1254599661dcc9fbf556b9d0b23b0";//5ea1254599661dcc9fbf556b9d0b23b0
            resp.timestamp = "20171218173109";
            resp.version = "1.0";
            resp.taskid = "1";
            string code = string.Empty;
            string msg = string.Empty;
            JsonResult J = Util.GetJsonDataByResult(BLL.TaskInfo.Material.Instance.GetTaskMaterialList(resp, ref code, ref msg), code, msg);
            string tt = Newtonsoft.Json.JsonConvert.SerializeObject(J);
        }

        [TestMethod]
        public void GetGetTaskOrderUrl()
        {
            var resp = new RequestTaskOrderUrl();
            resp.appId = "2";
            resp.appkey = "EFE3B2B2-D3E4-4B57-9BCC-A24AF0682C88";
            resp.sign = "c12881779111d6935321d47d8415f84c";//5ea1254599661dcc9fbf556b9d0b23b0
            resp.timestamp = "20171218173109";
            resp.version = "1.0";
            resp.taskid = "38";
            resp.useridentity = "ttttttt";
            string code = string.Empty;
            string msg = string.Empty;
            string tt = BLL.TaskInfo.TaskInfo.Instance.GetTaskOrderUrl(resp, ref code, ref msg);
        }


        [TestMethod]
        public void GetStatisticsByOrderUrl()
        {
            var resp = new RequestGetStatisticsByOrderUrl();
            resp.appId = "2";
            resp.appkey = "EFE3B2B2-D3E4-4B57-9BCC-A24AF0682C88";
            resp.sign = "6b215cc19e92021b96af59b7b16b4a71";//5ea1254599661dcc9fbf556b9d0b23b0
            resp.timestamp = "1514361878";
            resp.version = "1.0";
            //resp.begindate = "";
            resp.enddate = "2017-12-30";
            resp.orderurl = "http://news1.chitu.qichedaquan.com/materiel/chitunion/mobile/20171218/9568.html?utm_source=chitu&utm_term=iNGKHQ9vHm";
            string code = string.Empty;
            string msg = string.Empty;
            JsonResult J = Util.GetJsonDataByResult(BLL.TaskInfo.Statistics.Instance.GetStatisticsByOrderUrl(resp, ref code, ref msg), code, msg);
            string tt = Newtonsoft.Json.JsonConvert.SerializeObject(J);
        }


        [TestMethod]
        public void post()
        {
            string appkey = "4D76515B-86D5-4D09-9F2D-088D0FACC3F4";
            string postUrl = "http://www1.chitunion.com/api/ThirdBusiness/OrderStorage";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("appId", "2");
            dic.Add("appkey", "4D76515B-86D5-4D09-9F2D-088D0FACC3F4");
            dic.Add("version", "1.0");
            dic.Add("timestamp", "1111111");
            //dic.Add("page_index", "1");
            #region
            postUrl = string.Format(postUrl, "GetTaskOrderUrl");
            dic.Add("taskid", "37");
            dic.Add("useridentity", "teste");
            #endregion
            StringBuilder sb = new StringBuilder();
            StringBuilder spost = new StringBuilder();
            var order = dic.OrderBy(o => o.Key);
            foreach (var item in order)
            {
                sb.Append(item.Key + item.Value);
                spost.Append(item.Key + "=" + item.Value + "&");
            }

            string sign = GetMd5Hash(sb.ToString() + appkey);
            string paramData = spost.ToString() + "sign=" + sign;
            string postdate = "TaskId=38&UserIdentity=test&ChannelId=101002";
            string ret = string.Empty;
            try
            {
                //if (!postUrl.StartsWith("http://"))
                //    //return "";

                byte[] byteArray = Encoding.Default.GetBytes(postdate); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
            //return ret;
        }

        private static string GetMd5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        [TestMethod]
        public void VerifySign()
        {
            RequestGetTaskList req = new RequestGetTaskList();
            req.appId = "1";
            req.appkey = "EFE3B2B2-D3E4-4B57-9BCC-A24AF0682C88";
            req.tasktime = "2017-12-07 05:08:03";
            req.page_index = "1";
            req.version = "1.0";
            req.timestamp = "1514365683";
            req.tasktype = null;
            //req.sign = "4f91de0b3edc1a07b570851c24ad2cbf";
            string sign = Authentication.Instance.GetSign<RequestGetTaskList>(req, req.appkey);

        }

    }
}
