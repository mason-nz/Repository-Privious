﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.BUOC.Chitunion2017.NewWeb.AjaxServers
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {
        private string UploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        private string CleanImgURLPrefix = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CleanImgURLPrefix", false);
        private string AllowUploadFileHeaderInfo = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("AllowUploadFileHeaderInfo", false);
        private string ExitAddress = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress", false);
        private int SmallImageSize = 248;//生成缩略图尺寸

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private string action;

        /// <summary>
        /// 小写
        /// </summary>
        public string Action
        {
            get
            {
                return string.IsNullOrEmpty(this.action) ?
                    (this.action = HttpUtility.UrlDecode(this.Request["Action"] + "").Trim().ToLower())
                    : this.action;
            }
            set { this.action = value; }
        }

        //public string Msg { get; set; }

        /// <summary>
        /// 集中权限系统登录是的cookies的内容
        /// </summary>
        public string LoginCookiesContent
        {
            get
            {
                return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestStr("LoginCookiesContent");
            }
        }

        /// <summary>
        /// 是否生成缩略图，1--生成，0或其他不生成
        /// </summary>
        public int IsGenSmallImage
        {
            get
            {
                return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestInt("IsGenSmallImage");
            }
        }

        private static string ToJson(object target)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(target);
        }

        public void ProcessRequest(HttpContext context)
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("[ProcessRequest] start...");
            context.Response.ContentType = "text/plain";
            UploadResult ur = new UploadResult();
            ur.Status = 0;
            var serializer = new JavaScriptSerializer();
            //string msg = "";
            int category = -1;
            int userid = ITSC.Chitunion2017.Common.UserInfo.VerifyCookieContent(LoginCookiesContent, out category);
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("[ProcessRequest] userid=" + userid);
            if (userid > 0)
            {
                try
                {
                    switch (Action)
                    {
                        case "batchimport":
                            //AJAXHelper.WrapJsonResponse(BatchImport(out msg), "", msg);
                            BatchImport(ref ur);
                            //msg = "{'status':'1','msg':'" + Msg + "'}";
                            //ur.Status = 1;
                            //ur.Msg = Msg;
                            break;

                        default:
                            ur.Msg = "没有对应的操作";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("上传文件报错", ex);
                }
            }
            else
            {
                //msg = "{'status':'0','msg':'您还没有登陆'}";
                ur.Msg = "您还没有登陆";
            }
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("[ProcessRequest] end.序列化对象：" + serializer.Serialize(ur));
            context.Response.Write(serializer.Serialize(ur));
            context.Response.End();
        }

        /// <summary>
        /// 上传文件根目录 /UploadFiles/BackMoney/
        /// </summary>
        private const string UpladFilesPath = "/UploadFiles/";

        internal bool BatchImport(ref UploadResult ur)
        {
            //ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("[HandlerImport]BatchImport begin...");
            try
            {
                string fileName = string.Empty;
                string webFilePath = string.Empty;
                //保存文件
                this.SaveFile(out fileName, out webFilePath, ref ur);
                //ur.Msg = webFilePath;
                //ur.FileName = fileName;
                //ur.Status = 1;
                ////校验 插入
                //return DealDataImported(fileName, out msg);

                return true;
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[HandlerImport]BatchImport", ex);
                //BLL.Loger.Log4Net.Info("[HandlerImport]BatchImport errorMessage..." + ex.Message);
                ur.Msg = ex.Message;
                ur.Status = 0;
                return false;
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private void SaveFile(out string fileName, out string filePath, ref UploadResult ur)
        {
            ////先清空原有文件
            //ClearFiles(DateTime.Now.AddDays(-1));

            HttpPostedFile hpf = System.Web.HttpContext.Current.Request.Files["Filedata"];
            if (hpf.ContentLength > 0)
            {
                if (VerifyFileHeader(hpf))
                {
                    fileName = Path.GetFileName(hpf.FileName);
                    fileName = fileName.Replace("/", "")
                                       .Replace("+", "")
                                       .Replace(" ", "")
                                       .Replace("?", "")
                                       .Replace("=", "")
                                       .Replace("%", "")
                                       .Replace("&", "");
                    string extName = Path.GetExtension(fileName);
                    switch (extName.ToLower())
                    {
                        case ".jpg":
                        case ".gif":
                        case ".png":
                        case ".bmp":
                            System.Drawing.Image img = System.Drawing.Bitmap.FromStream(hpf.InputStream);
                            filePath = CleanImgURLPrefix + ITSC.Chitunion2017.BLL.Util.CleanImg(img);
                            break;
                        default:
                            ////添加文件路径信息
                            string fullName = GenPath(fileName, out filePath);
                            hpf.SaveAs(fullName);//保存上载文件
                            filePath = ExitAddress + filePath;
                            break;
                    }

                    ////生成缩略图逻辑
                    //if (IsGenSmallImage == 1)
                    //{
                    //    string smallFilePath = Path.GetDirectoryName(fullName) + "\\";
                    //    string smallFileName = Path.GetFileNameWithoutExtension(fullName) + "_sl" + Path.GetExtension(fullName);
                    //    smallFilePath += smallFileName;
                    //    if (XYAuto.Utils.ImageTool.MakeSmallImage(fullName, smallFilePath, SmallImageSize, ImageTool.LimitSideMode.Auto))
                    //    {
                    //        filePath += "|" + Path.GetDirectoryName(filePath) + "\\" + smallFileName;
                    //    }
                    //}


                    ur.Msg = filePath.Replace("\\", "/");
                    ur.FileName = fileName;
                    ur.Status = 1;
                    //return fullName;
                }
                else
                {
                    //File.Delete(fullName);
                    fileName = string.Empty;
                    filePath = string.Empty;
                    ur.Msg = "上传文件不是有效的格式";
                    ur.FileName = fileName;
                    ur.Status = 0;
                    //return fileName;
                }
            }
            else { throw new Exception("没有上传文件"); }
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="fileName">上传时文件名</param>
        /// <param name="webFilePath">生成后的url路径</param>
        /// <returns></returns>
        private string GenPath(string fileName, out string webFilePath)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            string name = Path.GetFileNameWithoutExtension(fileName);
            name = name + "$" + Guid.NewGuid().ToString() + ext;

            DateTime time = DateTime.Now;
            string relatedPath = string.Format(UpladFilesPath + "{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);
            webFilePath = relatedPath + name;
            //string dir = HttpContext.Current.Server.MapPath("~" + relatedPath);
            string dir = UploadFilePath + relatedPath;
            //string dir = UploadFilePath + relatedPath.Replace(@"/", "\\"); ;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Path.Combine(dir, name);
        }

        /// <summary>
        /// 验证上传文件的文件头信息
        /// </summary>
        /// <param name="hpf"></param>
        /// <returns></returns>
        private bool VerifyFileHeader(HttpPostedFile hpf)
        {
            if (hpf.FileName.ToLower().EndsWith(".mp4"))
            {
                return true;
            }
            string[] array = AllowUploadFileHeaderInfo.Split(',');
            Dictionary<string, string> FilesHeader = new Dictionary<string, string>();
            foreach (string extArray in array)
            {
                FilesHeader.Add(extArray.Split(':')[0], extArray.Split(':')[1]);
            }
            byte[] fileByte = new byte[2];//这里我们只读取文件长度的前两位用于判断就好了
            hpf.InputStream.Read(fileByte, 0, 2);
            //BinaryReader reader = new BinaryReader(uploadFileInputStream);
            //string fileClass;
            //byte buffer;
            //buffer = reader.ReadByte();
            //fileClass = buffer.ToString();
            //buffer = reader.ReadByte();
            //fileClass += buffer.ToString();
            //reader.Close();
            string fileClass = "";
            if (fileByte != null && fileByte.Length > 0)//图片数据是否为空
            {
                fileClass = fileByte[0].ToString() + fileByte[1].ToString();
            }
            return FilesHeader.ContainsValue(fileClass);
        }

        ///// <summary>
        ///// 生成文件名
        ///// </summary>
        ///// <param name="fileName">上传时文件名</param>
        ///// <param name="webFilePath">生成后的url路径</param>
        ///// <returns></returns>
        //private string GenPath(string fileName, out string webFilePath)
        //{
        //    string ext = Path.GetExtension(fileName);//文件后缀名
        //    string name = Path.GetFileNameWithoutExtension(fileName);
        //    name = name + "$" + Guid.NewGuid().ToString() + ext;

        //    DateTime time = DateTime.Now;
        //    string relatedPath = string.Format(UpladFilesPath + "{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);
        //    webFilePath = relatedPath + name;
        //    //string dir = HttpContext.Current.Server.MapPath("~" + relatedPath);
        //    string dir = UploadFilePath + relatedPath;
        //    //string dir = UploadFilePath + relatedPath.Replace(@"/", "\\"); ;
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }

        //    return Path.Combine(dir, name);
        //}

        /// <summary>
        /// 清除指定时间之前的所有文件
        /// </summary>
        //private void ClearFiles(DateTime datetime)
        //{
        //    //string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~" + UpladFilesPath));
        //    string path = UploadFilePath + UpladFilesPath;
        //    if (Directory.Exists(path))
        //    {
        //        string[] files = Directory.GetFiles(UploadFilePath + UpladFilesPath);
        //        foreach (string name in files)
        //        {
        //            FileInfo fi = new FileInfo(name);
        //            if (fi.CreationTime < datetime)
        //            {
        //                fi.Delete();
        //            }
        //        }
        //    }
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class UploadResult
    {
        /// <summary>
        /// 1-正常，0-异常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回正常路径
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回上传文件的文件名称（包含扩展名）
        /// </summary>
        public string FileName { get; set; }
    }
}