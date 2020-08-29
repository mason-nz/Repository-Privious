using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public class PageUtil
    {
        public static string GetUserName()
        {
            if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
            {
                return null;
            }
            string userName = HttpContext.Current.User.Identity.Name;
            if (userName.IndexOf('\\') > 0)
            {
                userName = userName.Substring(userName.IndexOf('\\') + 1);
            }
            return userName;
        }

        /// <summary>
        /// 查询顶级模块
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="classname">样式名</param>
        /// <param name="usrPath">页面url</param>
        /// <param name="userid">eid</param>
        /// <returns>模块字符串</returns>
        public static string GetBigModules(string template, string classname, string usrPath, int userid, ref bool result)
        {
            string returnstr = "";
            //DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(userid, ConfigurationManager.AppSettings["ThisSysID"].ToString(), "");
            DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(userid, ConfigurationManager.AppSettings["ThisSysID"].ToString());
            if (dtParent != null)
            {
                for (int i = 0; i < dtParent.Rows.Count; i++)
                {
                    string str_template = template;
                    string url = dtParent.Rows[i]["url"].ToString();
                    //if (string.IsNullOrEmpty(url.Trim()))
                    //{
                    url = GetSubTopOneUrl(dtParent.Rows[i]["moduleID"].ToString(), userid);
                    //}
                    str_template = str_template.Replace("{$lab_name$}", dtParent.Rows[i]["modulename"].ToString());
                    str_template = str_template.Replace("{$lab_url$}", url);
                    str_template = str_template.Replace("{$lab_moduleid$}", dtParent.Rows[i]["moduleID"].ToString());
                    if (dtParent.Rows[i]["moduleID"].ToString() == "SYS032MOD0001")
                    {
                        str_template = str_template.Replace("{$imgurl$}", "images/pic1.png");
                    }
                    else if (dtParent.Rows[i]["moduleID"].ToString() == "SYS032MOD1002")
                    {
                        str_template = str_template.Replace("{$imgurl$}", "images/pic2.png");
                    }
                    else
                    {
                        str_template = str_template.Replace("{$imgurl$}", "images/pic3.png");
                    }
                    if (url.ToLower().Contains(usrPath) || TestSubUrl(dtParent.Rows[i]["moduleID"].ToString(), userid, usrPath))
                    {
                        str_template = str_template.Replace("{$lab_class$}", "class='" + classname + "'");
                        result = true;
                    }
                    else
                    {
                        str_template = str_template.Replace("{$lab_class$}", "");
                    }

                    returnstr += str_template;
                }
            }
            return returnstr;

        }
        /// <summary>
        /// 判断url是否存在
        /// </summary>
        /// <param name="moduleID">父模块编号</param>
        /// <param name="userid">用户eid</param>
        /// <param name="url">页面url</param>
        /// <returns>存在返回true 不存在返回false</returns>
        private static bool TestSubUrl(string moduleID, int userid, string url)
        {
            bool returnstr = false;
            DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(userid, ConfigurationManager.AppSettings["ThisSysID"].ToString(), moduleID);
            if (dtParent != null)
            {
                foreach (DataRow newRow in dtParent.Rows)
                {

                    if (newRow["url"].ToString().ToLower() == url)
                    {
                        returnstr = true;
                        break;
                    }
                }
            }
            return returnstr;
        }
        /// <summary>
        /// 获取第一个子模块url
        /// </summary>
        /// <param name="moduleID">父模块编号</param>
        /// <param name="userid">用户eid</param>
        /// <returns>子模块url</returns>
        public static string GetSubTopOneUrl(string moduleID, int userid)
        {
            string returnstr = "";
            DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(userid, ConfigurationManager.AppSettings["ThisSysID"].ToString(), moduleID);
            if (dtParent != null)
            {
                if (dtParent.Rows.Count > 0)
                {
                    returnstr = dtParent.Rows[0]["url"].ToString();
                }
            }
            return returnstr;
        }
        /// <summary>
        /// 获取子模块
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="classname">样式名</param>
        /// <param name="usrPath">页面url</param>
        /// <param name="userid">用户eid</param>
        /// <param name="PID">父模块编号</param>
        /// <returns>子模块</returns>
        public static string GetSubModules(string template, string usrPath, int userid, string PID, ref bool result)
        {
            string returnstr = "";
            DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(userid, ConfigurationManager.AppSettings["ThisSysID"].ToString(), PID);
            if (dtParent != null)
            {
                for (int i = 0; i < dtParent.Rows.Count; i++)
                {
                    string str_template = template;
                    string url = dtParent.Rows[i]["url"].ToString();
                    if (string.IsNullOrEmpty(url.Trim()))
                    {
                        url = GetSubTopOneUrl(dtParent.Rows[i]["moduleID"].ToString(), userid);
                    }
                    str_template = str_template.Replace("{$lab_url$}", url);
                    if (url.ToLower() == usrPath)
                    {
                        str_template = str_template.Replace("{$lab_name$}", dtParent.Rows[i]["modulename"].ToString());
                        str_template = str_template.Replace("{$lab_class$}", "class='cur'");
                        result = true;
                    }
                    else
                    {
                        str_template = str_template.Replace("{$lab_name$}", dtParent.Rows[i]["modulename"].ToString());
                    }

                    returnstr += str_template;
                }
            }
            return returnstr;

        }
    }
}