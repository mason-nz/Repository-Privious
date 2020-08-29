using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.API.Upload
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {
        private readonly string UploadFilePath = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        private readonly string AllowUploadFileHeaderInfo = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("AllowUploadFileHeaderInfo", false);
        private readonly string ExitAddress = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Domin", false);
        private int SmallImageSize = 248;//生成缩略图尺寸

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private string action;

        public UploadFile(int smallImageSize)
        {
            SmallImageSize = smallImageSize;
        }

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

        /// <summary>
        /// 是否生成缩略图，1--生成，0或其他不生成
        /// </summary>
        public int IsGenSmallImage
        {
            get
            {
                return CTUtils.Html.RequestHelper.GetCurrentRequestInt("IsGenSmallImage");
            }
        }

        private static string ToJson(object target)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(target);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UploadResult ur = new UploadResult();
            ur.Status = 0;
            var serializer = new JavaScriptSerializer();
            int category = -1;
            int userid = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (userid > 0)
            {
                try
                {
                    switch (Action)
                    {
                        case "batchimport":
                            BatchImport(ref ur);
                            break;

                        default:
                            ur.Msg = "没有对应的操作";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    CTUtils.Log.Log4NetHelper.Default().Error("上传文件报错", ex);
                }
            }
            else
            {
                ur.Msg = "您还没有登陆";
            }

            context.Response.Write(serializer.Serialize(ur));
            context.Response.End();
        }

        /// <summary>
        /// 上传文件根目录 /UploadFiles/BackMoney/
        /// </summary>
        private const string UpladFilesPath = "/UploadFiles/";

        internal bool BatchImport(ref UploadResult ur)
        {
            try
            {
                string fileName = string.Empty;
                string webFilePath = string.Empty;
                //保存文件
                this.SaveFile(out fileName, out webFilePath, ref ur);
                return true;
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error("[HandlerImport]BatchImport", ex);
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
                        case ".png":
                        case ".bmp":
                        case ".jpeg":
                            System.Drawing.Image img = System.Drawing.Bitmap.FromStream(hpf.InputStream);
                            filePath = XYAuto.CTUtils.Image.FastDFSHelper.CleanImg(img);
                            break;
                        default:
                            ////添加文件路径信息
                            string fullName = GenPath(fileName, out filePath);
                            hpf.SaveAs(fullName);//保存上载文件
                            filePath = ExitAddress + filePath;
                            break;
                    }
                    ur.Msg = filePath.Replace("\\", "/");
                    ur.FileName = fileName;
                    ur.Status = 1;
                }
                else
                {
                    fileName = string.Empty;
                    filePath = string.Empty;
                    ur.Msg = "上传文件不是有效的格式";
                    ur.FileName = fileName;
                    ur.Status = 0;
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
            string[] array = AllowUploadFileHeaderInfo.Split(',');
            Dictionary<string, string> FilesHeader = new Dictionary<string, string>();
            foreach (string extArray in array)
            {
                FilesHeader.Add(extArray.Split(':')[0], extArray.Split(':')[1]);
            }
            byte[] fileByte = new byte[2];//这里我们只读取文件长度的前两位用于判断就好了
            hpf.InputStream.Read(fileByte, 0, 2);
            string fileClass = "";
            if (fileByte != null && fileByte.Length > 0)//图片数据是否为空
            {
                fileClass = fileByte[0].ToString() + fileByte[1].ToString();
            }
            return FilesHeader.ContainsValue(fileClass);
        }
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