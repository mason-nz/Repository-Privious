using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.AppAuth;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    public class TestController : ApiController
    {
        [CrossSite]
        //[HttpGet]
        [CTAppAuth]
        [HttpPost]
        public Common.JsonResult TestUrlSign([FromBody]ReqTestUrlSignDto request)
        {
            return Common.Util.GetJsonDataByResult(request);
        }
    }


    /// <summary>
    /// Demo测试请求类
    /// </summary>
    public class ReqTestUrlSignDto
    {

        public string appid { get; set; } = string.Empty;
        public string sign { get; set; } = string.Empty;
        public string version { get; set; } = string.Empty;

        public long timestamp { get; set; }

        /// <summary>
        /// 参数1
        /// </summary>
        public int para1 { get; set; }

        /// <summary>
        /// 参数2
        /// </summary>
        public bool para2 { get; set; }
    }
}