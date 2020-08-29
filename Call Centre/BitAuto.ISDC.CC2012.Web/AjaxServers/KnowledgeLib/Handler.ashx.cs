using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 操作类型（save:保存  sub:提交,RecordingSharingSave:录音共享保存）
        /// </summary>
        public string Action
        {
            get
            {
                if (HttpContext.Current.Request["act"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["act"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///  知识点ID
        /// </summary>
        public string KID
        {
            get
            {
                if (HttpContext.Current.Request["kid"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["kid"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///  知识点ID
        /// </summary>
        public string Title
        {
            get
            {
                if (HttpContext.Current.Request["title"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["title"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        /// <summary>
        ///  知识点ID
        /// </summary>
        public string Content
        {
            get
            {
                if (HttpContext.Current.Request["con"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["con"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string FileUrl
        {
            get
            {
                if (HttpContext.Current.Request["ul"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ul"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            switch (Action)
            {
                case "df":
                    DownloadFile(context);
                    break;
            }
        }

        private string AddFavorite()
        {
            return "";
        }

        private void DownloadFile(HttpContext context)
        {
            if (string.IsNullOrEmpty(FileUrl) || !File.Exists(context.Server.MapPath(FileUrl))) return;

            string FileName = Path.GetFileName(FileUrl);

            if (string.IsNullOrEmpty(FileUrl)) return;
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/octet-stream";
            context.Response.Buffer = false;

            //使用UTF-8对文件名进行编码
            var filenameT = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8).Replace("+", "%20");
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + filenameT + "\"");
            context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            context.Response.WriteFile(FileUrl);
            context.Response.End();
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