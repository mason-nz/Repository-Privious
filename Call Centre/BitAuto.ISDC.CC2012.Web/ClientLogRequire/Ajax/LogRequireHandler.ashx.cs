using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.IO;

namespace BitAuto.ISDC.CC2012.Web.ClientLogRequire.Ajax
{
    /// <summary>
    /// LogRequireHandler 的摘要说明
    /// </summary>
    public class LogRequireHandler : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action").ToString();
            }
        }
        public string LogDate
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LogDate").ToString();
            }
        }
        public string AgentID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AgentID").ToString();
            }
        }
        public string VendorID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("VendorID").ToString();
            }
        }
        public string Datas
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Datas").ToString();
            }
        }

        private string FilePath
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("filepath").ToString();
            }
        }
        private string AgentName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("agentname").ToString();
            }
        }
        private string VendorName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("vendorname").ToString();
            }
        }

        private string NoLogin
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("NoLogin").ToString();
            }
        }

        public int userid = -1;

        public static string NoLoginKey = "yiche.xian.!@$%^.982174653";

        public void ProcessRequest(HttpContext context)
        {
            //此页面可以不登录只用秘钥访问
            if (NoLogin != NoLoginKey)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                userid = BLL.Util.GetLoginUserID();
            }

            context.Response.ContentType = "text/plain";
            string msg = "";

            switch (Action.ToLower())
            {
                case "requirelog":
                    msg = RequireLog();
                    break;
                case "requirelogmutil":
                    msg = RequireLogMutil();
                    break;
                case "downloadfile":
                    msg = DownLoadFile();
                    break;
            }

            context.Response.Write(msg);
        }
        /// 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        private string DownLoadFile()
        {
            string msg = "";
            try
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    string root = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LogWebAbsolutePath", false).TrimEnd('\\');
                    string filepath = root + FilePath.Replace("/log/", "/").Replace("/", "\\");///log/代表虚拟目录，不代表实体位置
                    if (File.Exists(filepath))
                    {
                        msg = "{\"success\":true,\"msg\":\"\"}";
                    }
                    else
                    {
                        msg = "{\"success\":false,\"msg\":\"文件丢失，无法下载\"}";
                    }
                }
                else
                {
                    msg = "{\"success\":false,\"msg\":\"文件丢失，无法下载\"}";
                }
            }
            catch (Exception ex)
            {
                msg = "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";
            }
            return msg;
        }
        /// 请求日志
        /// <summary>
        /// 请求日志
        /// </summary>
        /// <returns></returns>
        private string RequireLog()
        {
            bool success = false;
            string error = "";

            try
            {
                DateTime logdate = CommonFunction.ObjectToDateTime(LogDate);
                int agentid = CommonFunction.ObjectToInteger(AgentID);
                int vendorid = CommonFunction.ObjectToInteger(VendorID);
                success = CreateRequireLogToDB(logdate, agentid, vendorid);
            }
            catch (Exception ex)
            {
                success = false;
                error = ex.Message;
            }
            return "{success:" + success.ToString().ToLower() + ",error:'" + error + "'}";
        }
        /// 请求日志-批量
        /// <summary>
        /// 请求日志-批量
        /// </summary>
        /// <returns></returns>
        private string RequireLogMutil()
        {
            bool success = false;
            string error = "";
            string success_msg = "";

            try
            {
                int num_s = 0;
                int num_f = 0;
                string[] dataones = Datas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string dataone in dataones)
                {
                    string[] keys = dataone.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime logdate = CommonFunction.ObjectToDateTime(keys[0]);
                    int agentid = CommonFunction.ObjectToInteger(keys[1]);
                    int vendorid = CommonFunction.ObjectToInteger(keys[2]);
                    if (CreateRequireLogToDB(logdate, agentid, vendorid))
                    {
                        num_s++;
                    }
                    else
                    {
                        num_f++;
                    }
                }
                if (num_f > 0 && num_s > 0)
                {
                    success_msg = num_s + "项请求成功，" + num_f + "项已在请求中";
                    success = true;
                }
                else if (num_s > 0)
                {
                    success_msg = "全部请求成功";
                    success = true;
                }
                else
                {
                    success_msg = "全部项都在请求中，不能重复请求";
                    success = false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                error = ex.Message;
            }
            return "{success:" + success.ToString().ToLower() + ",success_msg:'" + success_msg + "',error:'" + error + "'}";
        }
        /// 生成一条请求数据到数据库
        /// <summary>
        /// 生成一条请求数据到数据库
        /// </summary>
        /// <param name="logdate"></param>
        /// <param name="agentid"></param>
        /// <param name="vendorid"></param>
        /// <returns></returns>
        private bool CreateRequireLogToDB(DateTime logdate, int agentid, int vendorid)
        {
            ClientLogRequireInfo info = BLL.ClientLogRequire.Instance.GetClientLogRequireInfo(logdate, agentid, vendorid);
            if (info == null)
            {
                //新增
                info = new ClientLogRequireInfo();
                info.AgentID = agentid;
                info.LogDate = logdate;
                info.Vendor = vendorid;

                info.CreateUserID = userid;
                info.CreateTime = DateTime.Now;
            }
            else
            {
                if (info.Status_Value == 1)
                {
                    //请求中，无需再次请求
                    return false;
                }
                //修改
                info.LastUpdateUserID = userid;
                info.LastUpdateTime = DateTime.Now;
            }

            info.RequireID = userid;
            info.RequireDateTime = DateTime.Now;
            info.Status = 1;//请求中

            info.ResponseDateTime = null;
            info.ResponseSuccess = null;
            info.ResponseRemark = null;
            info.FilePath = null;

            if (info.RecID == null)
            {
                return CommonBll.Instance.InsertComAdoInfo(info);
            }
            else
            {
                return CommonBll.Instance.UpdateComAdoInfo(info);
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