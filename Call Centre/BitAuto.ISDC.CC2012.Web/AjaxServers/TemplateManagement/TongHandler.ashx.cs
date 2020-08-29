using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.WebService.EasyPass;
using BitAuto.ISDC.CC2012.Entities;
using System.Web.SessionState;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// TongHandler 的摘要说明
    /// </summary>
    public class TongHandler : IHttpHandler, IRequiresSessionState
    {

        public string ActionType
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("Action");
            }
        }

        public string mobile
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("mobile");
            }
        }


        #region 提交网销通参数

        public string Mobile
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("Mobile");
            }
        }

        public string UserName
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("UserName");
            }
        }

        public int ProvinceID
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("ProvinceID");
            }
        }

        public int CityID
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("CityID");
            }
        }

        public int UserID
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("UserID");
            }
        }

        public string MemberCode
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("MemberCode");
            }
        }

        public int CarTypeID
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("CarTypeID");
            }
        }

        public string TaskID
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("TaskID");
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string errMsg = "";
            string DatajsonStr = "";
            string ReturnJson = "";

            try
            {
                switch (ActionType)
                {
                    case "GetQiCheTongUser":
                        DatajsonStr = GetQiCheTongUser(out errMsg);
                        DatajsonStr = "\"" + DatajsonStr + "\"";
                        break;
                    case "SumbitWXT":
                        DatajsonStr = SumbitWXT(out errMsg);
                        break;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;

                string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
                BLL.EmailHelper.Instance.SendErrorMail(ex.StackTrace, "网销通出错", userEmail);
            }

            bool isSuccess = false;
            isSuccess = (errMsg == "" ? true : false);
            ReturnJson = "{\"Success\":" + isSuccess.ToString().ToLower() + ",\"Data\":" + DatajsonStr + ",\"Message\":\"" + errMsg + "\"}";

            context.Response.Write(ReturnJson);

        }

        private string SumbitWXT(out string errMsg)
        {
            errMsg = "";
            string json = "";

            json = FanXianHelper.Instance.SubmitFanxianOrder(UserID, UserName, Mobile, CarTypeID, int.Parse(MemberCode), CityID);


            #region 记日志

            UpdateOrderData updateLogModel = new UpdateOrderData();
            updateLogModel.APIType = 7;
            updateLogModel.CreateTime = DateTime.Now;
            updateLogModel.CreateUserID = BLL.Util.GetLoginUserID();
            updateLogModel.IsUpdate = 1;
            updateLogModel.TaskID = TaskID;
            updateLogModel.UpdateErrorMsg = "提交网销通订单接口：汽车通UserID：" + UserID.ToString() + ",用户姓名：" + UserName + ",用户手机号：" + Mobile + ",车款ID：" + CarTypeID.ToString() + ",经销商ID：" + MemberCode + ",用户所在城市ID：" + CityID.ToString() + "。返回信息：" + json;

            BLL.UpdateOrderData.Instance.Insert(updateLogModel);


            #endregion

            return json;
        }

        private string GetQiCheTongUser(out string errMsg)
        {
            int userid = 0;
            errMsg = "";
            //调用接口
            userid = WebService.QiCheTongHelper.Instance.GetUserIDByMobile(mobile);

            return userid.ToString();
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