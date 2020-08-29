using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.UploadFileInfo;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class UploadFileInfoTest
    {
        [TestMethod]
        public void UploadFileInfo_DoOperate_Test()
        {
            var createUserId = 99;
            var list = new List<Entities.UploadFileInfo.UploadFileInfo>()
            {
                new UploadFileInfo()
                {
                    CreateTime = DateTime.Now,
                    CreaetUserID = createUserId,
                    ExtendName = ".jpg",
                    FileName = "c6abbff8-001d-4b1c-812d-1f3c001223d6.jpg",
                    FilePah = "http://www.chitunion.com/UploadFiles/2017/3/20/11/test$c6abbff8-001d-4b1c-812d-1f3c001223d6.jpg",
                    FileSize = 33,
                    RelationID = 45,
                    RelationTableName = "Media_Weixin",
                    Type = (int)UploadFileEnum.MediaManage
                },
                new UploadFileInfo()
                {
                      CreateTime = DateTime.Now,
                    CreaetUserID = createUserId,
                    ExtendName = ".jpg",
                    FileName = "sss.jpg",
                    FilePah = "http://www.chitunion.com/UploadFiles/2017/3/20/11/test$sss.jpg",
                    FileSize = 330,
                    RelationID = 45,
                    RelationTableName = "Media_Weixin",
                    Type = (int)UploadFileEnum.MediaManage
                }
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.DoOperate(list);

            Assert.IsFalse(retValue.HasError);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void AE_SearchUploadFile_Test()
        {
            //授权AE的UserID ，包括自己
            var userIds = Common.UserInfo.GetAuthAEUserIDList(99);
            Console.WriteLine(userIds);
            //查找全部AE的用户相关的文件
            var fileList = BLL.UploadFileInfo.UploadFileInfo.Instance.GetList(
                new UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo>()
                {
                    AEUserId = userIds
                });
            Console.WriteLine(JsonConvert.SerializeObject(fileList));
        }

        [TestMethod]
        public void GetDirectoryLengthTest()
        {
            var url = "http://www.chitunion.com/UploadFiles/2017/3/20/11/test$sss.jpg";
            var fileTuple = BLL.Util.GetFileNameAndExtension(url);//获取文件及文件扩展名

            Console.WriteLine(string.Format("{0}---{1}", fileTuple.Item1, fileTuple.Item2));
            var path = @"D:\GitRoot\A5信息系统研发\销售业务管理平台\Chitunion\XYAuto.ITSC.Chitunion2017.Test\bin\Debug\StoreFile\微信.xlsx";
            var len = BLL.Util.GetFileSize(path);
            Console.WriteLine(len);

            var imagesurl1 = "/UploadFiles/2017/3/20/11/test$sss.jpg";
            imagesurl1 = imagesurl1.Replace(@"/", @"\"); //转换成绝对路径
            var paths = BLL.Util.Urlconvertorlocal(url);
            Console.WriteLine(imagesurl1);
        }

        [TestMethod]
        public void UrlTest()
        {
            var url =
                "http://www.chitunion.com/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.jpg";

            Console.WriteLine(new Uri(url).AbsolutePath);
            Console.WriteLine(new Uri(url).AbsoluteUri);
            Console.WriteLine(new Uri(url).Fragment);

            url = "http://www.chitunion.com/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.zip";

            Console.WriteLine(new Uri(url).AbsolutePath);
            long s = 3333L;

            Console.WriteLine();
        }

        [TestMethod]
        public void RoleTest()
        {
            var url =
                  "http://www.chitunion.com/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.jpg";

            var isUrl = BLL.Util.IsUrl(url);
            Console.WriteLine(isUrl);

            var end = url.Split('.')[1];
            isUrl = BLL.Util.IsUrl(url.Replace(end, ""));
            Console.WriteLine(isUrl);

            isUrl = BLL.Util.IsUrl("http://www.chitunion.com/UploadFiles/a/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9");
            Console.WriteLine(isUrl);
        }

        [TestMethod]
        public void ToAbsolutePathTest()
        {
            var url =
                 "http://www.chitunion11.com/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.jpg";

            Console.WriteLine(url.ToAbsolutePath(true));
            url = "";
            Console.WriteLine(url.ToAbsolutePath(true));
            url = null;
            Console.WriteLine(url.ToAbsolutePath(true));

            url = "/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.jpg";
            Console.WriteLine(url.ToAbsolutePath(true));

            var sbCode = new StringBuilder();

            Console.WriteLine(sbCode.ToString().TrimEnd(','));
        }
    }
}