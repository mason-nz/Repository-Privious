/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 11:51:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using NPOI.OpenXmlFormats.Dml;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8
{
    public class TwoBarCodeHistoryProvider : Base.CurrentOperateBase
    {
        private readonly RequestTwoBarCodeDto _context;
        private readonly ConfigEntity _configEntity;
        private const string UpladFilesPath = "/UploadFiles/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);

        public TwoBarCodeHistoryProvider(RequestTwoBarCodeDto context, ConfigEntity configEntity)
        {
            _context = context;
            _configEntity = configEntity;
        }

        public ReturnValue Excute()
        {
            //todo:都是先删除再插入
            /*
                1.前端传过来的参数，url地址生成二维码，用户输入可能是域名，可能是地址，需要拼接URL，xxx?productcode=项目编号&type=wx&mediacode=微信号
                2.存储生成的二维码物理路径
                3.存储到表：二维码的url地址
            */

            //基础参数校验
            var insertList = new List<Entities.Media.TwoBarCodeHistory>();
            var retValue = new ReturnValue();
            foreach (var item in _context.Item)
            {
                retValue = VerifyOfNecessaryParameters(_context);
                if (retValue.HasError)
                    return retValue;
                //todo:查找对应的微信名称
                var weixinInfo = Dal.Media.MediaWeixin.Instance.GetEntity(item.MediaId);

                if (weixinInfo == null)
                    return CreateFailMessage(retValue, "60001", $"当前媒体信息不存在,MediaId:{item.MediaId}");
                var qrCodeUrl = Generate($"{weixinInfo.Name}.png", GetContent(item, weixinInfo.Number));

                insertList.Add(new Entities.Media.TwoBarCodeHistory()
                {
                    OrderID = item.OrderId,
                    CreateUserID = _configEntity.CreateUserId,
                    MediaID = item.MediaId,
                    MediaType = item.MediaType,
                    URL = item.Url,
                    TwoBarUrl = qrCodeUrl
                });
            }

            return OperateTwoBarCodeHistory(retValue, insertList);
        }

        private ReturnValue OperateTwoBarCodeHistory(ReturnValue retValue, List<Entities.Media.TwoBarCodeHistory> insertList)
        {
            Dal.Media.TwoBarCodeHistory.Instance.Insert(insertList);
            return retValue;
        }

        /// <summary>
        /// 根据URL地址?productcode=项目编号&type=wx&mediacode=微信号，生成二维码图片，以微信公众号名称来命名，图片大小为430*430
        /// </summary>
        /// <returns></returns>
        private string GetContent(TwoBarCodeDto item, string wxNumber)
        {
            if (string.IsNullOrWhiteSpace(item.Url))
                return string.Empty;
            string content;
            if (item.Url.Contains("?"))
            {
                //代表不是域名，而是一个带参数的url地址
                content = $"{item.Url}&productcode={item.OrderId}&type={item.MediaType}&mediacode={wxNumber}";
            }
            else
            {
                content = $"{item.Url}?productcode={item.OrderId}&type={item.MediaType}&mediacode={wxNumber}";
            }
            return content;
        }

        /// <summary>
        /// 生成二维码地址:需要存到指定的文件夹下面
        /// </summary>
        /// <param name="fileName">文件名，（这里统一为微信名称）</param>
        /// <param name="content">二维码内容</param>
        /// <returns>保存了文件，返回对应的url</returns>
        private string Generate(string fileName, string content)
        {
            var tunplePath = GenPath(fileName);

            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = content,
                SaveFileName = tunplePath.Item1
            });

            provider.Generate();

            return tunplePath.Item2;
        }

        public string GenerateByLogo(string fileName, string content, string logoUrl)
        {
            var tunplePath = GenPath(fileName);
            var logoPath = GetLogoFilePath(logoUrl);
            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = content,
                SaveFileName = tunplePath.Item1,
                FileName = logoPath
            });
            //todo:如果logo url地址不存在，则生成没有logo的二维码
            //todo:？？？？？ 要是url地址是外域名的呢
            if (string.IsNullOrWhiteSpace(logoPath))
                provider.Generate();
            else
            {
                provider.GenerateByFile();
            }
            return tunplePath.Item2;
        }

        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GenPath(string fileName)
        {
            DateTime time = DateTime.Now;
            //UploadLoad
            string relatedPath = string.Format(UpladFilesPath + "{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);
            var webFilePath = relatedPath + fileName;
            //string dir = HttpContext.Current.Server.MapPath("~" + relatedPath);
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(Path.Combine(dir, fileName), webFilePath);
        }

        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GenTempPath(string fileName)
        {
            DateTime time = DateTime.Now;
            //UploadLoad
            string relatedPath = string.Format(UpladFilesPath + "temp/{0}/{1}/{2}/", time.Year, time.Month, time.Day);
            var webFilePath = relatedPath + fileName;
            //string dir = HttpContext.Current.Server.MapPath("~" + relatedPath);
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(Path.Combine(dir, fileName), webFilePath);
        }

        /// <summary>
        /// 获取媒体logo的物理path
        /// </summary>
        /// <param name="logoUrl">/UploadFiles/xxxx.jpg</param>
        /// <returns>c:load\UploadFiles\2017\7\21\16\Capture001$73747fc8-1e03-4dc9-80ee-adcf5528d622.png</returns>
        public string GetLogoFilePath(string logoUrl)
        {
            if (string.IsNullOrWhiteSpace(logoUrl)) return string.Empty;
            if (logoUrl.Contains("http:") || logoUrl.Contains("https:"))
            {
                //说明是远端图片。需要下载，放到Temp/down/,这里如果存在一样的图片，覆盖也无所谓，只用一次
                var fileName = System.IO.Path.GetFileName(logoUrl);
                if (string.IsNullOrWhiteSpace(fileName) || fileName.Equals("0"))
                {
                    fileName = $"{Guid.NewGuid()}.jpg";
                }
                var dirPath = $"{_uploadFilePath}\\Temp\\Down";//Path.Combine(_uploadFilePath, @"\Temp\Down");
                DirectoryAdd(dirPath);
                var saveFilePath = Path.Combine(dirPath, fileName);
                DownImage(logoUrl, saveFilePath);
                return saveFilePath;
            }

            return _uploadFilePath + logoUrl.Replace(@"/", "\\");
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="path">保存路径</param>
        public void DownImage(string url, string path)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.DownloadFile(url, path);
        }

        public Tuple<string, string, string> GetTempPhysicalPath(string ids)
        {
            DateTime time = DateTime.Now;
            /*
             1.先找出所有批量的下载的文件路径
             */

            var list = Dal.Media.TwoBarCodeHistory.Instance.GetList(
                 new TwoBarCodeHistoryQuery<Entities.Media.TwoBarCodeHistory>()
                 {
                     RecIds = ids
                 });

            if (list.Count == 0)
            {
                return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
            }
            var filesPath = new List<string>();

            var twoBarUrl = list.Select(s => s.TwoBarUrl);
            foreach (var item in twoBarUrl)
            {
                filesPath.Add(_uploadFilePath + item.ToAbsolutePath().Replace(@"/", "\\"));
            }

            return GetCompressPath(_uploadFilePath + "\\Temp", filesPath);
        }

        /// <summary>
        /// 获取压缩后的文件路径
        /// </summary>
        /// <param name="dirPath">压缩的文件路径</param>
        /// <param name="filesPath">多个文件路径</param>
        /// <param name="zipFileName">压缩文件名称，注意：是.zip后缀名</param>
        /// <returns></returns>
        public Tuple<string, string, string> GetCompressPath(string dirPath, List<string> filesPath, string zipFileName = "")
        {
            //dirPath = dirPath + "\\Temp";
            DirectoryAdd(dirPath);
            var zipPath = "";//返回压缩后的文件路径
            //string zipFileName;
            using (ZipFile zip = new ZipFile(System.Text.Encoding.Default)) //System.Text.Encoding.Default设置中文附件名称乱码，不设置会出现乱码
            {
                foreach (var file in filesPath)
                {
                    if (File.Exists(file))//文件是否存在
                        zip.AddFile(file, "");
                    //第二个参数为空，说明压缩的文件不会存在多层文件夹。比如C:\test\a\b\c.doc 压缩后解压文件会出现c.doc
                    //如果改成zip.AddFile(file);则会出现多层文件夹压缩，比如C:\test\a\b\c.doc 压缩后解压文件会出现test\a\b\c.doc
                }
                zipFileName = zipFileName ?? $"QR_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";
                zipPath = $@"{dirPath}{zipFileName.Replace(" ", "")}";
                zipPath = GetReplacePath(zipPath);
                Loger.Log4Net.InfoFormat($"下载二维码,GetCompressPath.zipPath:{zipPath}");
                zip.Save(zipPath);
            }
            return new Tuple<string, string, string>(zipPath, $"/Temp/{zipFileName}", zipFileName);
        }

        private string GetReplacePath(string rPath)
        {
            var rBuilder = new StringBuilder(rPath);
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                rBuilder.Replace(rInvalidChar.ToString(), string.Empty);
            }
            return rBuilder.ToString();
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