/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 11:04:16
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1
{
    [TestClass]
    public class QrCodeProviderTest
    {
        [TestMethod]
        public void Generate_test()
        {
            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = "https://www.baidu.com/",
                FileName = @"C:\Users\lix\Desktop\qr.jpg",
                SaveFileName = @"D:\qr2.jpg"
            });

            provider.Generate();
        }

        [TestMethod]
        public void GenerateByFile_test()
        {
            var logoUrl = "/UploadFiles/2017/7/24/14/logo$831363d6-92ce-4680-a715-97f359f895d3.png";
            var uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            var fileName = uploadFilePath + logoUrl.Replace(@"/", "\\");

            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = "https://www.baidu.com/",
                FileName = fileName,
                SaveFileName = @"D:\qr12.jpg"
            });

            provider.GenerateByFile();
        }

        [TestMethod]
        public void DownImage_test()
        {
            var savePath = @"D:\GitRoot\A5信息系统研发\销售业务管理平台\Chitunion\XYAuto.ITSC.Chitunion2017.Test\V1_1\Temp\";
            var downUrl =
                "http://img.weiboyi.com/vol1/7/102/102/m/a/96p48no1090811r79p9p40qn8613p9o9/KunshanDaily_avatar_1489531814.jpg";
            var fileName = System.IO.Path.GetFileName(downUrl);
            Console.WriteLine(fileName);
            downUrl = "http://wx.qlogo.cn/mmopen/zHyxxadDLvpZ3F10otLMkKQws4ciaP9Nggy8ZBK5alaQOtIncQpn0WCtmO8h1icjYDG25PY6GJkqQRTshZnROlg0nRm34ZlrrC/0";
            fileName = System.IO.Path.GetFileName(downUrl);
            Console.WriteLine(fileName);
            var provider =
                new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto(), new ConfigEntity());
            provider.DownImage(downUrl, Path.Combine(savePath, fileName));
        }

        [TestMethod]
        public void GetLogoFilePath_test()
        {
            var downUrl =
               "http://img.weiboyi.com/vol1/7/102/102/m/a/96p48no1090811r79p9p40qn8613p9o9/KunshanDaily_avatar_1489531814.jpg";
            downUrl = "http://wx.qlogo.cn/mmopen/zHyxxadDLvpZ3F10otLMkKQws4ciaP9Nggy8ZBK5alaQOtIncQpn0WCtmO8h1icjYDG25PY6GJkqQRTshZnROlg0nRm34ZlrrC/0";
            var provider =
                new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto(), new ConfigEntity());
            var path = provider.GetLogoFilePath(downUrl);
            Console.WriteLine(path);
        }

        [TestMethod]
        public void SerMapPath_test()
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/download/EditPlus64_xp85.com.zip");
            Console.WriteLine(filePath);

            filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/UploadFiles/2017/7/24/14/私人搭配师.png");
            Console.WriteLine(filePath);

            filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"http://www.chitunion.com/UploadFiles/2017/7/24/14/私人搭配师.png");
            Console.WriteLine(filePath);
        }

        [TestMethod]
        public void TwoBarCodeHistoryProvider_test()
        {
            var provider = new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto(), new ConfigEntity());
            var tunple = provider.GenPath("测试帐号.png");

            Console.WriteLine(JsonConvert.SerializeObject(tunple));
        }

        [TestMethod]
        public void TwoBarCodeHistoryProvider_insert_test()
        {
            var provider = new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto()
            {
                Item = new List<TwoBarCodeDto>()
                {
                    new TwoBarCodeDto()
                    {
                        MediaType = 14001,
                        MediaId = 14622,
                        OrderId = "ddddd",
                        Url = "https://www.baidu.com"
                    },
                    new TwoBarCodeDto()
                    {
                        MediaType = 14001,
                        MediaId = 14623,
                        OrderId = "ddddd",
                        Url = "http://www.chitunion.com/"
                    }
                }
            }, new ConfigEntity() { CreateUserId = 1192 });

            var retValue = provider.Excute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void TwoBarCodeHistoryProvider_GetTempPhysicalPath_test()
        {
            var provider = new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto()
            {
            }, new ConfigEntity() { CreateUserId = 1192 });

            var retValue = provider.GetTempPhysicalPath("1,2,7,12");
            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            using (new FileStream(retValue.Item1, FileMode.Open, FileAccess.Read))
            {
            }
        }

        [TestMethod]
        public void car_test()
        {
            var respMasterBrandList = new List<RespCarBrandDto>()
            {
                new RespCarBrandDto()
                {
                    MasterId = 1,
                    BrandName = "奔驰"
                },
                new RespCarBrandDto()
                {
                    MasterId = 2,
                    BrandName = "宝马"
                },
                new RespCarBrandDto()
                {
                    MasterId = 3,
                    BrandName = "Jeep"
                },new RespCarBrandDto()
                {
                    MasterId = 4,
                    BrandName = "标致"
                },new RespCarBrandDto()
                {
                    MasterId = 5,
                    BrandName = "雪铁龙"
                }
            };
            Console.WriteLine(JsonConvert.SerializeObject(respMasterBrandList));
        }

        [TestMethod]
        public void car_test2()
        {
            var respList = new List<RespCarSerialDto>()
            {
                new RespCarSerialDto()
                {
                    BrandId = 218,
                    ShowName = "宝马5系",
                    CarSerialId = 2682
                },
                new RespCarSerialDto()
                {
                    BrandId = 218,
                    ShowName = "宝马3系",
                    CarSerialId = 2178
                }
            };

            Console.WriteLine(JsonConvert.SerializeObject(respList));
        }

        [TestMethod]
        public void channel_ChannelInfoProvider_test()
        {
            var provder = new ChannelInfoProvider();

            var list = provder.Query(new RequestGetChannelDto()
            {
                MediaId = 19906,
                AdPosition1 = 6003,
                AdPosition2 = 7002,
                AdPosition3 = 8003,
                CooperateDate = "2017-08-24"
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}