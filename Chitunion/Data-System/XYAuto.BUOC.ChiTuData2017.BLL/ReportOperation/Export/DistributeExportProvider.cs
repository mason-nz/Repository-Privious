/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 10:13:11
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.AutoResolver;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Query;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Export
{
    /// <summary>
    /// auth:lixiong
    /// desc:导出数据：分发列表导出，分发明细导出，物料详情明细导出
    /// </summary>
    public class DistributeExportProvider : VerifyOperateBase
    {
        private const string UpladFilesPath = "/UploadFiles/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        private readonly string _todayDate = DateTime.Now.ToString("D");
        private readonly DistributeExportDto _contextDistributeExport;
        private static Lazy<Dictionary<ExportBusinessType, Func<string, Tuple<dynamic, string>>>> _queryDic;//lazy加载

        public DistributeExportProvider(DistributeExportDto contextDistributeExport)
        {
            _contextDistributeExport = contextDistributeExport;
            _queryDic = new Lazy<Dictionary<ExportBusinessType, Func<string, Tuple<dynamic, string>>>>(Init);
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns>item1:ReturnValue  item2:filePath  item3:fileName</returns>
        public Tuple<ReturnValue, string, string, string> Export()
        {
            //todo:1.先查询出页面呈现的数据（这里三种导出类型，参数会比较多）
            //todo:2.再进行导出数据功能操作
            var retValue = new ReturnValue();

            var exportList = GetQuery();
            if (exportList == null || exportList.Item1 == null)
            {
                var msg = exportList == null ? "exportList 为null,暂无数据" : exportList.Item2;
                return new Tuple<ReturnValue, string, string, string>(
                    CreateFailMessage(retValue, "20001", $"导出数据相关-查询数据源错误:" +
                                                         $"{msg}"), string.Empty, string.Empty, string.Empty);
            }
            var path = GetPath(exportList.Item2);
            var httpUrl = path.Item2;//前端http url地址

            Loger.Log4Net.Info($"导出数据：{JsonConvert.SerializeObject(exportList.Item1)}");
            //todo:导出
            SaveExcelToFile(exportList.Item1, path.Item1, exportList.Item2);

            var filePath = path.Item1 + $@"\{exportList.Item2}";
            return new Tuple<ReturnValue, string, string, string>(
                   CreateSuccessMessage(retValue, "0", "success"), filePath, exportList.Item2, httpUrl);
        }

        private Dictionary<ExportBusinessType, Func<string, Tuple<dynamic, string>>> Init()
        {
            var distributeDic = new Dictionary<ExportBusinessType, Func<string, Tuple<dynamic, string>>>()
            {
                {ExportBusinessType.Distribute , s=> GetDistribute() },
                {ExportBusinessType.DistributeDetails , s=> GetDistributeDetailsQuery() },
                {ExportBusinessType.MaterielDetails , s=> GetMaterielDetailsQuery() }
            };
            return distributeDic;
        }

        private void SaveExcelToFile(dynamic list, string dicFilePath, string fileName)
        {
            var headMsg = GetTemplateHeadMsg();
            switch (_contextDistributeExport.BusinessType)
            {
                case ExportBusinessType.Distribute:
                    //分发列表

                    new ExcelHelper<ExportDistributeDto>().SaveExcelToFile(list, dicFilePath, fileName, headMsg);
                    break;

                case ExportBusinessType.DistributeDetails:
                    if (_contextDistributeExport.ExportType == ExportTypeEnum.Daily)
                    {
                        new ExcelHelper<ExportDistributeDetailedDto>().SaveExcelToFile(list, dicFilePath, fileName, headMsg);
                    }
                    else
                    {
                        new ExcelHelper<ExportDistributeChannelDto>().SaveExcelToFile(list, dicFilePath, fileName, headMsg);
                    }
                    break;

                case ExportBusinessType.MaterielDetails:
                    if (_contextDistributeExport.DistributeType == (int)DistributeTypeEnum.QuanWangYu)
                        new ExcelHelper<ExportStatisticsChiTuDto>().SaveExcelToFile(list, dicFilePath, fileName, headMsg);
                    else
                    {
                        new ExcelHelper<ExportStatisticsQingNiaoAgentDto>().SaveExcelToFile(list, dicFilePath, fileName, headMsg);
                    }
                    break;
            }
        }

        public Tuple<dynamic, string> GetQuery()
        {
            if (_queryDic.Value.ContainsKey(_contextDistributeExport.BusinessType))
            {
                return _queryDic.Value[_contextDistributeExport.BusinessType].Invoke(string.Empty);
            }
            else
            {
                throw new ExportBusinessTypeException();
            }
        }

        /// <summary>
        /// 物料分发列表导出
        /// </summary>
        /// <returns></returns>
        private Tuple<dynamic, string> GetDistribute()
        {
            //_contextDistributeExport.PageSize = 50;
            var exportFileName = $"物料分发-{_todayDate}.xlsx";
            var providerQuery = new DistributeQuery(new ConfigEntity());
            var distributeList = providerQuery.GetQueryList(_contextDistributeExport);

            return GetDistributeForNextPage(distributeList.List, distributeList.TotalCount, _contextDistributeExport.PageIndex,
                exportFileName, providerQuery);
        }

        private Tuple<dynamic, string> GetDistributeForNextPage(List<RespDistributeListDto> respDistributeList, int totleCount,
            int pageIndex, string exportFileName, DistributeQuery query)
        {
            if (totleCount == pageIndex)
            {
                return new Tuple<dynamic, string>(
                    AutoMapper.Mapper.Map<List<RespDistributeListDto>, List<ExportDistributeDto>>(respDistributeList), exportFileName);
            }
            //var pageTotle = GetOffsetPageCount(totleCount, _contextDistributeExport.PageSize);
            //pageIndex++;
            //for (var i = pageIndex; i <= pageTotle; i++)
            //{
            //    _contextDistributeExport.PageIndex = i;
            //    respDistributeList.AddRange(query.GetQueryList(_contextDistributeExport).List);
            //}
            _contextDistributeExport.PageIndex = 1;
            _contextDistributeExport.PageSize = totleCount;
            respDistributeList.Clear();
            respDistributeList.AddRange(query.GetQueryList(_contextDistributeExport, "").List);
            if (!respDistributeList.Any())
            {
                return new Tuple<dynamic, string>(null, string.Empty);
            }
            return new Tuple<dynamic, string>(
                    AutoMapper.Mapper.Map<List<RespDistributeListDto>, List<ExportDistributeDto>>(respDistributeList),
                    exportFileName);
        }

        /// <summary>
        /// 分发明细导出(赤兔有渠道，其他暂无渠道)
        /// </summary>
        /// <returns></returns>
        private Tuple<dynamic, string> GetDistributeDetailsQuery()
        {
            string exportFileName;

            if (_contextDistributeExport.MaterielId <= 0)
                return new Tuple<dynamic, string>(null, "请输入：MaterielId");
            if (_contextDistributeExport.DistributeType < 0)
            {
                return new Tuple<dynamic, string>(null, "请输入合法的DistributeType");
            }
            if (_contextDistributeExport.ExportType == ExportTypeEnum.Daily)
            {
                //todo:1.现在直接查询出当前物料id下面所有的数据，分页以后数据量大了再优化
                var distributeDetailed = Dal.Distribute.MaterielDistributeDetailed.Instance.GetList(
                      new DistributeQuery<MaterielDistributeDetailed>()
                      {
                          MaterielId = _contextDistributeExport.MaterielId,
                          Source = _contextDistributeExport.DistributeType,
                          StartDate = _contextDistributeExport.StartDate,
                          EndDate = _contextDistributeExport.EndDate
                      });
                if (!distributeDetailed.Any())
                {
                    return new Tuple<dynamic, string>(null, string.Empty);
                }
                exportFileName = $"{_contextDistributeExport.MaterielId}-日数据-{_contextDistributeExport.StartDate}.xlsx";
                return new Tuple<dynamic, string>(
                    AutoMapper.Mapper
                        .Map<List<MaterielDistributeDetailed>, List<ExportDistributeDetailedDto>>(
                            distributeDetailed), exportFileName);
            }
            else
            {
                //todo:1.日结汇总下面的渠道
                var channelDetailed = Dal.Distribute.MaterielChannelDetailed.Instance.GetChannleList(
                     new DistributeQuery<MaterielChannelDetailed>()
                     {
                         MaterielId = _contextDistributeExport.MaterielId,
                         Source = _contextDistributeExport.DistributeType,
                         StartDate = _contextDistributeExport.StartDate,
                         EndDate = _contextDistributeExport.EndDate
                     });
                if (!channelDetailed.Any())
                {
                    return new Tuple<dynamic, string>(null, string.Empty);
                }
                exportFileName = $"{_contextDistributeExport.MaterielId}-渠道数据-{_contextDistributeExport.StartDate}.xlsx";
                return new Tuple<dynamic, string>(
                  AutoMapper.Mapper
                      .Map<List<MaterielChannelDetailed>, List<ExportDistributeChannelDto>>(
                          channelDetailed), exportFileName);
            }
        }

        private Tuple<dynamic, string> GetMaterielDetailsQuery()
        {
            if (_contextDistributeExport.DistributeType == (int)DistributeTypeEnum.QuanWangYu)
            {
                return GetMaterielDetailsQueryByChitu();
            }
            else if (_contextDistributeExport.DistributeType == (int)DistributeTypeEnum.QingNiaoAgent)
            {
                return GetMaterielDetailsQueryByQingNiaoAgent();
            }
            else
            {
                return new Tuple<dynamic, string>(null, "请输入：DistributeType");
            }
        }

        private Tuple<dynamic, string> GetMaterielDetailsQueryByChitu()
        {
            //todo:1.查询统计详情
            if (_contextDistributeExport.MaterielId <= 0)
                return new Tuple<dynamic, string>(null, "请输入：MaterielId");
            var detailedStatistics = Dal.Distribute.MaterielDetailedStatistics.Instance.GetStatisticsDetailedsList(
                new DistributeQuery<MaterielDetailedStatistics>()
                {
                    MaterielId = _contextDistributeExport.MaterielId,
                    Source = _contextDistributeExport.DistributeType,
                    StartDate = _contextDistributeExport.StartDate,
                    EndDate = _contextDistributeExport.EndDate
                });
            if (!detailedStatistics.Any())
            {
                return new Tuple<dynamic, string>(null, string.Empty);
            }
            var exportFileName = $"{_contextDistributeExport.MaterielId}-明细-{_todayDate}-（赤兔）.xlsx";
            return new Tuple<dynamic, string>(
                AutoMapper.Mapper
                     .Map<List<MaterielDetailedStatistics>, List<ExportStatisticsChiTuDto>>(
                         detailedStatistics), exportFileName);
        }

        private Tuple<dynamic, string> GetMaterielDetailsQueryByQingNiaoAgent()
        {
            //todo:1.查询统计详情
            if (_contextDistributeExport.MaterielId <= 0)
                return new Tuple<dynamic, string>(null, "请输入：MaterielId");
            var query = new RequestDistributeQueryDto()
            {
                DistributeType = _contextDistributeExport.DistributeType,
                MaterielId = _contextDistributeExport.MaterielId,
                StartDate = _contextDistributeExport.StartDate,
                EndDate = _contextDistributeExport.EndDate,
                PageSize = 50
            };
            var provider = new DetailedStatisticsQuery(null);
            var resp = provider.GetQueryList(query);

            if (resp.List == null || !resp.List.Any())
            {
                return new Tuple<dynamic, string>(null, string.Empty);
            }
            var exportDtos = new List<ExportStatisticsQingNiaoAgentDto>();
            resp.List.ForEach(s =>
            {
                s.Item.ForEach(t =>
                {
                    var dto = new ExportStatisticsQingNiaoAgentDto
                    {
                        Date = s.Date.ToString("yyyy-MM-dd"),
                        ArticleTypeName = t.ArticleTypeName,
                        ReadNumber = DistributeProfile.GetExportReplaceText(t.ReadNumber),
                        ForwardNumber = DistributeProfile.GetExportReplaceText(t.ForwardNumber),
                        LikeNumber = DistributeProfile.GetExportReplaceText(t.LikeNumber),
                        Pv = DistributeProfile.GetExportReplaceText(t.PV),
                        Uv = DistributeProfile.GetExportReplaceText(t.UV),
                        ClikcPv = "--",
                        ClikcUv = "--",
                        Title = DetailedStatisticsRsolver.GetExportTitle(t.ConentType, t.ConentTypeName, t.ArticleTypeName, t.Title)
                    };
                    exportDtos.Add(dto);
                    t.FootStatistics.ForEach(m =>
                    {
                        dto = new ExportStatisticsQingNiaoAgentDto
                        {
                            Date = s.Date.ToString("yyyy-MM-dd"),
                            ArticleTypeName = m.ArticleTypeName,
                            ForwardNumber = "--",
                            LikeNumber = "--",
                            ReadNumber = DistributeProfile.GetExportReplaceText(t.ReadNumber),
                            ClikcPv = DistributeProfile.GetExportReplaceText(m.ClickPV),
                            ClikcUv = DistributeProfile.GetExportReplaceText(m.ClickUV),
                            Pv = "--",
                            Uv = "--",
                            Title = m.ConentTypeName + "：" + m.ArticleTypeName
                        };
                        exportDtos.Add(dto);
                    });
                }
                );
            });

            var totleNumber = resp.TotalCount;
            var offestCount = GetOffsetPageCount(totleNumber, query.PageSize);
            query.PageIndex++;
            while (offestCount >= query.PageIndex)
            {
                resp = provider.GetQueryList(query);

                if (resp.List == null || !resp.List.Any())
                {
                    break;
                }
                resp.List.ForEach(s =>
                {
                    s.Item.ForEach(t =>
                    {
                        var dto = new ExportStatisticsQingNiaoAgentDto
                        {
                            Date = s.Date.ToString("yyyy-MM-dd"),
                            ArticleTypeName = t.ArticleTypeName,
                            ForwardNumber = DistributeProfile.GetExportReplaceText(t.ForwardNumber),
                            LikeNumber = DistributeProfile.GetExportReplaceText(t.LikeNumber),
                            Pv = DistributeProfile.GetExportReplaceText(t.PV),
                            Uv = DistributeProfile.GetExportReplaceText(t.UV),
                            Title = DetailedStatisticsRsolver.GetExportTitle(t.ConentType, t.ConentTypeName, t.ArticleTypeName, t.Title)
                        };
                        exportDtos.Add(dto);
                    }
                    );
                });
            }
            var exportFileName = $"{_contextDistributeExport.MaterielId}-明细-{_todayDate}-（经纪人）.xlsx";
            return new Tuple<dynamic, string>(
               exportDtos, exportFileName);
        }

        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GetPath(string fileName)
        {
            //UploadLoad
            string relatedPath = $"{UpladFilesPath}Temp/ExportExcel/Materiel_{DateTime.Now.ToString("yyyyMMddHHmmss")}/";
            var webFilePath = relatedPath + fileName;
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(dir, webFilePath);
        }

        public string GetTemplateHeadMsg()
        {
            var source = _contextDistributeExport.DistributeType > 0 ? ((DistributeTypeEnum)_contextDistributeExport.DistributeType).GetDescription() : "全网域/经纪人系统";
            if (_contextDistributeExport.BusinessType == ExportBusinessType.Distribute)
            {
                var expChildIpName = _contextDistributeExport.ExpChildIpName.IndexOf("--", StringComparison.Ordinal) >= 0
                    ? "未选择"
                    : _contextDistributeExport.ExpChildIpName;
                var expIpName = _contextDistributeExport.ExpIpName.IndexOf("--", StringComparison.Ordinal) >= 0
                    ? "未选择"
                    : _contextDistributeExport.ExpIpName;
                var expChannelName = _contextDistributeExport.ExpChannelName.IndexOf("--", StringComparison.Ordinal) >= 0
                    ? "未选择"
                    : _contextDistributeExport.ExpChannelName;
                var expCarSerialName = _contextDistributeExport.ExpCarSerialName.IndexOf("--", StringComparison.Ordinal) >= 0
                   ? "未选择"
                   : _contextDistributeExport.ExpCarSerialName;
                return
                    $@"
分发时间：{_contextDistributeExport.StartDate}—{_contextDistributeExport.EndDate}
分发渠道类型：{source}
分发操作人：{_contextDistributeExport.DistributeUser ?? "未选择"}
组装操作人：{_contextDistributeExport.AssembleUser ?? "未选择"}
品牌车型：{expCarSerialName}
场景/渠道：{expChannelName}
IP：{expIpName}
子IP：{ expChildIpName}
                    ";
            }
            else
            {
                var info = new DistributeProvider().GetMaterielInfo(_contextDistributeExport.MaterielId, _contextDistributeExport.DistributeType);
                if (info != null)
                    return $@"
物料ID：{_contextDistributeExport.MaterielId}
文章标题：{info.Title}
落地页URL：{info.Url}
分发时间：{info.DistributeTime}
分发渠道类型：{info.DistributeTypeName}
                    ";
                else
                {
                    return $@"
物料ID：{_contextDistributeExport.MaterielId}
文章标题：
落地页URL：
分发时间：
业务类型：
                    ";
                }
            }
        }
    }
}