using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using BitAuto.DSC.OASysRightManager2016.Common.Common;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using BitAuto.Utils.Config;
using BitAuto.Utils.Security;
using System.Text;

namespace BitAuto.DSC.APPReport2016.WebAPI.Filter
{
    public class LoginAuthorize : AuthorizeAttribute
    {
        ///// <summary>
        ///// 是否需要验证IP（默认需要）
        ///// </summary>
        //public bool IsCheckIP = true;

        private string SYSID = ConfigurationUtil.GetAppSettingValue("ThisSysID", false);

        //private string CheckRightModuleID = ConfigurationUtil.GetAppSettingValue("ReportModuleID", false);
        protected static string cookieName = "app-report";//仪表盘报表登录验证cookies的key

        /// <summary>
        /// 是否验证“仪表盘”功能点权限
        /// </summary>
        public string CheckReportRight = string.Empty;

        /// <summary>
        /// 是否需要验证登录（默认需要）
        /// </summary>
        public bool IsCheckLogin = true;

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            
            bool flag = true;
            int loginUserid = -1;
            try
            {
                if (IsCheckLogin)
                {
                    flag = VerifyEmployeeNumberByPage(out loginUserid);
                }
                if (flag && !string.IsNullOrEmpty(CheckReportRight))
                {
                    bool f2 = BitAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.CheckRight(CheckReportRight, SYSID, loginUserid);
                    flag = f2 & flag;
                    actionContext.ActionArguments.Add("CheckReportRight", f2);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("IsAuthorized验证失败", ex);
                flag = false;
            }
            //if (IsCheckIP)
            //{
            //    bool f2 = Common.Util.CheckIP();
            //    flag = f2 & flag;
            //}
            return flag;
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            Common.Loger.Log4Net.Info(string.Format("[LoginAuthorize]用户验证失败，用户IP {0}, 用户请求的地址为 {1},UrlRefer为 {2}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.UrlReferrer));

            var unAuthenResult = new JsonResult()
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = "身份验证失败",
                Result = false
            };

            if (actionContext.ActionArguments.ContainsKey("CheckReportRight")&&
                bool.Parse(actionContext.ActionArguments["CheckReportRight"].ToString())==false)
            {
                unAuthenResult = new JsonResult()
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Message = "功能权限验证失败",
                    Result = false
                };
            }

            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent(JsonConvert.SerializeObject(unAuthenResult))
            };
        }


        /// <summary>
        /// 页面访问时，验证参数UserID
        /// </summary>
        /// <returns>验证通过，返回True，否则返回False</returns>
        private bool VerifyEmployeeNumberByPage(out int userid)
        {
            string cookiesEncrypt = string.Empty;//加密字符串
            string employeeNumber = string.Empty;//员工编号
            bool flag = false;
            try 
	        {
                cookiesEncrypt = System.Web.HttpContext.Current.Request.Cookies[cookieName].Value.ToLower();
                employeeNumber = cookiesEncrypt.Split('|')[1].ToString();
                cookiesEncrypt = cookiesEncrypt.Split('|')[0].ToString();
	        }
	        catch (Exception ex)
	        {
                BLL.Loger.Log4Net.Error("访问仪表盘相关页面时，验证Cookies内容异常：",ex);
                userid = -1;
                return false;
	        }
            if (!string.IsNullOrEmpty(employeeNumber))
            {
                DateTime dt = DateTime.Now;
                string genEncrypt = DESEncryptor.MD5Hash(dt.ToString("MM|yyyy|dd") + employeeNumber + cookieName, Encoding.GetEncoding("utf-8"));
                string tempEncrypt = string.Empty;

                if ((dt.Hour == 23 && dt.Minute >= 45))
                {
                    tempEncrypt = DESEncryptor.MD5Hash(dt.AddDays(1).ToString("MM|yyyy|dd") + employeeNumber + cookieName, Encoding.GetEncoding("utf-8"));
                }
                else if ((dt.Hour == 0 && dt.Minute <= 15))
                {
                    tempEncrypt = DESEncryptor.MD5Hash(dt.AddDays(-1).ToString("MM|yyyy|dd") + employeeNumber + cookieName, Encoding.GetEncoding("utf-8"));
                }

                flag = string.IsNullOrEmpty(cookiesEncrypt) == false && (cookiesEncrypt == genEncrypt || cookiesEncrypt == tempEncrypt) ? true : false;
            }
            if (flag)
            {
                userid = BitAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.GetUserIDByEmployeeNumber(employeeNumber);
            }
            else
            {
                userid = -1;
            }
            return flag;
        }

    }
}