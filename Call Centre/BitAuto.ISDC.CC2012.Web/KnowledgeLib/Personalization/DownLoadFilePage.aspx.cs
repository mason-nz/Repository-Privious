using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    public partial class DownLoadFilePage : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string Action
        {
            get
            {
                if (Request["theAction"] != null)
                {
                    return HttpUtility.UrlDecode(Request["theAction"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string PostUrl
        {
            get
            {
                if (Request["theUrl"] != null)
                {
                    return HttpUtility.UrlDecode(Request["theUrl"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(PostUrl)) return;
                if (PostUrl.StartsWith("http"))
                {
                    try
                    {
                        DownLoadAudioFile(PostUrl);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                }
                else
                {
                    if (File.Exists(Server.MapPath(PostUrl)))
                    {
                        DownloadFile(Server.MapPath(PostUrl));
                    }
                }
            }
        }

        public void DownloadFile(string path)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                Response.Clear();
                Response.Charset = "GB2312";
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                // 添加头信息，为"文件下载/另存为"对话框指定默认文件名,去除空格
                var filenameT = HttpUtility.UrlEncode(Path.GetFileName(path), System.Text.Encoding.UTF8).Replace("+", "%20");

                Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + filenameT + "\"");

                // 添加头信息，指定文件大小，让浏览器能够显示下载进度
                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.WriteFile(file.FullName);
                Response.Flush();

                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('系统出现以下错误://n" + ex.Message + "!//n请尽快与管理员联系.')</script>");
            }
            finally
            {
                File.Delete(path);
                Response.End();
            }
        }
        public void DownLoadAudioFile(string audioUrl)
        {
            string fileName = Path.GetFileName(audioUrl);
            string strSaveDirectory = HttpContext.Current.Server.MapPath("/DownLoadedAudioFiles");
            if (!Directory.Exists(strSaveDirectory))
            {
                Directory.CreateDirectory(strSaveDirectory);
            }
            string savePath = strSaveDirectory + "\\" + fileName;
            //if (Directory.GetFiles(strSaveDirectory).Length > 0)
            //{
            //    foreach (string var in Directory.GetFiles(strSaveDirectory))
            //    {
            //        File.Delete(var);
            //    }
            //}

            Uri rl = new Uri(audioUrl);
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(rl, savePath);
                }
                DownloadFile(savePath);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}