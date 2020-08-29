/********************************************************
*创建人：hant
*创建时间：2018/1/22 18:26:16 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;
using System.Configuration;
using Senparc.Weixin.MP;
using XYAuto.Utils.Config;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class ImageOperate
    {


        public static readonly ImageOperate Instance = new ImageOperate();

        #region 二维码生成


        public string GetActivityQRcode(string QRurl, int width, int height, out string localimg)
        {
            string resultImg = string.Empty;
            localimg = string.Empty;
            try
            {
                bool count = true;
                string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", QRurl, width, height);
                string qr = GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
                ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qr);
                if (info.Status == 0)
                {
                    while (count)
                    {
                        if (RemoteFileExists(info.Result.CutImageUrl))
                        {
                            count = false;
                            resultImg = CombinImage(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ActivityQRcodeImg"), info.Result.CutImageUrl, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ActivityImgX")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ActivityImgY")), out localimg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[GetActivityQRcode]" + ex.ToString());
            }
            return resultImg;
        }

        public string GetActivityQRcode(string QRurl, int userid)
        {
            return CombinImage(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ActivityQRcodeImg"), QRurl, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ActivityImgX")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ActivityImgY")), userid); ;
        }

        public string GetFormatImg(string imgurl, string postdate)
        {
            return ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.PostWebRequest(imgurl, postdate);
        }

        private bool RemoteFileExists(string fileUrl)
        {
            bool result = false;

            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);

                response = req.GetResponse();

                result = response == null ? false : true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }

        #region 合并图片
        public string CombinImage(string imgBack, string img, int x, int y, out string localimg)
        {
            string InvitationQR = string.Empty;
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgBack);
            System.Drawing.Image imgWarter = ReturnImage(img);
            string file = System.Guid.NewGuid().ToString() + ".jpg";
            try
            {
                //Bitmap bt = new Bitmap(imgSrc);
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    g.DrawImage(imgWarter, new Rectangle(x, y,
                                                     imgWarter.Width,
                                                     imgWarter.Height),
                            0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
                    //ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    //EncoderParameters ep = new EncoderParameters();
                    //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                    //ImageCodecInfo ici = null;
                    //foreach (ImageCodecInfo codec in codecs)
                    //{
                    //    if (codec.MimeType == "image/jpeg")
                    //        ici = codec;
                    //}
                    //bt.Save(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file, ici, ep);
                    //bt.Dispose();
                }

                imgSrc.Save(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file, ImageFormat.Jpeg);
                string data = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalWebImage") + file;
                string qr = ShareOrderInfo.Instance.CreateGetHttpResponse(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CleanImg"), data);
                ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qr);
                InvitationQR = info.Result.ImageUrl;
                imgSrc.Dispose();
                imgWarter.Dispose();
                localimg = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file;
                //DeleteFile(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[CombinImage]" + ex.ToString());
                imgSrc.Dispose();
                throw;
            }

            return InvitationQR;
        }

        public string CombinImage(string imgBack, string img, int x, int y, int userid)
        {
            string InvitationQR = string.Empty;
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgBack);
            System.Drawing.Image imgWarter = CutForCustom(ReturnImage(img), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
            string file = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + userid + "\\yq.jpg";
            if (!Directory.Exists(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + userid))
            {
                Directory.CreateDirectory(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + userid);
            }
            try
            {
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    g.DrawImage(imgWarter, new Rectangle(x, y,
                                                     imgWarter.Width,
                                                     imgWarter.Height),
                            0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
                }

                imgSrc.Save(file, ImageFormat.Jpeg);
                InvitationQR = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalWebImage") + userid + "/yq.jpg";
                imgSrc.Dispose();
                imgWarter.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[CombinImage]" + ex.ToString());
                imgSrc.Dispose();
                throw;
            }

            return InvitationQR;
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        public Image ReturnImage(String filePath)
        {
            System.Net.WebRequest webreq = System.Net.WebRequest.Create(filePath);
            System.Net.WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            Image image;
            image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        private static Image ReturnImage(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            Image img = Image.FromStream(ms);
            return img;
        }
        #endregion



        public class ImgInfo
        {
            public int Status { get; set; }
            public Result Result { get; set; }
        }

        public class Result
        {
            public string CutImageUrl { get; set; }

            public string ImageUrl { get; set; }
        }

        #endregion

        #region 获取邀请图片的media_id
        public string GetMediaId(string appId, string file)
        {

            var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadTemporaryMedia(appId, UploadMediaFileType.image, file);
            return uploadResult.media_id;
        }

        public string SetImageFromImageToCdn(Image img)
        {
            string file = System.Guid.NewGuid().ToString() + ".jpg";
            string imgUrl = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg");
            if (!Directory.Exists(imgUrl))
            {
                Directory.CreateDirectory(imgUrl);
            }
            img.Save(imgUrl + "\\" + file, ImageFormat.Jpeg);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalWebImage") + file, img.Width, img.Height);
            Loger.Log4Net.Info("OriginHeadImageUrl参数：" + postdata);
            string qr = GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qr);
            DeleteFile(imgUrl + "\\" + file);
            return info.Result.CutImageUrl;
        }
        #endregion

        #region 本地化微信图片


        public string GetImgUrlByServerId(string serverId)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    MediaApi.Get(ConfigurationManager.AppSettings["WeixinAppId"], serverId, ms);
                    Image img = Bitmap.FromStream(ms, true);
                    return SetImageFromImageToCdn(img);
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"本地化微信图片serverId:{serverId} 失败GetImgUrlByServerId:" ,ex);
                return null;
            }
        }
        #endregion

        public string DownloadImage(string url)
        {
            string path = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg");
            if (url != null && url.Length > 0)
            {
                string filename = System.IO.Path.GetFileName(url);
                try
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        wc.Headers.Add("User-Agent", "Chrome");
                        wc.DownloadFile(url, path + "\\" + filename);
                        return path + "\\" + filename;
                    }
                }
                catch (Exception ex)
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[DownloadImage]报错", ex);
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public void DeleteFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                File.Delete(path);
            }
        }


        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="initImage">原图Image对象</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        /// <returns>返回裁剪后的Image对象</returns>
        public Image CutForCustom(Image initImage, int maxWidth, int maxHeight)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            //System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
            System.Drawing.Image templateImage;

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                //initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                return initImage;
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    //EncoderParameters ep = new EncoderParameters(1);
                    //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    //templateImage.Save(fileSaveUrl, ici, ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    //templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();

                }
            }

            //释放资源
            initImage.Dispose();
            return templateImage;
        }
    }
}
