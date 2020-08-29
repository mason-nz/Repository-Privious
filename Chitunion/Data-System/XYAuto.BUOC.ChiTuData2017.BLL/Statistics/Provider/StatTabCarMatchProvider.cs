/********************************************************
*创建人：lixiong
*创建时间：2017/11/24 17:13:22
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:车型匹配
    /// </summary>
    public class StatTabCarMatchProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载
        private static List<string> _latelyDaysList;
        private static Lazy<RespCarHeadDto<StatKeyValueCarMatchData>> _getCarMatchData;

        public StatTabCarMatchProvider(int latelyDays, string contextType)
        {
            _latelyDays = latelyDays;
            _contextType = contextType;
            _latelyDaysList = DataViewProvider.GetLatelyDays(_latelyDays);
            _queryDic = new Lazy<Dictionary<string, Func<int, dynamic>>>(Init);
            _getCarMatchData = new Lazy<RespCarHeadDto<StatKeyValueCarMatchData>>(GetCarMatchPackageMethod);
        }

        #region 初始化

        private Dictionary<string, Func<int, dynamic>> Init()
        {
            var dic = new Dictionary<string, Func<int, dynamic>>()
            {
                { GetCarMatchTypeEnum.cxpp_pie_nest.ToString(), s => GetCarMatchLeftPieNest()},
                { GetCarMatchTypeEnum.cxpp_bar_brush.ToString(), s => GetCarMatchRightBarBrush()},
                { GetCarMatchTypeEnum.cxpp_no.ToString(), s => GetCarMatchNo()},
                { GetCarMatchTypeEnum.cxpp_yes.ToString(), s => GetCarMatchYes()}
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
                { GetCarMatchTypeEnum.cxpp_pie_nest.ToString(),null},
                { GetCarMatchTypeEnum.cxpp_bar_brush.ToString(),null},
                { GetCarMatchTypeEnum.cxpp_no.ToString(),null},
                { GetCarMatchTypeEnum.cxpp_yes.ToString(),null}
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
        /// 车型匹配-左边-嵌套环形
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetCarMatchLeftPieNest()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statCarMatchList = Dal.Statistics.StatCarMatchStatistics.Instance.GetStatCarMatchSummaries(
                new StatSpiderQuery<StatCarMatchSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterChannelId = OperatorEnum.大于等于,
                    FilterAutoCategory = OperatorEnum.等于NULL
                });

            var respDto = new RespCarHeadDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                Data = new List<StatKeyValueCarMatchData>()
            };
            //头部顶上的汇总
            var statTotalList = statCarMatchList.FirstOrDefault(s => s.ChannelId == 0);
            respDto.Info = new List<StatInfo>()
                {
                    new StatInfo()
                    {
                        ArticleCount = statTotalList == null ? 0:statTotalList.MatchArticleCount,
                        Name = "已匹配车型"
                    },
                    new StatInfo()
                    {
                        ArticleCount = statTotalList == null ? 0:statTotalList.UnMatchArticleCount,
                        Name = "未匹配车型"
                    }
                };
            //圆环-圆心
            var statCenterCircle = statCarMatchList.Where(s => s.ChannelId > 0).OrderBy(s => s.ChannelId);

            respDto.DataLegend = statCenterCircle.Select(s => s.ChannelName).Distinct().ToArray();

            //todo:2.取出对应渠道的信息

            //已匹配
            respDto.Data = statCenterCircle.Select(s => new StatKeyValueCarMatchData
            {
                name = s.ChannelName,
                value = s.MatchArticleCount,
                TypeId = s.ChannelId,
                TypeMatchId = 1
            }).ToList();
            //未匹配
            respDto.Data.AddRange(statCenterCircle.Select(s => new StatKeyValueCarMatchData
            {
                name = s.ChannelName,
                value = s.UnMatchArticleCount,
                TypeId = s.ChannelId,
                TypeMatchId = 2
            }));
            //圆心
            respDto.Data.AddRange(statCenterCircle.Select(s => new StatKeyValueCarMatchData
            {
                name = s.ChannelName,
                value = s.UnMatchArticleCount + s.MatchArticleCount,
                TypeId = 0,//意思是 圆心
                //TypeMatchId = 2
            }));

            respDto.Data = respDto.Data.OrderBy(s => s.TypeId).ThenBy(s => s.TypeMatchId).ToList();

            return respDto;
        }

        /// <summary>
        /// 车型匹配-右边-正负条形图
        /// </summary>
        /// <returns></returns>
        public RespCarHeadRightBarExtendDto<StatTypeData> GetCarMatchRightBarBrush()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statCarMatchList = Dal.Statistics.StatCarMatchStatistics.Instance.GetStatCarMatchStatisticses(
                new StatSpiderQuery<StatCarMatchStatistics>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterChannelId = OperatorEnum.大于
                });

            var respDto = new RespCarHeadRightBarExtendDto<StatTypeData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                DicInfo = new List<Dto.Response.Grab.DicInfo>(),
            };

            var respMatchDic = new List<DicTypeInfo>()
            {
                new DicTypeInfo() {Name = "已匹配车型", TypeMatchId = 1},
                new DicTypeInfo() {Name = "未匹配车型", TypeMatchId = 2}
            };
            respDto.DicInfoMatch = respMatchDic;

            respDto.DataLegend = _latelyDaysList.ToArray();

            //todo:2.GroupBy渠道，然后分组取出对应渠道的信息
            respDto.Data = new List<StatTypeData>();
            var gbData = statCarMatchList.GroupBy(i => i.ChannelId, (i, v) => new
            {
                ChannelId = i,
                list = v,//这里的是一个集合，它应该是延时的，不能使用ToList()将它在代码块中变为立即执行的，同理不能使用First(),FirstOrDefault等实时查询的方法
            }).ToList();

            #region 日期补全

            gbData.ForEach(s =>
            {
                respDto.DicInfo.Add(new Dto.Response.Grab.DicInfo
                {
                    TypeId = s.ChannelId,
                    Name = s.list.Select(m => m.ChannelName).FirstOrDefault()
                });

                var dtoMatch = new StatTypeData()
                {
                    TypeId = s.ChannelId,
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                    TypeMatchId = 1,//已匹配
                };
                var dtoNotMatch = new StatTypeData()
                {
                    TypeId = s.ChannelId,
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                    TypeMatchId = 2,//未匹配
                };
                var dataListMatch = new List<int>();
                var dataListNotMatch = new List<int>();
                //7天或30天维度出发
                _latelyDaysList.ForEach(t =>
                {
                    var anyData = statCarMatchList.Where(a => a.Date.ToString("yyyy-MM-dd") == t
                                    && a.ChannelId == s.ChannelId).ToList();
                    dataListMatch.Add(anyData.Any() ? anyData.First().MatchArticleCount : 0);
                    dataListNotMatch.Add(anyData.Any() ? anyData.First().UnMatchArticleCount : 0);
                });
                dtoMatch.data = dataListMatch.ToArray();
                dtoNotMatch.data = dataListNotMatch.ToArray();

                respDto.Data.Add(dtoMatch);
                respDto.Data.Add(dtoNotMatch);
            });

            #endregion 日期补全

            return respDto;
        }

        /// <summary>
        /// 车型匹配-已匹配车型文章类别分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetCarMatchYes()
        {
            var value = _getCarMatchData.Value;
            var matchYes = new RespCarHeadDto<StatKeyValueCarMatchData>
            {
                Info = value.Info,
                Date = value.Date,
                Data = value.Data.Where(s => s.TypeMatchId == (int)CarMatchTypeEnum.Yes).OrderByDescending(s => s.value).ToList(),
                DataLegend = value.DataLegend
            };

            return matchYes;
        }

        /// <summary>
        /// 车型匹配-未匹配车型文章类别分布
        /// </summary>
        /// <returns></returns>
        public RespCarHeadDto<StatKeyValueCarMatchData> GetCarMatchNo()
        {
            var value = _getCarMatchData.Value;
            var matchNo = new RespCarHeadDto<StatKeyValueCarMatchData>
            {
                Info = value.Info,
                Date = value.Date,
                Data = value.Data.Where(s => s.TypeMatchId == (int)CarMatchTypeEnum.No).OrderByDescending(s => s.value).ToList(),
                DataLegend = value.DataLegend
            };
            return matchNo;
        }

        public RespCarHeadDto<StatKeyValueCarMatchData> GetCarMatchPackageMethod()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statCarMatchList = Dal.Statistics.StatCarMatchStatistics.Instance.GetStatCarMatchSummaries(
                new StatSpiderQuery<StatCarMatchSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    FilterChannelId = OperatorEnum.等于,
                    FilterAutoCategory = OperatorEnum.不等于NULL
                });

            var respDto = new RespCarHeadDto<StatKeyValueCarMatchData>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                DataLegend = statCarMatchList.Select(s => s.Category).Distinct().ToArray(),
                Data = new List<StatKeyValueCarMatchData>()
            };

            //todo:2.取出对应渠道的信息
            //已匹配
            respDto.Data = statCarMatchList.Select(s => new StatKeyValueCarMatchData
            {
                name = s.Category,
                value = s.MatchArticleCount,
                TypeId = s.ChannelId,
                TypeMatchId = (int)CarMatchTypeEnum.Yes
            }).ToList();
            //未匹配
            respDto.Data.AddRange(statCarMatchList.Select(s => new StatKeyValueCarMatchData
            {
                name = s.Category,
                value = s.UnMatchArticleCount,
                TypeId = s.ChannelId,
                TypeMatchId = (int)CarMatchTypeEnum.No
            }));

            return respDto;
        }
    }
}