using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.CTI
{
    /// <summary>
    /// CTIHelper 的摘要说明
    /// </summary>
    public class CTIHelper : IHttpHandler, IRequiresSessionState
    {
        #region Query Properties
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Action { get { return (Request["Action"] + "").Trim().ToLower(); } }

        private string userChoice;
        /// <summary>
        /// 电话呼入来源
        /// </summary>
        public string UserChoice
        {
            get
            {
                if (userChoice == null)
                {
                    userChoice = HttpUtility.UrlDecode((Request["UserChoice"] + "").Trim());
                }
                return userChoice;
            }
        }

        private string callID;
        /// <summary>
        /// CTI中通话ID
        /// </summary>
        public string CallID
        {
            get
            {
                if (callID == null)
                {
                    callID = HttpUtility.UrlDecode((Request["CallID"] + "").Trim());
                }
                return callID;
            }
        }

        private string recordID;
        /// <summary>
        /// 通话流水号
        /// </summary>
        public string RecordID
        {
            get
            {
                if (recordID == null)
                {
                    recordID = HttpUtility.UrlDecode((Request["RecordID"] + "").Trim());
                }
                return recordID;
            }
        }

        private string recordIDURL;
        /// <summary>
        /// 通话URL地址
        /// </summary>
        public string RecordIDURL
        {
            get
            {
                if (recordIDURL == null)
                {
                    recordIDURL = HttpUtility.UrlDecode((Request["RecordIDURL"] + "").Trim());
                }
                return recordIDURL;
            }
        }

        private string agentState;
        /// <summary>
        /// 坐席当前状态（枚举名称:就绪3、置忙4、事后处理5）
        /// </summary>
        public string AgentState
        {
            get
            {
                if (agentState == null)
                {
                    agentState = HttpUtility.UrlDecode((Request["AgentState"] + "").Trim());
                }
                return agentState;
            }
        }

        private string agentAuxState;
        /// <summary>
        /// 坐席置忙状态原因（int类型）
        /// </summary>
        public string AgentAuxState
        {
            get
            {
                if (agentAuxState == null)
                {
                    agentAuxState = HttpUtility.UrlDecode((Request["AgentAuxState"] + "").Trim());
                }
                return agentAuxState;
            }
        }

        private string calledNum;
        /// <summary>
        /// 当前被叫号码
        /// </summary>
        public string CalledNum
        {
            get
            {
                if (calledNum == null)
                {
                    calledNum = HttpUtility.UrlDecode((Request["CalledNum"] + "").Trim());
                }
                return calledNum;
            }
        }

        private string callerNum;
        /// <summary>
        /// 当前主叫号码
        /// </summary>
        public string CallerNum
        {
            get
            {
                if (CallerNum == null)
                {
                    callerNum = HttpUtility.UrlDecode((Request["CallerNum"] + "").Trim());
                }
                return callerNum;
            }
        }

        private string mediaType;
        /// <summary>
        /// 业务性质(枚举名称)
        /// </summary>
        public string MediaType
        {
            get
            {
                if (mediaType == null)
                {
                    mediaType = HttpUtility.UrlDecode((Request["MediaType"] + "").Trim());
                }
                return mediaType;
            }
        }

        private string userEvent;
        /// <summary>
        /// 当前CTI事件名称
        /// </summary>
        public string UserEvent
        {
            get
            {
                if (userEvent == null)
                {
                    userEvent = HttpUtility.UrlDecode((Request["UserEvent"] + "").Trim());
                }
                return userEvent;
            }
        }

        private string userName;
        /// <summary>
        /// 工号码
        /// </summary>
        public string UserName
        {
            get
            {
                if (userName == null)
                {
                    userName = HttpUtility.UrlDecode((Request["UserName"] + "").Trim());
                }
                return userName;
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool success = true;
                string result = "";
                string msg = "";

                switch (Action)
                {
                    case "established"://呼出接通
                        msg = UserEvent;
                        break;
                    case "released"://挂断
                        msg = UserEvent;
                        break;
                    case "ringing"://振铃
                        msg = UserEvent;
                        break;
                    default:
                        success = false;
                        msg = "请求参数错误";
                        break;
                }
                AJAXHelper.WrapJsonResponse(success, result, msg);
            }
            catch (Exception ex)
            {
                AJAXHelper.WrapJsonResponse(false, "", ex.Message);
            }
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