/********************************************************
*创建人：lixiong
*创建时间：2017/11/27 14:31:06
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Primary;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:初筛
    /// </summary>
    public class StatTabPrimaryProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载
        private static List<string> _latelyDaysList;

        public StatTabPrimaryProvider(int latelyDays, string contextType)
        {
            _latelyDays = latelyDays;
            _contextType = contextType;
            _latelyDaysList = DataViewProvider.GetLatelyDays(_latelyDays);
            _queryDic = new Lazy<Dictionary<string, Func<int, dynamic>>>(Init);
        }

        #region 初始化

        private Dictionary<string, Func<int, dynamic>> Init()
        {
            var dic = new Dictionary<string, Func<int, dynamic>>()
            {
                { GetCsTypeEnum.cs_pie_nest.ToString(), s => GetCsLeftPieNest()},
                { GetCsTypeEnum.cs_bar_brush.ToString(), s => GetCsRightBarBrush()},
                { GetCsTypeEnum.cs_head_cj.ToString(), s => GetCsHeadArticelCj()},
                 { GetCsTypeEnum.cs_head_zhcj.ToString(), s => GetCsHeadAccountCj()},
                   { GetCsTypeEnum.cs_head_wzfz.ToString(), s => GetCsHeadArticleFenZhi()},
                    { GetCsTypeEnum.cs_head_zhfz.ToString(), s => GetCsHeadAccountFenZhi()},
                    { GetCsTypeEnum.cs_head_wzzh.ToString(), s => GetCsArticleZhuanHua()}
            };
            return dic;
        }

        /// <summary>
        /// 返回数据的字段存储
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> GetGrabReturnDic()
        {
            return new Dictionary<string, dynamic>()
            {
               { GetCsTypeEnum.cs_pie_nest.ToString(), null},
                { GetCsTypeEnum.cs_bar_brush.ToString(), null},
                { GetCsTypeEnum.cs_head_cj.ToString(), null},
                 { GetCsTypeEnum.cs_head_zhcj.ToString(), null},
                   { GetCsTypeEnum.cs_head_wzfz.ToString(), null},
                    { GetCsTypeEnum.cs_head_zhfz.ToString(), null},
                     { GetCsTypeEnum.cs_head_wzzh.ToString(), null}
            };
        }

        /// <summary>
        /// 为了一次返回多个图表请求数据，dic 直接序列化
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> GetGrabData()
        {
            if (string.IsNullOrWhiteSpace(_contextType))
            {
                throw new ExportBusinessTypeException("请输入合法的Category");
            }
            var spType = _contextType.Split(',');
            var grabDic = GetGrabReturnDic();
            foreach (var s in spType)
            {
                if (_queryDic.Value.ContainsKey(s))
                {
                    grabDic[s] = _queryDic.Value[s].Invoke(0);
                }
            }

            return grabDic;
        }

        #endregion 初始化

        /// <summary>
        /// 初筛-左边-嵌套环形
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetCsLeftPieNest()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //渠道数据+汇总数据（累计统计+3个状态统计）
            var statPrimaryList = Dal.Statistics.StatPrimaryStatistics.Instance.GetStatPrimarySummaries(
                new StatSpiderQuery<Entities.Statistics.StatPrimarySummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterChannelId = OperatorEnum.大于等于,
                    FilterSceneId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.等于
                });

            var respDto = new RespCarHeadDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                Data = new List<StatKeyValueCarMatchData>()
            };
            //头部顶上的汇总(累计统计：)
            var statTotalList = statPrimaryList.Where(s => s.ChannelId == 0);
            respDto.Info =
                AutoMapper.Mapper.Map<List<Entities.Statistics.StatPrimarySummary>, List<StatInfo>>(statTotalList.ToList());
            //圆环-圆心(需要统计各渠道下面 3种状态下的总和，每种渠道会有3条数据存储)
            var statCenterCircle = statPrimaryList.Where(s => s.ChannelId > 0).OrderBy(s => s.ChannelId);

            respDto.DataLegend = statCenterCircle.Select(s => s.ChannelName).Distinct().ToArray();

            //todo:2.取出对应渠道的信息
            var gbList = statCenterCircle.GroupBy(i => i.ChannelId, (i, v) => new
            {
                ChannelId = i,
                list = v
            });
            //圆心：每种渠道会有3条数据存储,需要把每一种渠道累加
            foreach (var item in gbList.Select(t => new StatKeyValueCarMatchData()
            {
                TypeId = 0,//圆心数据为0
                TypeMatchId = 0,//圆心数据不关心状态
                name = t.list.Select(m => m.ChannelName).FirstOrDefault(),
                value = t.list.Sum(s => s.ArticleCount)
            }))
            {
                respDto.Data.Add(item);
            }

            //外环数据
            respDto.Data.AddRange(statCenterCircle.Select(s => new StatKeyValueCarMatchData
            {
                name = s.ChannelName,//这里的name 种类很多，直接取库里的，微信（可用，作废，置为腰）
                value = s.ArticleCount,
                TypeId = s.ChannelId,
                TypeMatchId = s.ConditionId
            }).ToList());

            respDto.Data = respDto.Data.OrderBy(s => s.TypeId).ThenBy(s => s.TypeMatchId).ToList();
            return respDto;
        }

        /// <summary>
        /// 初筛-右边-正负条形图
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarExtendDto<StatTypeData> GetCsRightBarBrush()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statCarMatchList = Dal.Statistics.StatPrimaryStatistics.Instance.GetStatPrimaryStatisticses(
                new StatSpiderQuery<Entities.Statistics.StatPrimaryStatistics>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterChannelId = OperatorEnum.大于,
                });

            var respDto = new RespCarHeadRightBarExtendDto<StatTypeData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                DicInfo = new List<Dto.Response.Grab.DicInfo>()
            };
            respDto.DataLegend = _latelyDaysList.ToArray();
            //todo:从数据源取分类（可用、作废、置为腰）
            var groupByConditionIdList = statCarMatchList.GroupBy(s => s.ConditionId, (s, v) => new
            {
                ConditionId = s,
                List = v
            }).ToList();

            var respMatchDic = new List<DicTypeInfo>();
            groupByConditionIdList.ForEach(s =>
            {
                var dicInfo = new DicTypeInfo
                {
                    TypeMatchId = s.ConditionId,
                    Name = s.List.Select(t => t.ConditionName).FirstOrDefault()
                };
                respMatchDic.Add(dicInfo);
            });
            respDto.DicInfoMatch = respMatchDic;

            //todo:2.GroupBy渠道，然后分组取出对应渠道的信息
            respDto.Data = new List<StatTypeData>();

            #region 日期补全

            var groupByChannelIdList = statCarMatchList.GroupBy(s => s.ChannelId, (s, v) => new
            {
                ChannelId = s,
                List = v
            }).ToList();

            groupByChannelIdList.ForEach(s =>
            {
                respDto.DicInfo.Add(new Dto.Response.Grab.DicInfo
                {
                    TypeId = s.ChannelId,
                    Name = s.List.Select(m => m.ChannelName).FirstOrDefault()
                });
                respMatchDic.ForEach(k =>
                {
                    var dto1 = new StatTypeData()
                    {
                        TypeId = s.ChannelId,
                        name = s.List.Select(t => t.ChannelName).FirstOrDefault(),
                        TypeMatchId = k.TypeMatchId,
                    };
                    var dataList = new List<int>();
                    //7天或30天维度出发
                    _latelyDaysList.ForEach(t =>
                    {
                        var anyData = s.List.Where(a => a.Date.ToString("yyyy-MM-dd") == t
                                        && a.ChannelId == s.ChannelId
                                        && a.ConditionId == k.TypeMatchId).ToList();
                        dataList.Add(anyData.Any() ? anyData.First().ArticleCount : 0);
                    });
                    dto1.data = dataList.ToArray();

                    respDto.Data.Add(dto1);
                });
            });

            #endregion 日期补全

            return respDto;
        }

        /// <summary>
        /// 初筛-初筛头部文章在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetCsHeadArticelCj()
        {
            return GetPrimaryArticlePieCjPackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Articel);
        }

        /// <summary>
        /// 初筛-初筛头部账号在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetCsHeadAccountCj()
        {
            return GetPrimaryArticlePieCjPackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Account);
        }

        /// <summary>
        /// 初筛-初筛头部文章在文章分值上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetCsHeadArticleFenZhi()
        {
            return GetPrimaryFenZhiPiePackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Articel, ScoreTypeEnum.文章分值类型);
        }

        /// <summary>
        /// 初筛-初筛头部账号在账号分值上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetCsHeadAccountFenZhi()
        {
            return GetPrimaryFenZhiPiePackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Account, ScoreTypeEnum.账号分值类型);
        }

        /// <summary>
        /// 初筛-初筛头部文章（腰部文章）在场景上的分布
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <param name="getTotalCountTypeEnum"></param>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetPrimaryArticlePieCjPackageMethod(
            StatArticleTypeEnum articleTypeEnum, GetTotalCountTypeEnum getTotalCountTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //场景数据-头部
            var statPrimaryList = Dal.Statistics.StatPrimaryStatistics.Instance.GetStatPrimarySummaries(
                new StatSpiderQuery<Entities.Statistics.StatPrimarySummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterSceneId = OperatorEnum.大于,
                    FilterChannelId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.等于,
                    //ArticleType = articleTypeEnum
                });

            var respDto = new RespCarHeadRightBarDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //todo:从数据源取分类（可用、作废、置为腰）
            var groupByConditionIdList = statPrimaryList.GroupBy(s => s.ConditionId, (s, v) => new
            {
                ConditionId = s,
                List = v
            });
            respDto.DicInfo = new List<Dto.Response.Grab.DicInfo>();
            groupByConditionIdList.ToList().ForEach(s =>
            {
                var dicInfo = new Dto.Response.Grab.DicInfo
                {
                    TypeId = s.ConditionId,
                    Name = s.List.Select(t => t.ConditionName).FirstOrDefault()
                };
                respDto.DicInfo.Add(dicInfo);
            });

            var statPrimaryOrderByList = statPrimaryList.OrderBy(s => s.SceneId);

            respDto.DataLegend = statPrimaryOrderByList.Select(s => s.SceneName).Distinct().ToArray();
            //todo:dicInfo里面的TypeId 去匹配TypeMatchId
            if (getTotalCountTypeEnum == GetTotalCountTypeEnum.Articel)
            {
                respDto.Data = statPrimaryOrderByList.Select(s => new StatKeyValueCarMatchData
                {
                    TypeId = s.ConditionId,
                    name = s.SceneName,
                    value = s.ArticleCount,
                    //TypeMatchId = s.ConditionId
                }).OrderByDescending(s => s.value).ToList();
            }
            else
            {
                respDto.Data = statPrimaryOrderByList.Select(s => new StatKeyValueCarMatchData
                {
                    TypeId = s.ConditionId,
                    name = s.SceneName,
                    value = s.AccountCount,
                    //TypeMatchId = s.ConditionId
                }).OrderByDescending(s => s.value).ToList();
            }

            return respDto;
        }

        /// <summary>
        /// 初筛-初筛头部文章在账号分值上的分布
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <param name="getTotalCountTypeEnum"></param>
        /// <param name="scoreTypeEnum"></param>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetPrimaryFenZhiPiePackageMethod(
            StatArticleTypeEnum articleTypeEnum,
            GetTotalCountTypeEnum getTotalCountTypeEnum,
            ScoreTypeEnum scoreTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //场景数据-头部
            var statPrimaryList = Dal.Statistics.StatPrimaryStatistics.Instance.GetStatPrimarySummaries(
                new StatSpiderQuery<Entities.Statistics.StatPrimarySummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterSceneId = OperatorEnum.等于,
                    FilterChannelId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.大于,
                    ScoreType = scoreTypeEnum,
                    //ArticleType = articleTypeEnum
                });

            var respDto = new RespCarHeadRightBarDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };

            //todo:从数据源取分类（可用、作废、置为腰）
            var groupByConditionIdList = statPrimaryList.GroupBy(s => s.ConditionId, (s, v) => new
            {
                ConditionId = s,
                List = v
            });
            respDto.DicInfo = new List<Dto.Response.Grab.DicInfo>();
            groupByConditionIdList.ToList().ForEach(s =>
            {
                var dicInfo = new Dto.Response.Grab.DicInfo
                {
                    TypeId = s.ConditionId,
                    Name = s.List.Select(t => t.ConditionName).FirstOrDefault()
                };
                respDto.DicInfo.Add(dicInfo);
            });

            var statPrimaryOrderByList = statPrimaryList.OrderBy(s => s.AaScoreType);

            respDto.DataLegend = statPrimaryOrderByList.Select(s => s.AAScoreTypeName).Distinct().ToArray();

            if (getTotalCountTypeEnum == GetTotalCountTypeEnum.Articel)
            {
                respDto.Data = statPrimaryOrderByList.Select(s => new StatKeyValueCarMatchData
                {
                    TypeId = s.ConditionId,
                    name = s.AAScoreTypeName,
                    value = s.ArticleCount,
                    //TypeMatchId = s.ConditionId
                }).OrderByDescending(s => s.value).ToList();
            }
            else
            {
                respDto.Data = statPrimaryOrderByList.Select(s => new StatKeyValueCarMatchData
                {
                    TypeId = s.ConditionId,
                    name = s.AAScoreTypeName,
                    value = s.AccountCount,
                    //TypeMatchId = s.ConditionId
                }).OrderByDescending(s => s.value).ToList();
            }

            return respDto;
        }

        public RespCsZhuanHuaDto GetCsArticleZhuanHua()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //场景数据-头部
            var statPrimaryList = Dal.Statistics.StatPrimaryStatistics.Instance.GetStatPrimarySummariesZhuanHua(startDate, endDate);
            var respDto = new RespCsZhuanHuaDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                Info = new List<StatInfo>()
            };
            //todo:1.先区分：ConditionId>0：可用入库文章数，ConditionId<=0：初筛文章数，
            //todo:2.区分：TypeId>0:初筛文章数(左边的列表，不包括总数)，TypeId<=0:初筛文章数总数（中间的）

            var dataLeft = statPrimaryList.Where(s => s.TypeId == 1);
            var dataRight = statPrimaryList.Where(s => s.TypeId == 2);

            var gbDataLeft = dataLeft.Where(s => s.ChannelId > 0).GroupBy(i => i.ChannelId, (i, v) => new
            {
                ChannelId = i,
                list = v,//这里的是一个集合，它应该是延时的，不能使用ToList()将它在代码块中变为立即执行的，同理不能使用First(),FirstOrDefault等实时查询的方法
            }).ToList();
            //因为抓取文章数 3种可用、作废、置为腰状态，会有3条数据，要sum一下
            foreach (var item in gbDataLeft.Select(t => new StatInfo()
            {
                TypeId = 0,
                Name = t.list.Select(m => m.ChannelName).FirstOrDefault(),
                ArticleCount = t.list.Sum(s => s.ArticleCount),
                AccountCount = t.list.Sum(s => s.AccountCount)
            }))
            {
                respDto.Info.Add(item);
            }

            foreach (var item in dataRight.Where(s => s.ChannelId > 0).Select(t => new StatInfo()
            {
                TypeId = 1,
                Name = t.ChannelName,
                ArticleCount = t.ArticleCount,
                AccountCount = t.AccountCount
            }))
            {
                respDto.Info.Add(item);
            }

            //ChannelId == 0 累计统计
            respDto.TotalInfo = new List<StatInfo>()
            {
                new StatInfo()
                {
                    Name = "初筛文章总数",
                    ArticleCount = statPrimaryList.Where(s=>s.ChannelId==0 && s.ConditionId ==0 && s.TypeId ==1).Select(s=>s.ArticleCount)
                    .FirstOrDefault(),
                    TypeId = 0,
                },
                new StatInfo()
                {
                    Name = "可用入库文章总数",
                    ArticleCount = statPrimaryList.Where(s => s.ConditionId == (int)ConditionTypeEnum.可用 && s.ChannelId==0 && s.TypeId ==1)
                    .Select(s=>s.ArticleCount).FirstOrDefault(),
                    TypeId = 1
                }
            };

            return respDto;
        }
    }
}