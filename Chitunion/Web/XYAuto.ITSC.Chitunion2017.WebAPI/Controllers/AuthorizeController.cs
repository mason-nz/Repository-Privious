using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using System.Net.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using System.Data;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    public class Menu
    {
        public string ModuleID { get; set; }
        public string Url { get; set; }
        public string Links { get; set; }
        public string ModuleName { get; set; }
        public List<Menu> SubModule { get; set; }
    }

    [CrossSite]
    public class AuthorizeController : ApiController
    {
        private static string KeyFromOtherSys = Utils.Config.ConfigurationUtil.GetAppSettingValue("KeyFromOtherSys");
        private static string EmbedChiTu_DesStr = Utils.Config.ConfigurationUtil.GetAppSettingValue("EmbedChiTu_DesStr");
        /// <summary>
        /// 获取当前登陆后，左侧菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMenuInfo()
        {
            //int.Parse("asdfas");
            List<Menu> lm = new List<Menu>();
            DataTable dt = Chitunion2017.Common.UserInfo.Instance.GetMenuModuleInfo(Chitunion2017.Common.UserInfo.GetLoginUserID());
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["Level"].ToString()) == 1)
                    {
                        Menu m = new Menu();
                        m.ModuleID = dr["ModuleID"].ToString();
                        m.Url = dr["Url"].ToString();
                        m.ModuleName = dr["ModuleName"].ToString();
                        m.Links = dr["Links"].ToString();
                        m.SubModule = GetSubMenu(dt, m.ModuleID);
                        lm.Add(m);
                    }
                }
            }
            return Util.GetJsonDataByResult(lm, "Success");
        }

        private List<Menu> GetSubMenu(DataTable dt, string p)
        {
            List<Menu> l = new List<Menu>();
            if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(p))
            {
                dt.DefaultView.RowFilter = "PID='" + p + "'";
                DataTable dtNew = dt.DefaultView.ToTable();
                if (dtNew != null && dtNew.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNew.Rows)
                    {
                        Menu m = new Menu();
                        m.ModuleID = dr["ModuleID"].ToString();
                        m.Url = dr["Url"].ToString();
                        m.Links = dr["Links"].ToString();
                        m.ModuleName = dr["ModuleName"].ToString();
                        m.SubModule = new List<Menu>();
                        l.Add(m);
                    }
                }
            }
            return l;
        }


        /// <summary>
        /// 获取当前登陆后的用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = true, IsCheckLogin = true)]
        public JsonResult GetLoginInfo()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int userid = Chitunion2017.Common.UserInfo.GetLoginUserID();
            DataTable dt = Chitunion2017.Common.Dal.UserInfo.Instance.GetLoginUserInfo(userid, Chitunion2017.Common.UserInfo.SYSID);//XYAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetEmployeeInfoByUserID(userid);
            if (dt != null && dt.Rows.Count > 0)
            {
                //dic["RealName"] = dt.Rows[0]["truename"].ToString();
                dic["UserName"] = dt.Rows[0]["username"].ToString();
                dic["Mobile"] = dt.Rows[0]["mobile"].ToString();
                int typeID = -1;
                if (!int.TryParse(dt.Rows[0]["Type"].ToString(), out typeID))
                {
                    typeID = -1;
                }
                dic["TypeID"] = typeID;
                dic["RoleIDs"] = dt.Rows[0]["roleIDs"].ToString();
                dic["Category"] = int.Parse(dt.Rows[0]["Category"].ToString());
                dic["UserID"] = userid;
                dic["IsLogin"] = true;
            }
            return Util.GetJsonDataByResult(dt, "Success");
        }

        /// <summary>
        /// 根据功能点ID串，验证当前用户是否有权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckRight(dynamic obj)
        {
            try
            {
                Dictionary<string, bool> dic = new Dictionary<string, bool>();
                string moduleIDs = Convert.ToString(obj.ModuleIDs);
                string[] list = moduleIDs.Split(',');
                if (list != null && list.Count() > 0)
                {
                    foreach (string moduleID in list)
                    {
                        bool f = Chitunion2017.Common.UserInfo.CheckRight(moduleID, Chitunion2017.Common.UserInfo.SYSID);
                        dic.Add(moduleID, f);
                    }
                }
                return Util.GetJsonDataByResult(dic);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("调用接口CheckRight报错", ex);
                return Util.GetJsonDataByResult(null, ex.Message, 500);
            }

        }


        //[HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "XXX")]
        //public JsonResult Login(string account, string pwd)
        //{
        //    Dictionary<string, object> dic = new Dictionary<string, object>();
        //    dic["LoginResult"] = "Success";

        //    dic["UserID"] = 001;
        //    dic["EmployeeNumber"] = "0001";
        //    dic["LoginCookies"] = "CookiesValueXXXXXXX";

        //    return Util.GetJsonDataByResult(dic, "Success", 123);
        //}


        //[HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001MOD0001")]
        //public JsonResult Exit(int userid)
        //{
        //    //bool flag = false;
        //    //if (string.IsNullOrEmpty(employeeNumber) == false && EmployeeInfo.DeleteUserLoginKey(employeeNumber))
        //    //{
        //    //    EmployeeInfo.clear();
        //    //    flag = true;
        //    //}
        //    return Util.GetJsonDataByResult(true, "清空登录信息成功");//: "参数异常，清空失败"
        //}


        ///// <summary>
        ///// 其他业务系统，登陆赤兔系统，单点登陆逻辑
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult SyncLoginFromOtherSys(string accessToken,int userID,string mobile)
        {
            try
            {
                if (!Chitunion2017.Common.AuthorizeLogin.Instance.Verification(accessToken))
                    return Util.GetJsonDataByResult(null, "AccessToken验证信息过期或不存在", 2);
                if (string.IsNullOrEmpty(mobile))
                    return Util.GetJsonDataByResult(null, "手机号为必填项!", 2);

                string msg = string.Empty;
                int retval = 0;
                retval = Chitunion2017.Common.AuthorizeLogin.Instance.p_UserBroker_Update(userID, mobile, out msg);

                switch (retval)
                {
                    case 0:
                        return Util.GetJsonDataByResult(null, "调用成功");
                    case 1:
                        return Util.GetJsonDataByResult(null, "手机号在赤兔系统中已经存在", 1);
                    default:
                        return Util.GetJsonDataByResult(null, "操作失败", -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("调用失败：" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "调用失败：" + ex.Message, -1);
            }                    
        }

        /// <summary>
        /// 获取临时的访问Token（其他业务系统，登陆赤兔系统时）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetTempTokenFromOtherSys(int appid,string encyptStr)
        {
            /*
             MD5生成规则：APPID+请求客户端IP+时间戳+EncyptStr    
             返回值：令牌内容，过期时间         
             */

            try
            {
                string[] keyArray = KeyFromOtherSys.Split(';');
                Dictionary<int, string> dict = new Dictionary<int, string>();
                foreach (var key in keyArray)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        string[] subKeyArray = key.Split(',');
                        dict.Add(Convert.ToInt32(subKeyArray[0]), subKeyArray[1]);
                    }
                }

                if (dict[appid].ToString() != encyptStr)
                    return Common.Util.GetJsonDataByResult(null, "调用失败：业务系统ID错误", -1);

                string requestIP = string.Empty;
                try
                {
                    requestIP = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
                }
                catch (Exception)
                {
                    requestIP = "192.168.100.1";
                }
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                long currentTiemStamp = Convert.ToInt64(ts.TotalMilliseconds);

                string md5Code = XYAuto.Utils.Security.DESEncryptor.MD5Hash(appid + requestIP + currentTiemStamp + encyptStr, System.Text.Encoding.UTF8);

                Dictionary<string, object> dictRetval = new Dictionary<string, object>();
                dictRetval.Add("accessToken", md5Code);
                dictRetval.Add("expires_in", 300);
                XYAuto.ITSC.Chitunion2017.Common.Entities.AuthorizeLogin model = new Chitunion2017.Common.Entities.AuthorizeLogin() {
                    APPID=appid,
                    IP=requestIP,
                    TimeStamp=currentTiemStamp,
                    MD5Code=md5Code,
                    CreateTime=DateTime.Now,
                    ModifyTime=DateTime.Now
                };
                XYAuto.ITSC.Chitunion2017.Common.AuthorizeLogin.Instance.Insert(model);
                return Common.Util.GetJsonDataByResult(dictRetval, "调用成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("调用失败：" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "调用失败：" + ex.Message, -1);
            } 
        }
    }
}
