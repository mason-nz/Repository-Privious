using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.Utils.Config;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.StockBroker;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers
{
    /// <summary>
    /// LoginManager 的摘要说明
    /// </summary>
    public class LoginManager : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        //protected static string SYSID = ConfigurationUtil.GetAppSettingValue("ThisSysID");

        #region 属性

        private string RequestUsername
        {
            get { return BLL.Util.GetCurrentRequestFormStr("username"); }
        }

        private string RequestPWD
        {
            get { return BLL.Util.GetCurrentRequestFormStr("pwd"); }
        }

        private string RequestCheckCode
        {
            get { return BLL.Util.GetCurrentRequestFormStr("checkCode"); }
        }

        private string RequestGourl
        {
            get { return BLL.Util.GetCurrentRequestFormStr("gourl"); }
        }

        /// <summary>
        /// 用户分类（29001—广告主；29002—媒体主）
        /// </summary>
        private int RequestCategory
        {
            get { return BLL.Util.GetCurrentRequestFormInt("category"); }
        }

        /// <summary>
        /// 标识是否内部用户，1-内部用户，其他-非内部用户
        /// </summary>
        private int RequestIsInside
        {
            get { return BLL.Util.GetCurrentRequestFormInt("isInside"); }
        }

        #endregion 属性

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;

            Login(context);
            context.Response.End();
        }

        private void Login(HttpContext context)
        {
            if ((RequestUsername != null && RequestUsername.Length > 0) && (RequestPWD != null && RequestPWD.Length > 0)
                 && (RequestCheckCode != null && RequestCheckCode.Length > 0)
                 && (RequestCategory > 0 || RequestIsInside == 1))
            {
                int ret = 0;
                string url = string.Empty;
                bool isRedirectURL = false;

                //账号
                string username = RequestUsername;
                if (context.Session["ValidateCode"] == null || context.Session["ValidateCode"].ToString().ToLower() != RequestCheckCode.ToLower())
                {
                    ret = -6;
                }
                else
                {
                    if (RequestCategory == 29001 && RequestIsInside != 1)//广告主
                    {
                        int dealerID = BLL.StockBroker.StockBroker.Instance.isStockBrokerUser(username);
                        if (dealerID > 0)//dealerID，若大于0，说明是库存经纪人那边的账号，需要调用他们的登陆接口
                        {
                            string msg = "";
                            LoginDto result = BLL.StockBroker.StockBroker.Instance.Login(username, RequestPWD, out msg);
                            if (result != null)
                            {
                                ret = dealerID;
                                if (result.status == 0)//账号停用时
                                {
                                    isRedirectURL = true;
                                    url = "http://j.chitunion.com/userInfo/toRecoverPWD";
                                    ////物理清除StockBroker表数据
                                    //BLL.StockBroker.StockBroker.Instance.DeleteByUserID(dealerID);
                                }
                            }
                            else
                            {
                                context.Response.Write("-7," + BLL.Util.EscapeString(msg));
                                context.Response.End();
                                return;
                            }
                        }
                        else
                        {
                            ret = Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, RequestCategory);
                        }
                    }
                    else if (RequestCategory == 29002 && RequestIsInside != 1)//媒体主
                    {
                        ret = Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, RequestCategory);
                    }
                    else if (RequestIsInside == 1)//内部用户
                    {
                        ret = Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, 29003);
                    }

                    context.Session["ValidateCode"] = null;
                    if (ret > 0)//登陆成功
                    {
                        Chitunion2017.Common.UserInfo.Instance.Passport(ret);
                        //    currentContext.Session["EmployeeNumber"] = BLL.Util.GetEmployeeNumberByUserID(ret);
                        //BLL.Util.LoginPassport(ret, sysID);
                        string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")

                        if (isRedirectURL)
                        {
                            context.Response.Write("-8," + BLL.Util.EscapeString(url));
                            context.Response.End();
                            return;
                        }
                        else if (!string.IsNullOrEmpty(RequestGourl))
                        {
                            gourl = RequestGourl;
                        }
                        else
                        {
                            string loginUserRoleIDs = Chitunion2017.Common.UserInfo.GetUserRoleIDs(ret);
                            Chitunion2017.Common.UserRole ur = new Chitunion2017.Common.UserRole(ret, loginUserRoleIDs);
                            if (RequestCategory == 29001 && ur != null && ur.IsAD)//广告主跳转统一这个页面
                            {
                                gourl = "/OrderManager/wx_list.html";
                            }
                            else
                            {
                                DataTable dtParent = Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(ret);
                                if (dtParent != null && dtParent.Rows.Count > 0)
                                {
                                    if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                                    {
                                        DataTable dtChild = Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(ret, dtParent.Rows[0]["moduleid"].ToString());
                                        if (dtChild.Rows.Count > 0)
                                        {
                                            gourl = dtChild.Rows[0]["url"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        gourl = dtParent.Rows[0]["url"].ToString();
                                    }
                                }
                            }
                        }
                        string content = string.Format("用户{1}(ID:{0})登录成功。", ret, username);
                        ret = 1;//登陆成功
                        Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Select, content);
                        url = gourl;
                    }
                }
                context.Response.Write(ret.ToString() + "," + url);
                context.Response.End();

                return;
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