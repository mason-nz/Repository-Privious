using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    /// <summary>
    /// FileLoad 的摘要说明
    /// </summary>
    public class FileLoad : IHttpHandler
    {

        //private const string UpladFilesPath = "UpLoadFile";
        public string UpladFilesPath = ConfigurationUtil.GetAppSettingValue("CommonLoadFile");
        public string CommonLoadDomain = ConfigurationUtil.GetAppSettingValue("CommonLoadDomain");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string retStr = "";
            HttpPostedFile file = context.Request.Files.Get(0);
            try
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                if (objCache[DateTime.Now.ToString("yyyy-MM-dd") + "_WenJianSum"] != null)
                {
                    string numberH = objCache[DateTime.Now.ToString("yyyy-MM-dd") + "_WenJianSum"].ToString();
                    int _numberH = 0;
                    int.TryParse(numberH, out _numberH);
                    string wenJianSum = ConfigurationUtil.GetAppSettingValue("WenJianSum");
                    if (!string.IsNullOrEmpty(wenJianSum))
                    {
                        int _wenJianSum = 0;
                        int.TryParse(wenJianSum, out _wenJianSum);
                        if (_numberH >= _wenJianSum)
                        {
                            context.Response.Write("[{'result':'failure'}]");
                            return;
                        }
                    }
                }
                if (file.ContentLength == 0)
                {
                    context.Response.Write("[{'result':'noFiles'}]");
                }
                else if (file.ContentLength > 1024 * 1024 * 5)
                {
                    context.Response.Write("[{'result':'failure'}]");
                }
                else
                {
                    //验证文件扩展名
                    bool flagextens = true;
                    flagextens = IsAllowedExtension(file.FileName);
                    if (!flagextens)
                    {
                        context.Response.Write("[{'result':'failure'}]");
                        return;
                    }
                    //

                    //验证文件头
                    bool flagheader = true;
                    flagheader = IsAllowedFileHeader(file.InputStream);
                    if (!flagheader)
                    {
                        context.Response.Write("[{'result':'failure'}]");
                        return;
                    }
                    //


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
                    filePath = filePath.Replace("\\", "--");
                    retStr += "{";
                    retStr += "'result':'succeed',";
                    retStr += "'RecID':null,";
                    retStr += "'FilePath':'" + filePath + "',";
                    retStr += "'FileName':'" + fileName + "',";
                    retStr += "'ExtendName':'" + fileExt + "',";
                    retStr += "'FileSize':'" + fileSize.ToString() + "'";
                    retStr += "}";

                    if (objCache[DateTime.Now.ToString("yyyy-MM-dd") + "_WenJianSum"] != null)
                    {
                        string number = objCache[DateTime.Now.ToString("yyyy-MM-dd") + "_WenJianSum"].ToString();
                        int _number = 0;
                        int.TryParse(number, out _number);
                        int num = _number + 1;
                        objCache[DateTime.Now.ToString("yyyy-MM-dd") + "_WenJianSum"] = num.ToString();
                    }
                    else
                    {
                        objCache.Insert(DateTime.Now.ToString("yyyy-MM-dd") + "_WenJianSum", 1, null, DateTime.Now.AddHours(24), TimeSpan.Zero);
                    }

                    context.Response.Write(retStr);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("上传文件失败:" + ex.Message);
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
            string relatedPath = string.Format(UpladFilesPath + "/{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);

            string dir = relatedPath;
            //string hostDomain = "http://" + HttpContext.Current.Request.Url.Host.ToString();
            //返回URL地址
            UrlPath = "//" + CommonLoadDomain + string.Format("/{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour).Replace("/", @"\");

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

        private bool IsAllowedFileHeader(Stream fs)
        {
            //filepath = System.Web.HttpContext.Current.Server.MapPath(filepath);
            //context.Request.GetBufferlessInputStream
            //System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
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
            //r.Close();
            //fs.Close();
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