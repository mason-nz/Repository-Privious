using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace BitAuto.DSC.IM2014.Server.Web.AjaxServers
{
    /// <summary>
    /// FileLoad 的摘要说明
    /// </summary>
    public class FileLoad : IHttpHandler
    {

        private const string UpladFilesPath = "UpLoadFile";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string retStr = "";

            HttpPostedFile file = context.Request.Files["Filedata"];

            try
            {
                if (file.ContentLength == 0)
                {
                    context.Response.Write("[{'result':'noFiles'}]");
                }
                else
                {
                    #region 保存文件

                    string retUrlPath = "";

                    //添加文件路径信息
                    string fullName = GenPath(file.FileName, out retUrlPath);
                    file.SaveAs(fullName);//保存上载文件

                    #endregion

                    #region 获取要返回的信息

                    string filePath = retUrlPath + Path.GetFileName(fullName);//文件路径
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);//文件名（不含扩展名）
                    string fileExt = Path.GetExtension(file.FileName); ;//扩展名
                    FileInfo info = new FileInfo(fullName);
                    long fileSize = info.Length;//文件大小（Byte）

                    #endregion

                    #region 返回Json值

                    retStr += "{";
                    retStr += "'result':'succeed',";
                    retStr += "'RecID':null,";
                    retStr += "'FilePath':'" + HttpUtility.UrlEncode(filePath) + "',";
                    retStr += "'FileName':'" + HttpUtility.UrlEncode(fileName) + "',";
                    retStr += "'ExtendName':'" + HttpUtility.UrlEncode(fileExt) + "',";
                    retStr += "'FileSize':'" + HttpUtility.UrlEncode(fileSize.ToString()) + "'";
                    //retStr += "'FilePath':'" + filePath + "',";
                    //retStr += "'FileName':'" + fileName + "',";
                    //retStr += "'ExtendName':'" + fileExt + "',";
                    //retStr += "'FileSize':'" + fileSize.ToString() + "'";

                    retStr += "}";

                    context.Response.Write(retStr);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("[{'result':'failure'}]");
            }
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenPath(string fileName, out string UrlPath)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            string name = Path.GetFileNameWithoutExtension(fileName);
            name = name + Guid.NewGuid().ToString() + ext;

            DateTime time = DateTime.Now;
            string relatedPath = string.Format("/" + UpladFilesPath + "/{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);

            string dir = HttpContext.Current.Server.MapPath("~" + relatedPath);

            //返回URL地址
            UrlPath = relatedPath.Replace("/", @"\");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Path.Combine(dir, name);
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