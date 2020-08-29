/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 9:52:12
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:人工清洗
    /// </summary>
    public class StatTabArtificialProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载
        private static List<string> _latelyDaysList;

        public StatTabArtificialProvider(int latelyDays, string contextType)
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
                { GetArtificialTypeEnum.rgqx_head_pie_nest.ToString(), s => GetRgqxHeadLeftPieNest()},
                { GetArtificialTypeEnum.rgqx_head_bar_brush.ToString(), s => GetRgqxHeadRightBarBrush()},
                { GetArtificialTypeEnum.rgqx_body_pie_nest.ToString(), s => GetRgqxBodyLeftPieNest()},
                { GetArtificialTypeEnum.rgqx_body_bar_brush.ToString(), s => GetRgqxBodyRightBarBrush()},
                { GetArtificialTypeEnum.rgqx_head_wzcj.ToString(), s => GetRgqxHeadArticleAtCj()},
                { GetArtificialTypeEnum.rgqx_head_zhcj.ToString(), s => GetRgqxHeadAccountAtCj()},
                { GetArtificialTypeEnum.rgqx_head_wzfz.ToString(), s => GetRgqxHeadArticleAtArticleFenZhi()},
                { GetArtificialTypeEnum.rgqx_head_zhfz.ToString(), s => GetRgqxHeadAccountAtAccountFenZhi()},
                { GetArtificialTypeEnum.rgqx_body_wzlb.ToString(), s => GetRgqxBodyArticleCategory()}
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
               { GetArtificialTypeEnum.rgqx_head_pie_nest.ToString(), null},
                { GetArtificialTypeEnum.rgqx_head_bar_brush.ToString(), null},
                { GetArtificialTypeEnum.rgqx_body_pie_nest.ToString(), null},
                { GetArtificialTypeEnum.rgqx_body_bar_brush.ToString(), null},
                { GetArtificialTypeEnum.rgqx_head_wzcj.ToString(), null},
                { GetArtificialTypeEnum.rgqx_head_zhcj.ToString(), null},
                { GetArtificialTypeEnum.rgqx_head_wzfz.ToString(), null},
                { GetArtificialTypeEnum.rgqx_head_zhfz.ToString(), null},
                { GetArtificialTypeEnum.rgqx_body_wzlb.ToString(), null}
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

        #region 头部文章系列

        /// <summary>
        /// 人工清洗-头部文章-左边-嵌套环形
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetRgqxHeadLeftPieNest()
        {
            return GetRgqxHeadLeftPieNestPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 人工清洗-头部文章-右边-正负条形图
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarExtendDto<StatTypeData> GetRgqxHeadRightBarBrush()
        {
            return GetRgqxHeadRightBarBrushPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 人工清洗-头部文章-清洗头部文章在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxHeadArticleAtCj()
        {
            return GetRgqxHeadArticleAccountPieCjPackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Articel);
        }

        /// <summary>
        /// 人工清洗-头部-头部帐号在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxHeadAccountAtCj()
        {
            return GetRgqxHeadArticleAccountPieCjPackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Account);
        }

        /// <summary>
        /// 清洗头部文章在文章分值上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxHeadArticleAtArticleFenZhi()
        {
            return GetRgqxHeadArticleAccountFenZhiPiePackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Articel,
                 ScoreTypeEnum.文章分值类型);
        }

        /// <summary>
        /// 清洗头部账号在账号分值上的分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxHeadAccountAtAccountFenZhi()
        {
            return GetRgqxHeadArticleAccountFenZhiPiePackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Account,
                 ScoreTypeEnum.账号分值类型);
        }

        #endregion 头部文章系列

        #region 腰部文章系列

        /// <summary>
        /// 人工清洗-腰部文章-左边-嵌套环形
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetRgqxBodyLeftPieNest()
        {
            return GetRgqxHeadLeftPieNestPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 人工清洗-腰部文章-右边-正负条形图
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarExtendDto<StatTypeData> GetRgqxBodyRightBarBrush()
        {
            return GetRgqxHeadRightBarBrushPackageMethod(StatArticleTypeEnum.Body);
        }

        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxBodyArticleCategory()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //场景数据-头部
            var statPrimaryList = Dal.Statistics.StatArtificialStatistics.Instance.GetStatArtificialSummaries(
                new StatSpiderQuery<Entities.Statistics.StatArtificialSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterSceneId = OperatorEnum.等于,
                    FilterChannelId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.等于,
                    ScoreType = ScoreTypeEnum.无,
                    ArticleType = StatArticleTypeEnum.Body,
                    FilterAutoCategory = OperatorEnum.不等于NULL
                });

            var respDto = new RespCarHeadRightBarDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            respDto.DataLegend = statPrimaryList.Select(s => s.Category).Distinct().ToArray();

            respDto.Data = statPrimaryList.Select(s => new StatKeyValueCarMatchData
            {
                name = s.Category,
                value = s.ArticleCount,
                //TypeMatchId = s.ConditionId
            }).OrderByDescending(s => s.value).ToList();

            return respDto;
        }

        #endregion 腰部文章系列

        #region 方法封装

        /// <summary>
        /// 人工清洗-头部文章（腰部文章）-左边-嵌套环形
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetRgqxHeadLeftPieNestPackageMethod(StatArticleTypeEnum statArticleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //渠道数据+汇总数据（累计统计+3个状态统计）
            var statTpList = Dal.Statistics.StatArtificialStatistics.Instance.GetStatArtificialHeadSummaries(
               startDate, endDate, statArticleTypeEnum);

            var respDto = new RespCarHeadDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //头部顶上的汇总(累计统计：)
            respDto.Info =
                AutoMapper.Mapper.Map<List<Entities.Statistics.StatArtificialSummary>, List<StatInfo>>(statTpList.Item1);
            //圆环-圆心(需要统计各渠道下面 3种状态下的总和，每种渠道会有3条数据存储)
            var statCenterCircle = statTpList.Item2.OrderBy(s => s.ChannelId);
            respDto.DataLegend = statCenterCircle.Select(s => s.ChannelName).Distinct().ToArray();
            respDto.Data = new List<StatKeyValueCarMatchData>();
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
        /// 人工清洗-头部文章（腰部文章）-右边-正负条形图
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarExtendDto<StatTypeData> GetRgqxHeadRightBarBrushPackageMethod(StatArticleTypeEnum statArticleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statCarMatchList = Dal.Statistics.StatArtificialStatistics.Instance.GetStatArtificialStatisticses(
                new StatSpiderQuery<Entities.Statistics.StatArtificialStatistics>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterSceneId = OperatorEnum.等于,
                    FilterChannelId = OperatorEnum.大于,
                    ArticleType = statArticleTypeEnum
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
            });
            var respMatchDic = new List<DicTypeInfo>();

            groupByConditionIdList.ToList().ForEach(s =>
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
        /// 人工清洗-头部文章在场景上的分布-（头部文章，头部帐号 2列）
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <param name="getTotalCountTypeEnum"></param>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxHeadArticleAccountPieCjPackageMethod(
            StatArticleTypeEnum articleTypeEnum, GetTotalCountTypeEnum getTotalCountTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //场景数据-头部
            var statPrimaryList = Dal.Statistics.StatArtificialStatistics.Instance.GetStatArtificialSummaries(
                new StatSpiderQuery<Entities.Statistics.StatArtificialSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterSceneId = OperatorEnum.大于,
                    FilterChannelId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.等于,
                    ArticleType = articleTypeEnum
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
        /// 人工清洗-清洗头部文章在文章分值上的分布,（头部文章，头部帐号）
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <param name="getTotalCountTypeEnum"></param>
        /// <param name="scoreTypeEnum"></param>
        /// <returns></returns>
        public RespCarHeadRightBarDto<StatKeyValueCarMatchData> GetRgqxHeadArticleAccountFenZhiPiePackageMethod(
            StatArticleTypeEnum articleTypeEnum,
            GetTotalCountTypeEnum getTotalCountTypeEnum,
            ScoreTypeEnum scoreTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //场景数据-头部
            var statPrimaryList = Dal.Statistics.StatArtificialStatistics.Instance.GetStatArtificialSummaries(
                new StatSpiderQuery<Entities.Statistics.StatArtificialSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterSceneId = OperatorEnum.等于,
                    FilterChannelId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.大于,
                    ScoreType = scoreTypeEnum,
                    ArticleType = articleTypeEnum
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

        #endregion 方法封装
    }
}