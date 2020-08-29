/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 11:06:39
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1_11
{
    public class MaterielProvider
    {
        private readonly RequestMaterielGetInfoDto _getInfoContext;
        private const string UpladFilesPath = "/UploadFiles/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);

        public MaterielProvider(ConfigEntity configEntity, RequestMaterielGetInfoDto getInfoContext)
        {
            _getInfoContext = getInfoContext;
        }

        #region 查询相关

        public RespMaterielInfoDto GetInfo()
        {
            if (_getInfoContext.MaterielId <= 0) return null;
            var tupInfo = Dal.Materiel.MaterielExtend.Instance.GetInfo(_getInfoContext.MaterielId, _getInfoContext.IsGetChannelInfo);

            if (tupInfo.Item1 == null) return null;

            var respMaterielInfo = AutoMapper.Mapper.Map<Entities.Materiel.MaterielExtend, RespMaterielInfoDto>(tupInfo.Item1);

            return GetChannelList(respMaterielInfo, tupInfo.Item2);
        }

        public RespMaterielInfoDto GetChannelList(RespMaterielInfoDto respMaterielInfo,
            List<Entities.Materiel.MaterielChannel> channelList)
        {
            if (respMaterielInfo == null || channelList == null) return respMaterielInfo;

            if (!channelList.Any())
                return respMaterielInfo;

            //MediaTypeName 汇总
            var groupByList = channelList.GroupBy(s => s.MediaTypeName).ToList();

            var baseCategrayList = groupByList.Where(s => s.Key.Equals("微信") || s.Key.Equals("微博") || s.Key.Equals("直播") || s.Key.Equals("视频")).ToList();
            //取差集：其他类型
            var expectedList = groupByList.Except(baseCategrayList);

            respMaterielInfo.ChannelItem = new List<ChannelItem>();
            //基本的媒体类型
            baseCategrayList.ForEach(item =>
            {
                respMaterielInfo.ChannelItem.Add(new ChannelItem()
                {
                    Name = item.Key,
                    Channelinfo = AutoMapper.Mapper.Map<List<Entities.Materiel.MaterielChannel>, List<Channelinfo>>(
                       channelList.Where(s => s.MediaTypeName.Equals(item.Key)).ToList())
                });
            });
            //其他类型
            foreach (var gb in expectedList)
            {
                //var item = gb.Key;
                respMaterielInfo.ChannelItem.Add(new ChannelItem()
                {
                    Name = gb.Key,
                    Channelinfo = AutoMapper.Mapper.Map<List<Entities.Materiel.MaterielChannel>, List<Channelinfo>>(
                        channelList.Where(s => s.MediaTypeName.Equals(gb.Key)).ToList())
                });
            }

            return respMaterielInfo;
        }

        #endregion 查询相关

        #region 下载、生成二维码

        public Tuple<string, string, string> GenerateDownload()
        {
            var materielInfo = Dal.Materiel.MaterielExtend.Instance.GetEntity(_getInfoContext.MaterielId);

            if (materielInfo == null)
            {
                return new Tuple<string, string, string>(string.Empty, $"物料id：{_getInfoContext.MaterielId} 不存在", string.Empty);
            }

            //todo：查找需要批量下载的数据id列表
            var channelList = Dal.Materiel.MaterielExtend.Instance.GetChannelList(_getInfoContext.ChannelIds);
            if (channelList.Count == 0)
            {
                return new Tuple<string, string, string>(string.Empty, $"没有找到待下载相关信息：{_getInfoContext.ChannelIds}", string.Empty);
            }

            var filePath = new List<string>();//文件的物理路径
            var filePathFolder = string.Empty;//文件夹

            channelList.ForEach(item =>
            {
                //todo:循环去生成二维码,文件名，（媒体类型+渠道名称+阅读原文）
                //todo:加到一个文件夹里面
                //todo:最后压缩此文件夹zip
                var fileName = $"{item.MediaTypeName}_{item.MediaNumber}_阅读原文.png";
                var qrCodeUrl = Generate(fileName, item.PromotionUrl);
                if (channelList.LastIndexOf(item) == 0)
                {
                    filePathFolder = qrCodeUrl.Item1;//获取文件夹路径
                }
                filePath.Add(qrCodeUrl.Item1 + fileName);
            });

            //todo:将文件夹压缩(压缩包的名称为物料名称.zip)
            var provider = new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto(), new ConfigEntity());
            return provider.GetCompressPath(filePathFolder, filePath, $"{materielInfo.MaterielName}.zip");
        }

        /// <summary>
        /// 生成二维码地址:需要存到指定的文件夹下面
        /// </summary>
        /// <param name="fileName">文件名，（媒体类型+渠道名称+阅读原文）</param>
        /// <param name="content">二维码内容</param>
        /// <returns>保存了文件，返回对应的url</returns>
        private Tuple<string, string> Generate(string fileName, string content)
        {
            var tunplePath = GetPath(fileName);

            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = content,
                SaveFileName = Path.Combine(tunplePath.Item1, fileName)
            });

            provider.Generate();

            //var pathFolder = tunplePath.Item1;//文件夹的路径

            return tunplePath;
        }

        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GetPath(string fileName)
        {
            //UploadLoad
            string relatedPath = $"{UpladFilesPath}Materiel/Temp/{DateTime.Now.ToString("yyyyMMddHHmmss")}/";
            var webFilePath = relatedPath + fileName;
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(dir, webFilePath);
        }

        #endregion 下载、生成二维码
    }
}