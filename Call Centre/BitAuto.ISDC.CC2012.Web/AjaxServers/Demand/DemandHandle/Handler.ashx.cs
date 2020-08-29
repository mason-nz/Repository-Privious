using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Demand.DemandHandle
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action { get { return (Request["Action"] + "").Trim(); } }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action)
            {
                case "getstatusnum": GetStatusNum(out msg);
                    break;
            }
            context.Response.Write("{ " + msg + "}");
        }

        //获取任务各状态下的数量
        private void GetStatusNum(out string msg)
        {
            msg = string.Empty;
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus));
            Hashtable ht = new Hashtable();

            BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery query = BLL.Util.BindQuery<BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery>(HttpContext.Current);
            if (query != null)
            {
                query.Status = 0;
                query.Where = " And YJKDemandInfo.Status<>" + (int)BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus.Revoke;
                Dictionary<int, int> dic = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandTagCount(query, BLL.Util.GetLoginUserID());
                
                dt.Columns.Add("count");
                foreach (KeyValuePair<int, int> kp in dic)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (kp.Key == int.Parse(dt.Rows[i]["value"].ToString()))
                        {
                            dt.Rows[i]["count"] = kp.Value;
                        }
                    }
                }
            }

            //拼接起来
            for (int i = 0, len = dt.Rows.Count; i < len; i++)
            {
                DataRow dr = dt.Rows[i];
                if(dr["Value"].ToString()!=((int)BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus.Revoke).ToString())
                {
                    string count = dr["count"].ToString();
                    if (string.IsNullOrEmpty(count)) { count = "0"; }
                    msg += "'" + dr["name"].ToString() + "':['" + dr["value"].ToString() + "','" + count + "'],";
                }
            }
            msg = msg.Substring(0, msg.Length - 1);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}