using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.MobileOperate
{
    /// <summary>
    /// 注释：SMSServiceHelper
    /// 作者：zhanglb
    /// 日期：2018/5/16 19:47:20
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class SMSUtils
    {
        #region Instance
        public static readonly SMSUtils Instance = new SMSUtils();
        private const int AppId = 3;//1-行圆汽车，2-汽车大全，3-赤兔联盟 
        private const string AppGuid = "5FEE03D0-D15D-4AFB-B2BA-CCA530B7007E";
        //private const int appID = 2;//1-行圆汽车，2-汽车大全，3-赤兔联盟 
        //private const string appGuid = "E31F0293-E3E0-47F8-B1C6-B4167B25A47C";
        private int _curentTimespan = 0;//获取密钥时的当前时间戳
        private readonly string _smsServiceUrl = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("SMSServiceURL");//发短信接口API
        #endregion

        /// <summary>
        /// 获取调用接口时，需要的密钥
        /// </summary>
        /// <returns>返回密钥内容</returns>
        private string GetGetPassSecret()
        {
            _curentTimespan = ConvertDateTimeInt(DateTime.Now);
            string str = ("@@" + AppId.ToString() + AppGuid + _curentTimespan.ToString() + "__").ToUpper();
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="mobileList">手机号列表，多个用逗号分隔</param>
        /// <param name="content">发送短信内容</param>
        /// <returns>返回SendMsgResult类型</returns>
        public SendMsgResult SendMsgImmediately(string[] mobileList, string content)
        {
            SendMsgResult smr = new SendMsgResult();
            smr.Result = false.ToString();
            smr.Message = "发送短信失败成功，参数有误";
            if (mobileList != null && mobileList.Count() > 0 && !string.IsNullOrEmpty(content))
            {
                string phonelist = string.Empty;
                foreach (string phone in mobileList)
                {
                    phonelist += phone + ",";
                }
                phonelist = phonelist.TrimEnd(',');
                //Dictionary<string, string> parameters = new Dictionary<string, string>();
                //parameters.Add("appid", appID.ToString());//应用编号,不为空，测试值：1
                //parameters.Add("passkey", GetGetPassSecret());//加密密钥,调用接口GetPassSecret，获取秘钥(也可以自己生成)
                //parameters.Add("notecount", mobileList.Count().ToString());//短信号码个数,不为空，默认值1
                //parameters.Add("phonelist", phonelist);//号码列表,不为空
                //parameters.Add("t", curentTimespan.ToString());//时间戳，获取秘钥时的时间戳：注从1970年开始算
                //parameters.Add("noteContent", content);//短信内容，不为空
                //parameters.Add("SendUserId", string.Empty);//发送人ID，可为空
                //parameters.Add("SendUserIp", string.Empty);//发送人IP，可为空
                string parameters =  $"appid={AppId}&passkey={GetGetPassSecret()}&notecount={mobileList.Count().ToString()}&phonelist={phonelist}&t={_curentTimespan}&noteContent={System.Web.HttpUtility.UrlEncode(content)}&SendUserId=&SendUserIp=";
                HttpWebResponse hwr = CTUtils.Html.HtmlHelper.HttpPostRequest(_smsServiceUrl, parameters, null);
                //HttpWebResponse hwr = Common.HttpHelper.CreateGetHttpResponse(SMSServiceURL + "?" + parameters);
                string msg = CTUtils.Html.HtmlHelper.GetResponseString(hwr);
                smr = Newtonsoft.Json.JsonConvert.DeserializeObject<SendMsgResult>(msg);
            }
            return smr;
        }


        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">发送短信内容</param>
        /// <returns>返回SendMsgResult类型</returns>
        public SendMsgResult SendMsgImmediately(string mobile, string content)
        {
            if (string.IsNullOrEmpty(mobile) == false && string.IsNullOrEmpty(content) == false)
            {
                return SendMsgImmediately(new string[] { mobile }, content);
            }
            return SendMsgImmediately(new string[] { string.Empty }, null);
        }
    }


    [Serializable]
    public class SendMsgResult
    {
        public string Result { get; set; }

        public string Message { get; set; }
    }
}
