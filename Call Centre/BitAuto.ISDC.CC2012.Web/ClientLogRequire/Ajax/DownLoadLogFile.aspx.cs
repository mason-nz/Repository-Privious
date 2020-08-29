using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.ClientLogRequire.Ajax
{
    public partial class DownLoadLogFile : System.Web.UI.Page
    {
        private string FilePath
        {
            get
            {
                return Request["FilePath"] == null ? "" :
                HttpUtility.UrlDecode(Request["FilePath"].ToString().Trim());
            }
        }
        private string AgentName
        {
            get
            {
                return Request["AgentName"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentName"].ToString().Trim());
            }
        }
        private string VendorName
        {
            get
            {
                return Request["VendorName"] == null ? "" :
                HttpUtility.UrlDecode(Request["VendorName"].ToString().Trim());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024MOD5009"))
            {
                DownLoadLog();
            }
            else
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }

        private void DownLoadLog()
        {
            try
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    string root = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LogWebAbsolutePath", false).TrimEnd('\\');
                    string filepath = root + FilePath.Replace("/log/", "/").Replace("/", "\\");
                    string downfilename = Path.GetFileNameWithoutExtension(filepath) + "_" + AgentName + "_" + VendorName + ".zip";
                    if (File.Exists(filepath))
                    {
                        FileInfo fi = new FileInfo(filepath);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.Buffer = false;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(downfilename, System.Text.Encoding.UTF8));
                        Response.AppendHeader("Content-Length", fi.Length.ToString());
                        Response.ContentType = "application/x-zip-compressed";
                        Response.WriteFile(filepath);
                        Response.Flush();
                        Response.Close();
                        Response.End();
                    }
                    else
                    {
                        Response.Write("<script>alert('文件丢失，无法下载！');window.history.back();</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('文件丢失，无法下载！');window.history.back();</script>");
                }
            }
            catch
            {
            }
        }
    }
}