using HtmlAgilityPack;
using log4net;
using log4net.Config;
/**
*----------Dragon be here!----------/
* 　　    ┏ ┓　.┏ ┓
* 　　┏━┛ ┻━━┛ ┻━━┓
* 　　┃　　    .　　  ┃ 
* 　　┃　 ┳┛　┗┳　 ┃
* 　　┃　　　　　　┃
* 　　┃　    ━┻━　　┃
* 　　┗━┓　　　  ┏━┛
* 　　    ┃  　　　┃
* 　　　 ┃  　　   ┗━━━━━━━━┓
* 　　　 ┃  　神兽保佑　  　  .┣┓
* 　　　 ┃　  永无BUG　　　 ┏┛
* 　　　 ┗┓┓┏━━━┳┓┏━━━━━━┛
* 　　　   ┗┻┛      ┗┻┛
*-------------------------------------/
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Topshelf;
using Xy.ImageFastDFS;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync
{
    class Program
    {
        private static Dictionary<String, ImageFormat> _imageFormats;

        public static Dictionary<String, ImageFormat> ImageFormats
        {
            get
            {
                return _imageFormats ?? (_imageFormats = GetImageFormats());
            }
        }

        private static Dictionary<String, ImageFormat> GetImageFormats()
        {
            var dic = new Dictionary<String, ImageFormat>();
            var properties = typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var format = property.GetValue(null, null) as ImageFormat;
                if (format == null) continue;
                dic.Add(("." + (property.Name.ToLower() == "jpeg" ? "jpg" : property.Name.ToLower())).ToLower(), format);
            }
            return dic;
        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));

            HostFactory.Run(x =>
            {
                x.Service<SyncService>(s =>
                {
                    s.ConstructUsing(name => new SyncService());
                    s.WhenStarted(tc =>
                    {
                        tc.Start();
                    });
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("微信公众号文章明细数据（用于统计榜单）");
                x.SetDisplayName("HBaseDataSync2017");
                x.SetServiceName("HBaseDataSync");
            });

            //string imgurl = "http://pic4.nipic.com/20091217/3885730_124701000519_2.jpg";
            //string newimgUrl = Util.Instance.GetUrlByCutForCustom(imgurl);
            //Console.Write(newimgUrl);
            #region 测试文章长度
            //string dd = "<blockquote style=\"padding: 15px; border - width: 1px; border - top - style: solid; border - right - style: solid; border - bottom - style: solid; border - color: rgb(221, 219, 217); white - space: normal; text - size - adjust: auto; color: rgb(51, 51, 51); font - family: ΢���ź�; font - size: 12px; border - radius: 5px; \"><h2><span style=\"color: rgb(0, 0, 0); background - color: rgb(242, 242, 242); \">阅读本文前，请您先点击上面的蓝色字体<strong><span style=\"font - size: 20px; background - color: rgb(255, 251, 0); \">“前端祝福”</span></strong><span style=\"font - size: 20px; \"></span>，再点击<strong><span style=\"font - size: 18px; \">“关注”</span></strong>，这样您就可以继续免费收到文章了。每天都有分享。完全是免费订阅，请放心关注。</span></h2></blockquote><p style=\"white - space: normal; max - width: 100 %; min - height: 1em; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; font - size: 15.5556px; line - height: 27.6543px; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; color: rgb(31, 73, 125); font - size: 15.5556px; line - height: 28.4444px; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; color: rgb(62, 62, 62); box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; font - size: 15.5556px; line - height: 27.6543px; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; color: rgb(31, 73, 125); font - size: 15.5556px; line - height: 28.4444px; box - sizing: border - box !important; word - wrap: break-word !important; \"><span style=\"max - width: 100 %; color: rgb(62, 62, 62); font - family: Simsun; line - height: 25px; font - size: 20px; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; color: rgb(247, 150, 70); font - family: sans - serif; font - size: 16px; box - sizing: border - box !important; word - wrap: break-word !important; \"><span style=\"max - width: 100 %; color: rgb(127, 127, 127); line - height: 16px; font - family: 宋体; font - size: 12px; background - color: rgb(248, 247, 245); box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"color: rgb(62, 62, 62); text - align: center; white - space: normal; widows: 1; font - family: 微软雅黑; font - size: 17.1429px; max - width: 100 %; box - sizing: border - box !important; word - wrap: break-word !important; \"><span style=\"max - width: 100 %; font - size: 20px; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; line - height: 2em; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; color: rgb(255, 0, 0); \"><strong style=\"max - width: 100 %; color: rgb(51, 51, 51); line - height: 21px; font - family: ΢���ź�; font - size: 12px; box - sizing: border - box !important; word - wrap: break-word !important; \"><span style=\"max - width: 100 %; color: rgb(127, 127, 127); line - height: 16px; font - family: 宋体; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"max - width: 100 %; color: rgb(255, 0, 0); \"><span style=\"max - width: 100 %; color: rgb(51, 51, 51); line - height: 21px; font - family: ΢���ź�; font - size: 12px; box - sizing: border - box !important; word - wrap: break-word !important; \"><strong style=\"white - space: normal; text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \"><span style=\"color: rgb(127, 127, 127); line - height: 16px; font - family: 宋体; font - size: 12px; \">提醒：请在WIFI下观看，土豪随意！</span></strong><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span><span style=\"text - size - adjust: auto; line - height: 25.6px; font - family: 微软雅黑; \">﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿</span></span></strong></span></strong></strong></strong></span></strong></span></strong></span></strong></strong></strong></strong></strong></strong></strong></strong></p><p style=\"white - space: normal; \"><p><strong style=\"line - height: 1.5em; \"><strong style=\"line - height: 25.6px; \"><strong style=\"font - size: 17.1429px; line - height: 25.6px; white - space: pre - wrap; color: rgb(255, 41, 65); \"><strong style=\"line - height: 25.6px; font - size: 17.1429px; \"><strong style=\"color: rgb(62, 62, 62); font - size: 16px; line - height: 2em; \"><strong style=\"white - space: normal; line - height: 1.5em; \"><strong style=\"font - size: 12px; line - height: 25.6px; text - indent: 2em; color: rgb(192, 0, 0); \"><strong style=\"line - height: 25.6px; \"><strong style=\"color: rgb(170, 0, 0); line - height: 25.6px; \"><strong style=\"line - height: 25.6px; color: rgb(192, 0, 0); \"><strong style=\"line - height: 25.6px; \"><strong style=\"color: rgb(170, 0, 0); line - height: 25.6px; \"><strong style=\"line - height: 25.6px; widows: 1; \"><strong style=\"line - height: 1.5em; \"><strong style=\"line - height: 25.6px; text - align: center; \"><strong style=\"font - size: 17.1429px; line - height: 25.6px; font - family: 微软雅黑; white - space: pre - wrap; color: rgb(255, 41, 65); \"><strong style=\"line - height: 25.6px; font - size: 17.1429px; \"><strong style=\"color: rgb(62, 62, 62); font - size: 16px; line - height: 2em; \"><strong style=\"white - space: normal; line - height: 1.5em; \"><strong style=\"font - size: 12px; line - height: 25.6px; text - indent: 2em; color: rgb(192, 0, 0); \"><strong style=\"line - height: 25.6px; \"><strong style=\"color: rgb(170, 0, 0); line - height: 25.6px; \"><strong style=\"line - height: 25.6px; color: rgb(192, 0, 0); \"><strong style=\"line - height: 25.6px; \"><strong style=\"color: rgb(170, 0, 0); line - height: 25.6px; \"><strong style=\"line - height: 25.6px; \"><strong style=\"line - height: 1.5em; \"><strong style=\"line - height: 25.6px; \"><span style=\"font - weight: 400; line - height: 25.6px; background - color: rgba(255, 255, 255, 0); \"><strong style=\"font - size: 17.1429px; line - height: 25.6px; white - space: pre - wrap; color: rgb(255, 41, 65); \"><strong style=\"line - height: 25.6px; font - size: 17.1429px; \"><strong style=\"color: rgb(62, 62, 62); font - size: 16px; line - height: 2em; \"><span style=\"color: rgb(178, 178, 178); font - size: 12px; line - height: 25.6px; \"><strong style=\"color: rgb(62, 62, 62); white - space: normal; line - height: 1.5em; \"><span style=\"font - size: medium; font - weight: 400; background - color: rgba(255, 255, 255, 0); \"><strong style=\"font - size: 12px; line - height: 25.6px; text - indent: 2em; color: rgb(192, 0, 0); \"><span style=\"margin - right: auto; margin - left: auto; width: auto; \"><strong style=\"line - height: 25.6px; \"><strong style=\"color: rgb(170, 0, 0); line - height: 25.6px; \"><strong style=\"line - height: 25.6px; color: rgb(192, 0, 0); \"><span style=\"margin - right: auto; margin - left: auto; width: auto; \"><strong style=\"line - height: 25.6px; \"><strong style=\"color: rgb(170, 0, 0); line - height: 25.6px; \"></strong></strong></span></strong></strong></strong></span></strong></span></strong></span></strong></strong></strong></span></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></strong></p><h1 style=\"font - size: 17.1429px; white - space: normal; line - height: 25.6px; font - family: Arial; \"><p style=\"margin - top: 10px; font - size: 22px; color: rgb(213, 25, 56); line - height: 2em; text - align: center; letter - spacing: 3px; text - shadow: rgb(175, 174, 174) 4px 3px 3px; \"><span style=\"font - size: 20px; \"><strong>热门文章推荐</strong></span></p><p style=\"margin - top: 10px; font - size: 16px; height: 1.5em; overflow: hidden; line - height: 1.5em; \"><span style=\"font - size: 18px; \"></span></p></h1><p><br></p>";
            //var doc = new HtmlDocument();
            //doc.LoadHtml(dd.Trim());

            //if (doc != null && BLL.Util.GetLength(doc.DocumentNode.InnerText) < 800)
            //{
            //    BLL.Loger.Log4Net.Info($"文章内容文字长度小于{800}，已剔除");
            //}

            #endregion


            #region 测试指定图片中，是否存在二维码
            //string imgUrl = "https://mmbiz.qpic.cn/mmbiz_png/wSxnWiaNBFsQwmaODs09oQkXCEJQlLAd1gViciav0uCfKQW5p5Vkt02gszEItaPLF2NVw3UHfLJGXrvibptnPBeOqQ/640?wx_fmt=png";
            //Image img = Util.Instance.GetImageByURL(imgUrl);
            //if (img!=null)
            //{
            //    string content = Util.Instance.ReadQrCodeText(img);
            //    Console.Write("图片二维码内容为："+ content);
            //}
            //else
            //{
            //    Console.Write("图片没有二维码");
            //}
            #endregion

            #region 清洗历史头图信息
            //int topNum = 1000;
            //int start = 0;

            //while (true)
            //{
            //    DataTable dt = DAL.ArticleInfo.Instance.GetHistoryHeadImgData(start, topNum);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        int index = 0;
            //        BLL.Loger.Log4Net.Info($"总共查询出{dt.Rows.Count}条记录：");
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            index++;
            //            int recid = int.Parse(dr["RecID"].ToString());
            //            string HeadImg = dr["HeadImg"].ToString();
            //            BLL.Loger.Log4Net.Info($"当前获取到第{index}条记录：RecID={recid}，HeadImg={HeadImg}");
            //            string HeadImgNew = Util.Instance.GetUrlByCutForCustom(HeadImg, HeadImg);
            //            if (!string.IsNullOrEmpty(HeadImgNew))
            //            {
            //                int result = DAL.ArticleInfo.Instance.UpdateHeadImgNewByRecID(recid, HeadImgNew);
            //                BLL.Loger.Log4Net.Info($"当前获取到第{index}条记录：RecID={recid}，HeadImg={HeadImg}，HeadImg={HeadImgNew}，更新结果为：{result}");
            //            }
            //            else
            //            {
            //                BLL.Loger.Log4Net.Info($"当前获取到第{index}条记录：RecID={recid}，HeadImg={HeadImg},裁剪后图片为空");
            //            }
            //        }
            //        start = int.Parse(dt.Rows[dt.Rows.Count - 1]["RecID"].ToString());
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            #endregion

            #region 剔除关键词统计信息
            //int start = 0;
            //int topNum = 1000;
            //int index = 0;
            //Dictionary<string, Dictionary<string, int>> list = new Dictionary<string, Dictionary<string, int>>();

            //string keywords = "教师节 国庆 重阳 十一 元旦 圣诞 立秋 处暑 白露 秋分 寒露 霜降 立冬 9月 10月 11月 最后一天 秋季 夏季 2017 年末 尾声 年终 长假 演员的诞生 小雪 致我们单纯的小美好 猎场 嘉年华 寻梦环游记 芳华 大雪 平安夜 澳门回归 江辰 胡一天 妖猫传 支付宝账单 东方快车谋杀案 天才枪手 中国有嘻哈";
            //string[] strKeywords = keywords.Split(' ');
            //foreach (string keyword in strKeywords)
            //{
            //    Dictionary<string, int> result = new Dictionary<string, int>();
            //    result.Add("total", 0);
            //    result.Add("头部", 0);
            //    result.Add("腰部", 0);
            //    result.Add("Title", 0);
            //    result.Add("Content", 0);
            //    list.Add(keyword, result);
            //}
            //while (true)
            //{
            //    DataTable dt = DAL.ArticleInfo.Instance.GetKeyWordsData(start, topNum);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            //int recid = int.Parse(dr["RecID"].ToString());
            //            string title = dr["Title"].ToString();
            //            string content = dr["Content"].ToString();
            //            int xyAttr = int.Parse(dr["XyAttr"].ToString());
            //            test_StatKeyword(list, title, content, xyAttr, strKeywords);
            //        }

            //        start = int.Parse(dt.Rows[dt.Rows.Count - 1]["RecID"].ToString());
            //        index++;
            //        BLL.Loger.Log4Net.Info($"当前获取到第{index * 1000}条记录：");
            //        BLL.Loger.Log4Net.Info(list);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //foreach (var item in list)
            //{
            //    BLL.Loger.Log4Net.Info(item.Key);
            //    BLL.Loger.Log4Net.Info(item.Value);
            //}

            #endregion

            #region 测试文档中，替换标签属性【style】中存的url内容
            //string dd = "<p>asdfasd</p><p>asdfasd</p><p>asdfasd</p><p>asdfasd</p><p>asdfasd</p><section masj=\"222\" style=\"font - size:16px; \" label=\"Powered by 135editor.com\" data-role=\"outer\"><section style=\"border - width: 0px; border - style: none; border - color: initial; box - sizing: border - box; \" data-id=\"90141\" data-tools=\"135编辑器\" class=\"_135editor\"><section style=\"border - width: 0px; border - style: initial; border - color: initial; margin - right: auto; margin - left: auto; text - align: center; box - sizing: border - box; \"><section masj=\"222\" style=\"display: inline - block; margin - right: auto; margin - left: auto; width: 30px; height: 24px; line - height: 27px; text - align: left; color: rgb(255, 255, 255); background - image: url(&quot; http://mmbiz.qpic.cn/mmbiz_png/CtIibiaZ2HWWNUicUkYNufR9oxia6HFj91S8JtF0Pa2WiaLwO5jesP4bib8Wl6yuV3z6xmsv59pmuXC3ZAettSSQXfzg/0?wx_fmt=png&quot;);background-size: auto 24px;background-repeat: no-repeat;\"><p style=\"border-width: 0px;border-style: initial;border-color: initial;margin-left: 7px;\">0<span title=\"\" data-original-title=\"\" class=\"autonum\">1</span></p></section></section></section></section>";
            //var doc = new HtmlDocument();
            //doc.LoadHtml(dd.Trim());

            //IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//*[contains(@style,\"url\")]");
            //foreach (HtmlNode item in list)
            //{
            //    string val = item.Attributes["style"].Value;
            //    Regex r = new Regex(@"((ht|f)tps?):\/\/[\w\-]+(\.[\w\-]+)+([\w\-\.,@?^=%&:\/~\+#]*[\w\-\@?^=%&\/~\+#])?");
            //    MatchCollection matchs = r.Matches(val);
            //    for (int i = 0; i < matchs.Count; i++)
            //    {
            //        string url = matchs[i].Value;
            //        if (url.EndsWith("&quot"))
            //        {
            //            url = url.Substring(0, url.Length - 5);
            //        }
            //        val = val.Replace(url, "http://www.baidi.com/");
            //    }
            //    item.Attributes["style"].Value = val;
            //}
            //StringBuilder sb = new StringBuilder();
            //using (var writer = new StringWriter(sb))
            //{
            //    doc.Save(writer);
            //}
            //Console.Write(sb.ToString());
            #endregion

            #region 测试文章中，出现span里面有“小程序”字样的清洗规则
            //string dd = "<div><ul>xx</ul></div><div><p style=\" margin - top: 0px; margin - bottom: 0px; padding: 0px; max - width: 100 %; color: rgb(62, 62, 62) \" helvetica=\"helvetica\" neue=\"neue\" hiragino=\"hiragino\" sans=\"sans\" gb=\"gb\" microsoft=\"microsoft\" yahei=\"yahei\" arial=\"arial\" sans-serif=\"sans - serif\" white-space:=\"white - space:\" normal=\"normal\" text-align:=\"text - align:\" center=\"center\" box-sizing:=\"box - sizing:\" border-box=\"border - box\" important=\"important\" word-wrap:=\"word - wrap:\" break-word=\"break-word\"><mp-miniprogram class=\"\" data-miniprogram-appid=\"wxd09bf0b416c75266\" data-miniprogram-path=\"pages / productDetail / productDetail ? vendor = jieya & amp; id = 70\" data-miniprogram-nickname=\"优选生活购\" data-miniprogram-avatar=\"http://ugc.apphao123.com/pt/images/201712/f4b2646ade185160f2aaf1ca19d3071c.jpg\" data-miniprogram-title=\"百龄洁牙粉-畅销台湾60年品牌\" data-miniprogram-imageurl=\"http://mmbiz.qpic.cn/mmbiz_gif/QISv5kiaOHCLq0ic0TqOXvnUav3sI38goMQFqpZO865k1fBw4iaP2fhXmjDnUAeWWJvtgzmSLusUCCTSBYjH8VDeA/0\" style=\"margin: 0px; padding: 0px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important;\"></mp-miniprogram><span class=\"weapp_display_element js_weapp_display_element\"><!-- <span class=\"weapp_card flex_context\">     <span class=\"weapp_card_hd\">         <span class=\"radius_avatar weapp_card_avatar\">             <img src=\"http://mmbiz.qpic.cn/mmbiz_png/Wnd5VexYCMQVEd0QnSNkEibWhCogao8we8Yzg0dJMvb8d2d0wQlo8lMubD1gicGaHiaNsLnG1EDT8ZnY8E9zEia2Hg/0?wx_fmt=png\">         </span>     </span>     <span class=\"weapp_card_bd flex_bd\">         <strong class=\"weapp_card_nickname\">优选生活购</strong>         <span class=\"weapp_card_logo\"><img class=\"icon_weapp_logo_mini\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABwAAAAcCAMAAABF0y+mAAAAb1BMVEUAAAB4it11h9x2h9x2h9x2htx8j+R8i+B1h9x2h9x3h92Snv91htt2h9x1h9x4h9x1h9x1h9x2idx1h9t2h9t1htt1h9x1h9x1htx2h9x1h912h9x4h913iN17juOOjuN1iNx2h9t4h958i+B1htvejBiPAAAAJHRSTlMALPLcxKcVEOXXUgXtspU498sx69DPu5+Yc2JeRDwbCYuIRiGBtoolAAAA3ElEQVQoz62S1xKDIBBFWYiFYImm2DWF///G7DJEROOb58U79zi4O8iOo8zuCRfV8EdFgbYE49qFQs8ksJInajOA1wWfYvLcGSueU/oUGBtPpti09uNS68KTMcrQ5jce4kmN/HKn9XVPAo702JEdx9hTUrWUqVrI3KwUmM1NhIWMKdwiGvpGMWZOAj1PZuzAxHwhVSplrajoseBnbyDHAwvrtvKKhdqTtFBkL8wO5ijcsS3G1JMNvQ5mdW7fc0x0+ZcnlJlZiflAomdEyFaM7qeK2JahEjy5ZyU7jC/q/Rz/DgqEuAAAAABJRU5ErkJggg==\" alt=\"\">小程序</span>     </span> </span> --> <span class=\"weapp_card app_context appmsg_card_context\">     <span class=\"weapp_card_bd\">         <span class=\"weapp_card_profile flex_context\">             <span class=\"radius_avatar weapp_card_avatar\">                 <img src=\"http://mmbiz.qpic.cn/mmbiz_png/Wnd5VexYCMQVEd0QnSNkEibWhCogao8we8Yzg0dJMvb8d2d0wQlo8lMubD1gicGaHiaNsLnG1EDT8ZnY8E9zEia2Hg/0?wx_fmt=png\">             </span>             <span class=\"weapp_card_nickname flex_bd\">优选生活购</span>         </span>         <span class=\"weapp_card_info\">             <span class=\"weapp_card_title\">百龄洁牙粉-畅销台湾60年品牌</span>             <span class=\"weapp_card_thumb_wrp\" style=\"background-image:url(http://mmbiz.qpic.cn/mmbiz_gif/QISv5kiaOHCLq0ic0TqOXvnUav3sI38goMQFqpZO865k1fBw4iaP2fhXmjDnUAeWWJvtgzmSLusUCCTSBYjH8VDeA/0);\"></span>         </span>     </span>     <span class=\"weapp_card_ft\">         <span class=\"weapp_card_logo\"><img class=\"icon_weapp_logo_mini\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABwAAAAcCAMAAABF0y+mAAAAb1BMVEUAAAB4it11h9x2h9x2h9x2htx8j+R8i+B1h9x2h9x3h92Snv91htt2h9x1h9x4h9x1h9x1h9x2idx1h9t2h9t1htt1h9x1h9x1htx2h9x1h912h9x4h913iN17juOOjuN1iNx2h9t4h958i+B1htvejBiPAAAAJHRSTlMALPLcxKcVEOXXUgXtspU498sx69DPu5+Yc2JeRDwbCYuIRiGBtoolAAAA3ElEQVQoz62S1xKDIBBFWYiFYImm2DWF///G7DJEROOb58U79zi4O8iOo8zuCRfV8EdFgbYE49qFQs8ksJInajOA1wWfYvLcGSueU/oUGBtPpti09uNS68KTMcrQ5jce4kmN/HKn9XVPAo702JEdx9hTUrWUqVrI3KwUmM1NhIWMKdwiGvpGMWZOAj1PZuzAxHwhVSplrajoseBnbyDHAwvrtvKKhdqTtFBkL8wO5ijcsS3G1JMNvQ5mdW7fc0x0+ZcnlJlZiflAomdEyFaM7qeK2JahEjy5ZyU7jC/q/Rz/DgqEuAAAAABJRU5ErkJggg==\" alt=\"\">小程序</span>     </span> </span> </span><span class=\"\" style=\"margin: 1em 0px; padding: 0px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; display: block;\"><span class=\"\" style=\" margin: 0px; padding: 10px 0px 0px; max-width: 100%; border: 1px solid #E1E1E1; background-color: #FDFDFD; line-height: 1.6; text-align: left;  border-radius: 3px; overflow: hidden; display: block; box-sizing: border-box !important; word-wrap: break-word !important; \"><span class=\"\" style=\"margin: 0px; padding: 0px 15px 15px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; display: block;\"><span class=\"\" style=\"margin: 0px; padding: 0px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; display: flex; -webkit-box-align: center; align-items: center; font-size: 12px; color: #8C8C8C;\"><span class=\"\" style=\"margin: -0.2em 5px 0px 0px; padding: 0px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; display: inline-block; background-color: #FFFFFF; border-radius: 50%; overflow: hidden; vertical-align: middle; width: 20px; height: 20px;\"><img data-src=\"https://mmbiz.qpic.cn/mmbiz_png/Wnd5VexYCMQVEd0QnSNkEibWhCogao8we8Yzg0dJMvb8d2d0wQlo8lMubD1gicGaHiaNsLnG1EDT8ZnY8E9zEia2Hg/640?wx_fmt=png\" style=\"margin: 0px; padding: 0px; display: block; width: auto !important; border-radius: 50%; background-color: rgb(238, 238, 238); box-sizing: border-box !important; word-wrap: break-word !important; visibility: visible !important; height: auto !important;\" src=\"https://mmbiz.qpic.cn/mmbiz_png/Wnd5VexYCMQVEd0QnSNkEibWhCogao8we8Yzg0dJMvb8d2d0wQlo8lMubD1gicGaHiaNsLnG1EDT8ZnY8E9zEia2Hg/640?wx_fmt=png&amp;tp=webp&amp;wxfrom=5&amp;wx_lazy=1\" class=\"\" data-fail=\"0\"></span><span class=\"\" style=\"margin: 0px; padding: 0px; max-width: 100%; word-wrap: break-word; -webkit-box-flex: 1; flex: 1 1 0%; word-break: break-all; display: -webkit-box; overflow: hidden; text-overflow: ellipsis; -webkit-box-orient: vertical; -webkit-line-clamp: 2; box-sizing: border-box !important;\">优选生活购</span></span><span class=\"\" style=\"margin: 0px; padding: 0px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; display: block;\"><span class=\"\" style=\"margin: 0px; padding: 0.3em 0px 0.75em; max-width: 100%; display: -webkit-box; overflow: hidden; text-overflow: ellipsis; -webkit-box-orient: vertical; -webkit-line-clamp: 2; box-sizing: border-box !important; word-wrap: break-word !important;\">百龄洁牙粉-畅销台湾60年品牌</span><span =\"\"=\"\" class=\"\" http=\"\" style=\"margin: 0px; padding: 0px 0px 510.391px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important;  display: block; overflow: hidden; background-repeat: no-repeat; background-position: center center; background-size: cover; background-image: url(&quot;&quot;)\"></span></span></span><span class=\"\" style=\"margin: 0px; padding: 0px 15px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; display: block; border-top: 1px solid #E1E1E1; line-height: 1.56em;\"><span class=\"\" style=\"margin: 0px; padding: 0px; max-width: 100%; box-sizing: border-box !important; word-wrap: break-word !important; color: #8C8C8C; font-size: 13px;\">小程序</span></span></span></span></p></div>";
            //var doc = new HtmlDocument();
            //doc.LoadHtml(dd.Trim());
            //IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//span[text()='小程序']");
            //if (list != null && list.Count() > 0)
            //{
            //    int count = 0;
            //    for (int j = 0; j < list.Count(); j++)
            //    {
            //        //string text = list.ToList()[j].InnerText;
            //        //if (Regex.IsMatch(text, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?") ||
            //        //    Regex.IsMatch(text, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)"))
            //        RemoveSpanTagByNode(list.ToList()[j]);
            //        count++;
            //    }
            //    if (count > 0)
            //    {
            //        BLL.Loger.Log4Net.Info($"文章ID为,在所有标签中，查找有正则匹配URL或Email的，共有{count}个，已全部剔除父级P标签");
            //    }
            //}
            //StringBuilder sb = new StringBuilder();
            //using (var writer = new StringWriter(sb))
            //{
            //    doc.Save(writer);
            //}
            //string result = sb.ToString();
            //Console.Write(result);
            #endregion

            //    HtmlNode newSpan = HtmlNode.CreateNode("<span></span>");
            //    foreach (var attrSpan in item.Attributes)
            //    {
            //        newSpan.SetAttributeValue(attrSpan.Name, attrSpan.Value);
            //    }
            //    //list.ToArray()[j].ParentNode
            //    item.ParentNode.ReplaceChild(newSpan, item);

            //}
            //StringBuilder sb = new StringBuilder();
            //using (var writer = new StringWriter(sb))
            //{
            //    doc.Save(writer);
            //}
            //string result = sb.ToString();

            ////string dd = "<qqmusic src=\" / cgi - bin / readtemplate ? t = tmpl / qqmusic_tmpl & amp; singer =% E5 % AD % 99 % E7 % 87 % 95 % E5 % A7 % BF % 20 -% 20 % E5 % AD % 99 % E7 % 87 % 95 % E5 % A7 % BF % 20 % E5 % 90 % 8C % E5 % 90 % 8D % E4 % B8 % 93 % E8 % BE % 91 & amp; music_name =% E5 % A4 % A9 % E9 % BB % 91 % E9 % BB % 91\" play_length=\"237000\" singer=\"孙燕姿 & nbsp; -&nbsp; 孙燕姿 & nbsp; 同名专辑\" music_name=\"天黑黑\" audiourl=\"http://ws.stream.qqmusic.qq.com/C100000CbUE00I998M.m4a?fromtag=46\" albumurl=\"/0/S/002UZ9ob4Ecg0S.jpg\" mid=\"000CbUE00I998M\" musicid=\"5211222\" frameborder=\"0\" scrolling=\"no\" class=\"res_iframe qqmusic_iframe js_editor_qqmusic\"></qqmusic>";
            ////var doc = new HtmlDocument();
            ////doc.LoadHtml(dd.Trim());
            ////IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//mpvoice|//qqmusic");
            ////if (list != null && list.Count() > 0)
            ////{
            ////    BLL.Loger.Log4Net.Info($"共有{list.Count()}个音频标签");
            ////    //return true;
            ////}
            //string content = "<p style=\"text - align: center; \"><span style=\"color: rgb(136, 136, 136); \"></span></p><p><img data-s=\"300,640\" data-type=\"jpeg\" data-src=\"https://mmbiz.qpic.cn/mmbiz_jpg/vDZwnvF2t2jOJXMPVXrAwmEFu4PsDckwCXiaodF8Jtjp4PoAKce08DvVEFv5Es6UW8yXcEMObPWhysxib8GZjzzQ/0?wx_fmt=jpeg\" data-copyright=\"0\" style=\"\" class=\"\" data-ratio=\"0.6103225806451613\" data-w=\"775\" src=\"http://img4.qcdqcdn.com/group4/M00/95/74/QQ0DAFoeXGKAAds1AAEaSzrQ9mc951.jpg\"></p><p style=\"text-align: center;\"><span style=\"color: rgb(136, 136, 136);\">澳大利亚是国土面积上的大国，世界战略中的小国。</span><br></p><p><br></p><p>英国《卫报》11月26日文章，原题：美国还是中国？澳大利亚以为不必二选一，实乃自欺欺人 特恩布尔在新加坡某大型国防会议上提醒注意中国欲成为地区头号强国的野心，呼吁美国及其亚洲盟友阻止中国。这是澳大利亚总理首次坦承中美间的战略竞争。但特恩布尔表达了巨大信心，认为美国会胜过中国。</p><p>　　</p><p>长期以来，堪培拉拒绝承认我们的主要盟友与我们的主要贸易伙伴之间在进行一场战略大竞争。诚然，澳不想在美中之间选择。这样就可继续依靠中国使我们富裕、依靠美国使我们安全。至今澳尚未被迫做出与一方为伍同时放弃另一方的孤注一掷的选择，但若美中竞争进一步升级，这一天迟早会到来。堪培拉和华盛顿犯了同样的错误：低估中国的实力并高估美国的实力。</p><p>　　</p><p>2011年以来，澳历届政府对美国在亚洲的领导地位予以言语支持，但拒绝做能被明确视为针对中国的实质性事情。我们一直说服华盛顿相信我们支持其对付中国，同时说服北京相信我们不会那样做。这是一种系统性的两面派政策。有人可能说此类政策不可避免。可问题是，它并没有奏效——我们或许只是自欺欺人。</p><p><br></p><p>北京肯定没上当，但他们也没不悦。他们并没期待我们支持中国反对美国，只是希望我们不要支持美国反对中国——即让澳成为中立者。这对他们也是一大胜利。所以，他们容忍澳口头支持同盟。迄今，他们得到自己想要的，所以澳未受惩罚。华盛顿显然明白这个道理，对我们不愿惹恼北京明显失望。同时，特朗普削弱了堪培拉的信心——对于美国在亚洲的未来以及华盛顿对澳盟友的重视的信心。</p><p>　　</p><p>堪培拉的一些人试图扭转局面，决定鼓励特朗普对抗中国，以便美国在此过程中将澳大利亚视为有价值的盟友。我们似乎仍抱着一种观念，认为美国仍将是亚洲主导，会帮澳对付中国，而中国会心悦诚服地接受这一切。澳政府又一次未能认识到国际环境的深刻转变。这种一厢情愿的政策，进一步暴露困扰澳多年的政治、政策系统性失败。</p>";
            //string url = "";
            //var result = WangyiDun.TextCheckApi.Instance.Check("1", RemoveHtmlTag(content), url);
            //Console.Write(result.msg);

            //string url = "http://mmbiz.qpic.cn/mmbiz_gif/bPJzOqwPBpblNo7F8TCia7t8WHArcN8XwrOQhLfiaxVpmY5hF8CziavV1MG1a8uszocI1A1SwpJ3tWXJM7KyPO4IA/0";
            //string url = "http://pic4.nipic.com/20091217/3885730_124701000519_2.jpg";
            //Image image = GetImageByURL(url);
            //Console.WriteLine(PhotoImageInsert(image).Length);


            #region 测试一段时间微信文章中图片的大小
            //int recid = 0;
            //long len = 0;
            //while (true)
            //{
            //    DataTable dt = DAL.ArticleInfo.Instance.GetTestData(recid);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            int nowRecID = int.Parse(dr["RecID"].ToString());
            //            string HeadImg = dr["HeadImg"].ToString();
            //            string Content = dr["Content"].ToString();
            //            if (HeadImg.Contains(".qcdqcdn.com"))
            //            {
            //                Image temp1 = GetImageByURL(HeadImg);
            //                if (temp1 != null)
            //                {
            //                    len += PhotoImageInsert(temp1).Length;
            //                    BLL.Loger.Log4Net.Info($"当前文章ID：{nowRecID}，图片URL：{HeadImg}，累计大小：{len}字节");
            //                }
            //            }

            //            var doc = new HtmlDocument();
            //            doc.LoadHtml(Content);
            //            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//img[@src]");
            //            if (list != null)
            //            {
            //                for (int j = 0; j < list.Count(); j++)
            //                {
            //                    string imgUrl = string.Empty;
            //                    if (list.ToList()[j].Attributes.Contains("src"))
            //                    {
            //                        imgUrl = list.ToList()[j].Attributes["src"].Value;
            //                    }
            //                    if (imgUrl.StartsWith("//"))
            //                    {
            //                        imgUrl = "http:" + imgUrl;
            //                    }
            //                    if (imgUrl.Contains(".qcdqcdn.com"))
            //                    {
            //                        Image temp2 = GetImageByURL(imgUrl);
            //                        if (temp2 != null)
            //                        {
            //                            len += PhotoImageInsert(temp2).Length;
            //                            BLL.Loger.Log4Net.Info($"当前文章ID：{nowRecID}，图片URL：{imgUrl}，累计大小：{len}字节");
            //                        }
            //                    }
            //                }
            //            }
            //            recid = nowRecID;
            //        }
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //BLL.Loger.Log4Net.Info($"统计结束，总计大小：{len}字节");
            #endregion


            //string path = System.AppDomain.CurrentDomain.BaseDirectory + "test.jpg";
            ////////////img.Save(path);

            //////////string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\test.jpg");
            //IFastDFSAdapter _iFastDFSAdapter = new StreamFastDFSAdapter(File.ReadAllBytes(path), "jpg");
            //string imageUrl = "http://192.168.3.71/" + _iFastDFSAdapter.Upload();

            //////string imageUrl = CleanImg(image, url);

            //Console.Write(imageUrl);

            //IFastDFSAdapter _iFastDFSAdapter2 = new StreamFastDFSAdapter();
            //////string imageUrl = "group4/M00/2D/2F/QQ0DAFl7S6OARoATAABStBI_uDI696.jpg";
            //bool flag = _iFastDFSAdapter2.Delete();

            ////Uri imgUrl = new Uri(imageUrl);
            ////string delPath = imgUrl.AbsolutePath + imgUrl.Query + imgUrl.Fragment;
            ////if (delPath.StartsWith("group1/"))
            ////{
            ////    delPath = delPath.Replace("group1/", string.Empty);

            ////}


            //Console.Write(flag);
        }

        //private static void RemoveSpanTagByNode(HtmlNode currentNode)
        //{
        //    if (currentNode != null)
        //    {
        //        if (currentNode.ParentNode != null &&
        //            currentNode.ParentNode.Name.ToLower().Trim() == "span")
        //        {
        //            RemoveSpanTagByNode(currentNode.ParentNode);
        //            if (currentNode != null)
        //            {
        //                currentNode.Remove();
        //            }
        //        }
        //        if (currentNode.Name.ToLower().Trim() == "span")
        //        {
        //            currentNode.Remove();
        //        }
        //    }
        //}

        private static void test_StatKeyword(Dictionary<string, Dictionary<string, int>> list, string title, string content, int xyAttr, string[] strKeywords)
        {
            foreach (string keyword in strKeywords)
            {
                bool flag_title = title.Contains(keyword);
                bool flag_content = content.Contains(keyword);
                if (flag_title || flag_content)
                {
                    //list[keyword] = 1;//   ["total"]= list.Keys[keyword]["total"] + 1;
                    list[keyword]["total"] = list[keyword]["total"] + 1;

                    if (flag_title)
                    {
                        list[keyword]["Title"] = list[keyword]["Title"] + 1;
                    }
                    if (flag_content)
                    {
                        list[keyword]["Content"] = list[keyword]["Content"] + 1;
                    }
                    switch (xyAttr)
                    {
                        case 1:
                            list[keyword]["头部"] = list[keyword]["头部"] + 1;
                            break;
                        case 2:
                            list[keyword]["腰部"] = list[keyword]["腰部"] + 1;
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        private static string RemoveHtmlTag(string html)
        {
            html = Regex.Replace(html, "<[^>]*>{1}", "", RegexOptions.IgnoreCase);
            html = html.Replace("\n", " ");
            html = html.Replace("\t", " ");
            html = html.Replace("\r", " ");
            html = html.Replace("&nbsp;", " ");
            html = html.Replace("&quot;", " ");
            html = Regex.Replace(html, " \\s{2,}", " ");

            return html;
        }


        //将Image转换成流数据，并保存为byte[]   
        private static byte[] PhotoImageInsert(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                else
                {
                    return null;
                }
                byte[] buffer = new byte[ms.Length];
                ////Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                //ms.Seek(0, SeekOrigin.Begin);
                //ms.Read(buffer, 0, buffer.Length);

                //ms.Position = 0;
                //ms.Read(buffer, 0, buffer.Length);
                //ms.Close();
                return buffer;
            }
        }

        public static Image GetImageByURL(string imgUrl)
        {
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imgUrl);
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                Image img = Image.FromStream(res.GetResponseStream());
                res.Dispose();
                return img;
            }
            catch (Exception ex)
            {
                //BLL.Loger.Log4Net.Error("GetImageByURL-Error", ex);
                return null;
            }
        }

        public static string CleanImg(Image img, string imgUrl)
        {
            string imageUrl = string.Empty;
            string extName = string.Empty;
            ImageFormat imageFormat = GetImageExtension(img, out extName);
            if (!string.IsNullOrWhiteSpace(extName))
            {
                IFastDFSAdapter _iFastDFSAdapter = new StreamFastDFSAdapter(ImageToByteArray(img, imageFormat, imgUrl), extName.TrimStart('.'));
                imageUrl = _iFastDFSAdapter.Upload();
            }
            return imageUrl;
        }

        public static byte[] ImageToByteArray(Image image, ImageFormat imageFormat, string imgUrl)
        {
            if (imageFormat == ImageFormat.Gif)
            {
                WebClient Client = new WebClient();
                return Client.DownloadData(imgUrl);
            }
            else
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }

        public static ImageFormat GetImageExtension(Image image, out string extName)
        {
            extName = string.Empty;
            if (ImageFormats != null && ImageFormats.Count > 0)
            {
                foreach (var pair in ImageFormats)
                {
                    if (pair.Value.Guid == image.RawFormat.Guid)
                    {
                        extName = pair.Key.ToString().Trim();
                        return pair.Value;
                    }
                }
            }
            return null;
        }
    }
}
