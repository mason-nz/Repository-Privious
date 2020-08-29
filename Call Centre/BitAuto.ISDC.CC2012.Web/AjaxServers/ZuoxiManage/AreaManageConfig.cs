using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.IO;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage
{
    /// <summary>
    /// 配置文件操作类
    /// </summary>
    public class AreaManageConfig
    {
        #region 属性
        /// 文件路径（不含名称）
        /// <summary>
        /// 文件路径（不含名称）
        /// </summary>
        private string FilePath = "";
        /// 文件名称（不含路径）
        /// <summary>
        /// 文件名称（不含路径）
        /// </summary>
        private const string FileName = "AreaManageConfig.xml";
        #endregion

        public AreaManageConfig(HttpServerUtility server)
        {
            FilePath = server.MapPath("~");
        }
        /// 读取文件
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ReadFile(string area)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                dic = CommonFunction.GetAllNodeContentByFile<string, string>(FilePath + "\\" + FileName, "key", "value", "/root/" + area);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(ex.Message + ex.StackTrace);
            }
            return dic;
        }
        /// 写入文件
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="list"></param>
        public void WriteFile(Dictionary<string, Dictionary<string, string>> list)
        {
            try
            {
                CommonFunction.SaveDictionaryToFile(list, FilePath + "\\" + FileName);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(ex.Message + ex.StackTrace);
            }
        }
        /// 查询登录人区域
        /// <summary>
        /// 查询登录人区域
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentUserArea()
        {
            List<string> list = new List<string>();
            try
            {
                string UserID = BLL.Util.GetLoginUserID().ToString();
                Dictionary<string, string> dic1 = ReadFile("北京");
                if (dic1.ContainsKey(UserID))
                {
                    list.Add("北京");
                }
                Dictionary<string, string> dic2 = ReadFile("西安");
                if (dic2.ContainsKey(UserID))
                {
                    list.Add("西安");
                }
                return list;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(ex.Message + ex.StackTrace);
            }
            return list;
        }
    }
}