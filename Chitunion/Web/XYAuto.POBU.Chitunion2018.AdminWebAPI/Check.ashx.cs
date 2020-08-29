using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI
{
    /// <summary>
    /// Check 的摘要说明
    /// </summary>
    public class Check : IHttpHandler
    {
        #region 属性
        private bool RequestNotCheckModule
        {
            get { return GetCurrentRequestQueryStr("NotCheckModule"); }
        }
        private bool RequestNotRedirectURL
        {
            get { return GetCurrentRequestQueryStr("NotRedirectURL"); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //int userid = -1;//员工EID
            //string truename = "";//真实姓名
            //string username = "";//域账号
            //string mobile = "";//注册时手机号码
            //int typeID = -1;//企业-1；个人-2
            //string roleIDs = string.Empty;//登陆人角色ID字符串
            //int category = -1;//用户分类（29001—广告主；29002—媒体主）

            string pid = "";//当前登陆用户
            string moduleID = "";
            //string butIDs = "";//当前登陆用户功能点ID串，用逗号分隔

            XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();

            //bool flag = Chitunion2017.Common.UserInfo.IsLogin(out userid, out category);
            bool flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
            if (flag)
            {
                if (!RequestNotCheckModule)//若请求时，传递参数NotCheckModule=true，则跳过权限验证逻辑
                {
                    pid = "";
                    moduleID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.CheckUserRight(ref pid, true);  // .Common.EmployeeInfo.checkUserRight(ref pid, true);
                }
                //DataTable dt = Chitunion2017.Common.Dal.UserInfo.Instance.GetLoginUserInfo(lu.UserID, Chitunion2017.Common.UserInfo.SYSID);//XYAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetEmployeeInfoByUserID(userid);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    //truename = dt.Rows[0]["truename"].ToString();
                //    username = dt.Rows[0]["username"].ToString();
                //    mobile = dt.Rows[0]["mobile"].ToString();
                //    if (!int.TryParse(dt.Rows[0]["Type"].ToString(), out typeID))
                //    {
                //        typeID = -1;
                //    }
                //    roleIDs = dt.Rows[0]["roleIDs"].ToString();
                //    category = int.Parse(dt.Rows[0]["Category"].ToString());
                //    butIDs = dt.Rows[0]["BUTIDs"].ToString();
                //}
            }

            if (flag)
            {
                int hasMsgCount = ITSC.Chitunion2017.BLL.WeChatOperateMsg.Instance.GetCount_MsgNoRead();
                context.Response.Write("document.write(\"<sc\"+\"ript>var CTLogin={ UserID:" + lu.UserID + ",IsLogin:" + flag.ToString().ToLower() + ",UserName:'" + lu.UserName + "',Mobile:'" + lu.Mobile + "',TypeID:" + lu.Type + ",RoleIDs:'" + lu.RoleIDs + "',Category:" + lu.Category + ",BUTIDs:'" + lu.BUTIDs + "',HasMsgCount:" + hasMsgCount + "};</scr\"+\"ipt>\");");
            }
            else
            {
                if (RequestNotRedirectURL)//未登录后，是否要跳转页面
                {
                    context.Response.Write("document.write(\"<sc\"+\"ript>var CTLogin={ UserID:-1,IsLogin:false,RealName:'',UserName:'',Mobile:'',TypeID:-1,RoleIDs:'',Category:-1,BUTIDs:''};</scr\"+\"ipt>\");");
                }
                else
                {
                    context.Response.Write("document.write(\"<sc\"+\"ript> alert('您尚未登录，请登录');window.location='" + XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress") + "/Login.aspx?gourl=' + encodeURIComponent(window.location + '');" + "</scr\"+\"ipt>\");");
                    //context.Response.Write("document.write(\"<sc\"+\"ript> if(typeof(layer)=='undefined'){alert('您尚未登录，请登录');window.location = '" + XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress") + "/index.html?gourl=' + encodeURIComponent(window.location + '');}else{layer.msg('您尚未登录，请登录',{'time': 2000},function(){window.location = '" + XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress") + "/index.html?gourl=' + encodeURIComponent(window.location + '');});}" + "</scr\"+\"ipt>\");");
                }
            }
        }

        private bool GetCurrentRequestQueryStr(string r)
        {
            bool flag = false;
            if (System.Web.HttpContext.Current.Request.QueryString[r] == null)
            {
                return flag;
            }
            else
            {
                string query = System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.QueryString[r].ToString().Trim().Replace("+", "%2B"));
                bool.TryParse(query, out flag);
                return flag;
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