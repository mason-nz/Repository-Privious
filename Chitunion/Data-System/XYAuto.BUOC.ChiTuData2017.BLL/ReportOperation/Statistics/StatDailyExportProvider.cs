/********************************************************
*创建人：lixiong
*创建时间：2017/11/30 10:31:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Statistics
{
    /// <summary>
    /// auth:lixiong
    /// desc:日汇总数据-导出
    /// </summary>
    public class StatDailyExportProvider : CurrentOperateBase
    {
        private readonly Lazy<Dictionary<string, Func<string, ReturnValue>>> _lazyQueryDic;//lazy加载
        private readonly StatDetailsExportProvider _statDetailsExportProvider;
        private readonly ReqDailyDto _reqDailyDto;
        private readonly ConfigEntity _configEntity;
        private readonly static int ExcelCount = DataCommon.GetAppSettingInt32Value("ExcelQuantity");

        public StatDailyExportProvider(ReqDailyDto reqDailyDto)
        {
            _reqDailyDto = reqDailyDto;
            _configEntity = new ConfigEntity();
            _statDetailsExportProvider = new StatDetailsExportProvider(new ReqDetailsDto());
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

        public ReturnValue DoExport()
        {
            return _lazyQueryDic.Value.ContainsKey(_reqDailyDto.TabType)
                  ? _lazyQueryDic.Value[_reqDailyDto.TabType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "20001", "请输入合法的 TabType");
        }

        #region 导出excel

        /// <summary>
        /// 导出-日汇总数据-抓取
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportGrab()
        {
            var resp = new StatDailyGrabQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"抓取-日汇总数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = _statDetailsExportProvider.GetStatPath(fileName, StatisticsDataTypeEnum.Daily);
            new ExcelHelper<RespDailyGrabDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl,
                Message = dicFilePath.Item1 + @"\" + fileName
            };
        }

        /// <summary>
        /// 导出-日汇总数据-机洗入库
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportJx()
        {
            var resp = new StatDailyJxQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"机洗入库-日汇总数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = _statDetailsExportProvider.GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespDailyJxDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-日汇总数据-初筛
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportCs()
        {
            var resp = new StatDailyCsQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"初筛-日汇总数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = _statDetailsExportProvider.GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespDailyCsDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-日汇总数据-车型匹配
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportCarMatch()
        {
            var resp = new StatDailyCarMatchQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"车型匹配-日汇总数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = _statDetailsExportProvider.GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespDailyCarMatchDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        /// <summary>
        /// 导出-日汇总数据-人工清洗
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExportRgqx()
        {
            var resp = new StatDailyRgqxQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"人工清洗-日汇总数据-{Guid.NewGuid()}.xlsx";
            var dicFilePath = _statDetailsExportProvider.GetStatPath(fileName, StatisticsDataTypeEnum.Details);
            new ExcelHelper<RespDailyRgqxDto>().SaveExcelToFile(resp.List, dicFilePath.Item1, fileName);
            var httpUrl = dicFilePath.Item2;//前端http url地址
            return new ReturnValue()
            {
                ReturnObject = httpUrl
            };
        }

        #endregion 导出excel

        #region 导出csv

        /// <summary>
        /// 导出-日汇总数据-抓取
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvGrab()
        {
            var resp = new StatDailyGrabQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"抓取-日汇总数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "Date","抓取日期"},
                { "ArticleTypeName","头腰文章类型"},
                { "ChannelName","抓取渠道"},
                { "ArticleCount","抓取文章量"},
                { "AccountCount","抓取账号量"}
            };

            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");

            resp.List.OrderByDescending(s => s.Date).ToList().ForEach(s =>
              {
                  strbData.AppendFormat($"{s.Date},{s.ArticleTypeName},{s.ChannelName},{s.ArticleCount},{s.AccountCount}");

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
        /// 导出-日汇总数据-机洗入库
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvJx()
        {
            var resp = new StatDailyJxQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"机洗入库-日汇总数据-{Guid.NewGuid()}.csv";

            var dicTitle = new Dictionary<string, string>()
            {
                { "Date","入库日期"},
                { "ArticleTypeName","头腰文章类型"},
                { "ChannelName","抓取渠道"},
                { "ArticleCount","抓取文章量"},
                { "StorageArticleCount","入库文章量"},
                { "AccountCount","抓取账号量"},
                { "StorageAccountCount","入库账号量"}
            };
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");

            resp.List.OrderByDescending(s => s.Date).ToList().ForEach(s =>
            {
                strbData.AppendFormat($"{s.Date},{s.ArticleTypeName},{s.ChannelName},{s.ArticleCount},{s.StorageArticleCount},{s.AccountCount},{s.StorageAccountCount}");

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
        /// 导出-日汇总数据-初筛
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvCs()
        {
            var resp = new StatDailyCsQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"初筛-日汇总数据-{Guid.NewGuid()}.csv";
            var dicTitle = new Dictionary<string, string>()
            {
                { "Date","初筛日期"},
                { "ChannelName","渠道"},
                { "ArticleCount","可用文章数"},
                { "AccountCount","可用账号数"},
                { "ToBodyArticleCount","置为腰文章数"},
                { "ToBodyAccountCount","置为腰账号数"},
                { "NotUseArticleCount","作废文章数"},
                { "NotUseAccountCount","作废账号数"}
            };
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");

            resp.List.OrderByDescending(s => s.Date).ToList().ForEach(s =>
            {
                strbData.AppendFormat($"{s.Date},{s.ChannelName},{s.ArticleCount},{s.AccountCount},{s.ToBodyArticleCount},{s.ToBodyAccountCount},{s.NotUseArticleCount},{s.NotUseAccountCount}");

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
        /// 导出-日汇总数据-车型匹配
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvCarMatch()
        {
            var resp = new StatDailyCarMatchQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"车型匹配-日汇总数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "Date","日期"},
                { "ChannelName","渠道"},
                { "MatchArticleCount","已匹配车型"},
                { "UnMatchArticleCount","未匹配车型"}
            };
            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.AppendFormat($"{dic.Value},");
            }
            strbData.Append("\n").Replace(",\n", "\n");

            resp.List.OrderByDescending(s => s.Date).ToList().ForEach(s =>
            {
                strbData.Append($"{s.Date},{s.ChannelName},{s.MatchArticleCount},{s.UnMatchArticleCount}");
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
        /// 导出-日汇总数据-人工清洗
        /// </summary>
        /// <returns></returns>
        public ReturnValue CsvRgqx()
        {
            var resp = new StatDailyRgqxQuery(_configEntity).GetQueryList(_reqDailyDto, "export");
            var fileName = $"人工清洗-日汇总数据-{Guid.NewGuid()}.csv";
            if (resp.List.Count > ExcelCount)
            {
                return CreateFailMessage(new ReturnValue(), "", "已超出设置最大数目");
            }
            var dicTitle = new Dictionary<string, string>()
            {
                { "Date","清洗日期"},
                { "ArticleTypeName","头腰文章类型"},
                { "ChannelName","渠道"},
                { "ArticleCount","可用文章数"},
                { "AccountCount","可用账号数"},
                { "NotUseArticleCount","作废文章数"},
                { "NotUseAccountCount","作废账号数"},
                { "ToBodyArticleCount","置为腰文章数"},
                { "ToBodyAccountCount","置为腰账号数"},
            };

            var strbData = new StringBuilder();

            foreach (var dic in dicTitle)
            {
                strbData.Append($"{dic.Value},");
            }
            strbData.Append("\n");

            resp.List.OrderByDescending(s => s.Date).ToList().ForEach(s =>
            {
                strbData.Append($"{s.Date},{s.ArticleTypeName},{s.ChannelName},{s.ArticleCount},{s.AccountCount},{s.NotUseArticleCount},{s.NotUseAccountCount}");
                strbData.Append($",{s.ToBodyArticleCount},{s.ToBodyAccountCount}");

                strbData.Append("\n");
            });

            var byteArray = System.Text.Encoding.Default.GetBytes(strbData.ToString().Replace(",\n", "\n"));

            return new ReturnValue()
            {
                ReturnObject = byteArray,
                Message = fileName
            };
        }

        #endregion 导出csv
    }
}