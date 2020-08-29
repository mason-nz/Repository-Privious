using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using BitAuto.EP.Common.Lib.Excel;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData
{
    public class ReadDataListClient
    {

        private const string ReadExcelFolderName = "ReadFile";
        private const string StoreExcelFolderName = "StoreFile";
        private readonly DataImport _dataImport = new DataImport();

        /// <summary>
        /// 一次性加载文件夹里面所有的文件，完成一个移动到已完成文件夹
        /// </summary>
        public void LoadFileDir()
        {
            var pathD = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ReadExcelFolderName);
            var pathMove = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StoreExcelFolderName);
            if (!Directory.Exists(pathD))
                Directory.CreateDirectory(pathD);
            var files = new DirectoryInfo(pathD).GetFiles();//所有的文件,不支持 Directory.GetFiles("*.xls|*.xlsx");
            if (files.Length == 0)
            {
                Loger.Log4Net.Debug("文件夹为空。暂时任务");
                return;
            }
            foreach (var file in files)
            {
                if (!file.Name.EndsWith(".xls") && !file.Name.EndsWith(".xlsx")) continue;
                Loger.Log4Net.InfoFormat("任务开始：{0}", file);

                var mediaType = GeTypeEnum(file.Name);
                GetExcelHelper(file.FullName, mediaType);

                Loger.Log4Net.InfoFormat("任务结束：{0}", file);
                //移除文件到别的文件夹
                MoveFile(file.FullName, pathMove, string.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), file.Name));
            }
        }

        public void MoveFile(string sourceFile, string moveFile, string fileName)
        {
            var pathMove = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StoreExcelFolderName);
            if (!Directory.Exists(pathMove))
                Directory.CreateDirectory(pathMove);
            File.Move(sourceFile, Path.Combine(pathMove, fileName));
        }

        public MediaTypeEnum GeTypeEnum(string fileName)
        {
            if (fileName.Contains(MediaTypeEnum.微信.ToString()))
            {
                return MediaTypeEnum.微信;
            }
            else if (fileName.Contains(MediaTypeEnum.APP.ToString()))
            {
                return MediaTypeEnum.APP;
            }
            else if (fileName.Contains(MediaTypeEnum.微博.ToString()))
            {
                return MediaTypeEnum.微博;
            }
            else if (fileName.Contains(MediaTypeEnum.直播.ToString()))
            {
                return MediaTypeEnum.直播;
            }
            else if (fileName.Contains(MediaTypeEnum.视频.ToString()))
            {
                return MediaTypeEnum.视频;
            }
            else
            {
                throw new Exception("没有从文件名中找到对应的媒体类型");
            }
        }

        public void ReadFromExcelFile(string filePath, MediaTypeEnum mediaType)
        {
            //var filePath = @"E:\导入数据测试\test.xlsx";

            ExcelHelper<T> excelHelper = new ExcelHelper<T>();
            var importDataList = excelHelper.GetListFromFile(filePath).ToList();
            foreach (var item in importDataList)
            {
                //_dataImport.InsertImportData(item, mediaType);
            }
        }

        public bool GetExcelHelper(string filePath, MediaTypeEnum mediaType)
        {
            var dto = new Import_PCAPPDTO();
            switch (mediaType)
            {
                case MediaTypeEnum.微信:
                    var excelHelper = new ExcelHelper<ImportMediaWeiXinDto>();
                    var importDataList = excelHelper.GetListFromFile(filePath).ToList();
                    foreach (var item in importDataList)
                    {
                        //找到创建人的用户Id
                        //媒体
                        dto.MediaWeixin = Mapper.Map<ImportMediaWeiXinDto, Entities.Media.MediaWeixin>(item);
                        //刊例
                        dto.PubBasicInfo = Mapper.Map<ImportMediaWeiXinDto, Entities.Publish.PublishBasicInfo>(item);
                        //刊例详情
                        dto.PubDetailList = Mapper.Map<List<Entities.Publish.PublishDetailInfo>, List<Entities.Publish.PublishDetailInfo>>(item.PublishDetailList);
                        //互动参数
                        dto.InteractionWeixin = Mapper.Map<ImportMediaWeiXinDto, Entities.Interaction.InteractionWeixin>(item);
                        if (_dataImport.InsertImportData(dto, mediaType) > -1)
                        {
                            Console.WriteLine(string.Format("微信:{0} ,入库成功！", dto.MediaWeixin.Name));
                        }
                        else
                        {
                            Loger.Log4Net.ErrorFormat("微信:{0} ,失败！", dto.MediaWeixin.Name);
                            Console.WriteLine(string.Format("微信:{0} ,失败！", dto.MediaWeixin.Name));
                            break;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }
    }
}
