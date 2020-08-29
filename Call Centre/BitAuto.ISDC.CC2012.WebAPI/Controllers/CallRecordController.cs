using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using BitAuto.ISDC.CC2012.WebAPI.Helper;
using Newtonsoft.Json;
using BitAuto.ISDC.CC2012.Entities;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{
    public class CallRecordController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage QueryAniDnis(string Oriani, string OriDnis)
        {
            CallRecordQueryModel query = new CallRecordQueryModel();
            query.QueryType = 1;
            query.Oriani = Oriani;
            query.OriDnis = OriDnis;
            return Query(query);
        }
        [HttpGet]
        public HttpResponseMessage QueryAni(string Oriani)
        {
            CallRecordQueryModel query = new CallRecordQueryModel();
            query.QueryType = 2;
            query.Oriani = Oriani;
            return Query(query);
        }
        [HttpGet]
        public HttpResponseMessage QueryAni(string Oriani, int top)
        {
            CallRecordQueryModel query = new CallRecordQueryModel();
            query.QueryType = 2;
            query.Oriani = Oriani;
            query.Top = Math.Min(top, query.Top);
            return Query(query);
        }
        [HttpGet]
        public HttpResponseMessage QueryDnis(string OriDnis)
        {
            CallRecordQueryModel query = new CallRecordQueryModel();
            query.QueryType = 3;
            query.OriDnis = OriDnis;
            return Query(query);
        }
        [HttpGet]
        public HttpResponseMessage QueryDnis(string OriDnis, int top)
        {
            CallRecordQueryModel query = new CallRecordQueryModel();
            query.QueryType = 3;
            query.OriDnis = OriDnis;
            query.Top = Math.Min(top, query.Top);
            return Query(query);
        }
        [HttpGet]
        public HttpResponseMessage QueryCallId(string callid)
        {
            CallRecordQueryModel query = new CallRecordQueryModel();
            query.QueryType = 4;
            query.CallID = callid;
            return Query(query);
        }

        /// 查询话务大数据接口具体实现
        /// <summary>
        /// 查询话务大数据接口具体实现
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(CallRecordQueryModel query)
        {
            HttpHBResult result = new HttpHBResult();

            try
            {
                //校验接口
                bool flag = CommonHelper.CheckIP("[查询话务大数据接口]");
                if (!flag)
                {
                    throw new Exception("IP地址非法");
                }
                //校验参数
                CommonHelper.Log("查询方式：" + query.QueryType);
                string error = "";
                if (!query.Validate(out error))
                {
                    CommonHelper.Log(error);
                    throw new Exception(error);
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //执行查询
                var hbquery = query.GetCallRecordQuery();
                CommonHelper.Log("查询条件：" + hbquery);
                CallRecordResult info = BLL.CallRecordForHB.Instance.GetCallRecordFromHabse(hbquery);
                CommonHelper.Log("查询结果：" + (info != null ? info.ToString() : "null"));
                sw.Stop();
                //返回结果
                result.Result = info != null;
                result.Error = (info != null ? "" : "查询失败，发生某种未知的错误");
                result.Data = (info != null ? info.JsonString : "");
                result.QueryMilliseconds = sw.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                CommonHelper.Log("查询话务大数据接口 异常", ex);
                result.Result = false;
                result.Error = ex.Message;
                result.Data = "";
            }
            string content = JsonConvert.SerializeObject(result);
            return new HttpResponseMessage { Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json") };
        }
    }
}
