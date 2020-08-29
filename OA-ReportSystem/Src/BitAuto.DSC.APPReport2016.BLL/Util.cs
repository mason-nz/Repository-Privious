using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Util
    {
        /// <summary>
        ///配置邮件接收者，用分号分隔
        /// </summary>
        public static string[] Reciver
        {
            get
            {
                return ConfigurationManager.AppSettings["Reciver"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// 获取登录者真实姓名
        /// </summary>
        /// <returns></returns>
        public static string GetLoginRealName()
        {
            string realName = string.Empty;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            if (webHttp != null && webHttp.Session["truename"] != null && webHttp.Session["truename"].ToString() != "")
            {
                realName = webHttp.Session["truename"].ToString().Trim();
            }
            else
            {
                //throw new Exception("无法读取相关session");
                //if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                //{
                //    string username = HttpContext.Current.User.Identity.Name.ToLower();
                //    if (username.StartsWith("tech\\"))
                //    {
                //        username = username.Substring(5, username.Length - 5);
                //    }
                //    int ret = Common.EmployeeInfo.Login(username);
                //    if (ret > 0)
                //    {
                //        Common.EmployeeInfo.passport(ret);
                //        return GetLoginRealName();
                //    }
                //}
            }
            return realName;
        }

        public static string GetCurrentRequestFormStr(string r)
        {
            if (System.Web.HttpContext.Current.Request.Form[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.Form[r].ToString().Trim());
            }
        }

        public static int GetCurrentRequestFormInt(string r)
        {
            int val = -1;
            try
            {
                if (System.Web.HttpContext.Current.Request.Form[r] != null)
                {
                    val = int.Parse(System.Web.HttpContext.Current.Request.Form[r].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
        }

        public static string GetCurrentRequestQueryStr(string r)
        {
            if (System.Web.HttpContext.Current.Request.QueryString[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.QueryString[r].ToString().Trim());
            }
        }

        public static int GetCurrentRequestQueryInt(string r)
        {
            int val = -1;
            try
            {
                if (System.Web.HttpContext.Current.Request.QueryString[r] != null)
                {
                    val = int.Parse(System.Web.HttpContext.Current.Request.QueryString[r].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
        }
        public static string GetCurrentRequestStr(string r)
        {
            if (System.Web.HttpContext.Current.Request[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request[r].ToString().Trim());
            }
        }

        public static int GetCurrentRequestInt(string r)
        {
            int val = -1;
            try
            {
                if (System.Web.HttpContext.Current.Request[r] != null)
                {
                    val = int.Parse(System.Web.HttpContext.Current.Request[r].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
        }
        /// <summary>
        /// ASPNET实现javascript的escape 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回加密后字符串</returns>
        public static string EscapeString(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] ba = System.Text.Encoding.Unicode.GetBytes(str);
            for (int i = 0; i < ba.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(ba[i + 1].ToString("X2"));
                sb.Append(ba[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GetWeekNameByDate(DateTime date)
        {
            string week = string.Empty;
            switch (date.DayOfWeek.ToString())
            {
                case "Monday":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期六";
                    break;
                case "Sunday":
                    week = "星期日";
                    break;
            }
            return week;
        }

        /// 重置序号
        /// <summary>
        /// 重置序号
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="orderby"></param>
        /// <param name="totalcount"></param>
        public static int SetNoForDataTable(ref DataTable dt, string orderby, int totalcount, int lastno)
        {
            //算法：数据相同，序号不同 例如：1 2 3 4
            //（后期修改：数据相同，序号相同，下一位序号不存在 例如：1 2 2 4）
            int newlastno = 0;
            int start = lastno;
            //添加新的序号字段
            dt.Columns.Add("No", typeof(int));
            if (orderby.ToLower().Contains(" asc"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //序号递减
                    if (lastno > 0)
                    {
                        newlastno = --start;
                    }
                    else
                    {
                        //第一页数据，通过总数和原始数据计算
                        newlastno = totalcount - CommonFunction.ObjectToInteger(dr["RowNumber"]) + 1;
                    }
                    dr["NO"] = newlastno;
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //序号递增
                    if (lastno > 0)
                    {
                        newlastno = ++start;
                    }
                    else
                    {
                        //第一页数据，通过原始数据计算
                        newlastno = CommonFunction.ObjectToInteger(dr["RowNumber"]);
                    }
                    dr["NO"] = newlastno;
                }
            }
            return newlastno;
        }
    }
}
