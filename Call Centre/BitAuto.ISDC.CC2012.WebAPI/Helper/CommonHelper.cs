using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using System.Configuration;
using System.IO;
using System.Net.Http;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.WebAPI.Helper
{
    public static class CommonHelper
    {
        public static string STRSQLCONN = ConfigurationManager.AppSettings["ConnectionStrings_Holly_Business"];
        public static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

        /// 生成返回的xml字符串
        /// <summary>
        /// 生成返回的xml字符串
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateXMLResult(string result)
        {
            string content = "";
            FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "XmlTemplates\\HollyResultTemplate.xml");
            if (info.Exists)
            {
                using (StreamReader sr = info.OpenText())
                {
                    content = sr.ReadToEnd();
                }
                content = content.Replace("@Result@", result);
            }
            return new HttpResponseMessage { Content = new StringContent(content, System.Text.Encoding.UTF8, "application/xml") };
        }

        /// 生成返回的xml字符串
        /// <summary>
        /// 生成返回的xml字符串
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCallInXMLResult(CCallInInfo result)
        {
            string content = "";
            FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "XmlTemplates\\AddCallSampleInTemplate.xml");
            if (info.Exists)
            {
                using (StreamReader sr = info.OpenText())
                {
                    content = sr.ReadToEnd();
                }
                if (result != null)
                {
                    content = content.Replace("@Result@", result.Result).Replace("@Id@", result.Id).Replace("@mphone@", result.mphone).Replace("@voiceCode@", result.voiceCode).Replace("@skillId@", result.skillId).Replace("@ivrNo@", result.ivrNo);
                    CommonHelper.Log("调用成功。" + content);
                }
                else
                {
                    content = content.Replace("@Result@", "0").Replace("@Id@", "").Replace("@mphone@", "").Replace("@voiceCode@", "").Replace("@skillId@", "").Replace("@ivrNo@", "");
                }

            }

            return new HttpResponseMessage { Content = new StringContent(content, System.Text.Encoding.UTF8, "application/xml") };
        }

        /// 转换枚举
        /// <summary>
        /// 转换枚举
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public static Business ConvertBusiness(int business)
        {
            if (business >= 0 && business <= 3)
            {
                return (Business)Enum.Parse(typeof(Business), business.ToString());
            }
            else return Business.未知;
        }
        /// 校验IP是否正确
        /// <summary>
        /// 校验IP是否正确
        /// </summary>
        /// <returns></returns>
        public static bool CheckIP()
        {
            var IPSIgnore = ConfigurationManager.AppSettings["IPSIgnore"].Split(',');
            string userIP = HttpContext.Current.Request.UserHostAddress;
            userIP = userIP.LastIndexOf(".") > 0 ? userIP.Substring(0, userIP.LastIndexOf("."))+"." : userIP;
            try
            {
                if (!string.IsNullOrEmpty(IPSIgnore.FirstOrDefault(s => s.StartsWith(userIP))))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        /// 公共验证合力数据
        /// <summary>
        /// 公共验证合力数据
        /// </summary>
        /// 
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool CheckIP(string title)
        {
            bool flag = true;
            if (!CommonHelper.CheckIP())
            {
                string userIP = HttpContext.Current.Request.UserHostAddress;
                CommonHelper.Log(title + " IP验证不通过 " + userIP);
                flag = false;
            }
            return flag;
        }
        /// 日志
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="mes"></param>
        public static void Log(string mes)
        {
            BLL.Util.LogForService("log\\hollylog", "info", mes);
            ClearLog();
        }
        /// 日志
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="ex"></param>
        public static void Log(string mes, Exception ex)
        {
            BLL.Util.LogForService("log\\hollylog", "info", mes);
            BLL.Util.LogForService("log\\hollylog", "info", ex.Message);
            BLL.Util.LogForService("log\\hollylog", "info", ex.StackTrace);
        }
        /// 保留1个月的日志
        /// <summary>
        /// 保留1个月的日志
        /// </summary>
        public static void ClearLog()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log\\hollylog";
            if (Directory.Exists(path))
            {
                foreach (FileInfo finfo in new DirectoryInfo(path).GetFiles())
                {
                    if (finfo.Extension == ".log")
                    {
                        string name = Path.GetFileNameWithoutExtension(finfo.FullName);
                        DateTime fdate = CommonFunction.ObjectToDateTime(name);
                        if (fdate < DateTime.Today.AddMonths(-1))
                        {
                            finfo.Delete();
                        }
                    }
                }
            }
        }
        /// 被叫号码处理（座机去掉区号）
        /// <summary>
        /// 被叫号码处理（座机去掉区号）
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string BeijiaoProcess(string phone)
        {
            //去掉区号
            if (phone.Length > 8)
            {
                phone = phone.Substring(phone.Length - 8, 8);
            }
            return phone;
        }
        /// 查询西安库外呼任务明细表
        /// <summary>
        /// 查询西安库外呼任务明细表
        /// </summary>
        /// <param name="Pro"></param>
        /// <param name="strPhone"></param>
        /// <param name="strACStatus"></param>
        /// <param name="pageSize"></param>
        /// <param name="indexPage"></param>
        /// <param name="totalRcorder"></param>
        /// <returns></returns>
        public static DataTable GetAutoCallItemMoniterList(string Pro, string strPhone, string strACStatus, int pageSize, int indexPage, out int totalRcorder)
        {
            totalRcorder = 0;

            if (string.IsNullOrEmpty(STRSQLCONN))
            {
                Log("CC西安库连接字符串不能为空");
                return null;
            }

            string strWhere = " where a.status=1 ";

            if (!string.IsNullOrEmpty(Pro))
            {
                strWhere += " and a.ProjectID= '" + Pro + "' ";
            }

            if (!string.IsNullOrEmpty(strPhone))
            {
                strWhere += " and a.phone like '%" + strPhone + "%' ";
            }
            if (!string.IsNullOrEmpty(strACStatus))
            {
                strWhere += " and a.ACStatus in (" + strACStatus + ") ";
            }

            //SqlConnection conn = new SqlConnection(strSqlconn);
            SqlParameter[] paras = {
             new SqlParameter("@where", SqlDbType.NVarChar, 40000),
             new SqlParameter("@order", SqlDbType.NVarChar, 200),
             new SqlParameter("@pagesize", SqlDbType.Int, 4),
             new SqlParameter("@indexpage", SqlDbType.Int, 4),
             new SqlParameter("@totalRecorder", SqlDbType.Int, 4)};
            string orderStr="  CASE  a.ACStatus WHEN 2 THEN 0 WHEN 1 THEN 1 WHEN 0 THEN 2 ELSE a.ACStatus END,b.CreateTime DESC ";
            paras[0].Value = strWhere;
            paras[1].Value = orderStr;
            paras[2].Value = pageSize;
            paras[3].Value = indexPage;
            paras[4].Direction = ParameterDirection.Output;
            DataSet ds = new DataSet();
            BLL.Loger.Log4Net.Info(string.Format("[p_AutoCallMonitorDetail],strWhere={0},order={1},pagesize={2},indexpage={3}",
                strWhere, orderStr, pageSize, indexPage));
            ds = SqlHelper.ExecuteDataset(STRSQLCONN, CommandType.StoredProcedure, "p_AutoCallMonitorDetail", paras);
            
            totalRcorder = Convert.ToInt32(paras[4].Value);
            BLL.Loger.Log4Net.Info(string.Format("[p_AutoCallMonitorDetail],totalRecorder={0}", totalRcorder));
            if (ds.Tables.Count < 1)
            {
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }
    }
}