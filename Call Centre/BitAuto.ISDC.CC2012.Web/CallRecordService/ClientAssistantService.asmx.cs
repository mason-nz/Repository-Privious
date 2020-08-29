using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.Entities;
using System.Xml;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// ClientAssistantService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ClientAssistantService : System.Web.Services.WebService
    {
        /// 不校验IP的验证方式
        /// <summary>
        /// 不校验IP的验证方式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CheckKey(string key)
        {
            return key == "yiche-ClineLog-!@#$#@!";
        }

        [WebMethod(Description = "坐席推送日志")]
        public bool PushClientLogForAgent(string key, byte[] data, DateTime date, int userid, Vender vender)
        {
            bool filesuccess = false;
            string reason = "";
            string filepath = "";
            try
            {
                if (!CheckKey(key))
                {
                    BLL.Loger.Log4Net.Info("[坐席推送日志] 授权码错误");
                    filesuccess = false;
                    reason = "[坐席推送日志] 授权码错误";
                }

                if (data != null)
                {
                    string root = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LogWebAbsolutePath", false).TrimEnd('\\')
                        + "\\clientlog_" + vender.ToString().ToLower() + "\\" + userid + "\\";
                    string filename = date.ToString("yyyy_MM_dd") + "_" + userid + ".zip";
                    BLL.Util.BinaryToFile(root + filename, data);
                    filesuccess = true;
                    reason = "";
                    filepath = "/log/clientlog_" + vender.ToString().ToLower() + "/" + userid + "/" + filename;
                }
                else
                {
                    filesuccess = false;
                    reason = "客户端日志已不存在";
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[坐席推送日志] 文件保存异常", ex);
                filesuccess = false;
                reason = ex.Message;
            }

            //数据入库
            try
            {
                ClientLogRequireInfo info = BLL.ClientLogRequire.Instance.GetClientLogRequireInfo(date, userid, (int)vender);
                if (info == null)
                {
                    //新增
                    info = new ClientLogRequireInfo();
                    info.AgentID = userid;
                    info.LogDate = date;
                    info.Vendor = (int)vender;

                    info.CreateUserID = userid;
                    info.CreateTime = DateTime.Now;
                }
                else
                {
                    //修改
                    info.LastUpdateUserID = userid;
                    info.LastUpdateTime = DateTime.Now;
                }
                info.Status = 2;//已响应

                info.ResponseDateTime = DateTime.Now;
                info.ResponseSuccess = filesuccess ? 1 : 2;//0 未知 1 成功 2 失败
                info.ResponseRemark = reason;
                info.FilePath = filepath;

                if (info.RecID == null)
                {
                    CommonBll.Instance.InsertComAdoInfo(info);
                }
                else
                {
                    CommonBll.Instance.UpdateComAdoInfo(info);
                }
                return true;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[坐席推送日志] 数据入库异常", ex);
                return false;
            }
        }
        [WebMethod(Description = "获取服务端的版本号")]
        public string GetClientServerVersion(string key, string ServerVersionsName)
        {
            try
            {
                if (!CheckKey(key))
                {
                    BLL.Loger.Log4Net.Info("[获取服务端的版本号] 授权码错误");
                    return null;
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + "down\\config.xml");
                XmlNode root = doc.SelectSingleNode("Userconfig/" + ServerVersionsName);
                string value = root.InnerText.Trim();
                return value;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[获取服务端的版本号] 异常", ex);
                return null;
            }
        }
        [WebMethod(Description = "获取相关用户和版本的所有请求")]
        public DataTable GetClientLogRequireInfo(string key, int userid, Vender vender)
        {
            try
            {
                if (!CheckKey(key))
                {
                    BLL.Loger.Log4Net.Info("[获取相关用户和版本的所有请求] 授权码错误");
                    return null;
                }

                return BLL.ClientLogRequire.Instance.GetClientLogRequireInfo(userid, (int)vender);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[获取相关用户和版本的所有请求] 异常", ex);
                return null;
            }
        }

        //[WebMethod(Description = "下载坐席日志")]
        //public byte[] GetClientLogForManage(string key, DateTime date, int userid, Vender vender)
        //{
        //    try
        //    {
        //        if (!CheckKey(key))
        //        {
        //            BLL.Loger.Log4Net.Info("[下载坐席日志] 授权码错误");
        //            return null;
        //        }

        //        string root = Server.MapPath("/log/clientlog_" + vender.ToString().ToLower()) + "\\" + userid + "\\";
        //        string filename = date.ToString("yyyy_MM_dd") + "_" + userid + ".log";
        //        return BLL.Util.FileToBinary(root + filename);
        //    }
        //    catch (Exception ex)
        //    {
        //        BLL.Loger.Log4Net.Info("[下载坐席日志] 异常", ex);
        //        return null;
        //    }
        //}
    }
}
