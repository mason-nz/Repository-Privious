using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using XYAuto.BUOC.ChiTuData2017.NunitTest.Common;

namespace XYAuto.BUOC.ChiTuData2017.NunitTest
{
    [TestFixture]
    public class TaskAPIHelperTest
    {
        private string appId = "";
        private string appkey = "";
        private string apiDomain = "";

        [SetUp]
        public void TaskHelper()
        {
            appId = "2";
            appkey = "EFE3B2B2-D3E4-4B57-9BCC-A24AF0682C88";
            apiDomain = "http://api1.chitunion.com";
        }

        /// <summary>
        /// 获取任务列表接口(92001:内容分发，92002:贴片广告)
        /// </summary>
        [Test]
        public void GetTaskListTest([Values(92001, 92002, null)] int? tasktype)
        {
            string tasktime = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd hh:mm:ss");//"2017-12-12 13:50:50";
            int timestamp = Common.Util.ConvertDateTimeInt(DateTime.Now);
            string url = apiDomain + "/TaskInfo/GetTaskList";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("appId", appId);
            dict.Add("appkey", appkey);
            dict.Add("tasktime", tasktime);
            dict.Add("page_index", "1");
            if (tasktype != null)
            {
                dict.Add("tasktype", tasktype.Value.ToString());//非必填项
            }
            dict.Add("version", "1.0");
            dict.Add("timestamp", timestamp.ToString());

            string sign = Common.HttpHelper.OrderPara(dict) + appkey;
            dict.Add("sign", Common.Util.GetMd5Hash(sign));

            var resp = Common.HttpHelper.CreatePostHttpResponse(url, dict, null, (int)RequestContentType.JSON);
            string respMsg = Common.HttpHelper.GetResponseString(resp);
            TaskListResult tr = JsonConvert.DeserializeObject<TaskListResult>(respMsg);

            Assert.AreEqual(true, tr.data.List.Count > 0);
        }

        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="taskid"></param>
        [Test]
        public void GetTaskStatusTest([Values(10, 75, 37, 84)] int taskid)
        {
            int timestamp = Common.Util.ConvertDateTimeInt(DateTime.Now);
            string url = apiDomain + "/TaskInfo/GetTaskStatus";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("appId", appId);
            dict.Add("appkey", appkey);
            dict.Add("taskid", taskid.ToString());
            dict.Add("version", "1.0");
            dict.Add("timestamp", timestamp.ToString());

            string sign = Common.HttpHelper.OrderPara(dict) + appkey;
            dict.Add("sign", Common.Util.GetMd5Hash(sign));

            var resp = Common.HttpHelper.CreatePostHttpResponse(url, dict, null, (int)RequestContentType.JSON);
            string respMsg = Common.HttpHelper.GetResponseString(resp);//.Trim('\"').Replace("\\", "")
            TaskStatus tr = JsonConvert.DeserializeObject<TaskStatus>(respMsg);

            Assert.AreEqual(true, tr.status > 0);
        }

        /// <summary>
        /// 获取任务素材列表
        /// </summary>
        /// <param name="taskid"></param>
        [Test]
        public void GetTaskMaterialListTest([Values(10, 75, 37, 84)] int taskid)
        {
            int timestamp = Common.Util.ConvertDateTimeInt(DateTime.Now);
            string url = apiDomain + "/TaskInfo/GetTaskMaterialList";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("appId", appId);
            dict.Add("appkey", appkey);
            dict.Add("taskid", taskid.ToString());
            dict.Add("version", "1.0");
            dict.Add("timestamp", timestamp.ToString());

            string sign = Common.HttpHelper.OrderPara(dict) + appkey;
            dict.Add("sign", Common.Util.GetMd5Hash(sign));

            var resp = Common.HttpHelper.CreatePostHttpResponse(url, dict, null, (int)RequestContentType.JSON);
            string respMsg = Common.HttpHelper.GetResponseString(resp);
            TaskMaterialList tr = JsonConvert.DeserializeObject<TaskMaterialList>(respMsg);

            Assert.AreEqual(true, !string.IsNullOrEmpty(tr.data.Title));
        }

        /// <summary>
        /// 获取用户推广链接
        /// </summary>
        /// <param name="taskid"></param>
        [Test]
        public void GetTaskOrderUrlTest([Values(10, 75, 37, 84)] int taskid, [Values("test-masj01", "34", "56", "")] string dataid)
        {
            int timestamp = Common.Util.ConvertDateTimeInt(DateTime.Now);
            string url = apiDomain + "/TaskInfo/GetTaskOrderUrl";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("appId", appId);
            dict.Add("appkey", appkey);
            dict.Add("taskid", taskid.ToString());
            dict.Add("useridentity", dataid);
            dict.Add("version", "1.0");
            dict.Add("timestamp", timestamp.ToString());

            string sign = Common.HttpHelper.OrderPara(dict) + appkey;
            dict.Add("sign", Common.Util.GetMd5Hash(sign));

            var resp = Common.HttpHelper.CreatePostHttpResponse(url, dict, null, (int)RequestContentType.JSON);
            string respMsg = Common.HttpHelper.GetResponseString(resp);
            TaskOrderUrl tr = JsonConvert.DeserializeObject<TaskOrderUrl>(respMsg);

            Assert.AreEqual(true, !string.IsNullOrEmpty(tr.orderurl));
        }


        /// <summary>
        /// 获取按推广链接的统计数据
        /// </summary>
        /// <param name="taskid"></param>
        [Test]
        public void GetStatisticsByOrderUrlTest([Values("http://news1.chitu.qichedaquan.com/materiel/chitunion/mobile/20171218/9568.html?utm_source=chitu&utm_term=iNGKHQ9vHm", "http://news1.chitu.qichedaquan.com/ct_m/20171227/9590.html?utm_source=chitu&utm_term=Mkr3Cwttpk")] string orderurl)
        {
            int timestamp = Common.Util.ConvertDateTimeInt(DateTime.Now);
            string url = apiDomain + "/TaskInfo/GetStatisticsByOrderUrl";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("appId", appId);
            dict.Add("appkey", appkey);

            dict.Add("orderurl", orderurl);
            //dict.Add("begindate", "");
            dict.Add("enddate", "2017-12-30");
            dict.Add("page_index", "1");

            dict.Add("version", "1.0");
            dict.Add("timestamp", timestamp.ToString());

            string sign = Common.HttpHelper.OrderPara(dict) + appkey;
            dict.Add("sign", Common.Util.GetMd5Hash(sign));

            var resp = Common.HttpHelper.CreatePostHttpResponse(url, dict, null, (int)RequestContentType.JSON);
            string respMsg = Common.HttpHelper.GetResponseString(resp);
            TaskStatisticsResult tr = JsonConvert.DeserializeObject<TaskStatisticsResult>(respMsg);

            Assert.AreEqual(true, tr.data.List.Count > 0);
        }

        /// <summary>
        /// 获取授权码
        /// </summary>
        [Test]
        public void GetAppKeyTest()
        {
            int timestamp = Common.Util.ConvertDateTimeInt(DateTime.Now);
            string url = apiDomain + "/TaskInfo/GetAppKey";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("appId", appId);
            dict.Add("appkey", appkey);
            dict.Add("version", "1.0");
            dict.Add("timestamp", timestamp.ToString());

            string sign = Common.HttpHelper.OrderPara(dict) + appkey;
            dict.Add("sign", Common.Util.GetMd5Hash(sign));

            var resp = Common.HttpHelper.CreatePostHttpResponse(url, dict, null, (int)RequestContentType.JSON);
            string respMsg = Common.HttpHelper.GetResponseString(resp);
            AppKeyResult tr = JsonConvert.DeserializeObject<AppKeyResult>(respMsg);

            Assert.AreEqual(true, !string.IsNullOrEmpty(tr.appkey));
        }
    }

    #region 接口返回实体

    /// <summary>
    /// 获取任务列表接口——接口返回json结构
    /// </summary>
    public class TaskListResult
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public TaskList data { get; set; }
    }

    public class TaskList
    {
        public List<TaskInfo> List { get; set; }
    }

    /// <summary>
    /// 任务基本信息
    /// </summary>
    public class TaskInfo
    {
        public int TaskID { get; set; }

        public string Title { get; set; }

        public int CategoryID { get; set; }

        public string ImgUrl { get; set; }

        public decimal CPCPrice { get; set; }
        public decimal CPLPrice { get; set; }
        public int TaskType { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int Status { get; set; }
        public int TakeCount { get; set; }
        public int RuleCount { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 任务状态——接口返回json结构
    /// </summary>
    public class TaskStatus
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public int status { get; set; }
    }


    /// <summary>
    /// 获取任务素材列表——接口返回json结构
    /// </summary>
    public class TaskMaterialList
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public TaskDetailInfo data { get; set; }
    }

    public class TaskDetailInfo
    {
        public string Title { get; set; }
        public string MaterielUrl { get; set; }
        public TaskDetailHead Head { get; set; }
        public List<TaskDetailWaist> Waist { get; set; }
        public TaskDetailFoot Foot { get; set; }
    }

    public class TaskDetailHead
    {
        public string Content { get; set; }
    }

    public class TaskDetailFoot
    {
        public string Content { get; set; }
    }

    public class TaskDetailWaist
    {
        public string Title { get; set; }
        public string HeadImg { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// 获取用户推广链接——接口返回json结构
    /// </summary>
    public class TaskOrderUrl
    {
        public string code { get; set; }
        public string msg { get; set; } = string.Empty;
        public string orderurl { get; set; }
    }


    /// <summary>
    /// 获取按推广链接的统计数据——接口返回json结构
    /// </summary>
    public class TaskStatisticsResult
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public TaskStatisticsList data { get; set; }
    }

    public class TaskStatisticsList
    {
        public List<TaskStatisticsskInfo> List { get; set; }
    }

    public class TaskStatisticsskInfo
    {
        public string Date { get; set; }
        public int TaskId { get; set; }
        public string OrderUrl { get; set; }
        public int PV { get; set; }
        public int UV { get; set; }
        public int Clue { get; set; }
    }

    /// <summary>
    /// 获取授权码——接口返回json结构
    /// </summary>
    public class AppKeyResult
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public string appkey { get; set; }
    }

    #endregion
}
