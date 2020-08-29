using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace XYAuto.ITSC.Chitunion2017.WebService.GSData
{
    public class GSDataHelper
    {
        #region Instance
        public static readonly GSDataHelper Instance = new GSDataHelper();
        private string GSDataAPI_APP_ID = WebConfigurationManager.AppSettings["GSDataAPI_APP_ID"];//发短信接口API
        private string GSDataAPI_APP_KEY = WebConfigurationManager.AppSettings["GSDataAPI_APP_KEY"];//发短信接口API
        private string GSDataAPI_BaseURL = WebConfigurationManager.AppSettings["GSDataAPI_BaseURL"];//发短信接口API
        #endregion


        public GSDataGroupResult GetGroupInfo()
        {
            string url = GSDataAPI_BaseURL + "/api/wx/wxapi/group_name";      //接口地址
            Dictionary<string, object> postData = new Dictionary<string, object>();     //post参数，按接口要求添加。不用添加appid和appkey
            GSDataSDK api = new GSDataSDK(GSDataAPI_APP_ID, GSDataAPI_APP_KEY);       //初始化接口
            string result = api.Call(postData, url);               //调用接口

            GSDataResult gsDataResult = JsonConvert.DeserializeObject<GSDataResult>(result);
            if (gsDataResult != null && gsDataResult.ReturnCode == "1001")
            {
                GSDataGroupResult gr = JsonConvert.DeserializeObject<GSDataGroupResult>(gsDataResult.ReturnData.ToString());
                if (gr != null && gr.GroupCount > 0)
                {
                    return gr;
                }
            }
            return null;
        }
    }
}
