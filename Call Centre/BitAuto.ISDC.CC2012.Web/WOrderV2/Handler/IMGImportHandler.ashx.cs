using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.Util;
using System.Web.Script.Serialization;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web
{
    /// <summary>
    /// IMGImportHandler 的摘要说明
    /// </summary>
    public class IMGImportHandler : IHttpHandler
    {
        #region 上传参数
        /// <summary>
        /// 集中权限系统登录是的cookies的内容
        /// </summary>
        private string LoginCookiesContent
        {
            get
            {
                return System.Web.HttpContext.Current.Request.QueryString["LoginCookiesContent"];
            }
        }
        /// <summary>
        /// action参数
        /// </summary>
        private string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("action");
            }
        }
        /// <summary>
        /// 文件存放永久路径（(ProjectTypePath 枚举)）
        /// </summary>
        private int StoragePathType
        {
            get
            {
                return CommonFunction.ObjectToInteger(BLL.Util.GetCurrentRequestStr("StoragePathType"));
            }
        }
        #endregion

        #region 删除参数
    
        /// <summary>
        /// 删除大图路径
        /// </summary>
        private string FileAllPath
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("fileAllPath");
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            try
            {
           
                    switch (Action)
                    {

                        case "imgImport":
                            if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                            {
                                string msg = "";
                                bool flag;
                                UploadIMGModel retModel = UploadOfficeSupplyImg(out msg, out flag);
                                JavaScriptSerializer jsserializer = new JavaScriptSerializer();

                                AJAXHelper.WrapJsonResponse(flag, msg, jsserializer.Serialize(retModel));
                            }
                            break;

                        case "deleteIMG":
                            if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                            {
                                bool flagDel = DeleteIMG();
                                AJAXHelper.WrapJsonResponse(flagDel, ",请重新删除！", "");
                            
                            }
                            break;
                        case "getSysLoginCookieName":

                            AJAXHelper.WrapJsonResponse(true, BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName, "");
                            break;

                        default:
                            AJAXHelper.WrapJsonResponse(false, "没有对应的操作", "没有对应的操作");
                            break;
                   
                }
            }
            catch (Exception ex)
            {
                AJAXHelper.WrapJsonResponse(false, "操作失败！", ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private bool DeleteIMG()
        {
            bool flag = true;        
           string filePath =  BitAuto.ISDC.CC2012.BLL.Util.GetUploadWebRoot() + ((BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath)StoragePathType).ToString() +FileAllPath;
          
            try//正常情况下不会出异常
            {
                string[] smallFileAllPathArr = filePath.Split('.');
                string smallFileAllPath = "";
                smallFileAllPath = smallFileAllPathArr[0] + "_small." + smallFileAllPathArr[1];
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    
                }
                if (File.Exists(smallFileAllPath))
                {
                    File.Delete(smallFileAllPath);
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        private UploadIMGModel UploadOfficeSupplyImg(out string msg, out bool flag)
        {
            msg = "";
            flag = true;
            UploadIMGModel retModel = null;
            try
            {
                HttpPostedFile hpf = System.Web.HttpContext.Current.Request.Files["Filedata"];

                if (hpf.ContentLength > 0)
                {
                    retModel = SaveFile(hpf);
                }

            }
            catch (Exception exp)
            {
                msg = "上传图片异常";
                flag = false;
                //  BLL.Loger.LogInfo.Error("【上传图片异常】：" + exp.Message);
            }

            return retModel;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        private UploadIMGModel SaveFile(HttpPostedFile hpf)
        {
            if (hpf.ContentLength > 0)
            {
                string upladFilesPath = GetUpladFilesPath();
                string guidFileName = Guid.NewGuid().ToString();
                string fileStorageName = "";
                string smallFileName = "";
                string fullName = GenPath(hpf.FileName, guidFileName, out fileStorageName);   //添加文件路径信息
                string retFullName = GenRetPath(hpf.FileName, guidFileName, out fileStorageName);
                string smallFileAllPath = GenSmallPath(hpf.FileName, guidFileName, out smallFileName);
                UploadIMGModel retModel = new UploadIMGModel();                
                retModel.FileRealName = hpf.FileName;
                retModel.FileSize = (Math.Round((double)hpf.ContentLength / 1024)).ToString();
                retModel.FileType = Path.GetExtension(hpf.FileName).ToLower().Replace(".","");
                retModel.FileAllPath = retFullName;
                retModel.SmallFileVirtuPath = GenSmallVirtuPath(hpf.FileName, guidFileName, out smallFileName);
                if (!Directory.Exists(upladFilesPath))
                {
                    Directory.CreateDirectory(upladFilesPath);
                }
                hpf.SaveAs(fullName);//保存上载文件
              //  BitAuto.Utils.ImageTool.MakeSmallImage(fullName, smallFileAllPath, 200, ImageTool.LimitSideMode.Auto);//保存缩略图
                IMGUploadHelper.MakeSmallImage(fullName, smallFileAllPath, 200, ImageTool.LimitSideMode.Auto);//保存缩略图

                return retModel;
            }

            else { throw new Exception("没有上传文件"); }
        }
        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenPath(string fileName, string guidFileName, out string fileStorageName)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            //fileStorageName = Path.GetFileNameWithoutExtension(fileName);
            //fileStorageName = fileStorageName + "€" + Guid.NewGuid().ToString() + ext;
            fileStorageName = guidFileName + ext;
            string UpladFilesPath = GetUpladFilesPath();
            if (!Directory.Exists(UpladFilesPath))
            {
                Directory.CreateDirectory(UpladFilesPath);
            }

            return Path.Combine(UpladFilesPath, fileStorageName);
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenRetPath(string fileName, string guidFileName, out string fileStorageName)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            //fileStorageName = Path.GetFileNameWithoutExtension(fileName);
            //fileStorageName = fileStorageName + "€" + Guid.NewGuid().ToString() + ext;
            fileStorageName = guidFileName + ext;
            string UpladFilesPath = GetRetUpladFilesPath();           

            return Path.Combine(UpladFilesPath, fileStorageName);
        }
        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenSmallVirtuPath(string fileName, string guidFileName, out string smallFileName)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            string UpladFilesPath =  GetSmallVirtuFilesPath();         
            smallFileName = guidFileName + "_small" + ext;
            return Path.Combine(UpladFilesPath, smallFileName);
        }
        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenSmallPath(string fileName, string guidFileName, out string smallFileName)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            string UpladFilesPath = GetUpladFilesPath();
            smallFileName = guidFileName + "_small" + ext;
            if (!Directory.Exists(UpladFilesPath))
            {
                Directory.CreateDirectory(UpladFilesPath);
            }

            return Path.Combine(UpladFilesPath, smallFileName);
        }

      

        //获取文件存放目录文件夹  带BitAuto.ISDC.CC2012.BLL.Util.GetUploadWebRoot() 
        private string GetUpladFilesPath()
        {

            return BitAuto.ISDC.CC2012.BLL.Util.GetUploadWebRoot().Replace('\\','/') + ((BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath)StoragePathType).ToString() + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
        }

        //获取文件存放目录文件夹  带BitAuto.ISDC.CC2012.BLL.Util.GetUploadWebRoot() 
        private string GetSmallVirtuFilesPath()
        {

            return "/upload/" + ((BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath)StoragePathType).ToString() + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
        }

        //获取不带根路径的文件存放目录文件夹  不带BitAuto.ISDC.CC2012.BLL.Util.GetUploadWebRoot() 和other 等
        private string GetRetUpladFilesPath()
        {
            return  "/"+DateTime.Now.ToString("yyyyMMdd") + "/";
        }

    }
    public class UploadIMGModel
    {     
        public string FileRealName { get; set; }     
        public string SmallFileVirtuPath { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string FileAllPath { get; set; }
    }

    public class IMGUploadHelper
    {

        // BitAuto.Utils.ImageTool
        public static bool MakeSmallImage(string srcFileFullName, string disFileFullName, int maxSideLength, ImageTool.LimitSideMode limitMode)
        {
            Image image = null;
            try
            {
                image = Image.FromFile(srcFileFullName);
            }
            catch
            {
                return false;
            }
            decimal d = 1m;
            switch (limitMode)
            {
                case ImageTool.LimitSideMode.Width:
                    if (image.Width <= maxSideLength)
                    {
                        File.Copy(srcFileFullName, disFileFullName);
                        if (image != null)
                        {
                            image.Dispose();
                        }
                        return true;
                    }
                    d = Convert.ToDecimal(maxSideLength) / image.Width;
                    break;
                case ImageTool.LimitSideMode.Height:
                    if (image.Height <= maxSideLength)
                    {
                        File.Copy(srcFileFullName, disFileFullName);
                        if (image != null)
                        {
                            image.Dispose();
                        }
                        return true;
                    }
                    d = Convert.ToDecimal(maxSideLength) / image.Height;
                    break;
                case ImageTool.LimitSideMode.Auto:
                    if (image.Width >= image.Height)
                    {
                        if (image.Width <= maxSideLength)
                        {
                            File.Copy(srcFileFullName, disFileFullName);
                            if (image != null)
                            {
                                image.Dispose();
                            }
                            return true;
                        }
                        d = Convert.ToDecimal(maxSideLength) / image.Width;
                    }
                    else
                    {
                        if (image.Height <= maxSideLength)
                        {
                            File.Copy(srcFileFullName, disFileFullName);
                            if (image != null)
                            {
                                image.Dispose();
                            }
                            return true;
                        }
                        d = Convert.ToDecimal(maxSideLength) / image.Height;
                    }
                    break;
            }
            Size smallSize = new Size(Convert.ToInt32(image.Width * d), Convert.ToInt32(image.Height * d));
            return MakeSmallImage(image, disFileFullName, smallSize, 75);
        }
        // BitAuto.Utils.ImageTool
        private static bool MakeSmallImage(Image srcImage, string disFileFullName, Size smallSize, int quality)
        {
            bool result = true;
            Bitmap bitmap = null;
            try
            {
                ImageFormat rawFormat = srcImage.RawFormat;
                bitmap = new Bitmap(srcImage, smallSize);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, smallSize.Width, smallSize.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
                EncoderParameters encoderParameters = new EncoderParameters();
                long[] value = new long[]
		{
			(long)quality
		};
                EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, value);
                encoderParameters.Param[0] = encoderParameter;
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo imageCodecInfo = null;
                for (int i = 0; i < imageEncoders.Length; i++)
                {
                    if (imageEncoders[i].FormatDescription.Equals("JPEG"))
                    {
                        imageCodecInfo = imageEncoders[i];
                        break;
                    }
                }
                if (imageCodecInfo != null)
                {
                    bitmap.Save(disFileFullName, imageCodecInfo, encoderParameters);
                }
                else
                {
                    bitmap.Save(disFileFullName, rawFormat);
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (srcImage != null)
                {
                    srcImage.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }
            return result;
        }


  



    }



 
}