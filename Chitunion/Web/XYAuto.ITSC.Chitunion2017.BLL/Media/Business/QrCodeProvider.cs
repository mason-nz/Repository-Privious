/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 10:33:50
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business
{
    public class QrCodeProviderConfig
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片logo物理路径 H:\桌面\截图\102.jpg
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 图片二维码保存物理路径
        /// </summary>
        public string SaveFileName { get; set; }

        public int Width { get; set; } = 430;

        public int Height { get; set; } = 430;

        public int Margin { get; set; } = 1;

        /// <summary>
        /// 扩展名 ImageFormat.Png
        /// </summary>
        public ImageFormat FileExt { get; set; } = ImageFormat.Png;
    }

    public class QrCodeProvider
    {
        private readonly QrCodeProviderConfig _config;

        public QrCodeProvider(QrCodeProviderConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        public void GenerateByFile()
        {
            //Logo 图片
            Bitmap logo = new Bitmap(_config.FileName);
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>
            {
                {EncodeHintType.CHARACTER_SET, "UTF-8"},
                {EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H}
            };

            //生成二维码
            BitMatrix bm = writer.encode(_config.Content, BarcodeFormat.QR_CODE, _config.Width, _config.Height, hint);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            Bitmap map = barcodeWriter.Write(bm);

            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            try
            {
                using (Graphics g = Graphics.FromImage(bmpimg))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(map, 0, 0);
                }
                //将二维码插入图片
                Graphics myGraphic = Graphics.FromImage(bmpimg);
                //白底
                myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
                myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);

                //保存成图片
                bmpimg.Save(_config.SaveFileName, _config.FileExt);
            }
            catch (Exception)
            {
                bmpimg.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 合并两张图片
        /// </summary>
        /// <param name="imgBack"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public string CombinImage(string imgBack, string img)
        {
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgBack);
            System.Drawing.Image imgWarter = System.Drawing.Image.FromFile(img);

            try
            {
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    g.DrawImage(imgWarter, new Rectangle(imgSrc.Width - imgWarter.Width,
                                                     imgSrc.Height - imgWarter.Height,
                                                     imgWarter.Width,
                                                     imgWarter.Height),
                            0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
                }
                //保存成图片
                imgSrc.Save(_config.SaveFileName, _config.FileExt);
            }
            catch (Exception)
            {
                imgSrc.Dispose();
                throw;
            }
            return _config.SaveFileName;
        }

        /// <summary>
        /// 合并两张图片
        /// </summary>
        /// <param name="imgBack"></param>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string CombinImage(string imgBack, string img, int x, int y)
        {
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgBack);
            System.Drawing.Image imgWarter = System.Drawing.Image.FromFile(img);

            try
            {
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    g.DrawImage(imgWarter, new Rectangle(x, y,
                                                     imgWarter.Width,
                                                     imgWarter.Height),
                            0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
                }
                //保存成图片
                imgSrc.Save(_config.SaveFileName, _config.FileExt);
            }
            catch (Exception)
            {
                imgSrc.Dispose();
                throw;
            }
            return _config.SaveFileName;
        }

        /// <summary>
        /// 将二维码放到图片中
        /// </summary>
        /// <param name="tempQrFilePath">会生成一个临时的二维码文件存储，然后贴片到图片中</param>
        /// <returns></returns>
        public string GenerateQrIntoImage(string tempQrFilePath, int x, int y)
        {
            var gen = new QrCodeProviderConfig()
            {
                Content = _config.Content,
                SaveFileName = tempQrFilePath,
                Width = _config.Width,
                Height = _config.Height
            };
            var provider = new QrCodeProvider(gen);

            provider.Generate();
            CombinImage(_config.FileName, gen.SaveFileName, x, y);
            return _config.SaveFileName;
        }

        /// <summary>
        /// 生成二维码,保存成图片
        /// </summary>
        public void Generate()
        {
            BarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
            QrCodeEncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = _config.Width,
                Height = _config.Height,
                Margin = _config.Margin
            };
            //设置内容编码
            //设置二维码的宽度和高度
            //设置二维码的边距,单位不是固定像素

            writer.Options = options;

            //DirectoryAdd(_config.SaveFileName);
            using (Bitmap map = writer.Write(_config.Content))
            {
                map.Save(_config.SaveFileName, _config.FileExt);
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        public static bool DirectoryAdd(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //新建文件夹
                return true;
            }
            return false;
        }
    }
}