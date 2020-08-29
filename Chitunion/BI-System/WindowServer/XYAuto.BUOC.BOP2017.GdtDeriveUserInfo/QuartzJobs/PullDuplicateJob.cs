using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XYAuto.BUOC.BOP2017.GdtDeriveUserInfo.QuartzJobs
{
    public sealed class PullDuplicateJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            DataSet ds = BLL.GDT.GDTDeriveUserInfo.Instance.GetClueIds(1, DateTime.Now.AddHours(-5));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddmmss");
                StringBuilder clues = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    clues.Append(dr["ID"] + ",");
                }
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appkey", ConfigurationManager.AppSettings["AppKey"].ToString());
                dic.Add("appsecret", ConfigurationManager.AppSettings["AppSecret"].ToString());
                dic.Add("timestamp", timestamp);
                dic.Add("ClueIDs",clues.ToString().TrimEnd(','));

                StringBuilder postdate = new StringBuilder();
                Dictionary<string,string> orderby = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                foreach (var item in orderby)
                {
                    postdate.Append(item.Value);
                }
                string signature = HttpUtility.UrlEncode(CommonHelper.SHA1(postdate.ToString()));
                string postdata = "ClueIDs=" + clues.ToString().TrimEnd(',');
                string result = PushClueData(postdata, signature, timestamp);

                Duplicate duplicate = Newtonsoft.Json.JsonConvert.DeserializeObject<Duplicate>(result);
                if (duplicate != null && duplicate.Success.ToLower()=="true" && duplicate.Data.Length > 0)
                {
                    List<Data> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Data>>(duplicate.Data.ToString());
                    foreach (var item in list)
                    {
                        Entities.GDT.GDTDuplicate entity = new Entities.GDT.GDTDuplicate();
                        entity.ClueID = item.ClueID;
                        entity.Results =item.Results;
                        entity.Reason = item.Reason;
                        int id = BLL.GDT.GDTDuplicate.Instance.Insert(entity);
                        if (id > 0)
                        {
                            //DeriveUserInfo Status  NULL：未推送  1：推送成功  2：小去重获取线索成功
                            BLL.GDT.GDTDeriveUserInfo.Instance.UpdaetDeriveUserInfoByClueId(entity.ClueID,2);
                        }
                    }
                }
            }
        }

        private static string PushClueData(string postdata, string signature, string timestamp)
        {
            string url = string.Format(ConfigurationManager.AppSettings["Duplicate"].ToString(), signature, timestamp);
            string result = CommonHelper.PostHttp(url, postdata, "application/x-www-form-urlencoded");
            return result;
        }
    }

    public class Duplicate
    {
        public string Success { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Data { get; set; }
    }

    public class Data
    {
        public int ClueID { get; set; }
        public int Results { get; set; }
        public string Reason { get; set; }
    }
}
