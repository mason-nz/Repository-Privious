using GeetestSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class GeetestHelper
    {
        //public static readonly BLL.GeetestHelper Instance = new BLL.GeetestHelper("");
        private string _geetestConfig_ID = ConfigurationUtil.GetAppSettingValue("GeetestConfig_ID", false);
        private string _geetestConfig_Key = ConfigurationUtil.GetAppSettingValue("GeetestConfig_Key", false);


        public GeetestHelper(string config_id, string config_key)
        {
            _geetestConfig_ID = config_id;
            _geetestConfig_Key = config_key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequestGeetestChallenge"></param>
        /// <param name="RequestGeetestValidate"></param>
        /// <param name="RequestGeetestSeccode"></param>
        /// <returns></returns>
        public bool VerifyCode(string RequestGeetestChallenge, string RequestGeetestValidate, string RequestGeetestSeccode)
        {
            try
            {
                GeetestLib geetest = new GeetestLib(_geetestConfig_ID, _geetestConfig_Key);
                Byte gt_server_status_code = (Byte)System.Web.HttpContext.Current.Session[GeetestLib.gtServerStatusSessionKey];
                //String userID = (String)Session["userID"];
                int result = 0;
                //String challenge = Request.Form.Get(GeetestLib.fnGeetestChallenge);
                //String validate = Request.Form.Get(GeetestLib.fnGeetestValidate);
                //String seccode = Request.Form.Get(GeetestLib.fnGeetestSeccode);
                if (gt_server_status_code == 1)
                {
                    result = geetest.enhencedValidateRequest(RequestGeetestChallenge, RequestGeetestValidate, RequestGeetestSeccode);
                }
                else
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"调用极验接口验证失败,Challenge={RequestGeetestChallenge},Validate={RequestGeetestValidate},Seccode={RequestGeetestSeccode}");
                    result = geetest.failbackValidateRequest(RequestGeetestChallenge, RequestGeetestValidate, RequestGeetestSeccode);
                }

                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error($"调用极验接口验证逻辑异常,Challenge={RequestGeetestChallenge},Validate={RequestGeetestValidate},Seccode={RequestGeetestSeccode}", ex);
                return false;
            }
        }
    }
}
