using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.CTUtils.Log;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.Service.WeixinJSSDK
{
    /// <summary>
    /// 注释：WeixinJSSDKService
    /// 作者：lihf
    /// 日期：2018/5/16 10:56:56
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WeixinJSSDKService
    {
        #region 初始化
        private WeixinJSSDKService() { }
        private static readonly Lazy<WeixinJSSDKService> Linstance = new Lazy<WeixinJSSDKService>(() => new WeixinJSSDKService());
        public static WeixinJSSDKService Instance => Linstance.Value;
        #endregion

        #region 获取邀请图片的mediaid

        /// <summary>
        /// 获取邀请图片的mediaid
        /// </summary>
        /// <param name="reqDto"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public object GetInvitationMediaId(XYAuto.ChiTu2018.Service.WeixinJSSDK.Dto.GetInvitationMediaId.ReqDto reqDto,out string errorMsg)
        {
            errorMsg = string.Empty;
            var currentUserId = UserInfo.GetLoginUserID();

            var lewxUser = new LEWeiXinUserBO().GetModelByUserId(currentUserId);
            if (lewxUser == null)
            {
                errorMsg = "当前未登录或用户不存在";
                return null;
            }

            var userQC = CreateQrCode(reqDto.appId, currentUserId, Senparc.Weixin.MP.QrCode_ActionName.QR_SCENE);
            var invitationQR = GetActivityQRcode(userQC, currentUserId);
            return new Service.WeixinJSSDK.Dto.GetInvitationMediaId.ResDto()
            {
                MediaID = string.Empty,
                InvitationQR = invitationQR
            };
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="userId"></param>
        /// <param name="actionname">Senparc.Weixin.MP.QrCode_ActionName 枚举类</param>
        /// <returns></returns>
        public string CreateQrCode(string appId, int userId, Senparc.Weixin.MP.QrCode_ActionName actionname = Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE)
        {
            try
            {
                var json = new StringBuilder();
                var qrCodeResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(appId, 2592000, userId, actionname, json.ToString());
                return QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[CreateQrCode]报错", ex);
                return null;
            }
        }
        public string GetActivityQRcode(string QRurl, int userid)
        {
            return CombinImage(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ActivityQRcodeImg"), QRurl, Convert.ToInt32(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ActivityImgX")), Convert.ToInt32(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ActivityImgY")), userid); ;
        }
        public string CombinImage(string imgBack, string img, int x, int y, int userid)
        {
            string InvitationQR = string.Empty;
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgBack);
            System.Drawing.Image imgWarter = CutForCustom(ReturnImage(img), Convert.ToInt32(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
            string file = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + userid + "\\yq.jpg";
            if (!Directory.Exists(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + userid))
            {
                Directory.CreateDirectory(XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + userid);
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
                InvitationQR = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("LocalWebImage") + userid + "/yq.jpg";
                imgSrc.Dispose();
                imgWarter.Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[CombinImage]" + ex.ToString());
                imgSrc.Dispose();
                throw;
            }

            return InvitationQR;
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
        #endregion

        #region 分享动作保存日志接口，用户在微信里点击分享触发记录日志

        public object ShareLog(string url, out string errorMsg)
        {
            errorMsg = string.Empty;
            var currentUserId = -2;
            try
            {
                currentUserId = UserInfo.GetLoginUserID();
            }
            catch (Exception)
            {
                currentUserId = 1299;
            }
            var lewxUser = new LEWeiXinUserBO().GetModelByUserId(currentUserId);
            if (lewxUser == null)
            {
                errorMsg = "当前未登录或用户不存在";                
            }

            Log4NetHelper.Default().Info($"[ShareLog]UserID:{currentUserId},UserOpenID:{lewxUser?.openid},url:{url}");
            return null;
        }
        #endregion
    }
}
