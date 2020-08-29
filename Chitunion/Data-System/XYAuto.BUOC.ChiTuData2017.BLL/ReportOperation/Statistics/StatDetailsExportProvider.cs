/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 20:01:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Statistics
{
    /// <summary>
    /// auth:lixiong
    /// desc:明细数据-导出
    /// </summary>
    public class StatDetailsExportProvider : CurrentOperateBase
    {
        private const string UpladFilesPath = "/UploadFiles/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        private readonly string _todayDate = DateTime.Now.ToString("yyyy-MM-dd");
        private readonly Lazy<Dictionary<string, Func<string, ReturnValue>>> _lazyQueryDic;//lazy加载
        private readonly static int ExcelCount = DataCommon.GetAppSettingInt32Value("ExcelQuantity");

        private readonly ReqDetailsDto _reqDetailsDto;
        private readonly ConfigEntity _configEntity;

        public StatDetailsExportProvider(ReqDetailsDto reqDetailsDto)
        {
            _reqDetailsDto = reqDetailsDto;
            _configEntity = new ConfigEntity();
            _lazyQueryDic = new Lazy<Dictionary<string, Func<string, ReturnValue>>>(InitQuery);
        }

        private Dictionary<string, Func<string, ReturnValue>> InitQuery()
        {
            //return new Dictionary<string, Func<string, ReturnValue>>()
            //    {
            //        { GetDailyTypeEnum.grab.ToString(), s =>  ExportGrab()},
            //        { GetDailyTypeEnum.jxrk.ToString(), s =>  ExportJx()},
            //        { GetDailyTypeEnum.cxpp.ToString(), s =>  ExportCarMatch()},
            //        { GetDailyTypeEnum.cs.ToString(), s =>  ExportCs()},
            //        { GetDailyTypeEnum.rgqx.ToString(), s =>  ExportRgqx()}
            //    };
            return new Dictionary<string, Func<string, ReturnValue>>()
                {
                    { GetDailyTypeEnum.grab.ToString(), s =>  CsvGrab()},
                    { GetDailyTypeEnum.jxrk.ToString(), s =>  CsvJx()},
                    { GetDailyTypeEnum.cxpp.ToString(), s =>  CsvCarMatch()},
                    { GetDailyTypeEnum.cs.ToString(), s =>  CsvCs()},
                    { GetDailyTypeEnum.rgqx.ToString(), s =>  CsvRgqx()}
                };
        }

        #region 导出excel

        public ReturnValue DoExport()
        {
            return _lazyQueryDic.Value.ContainsKey(_reqDetailsDto.TabType)
                  ? _lazyQueryDic.Value[_reqDetailsDto.TabType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "20001", "请输入合法的 TabType");
        }

        /// <summary>
        /// 导出-明细数据-抓取
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportGrab()
        {
            var resp = new StatDetailsGrabQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"抓取-明细数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespStatDetailsGrabDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-明细数据-机洗入库
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportJx()
        {
            var resp = new StatDetailsJxQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"机洗入库-明细数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespStatDetailsJxDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-明细数据-初筛
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportCs()
        {
            var resp = new StatDetailsCsQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"初筛-明细数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespStatDetailsCsDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-明细数据-车型匹配
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportCarMatch()
        {
            var resp = new StatDetailsCarMatchQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"车型匹配-明细数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespStatDetailsCarMatchDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-明细数据-人工清洗
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportRgqx()
        {
            var resp = new StatDetailsRgqxQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"人工清洗-明细数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespStatDetailsRgqxDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <param name="statisticsDataTypeEnum"></param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GetStatPath(string fileName, StatisticsDataTypeEnum statisticsDataTypeEnum)
        {
            //UploadLoad
            var folder = statisticsDataTypeEnum.ToString();
            string relatedPath = $"{UpladFilesPath}ExportExcel/Statistics/{folder}/{_todayDate}/";
            var webFilePath = relatedPath + fileName;
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(dir, webFilePath);
        }

        #endregion 导出excel

        #region 导出csv

        /// <summary>
        /// 导出-明细数据-抓取
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvGrab()
        {
            var resp = new StatDetailsGrabQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"抓取-明细数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "ArticleId","文章ID"},
                { "Title","标题"},
                { "ArticleTypeName","头腰类型"},
                { "ChannelName","渠道"},
                { "ArticlePublishTime","发布时间"},
                { "ArticleSpiderTime","抓取时间"},
                { "SceneName","场景"},
                { "AccountName","账号"}
            };

            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");
            resp.List.OrderByDescending(s => s.ArticleSpiderTime).ToList().ForEach(s =>
              {
                  strbData.AppendFormat($"{s.ArticleId},{FilterText(s.Title)},{s.ArticleTypeName},{s.ChannelName},{s.ArticlePublishTime},{s.ArticleSpiderTime}");
                  strbData.AppendFormat($",{s.SceneName},{s.AccountName}");

                  strbData.Append("\n");
              });

            var byteArray = System.Text.Encoding.Default.GetBytes(strbData.ToString());
            return new ReturnValue()
            {
                ReturnObject = byteArray,
                Message = fileName
            };
        }

        /// <summary>
        /// 导出-明细数据-机洗入库
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvJx()
        {
            var resp = new StatDetailsJxQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"机洗入库-明细数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "ArticleId","文章ID"},
                { "Title","标题"},
                { "ArticleTypeName","头腰类型"},
                { "ChannelName","渠道"},
                { "ArticlePublishTime","发布时间"},
                { "ArticleSpiderTime","抓取时间"},
                { "StorageTime","入库时间"},
                { "SceneName","场景"},
                { "AccountName","账号"},
                { "AccountScore","账号分值"},
                { "ArticleScore","文章分值"}
            };
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");
            resp.List.OrderByDescending(s => s.StorageTime).ToList().ForEach(s =>
              {
                  strbData.Append($"{s.ArticleId},{FilterText(s.Title)},{s.ArticleTypeName},{s.ChannelName},{s.ArticlePublishTime},{s.ArticleSpiderTime}");
                  strbData.Append($",{s.StorageTime},{s.SceneName},{s.AccountName},{s.AccountScore},{s.ArticleScore}");

                  strbData.Append("\n");
              });

            Console.WriteLine(strbData.ToString());
            var byteArray = System.Text.Encoding.Default.GetBytes(strbData.ToString());
            return new ReturnValue()
            {
                ReturnObject = byteArray,
                Message = fileName
            };
        }

        /// <summary>
        /// 导出-明细数据-初筛
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvCs()
        {
            var resp = new StatDetailsCsQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"初筛-明细数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "ArticleId","文章ID"},
                { "Title","标题"},
                { "IsOriginal","是否原创"},
                { "ChannelName","渠道"},
                { "ArticlePublishTime","发布时间"},
                { "ArticleSpiderTime","抓取时间"},
                { "PrimaryTime","初筛时间"},
                { "SceneName","场景"},
                { "AccountName","账号"},
                { "AccountScore","账号分值"},
                { "ArticleScore","文章分值"},
                { "ConditionName","初筛状态"},
                { "Reason","原因"}
            };
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.Append($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");
            resp.List.OrderByDescending(s => s.PrimaryTime).ToList().ForEach(s =>
              {
                  strbData.Append($"{s.ArticleId},{FilterText(s.Title)},{s.IsOriginal},{s.ChannelName},{s.ArticlePublishTime},{s.ArticleSpiderTime}");
                  strbData.Append($",{s.PrimaryTime},{s.SceneName},{s.AccountName},{s.AccountScore},{s.ArticleScore},{s.ConditionName},{FilterText(s.Reason)}");

                  strbData.Append("\n");
              });
            var byteArray = System.Text.Encoding.Default.GetBytes(strbData.ToString());
            return new ReturnValue()
            {
                ReturnObject = byteArray,
                Message = fileName
            };
        }

        /// <summary>
        /// 导出-明细数据-车型匹配
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvCarMatch()
        {
            var resp = new StatDetailsCarMatchQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"车型匹配-明细数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "ArticleId","文章ID"},
                { "Title","标题"},
                { "ChannelName","渠道"},
                { "ArticlePublishTime","发布时间"},
                { "ArticleSpiderTime","抓取时间"},
                { "MatchCarTime","匹配车型时间"},
                { "BrandName","品牌"},
                { "SerialName","车型"},
                { "ArticleScore","文章分值"},
                { "MatchName","状态"}
            };
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");
            resp.List.OrderByDescending(s => s.MatchCarTime).ToList().ForEach(s =>
              {
                  strbData.Append($"{s.ArticleId},{FilterText(s.Title)},{s.ChannelName},{s.ArticlePublishTime},{s.ArticleSpiderTime}");
                  strbData.Append($",{s.MatchCarTime},{s.BrandName},{s.SerialName},{s.ArticleSorce},{s.MatchName}");

                  strbData.Append("\n");
              });
            var byteArray = System.Text.Encoding.Default.GetBytes(strbData.ToString());
            return new ReturnValue()
            {
                ReturnObject = byteArray,
                Message = fileName
            };
        }

        /// <summary>
        /// 导出-明细数据-人工清洗
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvRgqx()
        {
            var resp = new StatDetailsRgqxQuery(_configEntity).GetQueryList(_reqDetailsDto, "export");
            var fileName = $"人工清洗-明细数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "ArticleId","文章ID"},
                { "Title","标题"},
                { "ArticleTypeName","头腰类型"},
                { "ChannelName","渠道"},
                { "ArticlePublishTime","发布时间"},
                { "ArticleSpiderTime","抓取时间"},
                { "CleanTime","清洗时间"},
                { "SceneName","场景"},
                { "AccountName","账号"},
                { "AccountScore","账号分值"},
                { "ArticleScore","文章分值"},
                { "ConditionName","状态"},
                { "Reason","原因"}
            };
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.Append($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");
            resp.List.OrderByDescending(s => s.CleanTime).ToList().ForEach(s =>
              {
                  strbData.Append($"{s.ArticleId},{FilterText(s.Title)},{s.ArticleTypeName},{s.ChannelName},{s.ArticlePublishTime},{s.ArticleSpiderTime}");
                  strbData.Append($",{s.CleanTime},{s.SceneName},{s.AccountName},{s.AccountScore},{s.ArticleScore},{s.ConditionName},{FilterText(s.Reason)}");

                  strbData.Append("\n");
              });
            var byteArray = System.Text.Encoding.Default.GetBytes(strbData.ToString());
            return new ReturnValue()
            {
                ReturnObject = byteArray,
                Message = fileName
            };
        }

        public string FilterText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return value.Replace(",", "&#44").Replace("，", "&#44");
        }

        #endregion 导出csv
    }

    public enum StatisticsDataTypeEnum
    {
        Daily,
        Details
    }
}