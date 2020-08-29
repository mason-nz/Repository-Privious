using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
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

            //HttpPostedFile file = context.Request.Files["Filedata"];
            HttpPostedFile file = context.Request.Files.Get(0);

            try
            {
                //context.Response.Write("[{'result':'noFiles'}]");
                //context.Response.End();
                //return;

                if (file.ContentLength == 0)
                {
                    context.Response.Write("[{'result':'noFiles'}]");
                }
                else
                {
         
                    //验证文件扩展名
                    bool flagextens = true;
                    flagextens = IsAllowedExtension(file.FileName);

                    if (!flagextens)
                    {
                        context.Response.Write("[{'result':'failure'}]");
                        //context.Response.End();
                        return;
                    }
                    //



                    string retUrlPath = "";

                    //添加文件路径信息
                    string fullName = GenPath(file.FileName, out retUrlPath);
                    file.SaveAs(fullName);//保存上载文件
                    

                    #region 获取要返回的信息

                    string filePath = retUrlPath + Path.GetFileName(fullName);//文件路径
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);//文件名（不含扩展名）
                    string fileExt = Path.GetExtension(file.FileName); ;//扩展名
                    FileInfo info = new FileInfo(fullName);
                    long fileSize = info.Length;//文件大小（Byte）

                    #endregion




                    bool flagheader = true;
                    flagheader = IsAllowedFileHeader(fullName);

                    if (!flagheader)
                    {
                        File.Delete(fullName);
                        context.Response.Write("[{'result':'failure'}]");
                        //context.Response.End();
                        return;
                    }


                    //#region 返回Json值
                    filePath = filePath.Replace("\\", "--");
                    retStr += "{";
                    retStr += "'result':'succeed',";
                    retStr += "'RecID':null,";
                    retStr += "'FilePath':'" + filePath + "',";
                    //retStr += "'FilePath':'" + filePath + "',";
                    retStr += "'FileName':'" + fileName + "',";
                    //retStr += "'FileName':'" + HttpUtility.UrlEncode(fileName) + "',";
                    retStr += "'ExtendName':'" + fileExt + "',";
                    retStr += "'FileSize':'" + fileSize.ToString() + "'";
                    //retStr += "'FilePath':'" + filePath + "',";
                    //retStr += "'FileName':'" + fileName + "',";
                    //retStr += "'ExtendName':'" + fileExt + "',";
                    //retStr += "'FileSize':'" + fileSize.ToString() + "'";

                    retStr += "}";

                    context.Response.Write(retStr);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("[{'result':'failure'}]");
            }

        }

        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns>返回结果</returns>
        public Boolean IsImage(string path)
        {
            return false;
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
        /// <summary>
        /// 验证扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool IsAllowedExtension(string fileName)
        {
            string strExtension = "";
            string[] arrExtension = { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".pps", ".pdf", ".txt" };
            if (fileName != string.Empty)
            {
                strExtension = fileName.Substring(fileName.LastIndexOf("."));
                for (int i = 0; i < arrExtension.Length; i++)
                {
                    if (strExtension.Equals(arrExtension[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsAllowedFileHeader(string filepath)
        {
            //filepath = System.Web.HttpContext.Current.Server.MapPath(filepath);
            //context.Request.GetBufferlessInputStream
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();

            }
            catch
            {

            }
            r.Close();
            fs.Close();
            //            jpg: 255,216
            //gif: 71,73
            //bmp: 66,77
            //png: 137,80
            //doc: 208,207
            //docx: 80,75
            //xls: 208,207
            //xlsx: 80,75
            if (fileclass == "255216" || fileclass == "7173" || fileclass == "6677" || fileclass == "13780" || fileclass == "208207" || fileclass == "8075" || fileclass == "208207" || fileclass == "8075")//说明255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
            {
                return true;
            }
            else
            {
                return false;
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