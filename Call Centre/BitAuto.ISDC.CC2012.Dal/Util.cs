using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BitAuto.Utils.Data;
using BitAuto.Utils.Config;
using System.Text.RegularExpressions;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class Util : DataBase
    {
        #region Instance
        public static readonly Util Instance = new Util();
        #endregion

        #region Contructor
        protected Util()
        { }
        #endregion

        public static DateTime GetDBDateTime()
        {
            int count = 0;
            string sqlStr = "SELECT getdate()  ";

            SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            while (reader.Read())
            {
                return reader.GetDateTime(0);
            }
            return DateTime.Now;
        }

        public static string GetAppSettingValue(string key, string defaultValue = "")
        {
            try
            {
                return ConfigurationUtil.GetAppSettingValue(key);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 根据集中权限UserID，去集中权限表中，后去员工编号信息
        /// </summary>
        /// <param name="userid">集中权限UserID</param>
        /// <returns>返回员工编号信息</returns>
        public string GetEmployeeNumberByUserID(int userid)
        {
            string sql = string.Format(@"SELECT UserCode FROM dbo.UserInfo WHERE UserID={0}", userid);
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(ConnectionStrings_SYS, CommandType.Text, sql));
        }

        /// <summary>
        /// 获取登录者真实姓名
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserIDNotCheck()
        {
            string realName = string.Empty;
            try
            {
                System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
                if (webHttp != null && webHttp.Session != null && webHttp.Session["userid"] != null && webHttp.Session["userid"].ToString() != "")
                {
                    realName = webHttp.Session["userid"].ToString().Trim();
                }
            }
            catch (Exception)
            {
                realName = string.Empty;
            }
            return realName;
        }

        /// <summary>
        /// 获取登录者真实姓名
        /// </summary>
        /// <returns></returns>
        public static string GetLoginRealName()
        {
            string realName = string.Empty;
            try
            {
                System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
                if (webHttp != null && webHttp.Session != null && webHttp.Session["truename"] != null && webHttp.Session["truename"].ToString() != "")
                {
                    realName = webHttp.Session["truename"].ToString().Trim();
                }
            }
            catch (Exception)
            {
                realName = string.Empty;
            }
            return realName;
        }

        /// <summary>
        /// sql过滤（仅用于In的情况）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SqlFilterByInCondition(string source)
        {
            if (source.Trim().StartsWith("'") && source.Trim().EndsWith("'"))
            {
                string temp = source.Trim().Trim('\'');
                string[] array = Regex.Split(temp, @"\'[\s]*,[\s]*\'", RegexOptions.IgnoreCase);

                StringBuilder sb = new StringBuilder();
                foreach (string item in array)
                {
                    sb.Append("'" + StringHelper.SqlFilter(item) + "',");
                }
                return sb.ToString().TrimEnd(',');
            }
            else
            {
                return StringHelper.SqlFilter(source);
            }
        }
    }
}
