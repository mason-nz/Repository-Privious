using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Net;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CallRecordForHB
    {
        public static CallRecordForHB Instance = new CallRecordForHB();
        public static string HbaseInterface_CallRecord = CommonFunction.ObjectToString(System.Configuration.ConfigurationManager.AppSettings["HbaseInterface_CallRecord"]);
        /// 查询话务数据
        /// <summary>
        /// 查询话务数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public CallRecordResult GetCallRecordFromHabse(CallRecordQuery query)
        {
            if (string.IsNullOrEmpty(HbaseInterface_CallRecord))
            {
                throw new Exception("HbaseInterface_CallRecord未配置！");
            }
            HttpWebResponse rep = HttpHelper.CreatePostHttpResponse(HbaseInterface_CallRecord, query.ToString(), null);
            string data = HttpHelper.GetResponseString(rep);
            CallRecordResult result = (CallRecordResult)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(CallRecordResult));
            result.JsonString = data;
            return result;
        }
    }
}
