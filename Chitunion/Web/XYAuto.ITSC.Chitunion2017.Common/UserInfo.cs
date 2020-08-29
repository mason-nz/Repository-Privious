using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.Utils.Config;
using System.Web;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Common
{
    public class UserInfo
    {
        private string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");

        public static readonly UserInfo Instance = new UserInfo();

        protected static string strCookieKey = "16C99FAE-DACE-4793-9156-41FC0A33C09D";
        protected static string cookieName = "ct-uinfo";
        public static string WebDomain = ConfigurationUtil.GetAppSettingValue("WebDomain");
        public static string SYSID = "SYS001";
        public static string DefaultPwd = "111111";
        private static int LoginCookieOverdueDays = int.Parse(string.IsNullOrEmpty(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginCookieOverdueDays", false)) ? "1" : XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginCookieOverdueDays", false));

        #region Contructor

        protected UserInfo()
        {
        }

        #endregion Contructor

        /// <summary>
        /// 登陆逻辑
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="category">用户分类（29001—广告主；29002—媒体主）</param>
        /// <returns>返回用户ID</returns>
        public int Login(string userName, string pwd, int category)
        {
            pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(pwd + category + LoginPwdKey, System.Text.Encoding.UTF8);
            return Dal.UserInfo.Instance.Login(userName, pwd, category);
        }

        /// <summary>
        /// 登陆后写入Cookies
        /// </summary>
        /// <param name="userid">登陆UserID</param>
        /// <returns>返回Cookies内容</returns>
        public string Passport(int userid)
        {
            DataTable dt = Dal.UserInfo.Instance.Passport(userid, SYSID);

            if (dt != null && dt.Rows.Count > 0)
            {
                System.Web.HttpContext WEBHTTP = System.Web.HttpContext.Current;

                string userName = dt.Rows[0]["userName"].ToString();
                //string trueName = dt.Rows[0]["trueName"].ToString();
                string roleIDs = dt.Rows[0]["roleIDs"].ToString();
                string mobile = dt.Rows[0]["Mobile"].ToString();
                string type = dt.Rows[0]["Type"].ToString();
                //string userid = dt.Rows[0]["UserID"].ToString();
                string cookieValue = XYAuto.Utils.Security.DESEncryptor.Encrypt(userid.ToString(), strCookieKey);//userid
                cookieValue += "|" + WEBHTTP.Server.UrlEncode(XYAuto.Utils.Security.DESEncryptor.Encrypt(userName, strCookieKey));//username
                //cookieValue += "|" + WEBHTTP.Server.UrlEncode(XYAuto.Utils.Security.DESEncryptor.Encrypt(trueName, strCookieKey));//truename
                cookieValue += "|" + XYAuto.Utils.Security.DESEncryptor.Encrypt(roleIDs, strCookieKey);//roleids
                cookieValue += "|" + XYAuto.Utils.Security.DESEncryptor.Encrypt(mobile, strCookieKey);//mobile
                cookieValue += "|" + XYAuto.Utils.Security.DESEncryptor.Encrypt(type, strCookieKey);//type[用户类型：1-企业；2-个人]
                cookieValue += "|" + XYAuto.Utils.Security.DESEncryptor.Encrypt(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strCookieKey);//当前登陆时间
                cookieValue = XYAuto.Utils.Security.DESEncryptor.Encrypt(cookieValue, strCookieKey);
                WEBHTTP.Response.Cookies[cookieName].Value = cookieValue;
                WEBHTTP.Response.Cookies[cookieName].Domain = WebDomain;
                return cookieValue;
            }
            return null;
        }

        /// <summary>
        /// 得到大菜单根据用户id
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public DataTable GetParentModuleInfoByUserID(int userID)
        {
            return Dal.UserInfo.Instance.GetParentModuleInfoByUserID(userID.ToString(), SYSID);
        }

        /// <summary>
        /// 得到某个用户的所有资源
        /// </summary>
        /// <returns>用户所有资源</returns>
        public DataTable GetChildModuleByUserId(int userId, string pid)
        {
            return Dal.UserInfo.Instance.GetChildModuleByUserId(userId, SYSID, pid);
        }

        /// <summary>
        /// 获取菜单信息（筛选层级小于等于2级的）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public DataTable GetMenuModuleInfo(int userId)
        {
            return Dal.UserInfo.Instance.GetMenuModuleInfo(userId, SYSID);
        }

        /// <summary>
        /// 清除用户登录身份信息
        /// </summary>
        /// <returns></returns>
        public static void Clear()
        {
            System.Web.HttpContext WEBHTTP = System.Web.HttpContext.Current;

            try
            {
                WEBHTTP.Response.Cookies[cookieName].Value = null;
                WEBHTTP.Response.Cookies[cookieName].Domain = WebDomain;
                WEBHTTP.Session.Clear();
            }
            catch
            { }
            finally { }
        }

        /// <summary>
        /// 检查用户是否登陆
        /// </summary>
        public static void Check()
        {
            int userid = -1;
            int category = -1;
            if (IsLogin(out userid, out category) == false)
            {
                System.Web.HttpContext WEBHTTP = System.Web.HttpContext.Current;
                string GOUrl;
                if (WEBHTTP.Request.ServerVariables["SERVER_PORT"] == "80")
                {
                    GOUrl = "http://" + WEBHTTP.Request.ServerVariables["SERVER_NAME"] + WEBHTTP.Request.ServerVariables["SCRIPT_NAME"] + "?" + WEBHTTP.Request.ServerVariables["QUERY_STRING"];
                }
                else
                {
                    GOUrl = "http://" + WEBHTTP.Request.ServerVariables["SERVER_NAME"] + ":" + WEBHTTP.Request.ServerVariables["SERVER_PORT"] + WEBHTTP.Request.ServerVariables["SCRIPT_NAME"] + "?" + WEBHTTP.Request.ServerVariables["QUERY_STRING"];
                }
                string appPath = (System.Web.HttpContext.Current.Request.ApplicationPath == "/") ? "" : System.Web.HttpContext.Current.Request.ApplicationPath;
                XYAuto.Utils.ScriptHelper.ShowAlertAndRedirectScript("您还没有登录,请先登录!", ConfigurationUtil.GetAppSettingValue("ExitAddress") + "/Login.aspx?gourl=" + WEBHTTP.Server.UrlEncode(GOUrl));
            }
        }

        /// <summary>
        /// 验证Cookies
        /// </summary>
        /// <param name="cookieContent">Cookies内容</param>
        /// <returns>返回当前登陆人UserID</returns>
        public static int VerifyCookieContent(string cookieContent, out int category)
        {
            int userid = -1;
            category = -1;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            try
            {
                string cookieValue = XYAuto.Utils.Security.DESEncryptor.Decrypt(cookieContent, strCookieKey);
                string[] strCookieArr = cookieValue.Split('|');

                int tempUserid = int.Parse(XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[0], strCookieKey));
                string userName = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[1]), strCookieKey);
                //string truename = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[2]), strCookieKey);
                string roleIDs = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[2], strCookieKey);
                string mobile = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[3], strCookieKey);
                string type = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[4], strCookieKey);
                //DateTime loginDT = DateTime.Parse(XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[6], strCookieKey));

                //TimeSpan ts = new TimeSpan();
                //ts = DateTime.Now - loginDT;
                //if (ts.TotalDays <= LoginCookieOverdueDays)
                //{
                DataTable dt = Dal.UserInfo.Instance.Passport(tempUserid, SYSID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (userName == dt.Rows[0]["userName"].ToString() &&
                        //truename == dt.Rows[0]["truename"].ToString() &&
                        roleIDs == dt.Rows[0]["roleIDs"].ToString() &&
                        mobile == dt.Rows[0]["mobile"].ToString() &&
                        type == dt.Rows[0]["type"].ToString())
                    {
                        userid = tempUserid;
                        category = int.Parse(dt.Rows[0]["category"].ToString());
                        return userid;
                    }
                }
                //}
                //else
                //{
                //    Util.Log("log", "error", string.Format("登陆是Cookies过期，当时时间为：{0}，现在时间为：{1}", loginDT, DateTime.Now));
                //}
            }
            catch (System.Exception e)
            {
                Util.Log("log", "error", e.StackTrace.ToString() + e.Message.ToString());
                userid = -1;
            }
            return userid;
        }

        /// <summary>
        /// 判断当前访问用户是否登陆,如果已登录，完成用户信息加载
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin(out LoginUser lu)
        {
            bool result = false;
            //userid = -1;
            //category = -1;
            lu = null;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;

            if (webHttp.Request.Cookies[cookieName] != null &&
                webHttp.Request.Cookies[cookieName].Value != null &&
                webHttp.Request.Cookies[cookieName].Value.ToString() != "")
            {
                lu = VerifyCookieContent(webHttp.Request.Cookies[cookieName].Value);
                if (lu != null && lu.UserID > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        public static bool IsLogin(string cookieValue, out int userId, out LoginUser lu)
        {
            bool result = false;
            userId = -1;
            //category = -1;
            lu = null;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                lu = VerifyCookieContent(cookieValue);
                if (lu != null && lu.UserID > 0)
                {
                    userId = lu.UserID;
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断当前访问用户是否登陆,如果已登录，完成用户信息加载
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin(out int userid, out int category)
        {
            bool result = false;
            userid = -1;
            category = -1;

            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;

            if (webHttp.Request.Cookies[cookieName] != null &&
                webHttp.Request.Cookies[cookieName].Value != null &&
                webHttp.Request.Cookies[cookieName].Value.ToString() != "")
            {
                userid = VerifyCookieContent(webHttp.Request.Cookies[cookieName].Value, out category);
                if (userid > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        private static LoginUser VerifyCookieContent(string cookieContent)
        {
            LoginUser lu = null;
            int userid = -1;
            int category = -1;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            try
            {
                string cookieValue = XYAuto.Utils.Security.DESEncryptor.Decrypt(cookieContent, strCookieKey);
                string[] strCookieArr = cookieValue.Split('|');

                int tempUserid = int.Parse(XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[0], strCookieKey));
                string userName = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[1]), strCookieKey);
                //string truename = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[2]), strCookieKey);
                string roleIDs = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[2], strCookieKey);
                string mobile = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[3], strCookieKey);
                string type = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[4], strCookieKey);
                //DateTime loginDT = DateTime.Parse(XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[6], strCookieKey));

                //TimeSpan ts = new TimeSpan();
                //ts = DateTime.Now - loginDT;
                //if (ts.TotalDays <= LoginCookieOverdueDays)
                //{
                DataTable dt = Dal.UserInfo.Instance.Passport(tempUserid, SYSID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (userName == dt.Rows[0]["userName"].ToString() &&
                        //truename == dt.Rows[0]["truename"].ToString() &&
                        roleIDs == dt.Rows[0]["roleIDs"].ToString() &&
                        mobile == dt.Rows[0]["mobile"].ToString() &&
                        type == dt.Rows[0]["type"].ToString())
                    {
                        userid = tempUserid;
                        //category = int.Parse(dt.Rows[0]["category"].ToString());
                        //return userid;
                        lu = new LoginUser();
                        lu.UserID = userid;
                        lu.Category = int.Parse(dt.Rows[0]["category"].ToString());
                        lu.RoleIDs = roleIDs;
                        lu.Mobile = mobile;
                        if (string.IsNullOrEmpty(type))
                        {
                            lu.Type = -1;
                        }
                        else
                        {
                            lu.Type = int.Parse(type);
                        }
                        lu.UserName = userName;
                        lu.Source = int.Parse(dt.Rows[0]["Source"].ToString());
                        lu.BUTIDs = dt.Rows[0]["BUTIDs"].ToString();
                        return lu;
                    }
                }
            }
            catch (System.Exception e)
            {
                Util.Log("log", "error", e.StackTrace.ToString() + e.Message.ToString());
                userid = -1;
            }
            return lu;
        }

        public static Chitunion2017.Common.LoginUser GetLoginUser()
        {
            Chitunion2017.Common.LoginUser lu = new Chitunion2017.Common.LoginUser();
            if (IsLogin(out lu))
            {
                return lu;
            }
            return null;
        }

        /// <summary>
        /// 获取当前登陆系统用户ID
        /// </summary>
        /// <returns>返回UserID</returns>
        public static int GetLoginUserID()
        {
            int userid = -1;
            int category = -1;
            Chitunion2017.Common.LoginUser lu = new Chitunion2017.Common.LoginUser();
            if (IsLogin(out lu))
            {
                return lu.UserID;
            }
            return userid;
        }

        /// <summary>
        /// 获取当前登陆人角色ID（多个角色，用逗号分隔）
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserRoleIDs()
        {
            int userid = -1;
            int category = -1;
            string roleIDs = string.Empty;
            Chitunion2017.Common.LoginUser lu = new Chitunion2017.Common.LoginUser();
            if (IsLogin(out lu))
            {
                DataTable dt = Common.Dal.UserInfo.Instance.GetLoginUserInfo(userid, Common.UserInfo.SYSID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["roleIDs"].ToString();
                }
                return lu.RoleIDs;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据UserID，获取角色ID串
        /// </summary>
        /// <param name="userid">UserID</param>
        /// <returns>返回角色ID串</returns>
        public static string GetUserRoleIDs(int userid)
        {
            if (userid > 0)
            {
                DataTable dt = Common.Dal.UserInfo.Instance.GetLoginUserInfo(userid, Common.UserInfo.SYSID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["roleIDs"].ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取授权AE的UserID ‘,’拼接字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetAuthAEUserIDList(int userId)
        {
            return Dal.UserInfo.Instance.GetAuthAEUserIDList(userId);
        }

        /// <summary>
        /// 验证用户是否具有某个功能点或者模块的权限
        /// </summary>
        /// <param name="moduleID">模块或功能点ID</param>
        /// <param name="sysID">系统ID</param>
        /// <returns></returns>
        public static bool CheckRight(string moduleID, string sysID)
        {
            bool haveRight = false;
            int userID = -1;
            int category = -1;
            if (IsLogin(out userID, out category))
            {
                DataTable dt = Instance.GetModuleInfoByUserIdAndSysID(userID, sysID);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["moduleid"].ToString() == moduleID.Trim())
                        {
                            haveRight = true;
                            break;
                        }
                    }
                }
            }
            return haveRight;
        }

        public static bool Authorize(string moduleIds, int userId, string sysId)
        {
            if (string.IsNullOrWhiteSpace(moduleIds)) return false;
            //待校验的模块id集合
            var verifyModuleIds = moduleIds.Split(',').ToList();
            //用户拥有的模块集合
            var userModuleList = GetUserModuleList(userId, sysId);

            return userModuleList.Intersect(verifyModuleIds).Any();
        }

        public static List<string> GetUserModuleList(int userId, string sysId)
        {
            var listModule = new List<string>();
            DataTable dt = Instance.GetModuleInfoByUserIdAndSysID(userId, sysId);
            if (dt != null)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    listModule.Add(dt.Rows[i]["moduleid"].ToString());
                }
            }
            return listModule;
        }

        /// <summary>
        /// 得到某个用户，某个系统的所有资源
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="sysID">系统ID</param>
        /// <returns>用户所有资源</returns>
        private DataTable GetModuleInfoByUserIdAndSysID(int userId, string sysID)
        {
            return Dal.UserInfo.GetModuleByUserIdAndSysID(userId, sysID);
        }

        /// <summary>
        /// 根据Request.Url,来验证权限
        /// </summary>
        /// <param name="pid">返回模块的PID</param>
        /// <param name="isApplyScript">是否适用于script引用方式</param>
        /// <returns>返回模块权限ID</returns>
        public static string CheckUserRight(ref string pid, bool isApplyScript = false)
        {
            Check();
            bool haveRight = false;
            Uri uri = null;
            string moduleID = string.Empty;
            if (isApplyScript && HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
            {
                uri = new Uri(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]);
                moduleID = Dal.UserInfo.Instance.GetModuleIDByNoSysID(uri);
            }
            else
            {
                uri = HttpContext.Current.Request.Url;
                moduleID = Dal.UserInfo.Instance.GetModuleID(uri, SYSID);
            }
            int userid = GetLoginUserID();
            if (!string.IsNullOrEmpty(moduleID) && userid > 0)
            {
                DataTable dt = Dal.UserInfo.Instance.GetModuleByUserId(userid);
                if (dt != null)
                {
                    DataRow[] drList = dt.Select(string.Format("moduleid='{0}'", moduleID));
                    if (drList.Length > 0)
                    {
                        haveRight = true;
                    }
                }
            }
            else
            {
                haveRight = false;
            }
            if (!haveRight)
            {
                if (isApplyScript)
                {
                    HttpContext.Current.Response.Write("document.write(\"<sc\"+\"ript>alert('对不起，您还没权限访问此页面!');history.go(-1);</scr\"+\"ipt>\");");
                    //HttpContext.Current.Response.Write("var CTErrorRight = {\"Status\": 200,\"Message\": \"功能权限验证失败\",\"Result\": false,\"IsOverdue\": null}");
                    HttpContext.Current.Response.End();
                }
                else
                {
                    XYAuto.Utils.ScriptHelper.ShowAlertScript("对不起，您还没权限访问此页面!");
                }
            }
            if (!string.IsNullOrEmpty(moduleID))
            {
                pid = Dal.UserInfo.Instance.GetPIDByModuleID(moduleID);
            }
            return moduleID;
        }

        /// <summary>
        /// 获取用户角色
        /// ls
        /// </summary>
        /// <returns></returns>
        public static UserRole GetUserRole()
        {
            //return new UserRole(1288, "SYS001RL00001");
            try
            {
                GetLoginUserID();
            }
            catch (Exception)
            {
                int uid = -2;
                int.TryParse(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("APP_ImportPublishUserID").ToString(), out uid);
                return new UserRole(uid, "SYS001RL00005");
            }
            int userid = GetLoginUserID();
            string roles = GetLoginUserRoleIDs();
            return new UserRole(userid, roles);
        }

        public static UserRole GetUserRole(string cookieValue)
        {
            int userid = -1;
            string roles = string.Empty;
            string roleIDs = string.Empty;
            Chitunion2017.Common.LoginUser lu = new Chitunion2017.Common.LoginUser();
            if (IsLogin(cookieValue, out userid, out lu))
            {
                if (userid != -1)
                {
                    DataTable dt = Common.Dal.UserInfo.Instance.GetLoginUserInfo(userid, Common.UserInfo.SYSID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        roles = dt.Rows[0]["roleIDs"].ToString();
                    }
                }
            }
            return new UserRole(userid, roles);
        }
    }

    public class UserRole
    {
        public UserRole(int userID, string roles)
        {
            UserID = userID;
            Roles = roles;
            IsAdministrator = roles.Contains("SYS001RL" + ((int)UserRoleEnum.超级管理员).ToString().PadLeft(5, '0'));
            IsAE = roles.Contains("SYS001RL" + ((int)UserRoleEnum.AE).ToString().PadLeft(5, '0'));
            IsYY = roles.Contains("SYS001RL" + ((int)UserRoleEnum.运营).ToString().PadLeft(5, '0'));
            IsMedia = roles.Contains("SYS001RL" + ((int)UserRoleEnum.媒体主).ToString().PadLeft(5, '0'));
            IsAD = roles.Contains("SYS001RL" + ((int)UserRoleEnum.广告主).ToString().PadLeft(5, '0'));
            IsCH = roles.Contains("SYS001RL" + ((int)UserRoleEnum.策划).ToString().PadLeft(5, '0'));
            IsADAudit = roles.Contains("SYS001RL" + ((int)UserRoleEnum.广告审核).ToString().PadLeft(5, '0'));
            IsADSale = roles.Contains("SYS001RL" + ((int)UserRoleEnum.销售).ToString().PadLeft(5, '0'));
            IsADYY = roles.Contains("SYS001RL" + ((int)UserRoleEnum.广告运营).ToString().PadLeft(5, '0'));
            IsLabelOpt = roles.Contains("SYS001RL" + ((int)UserRoleEnum.打标签).ToString().PadLeft(5, '0'));
            IsLabelAudit = roles.Contains("SYS001RL" + ((int)UserRoleEnum.标签审核).ToString().PadLeft(5, '0'));
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 角色字符串
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsAdministrator { get; set; }

        /// <summary>
        /// 是否是AE
        /// </summary>
        public bool IsAE { get; set; }

        /// <summary>
        /// 是否是运营
        /// </summary>
        public bool IsYY { get; set; }

        /// <summary>
        /// 是否是媒体主
        /// </summary>
        public bool IsMedia { get; set; }

        /// <summary>
        /// 是否是广告主
        /// </summary>
        public bool IsAD { get; set; }

        /// <summary>
        /// 是否是策划
        /// </summary>
        public bool IsCH { get; set; }

        /// <summary>
        /// 是否是广告审核
        /// </summary>
        public bool IsADAudit { get; set; }
        /// <summary>
        /// 是否是广告销售
        /// </summary>
        public bool IsADSale { get; set; }
        public bool IsADYY { get; set; }
        /// <summary>
        /// 是否是打标签
        /// </summary>
        public bool IsLabelOpt { get; set; }
        /// <summary>
        /// 是否是标签审核
        /// </summary>
        public bool IsLabelAudit { get; set; }
    }

    public enum UserRoleEnum
    {
        超级管理员 = 1,
        广告主,
        媒体主,
        运营,
        AE,
        策划,
        广告审核,
        销售,
        打标签,
        标签审核,
        广告运营 = 11
    }

    public class LoginUser
    {
        /// <summary>
        /// 登陆UserID
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        /// 用户分类（29001—广告主；29002—媒体主）
        /// </summary>
        public int Category { set; get; }

        /// <summary>
        /// 当前登陆这权限信息，多个用逗号分隔
        /// </summary>
        public string RoleIDs { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 登陆者账号信息，目前与手机号相同
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 账号类型：1001——企业，1002——个人
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        /// 注册来源：3001自营，3002自助
        /// </summary>
        public int Source { set; get; }

        /// <summary>
        /// 登陆者功能点列表，多个用逗号分隔
        /// </summary>
        public string BUTIDs { set; get; }
    }
}