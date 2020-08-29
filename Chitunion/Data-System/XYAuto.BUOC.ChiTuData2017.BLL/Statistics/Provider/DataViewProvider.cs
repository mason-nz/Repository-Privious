/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 13:53:48
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.DataView;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:数据概括
    /// </summary>
    public class DataViewProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private static List<string> _latelyDaysList;

        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载

        public DataViewProvider(int latelyDays, string contextType)
        {
            _latelyDays = latelyDays;
            _contextType = contextType;
            _queryDic = new Lazy<Dictionary<string, Func<int, dynamic>>>(Init);
            _latelyDaysList = GetLatelyDays(_latelyDays);
        }

        #region 初始化

        private Dictionary<string, Func<int, dynamic>> Init()
        {
            var dic = new Dictionary<string, Func<int, dynamic>>()
            {
                { GetDataViewTypeEnum.grab_head.ToString(), s => GetHeadArticleGarb()},
                { GetDataViewTypeEnum.grab_body.ToString(), s => GetBodyArticleGarb()},
                { GetDataViewTypeEnum.jx_head.ToString(), s => GetHeadArticleJx()},
                { GetDataViewTypeEnum.jx_body.ToString(), s => GetBodyArticleJx()},
                { GetDataViewTypeEnum.cxpp_body.ToString(), s => GetBodyArticleCarMatch()},
                { GetDataViewTypeEnum.wlfz.ToString(), s => DataProfilingBll.Instance.GetEncapsulateGather(_latelyDays)},
                { GetDataViewTypeEnum.wlff.ToString(), s => DataProfilingBll.Instance.GetDistributeGather(_latelyDays)},
                { GetDataViewTypeEnum.zf.ToString(), s => DataProfilingBll.Instance.GetForwardGather(_latelyDays)},
                { GetDataViewTypeEnum.xshq.ToString(), s => DataProfilingBll.Instance.GetClueGather(_latelyDays)},
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
                { GetDataViewTypeEnum.grab_head.ToString(), null},
                { GetDataViewTypeEnum.grab_body.ToString(), null},
                { GetDataViewTypeEnum.jx_head.ToString(), null},
                { GetDataViewTypeEnum.jx_body.ToString(), null},
                { GetDataViewTypeEnum.cxpp_body.ToString(), null},
                { GetDataViewTypeEnum.wlfz.ToString(), null},
                { GetDataViewTypeEnum.wlff.ToString(), null},
                { GetDataViewTypeEnum.zf.ToString(), null},
                { GetDataViewTypeEnum.xshq.ToString(), null},
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
        /// 数据概况-头部文章抓取
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetHeadArticleGarb()
        {
            return GetHeadArticleGarbPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 数据概况-腰部文章抓取
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetBodyArticleGarb()
        {
            return GetHeadArticleGarbPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 数据概况-头部文章机洗入库
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetHeadArticleJx()
        {
            return GetHeadArticleJxPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 数据概况-腰部文章机洗入库
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetBodyArticleJx()
        {
            return GetHeadArticleJxPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 数据概况-腰部文章车型匹配
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetBodyArticleCarMatch()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //item1:累计统计（渠道）  item2:每天的累计统计
            var statTpList = Dal.Statistics.StatCarMatchStatistics.Instance.GetHeadArticleList(
                startDate, endDate, StatArticleTypeEnum.Body);

            var respDto = new RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //头部顶上的汇总(累计统计：)
            //左边的圆形
            respDto.Info = statTpList.Item1.Select(s => new StatInfo
            {
                Name = s.ChannelName,
                TypeId = s.ChannelId,
                ArticleCount = s.MatchArticleCount
            }).ToList();

            var leftPieList = statTpList.Item1.Where(s => s.ChannelId > 0).OrderBy(s => s.ChannelId);
            respDto.DataPie = new DataPieDto<StatKeyValueBaseData>
            {
                DataLegend = leftPieList.Select(s => s.ChannelName).Distinct().ToArray(),
                Data = leftPieList.Select(s => new StatKeyValueBaseData
                {
                    name = s.ChannelName,
                    value = s.MatchArticleCount
                }).ToList()
            };

            //todo:2.取出对应渠道的信息
            var rigthBarList = statTpList.Item2.OrderBy(s => s.ChannelId);
            var groupData = rigthBarList.GroupBy(s => s.ChannelId, (s, v) => new
            {
                ChannelId = s,
                list = v.OrderBy(t => t.Date)
            }).ToList();

            respDto.DataBar = new DataBarDto<List<StatTypeData>>()
            {
                DataLegend = _latelyDaysList.ToArray(),
                DicInfo = new List<Dto.Response.Grab.DicInfo>(),
                Data = new List<StatTypeData>()
            };
            groupData.ForEach(s =>
            {
                var dicDto = new Dto.Response.Grab.DicInfo()
                {
                    TypeId = s.ChannelId,
                    Name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };
                respDto.DataBar.DicInfo.Add(dicDto);

                var statTypeData = new StatTypeData()
                {
                    TypeId = s.ChannelId,
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };

                var dataList = new List<int>();
                //7天或30天维度出发
                _latelyDaysList.ForEach(t =>
                {
                    var anyData = s.list.Where(a => a.Date.ToString("yyyy-MM-dd") == t && a.ChannelId == s.ChannelId).ToList();
                    dataList.Add(anyData.Any() ? anyData.First().MatchArticleCount : 0);
                });

                statTypeData.data = dataList.ToArray();//已匹配数据
                respDto.DataBar.Data.Add(statTypeData);
            });

            return respDto;
        }

        /// <summary>
        /// 数据概括-昨日数据
        /// </summary>
        /// <returns></returns>
        public RespDvYesterdayDto GetDvYesterday()
        {
            var statYesterday = Dal.Statistics.StatDataProfiling.Instance.GetInfo(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

            return AutoMapper.Mapper.Map<Entities.Statistics.StatDataProfiling, RespDvYesterdayDto>(statYesterday);
        }

        #region 方法封装

        /// <summary>
        /// 数据概况-头部文章抓取（腰部文章）
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetHeadArticleGarbPackageMethod(
            StatArticleTypeEnum statArticleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //item1:累计统计（渠道）  item2:每天的累计统计
            var statTpList = Dal.Statistics.StatSpiderStatistics.Instance.GetDataViewHeadArticleList(
                startDate, endDate, statArticleTypeEnum);

            var respDto = new RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //头部顶上的汇总(累计统计：)
            //左边的圆形
            respDto.Info = AutoMapper.Mapper.Map<List<Entities.Statistics.StatSpiderSummary>, List<StatInfo>>(statTpList.Item1);
            var leftPieList = statTpList.Item1.Where(s => s.ChannelId > 0).OrderBy(s => s.ChannelId);
            respDto.DataPie = new DataPieDto<StatKeyValueBaseData>
            {
                DataLegend = leftPieList.Select(s => s.ChannelName).Distinct().ToArray(),
                Data = leftPieList.Select(s => new StatKeyValueBaseData
                {
                    name = s.ChannelName,
                    value = s.ArticleCount
                }).ToList()
            };

            //todo:2.取出对应渠道的信息
            var rigthBarList = statTpList.Item2.OrderBy(s => s.ChannelId);

            respDto.DataBar = new DataBarDto<List<StatTypeData>>()
            {
                DataLegend = _latelyDaysList.ToArray(),
                DicInfo = new List<Dto.Response.Grab.DicInfo>(),
                Data = new List<StatTypeData>()
            };

            #region 日期补全

            var groupData = rigthBarList.GroupBy(s => s.ChannelId, (s, v) => new
            {
                ChannelId = s,
                list = v.OrderBy(t => t.Date)
            }).ToList();

            groupData.ForEach(s =>
            {
                var dicDto = new Dto.Response.Grab.DicInfo()
                {
                    TypeId = s.ChannelId,
                    Name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };
                respDto.DataBar.DicInfo.Add(dicDto);

                var statTypeData = new StatTypeData()
                {
                    TypeId = s.ChannelId,
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };

                var dataList = new List<int>();
                //7天或30天维度出发
                _latelyDaysList.ForEach(t =>
                {
                    var anyData = s.list.Where(a => a.Date.ToString("yyyy-MM-dd") == t && a.ChannelId == s.ChannelId).ToList();
                    dataList.Add(anyData.Any() ? anyData.First().ArticleCount : 0);
                });

                statTypeData.data = dataList.ToArray();//已匹配数据
                respDto.DataBar.Data.Add(statTypeData);
            });

            #endregion 日期补全

            return respDto;
        }

        /// <summary>
        /// 数据概况-头部文章机洗入库
        /// </summary>
        /// <returns></returns>
        public RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>> GetHeadArticleJxPackageMethod(
            StatArticleTypeEnum statArticleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //item1:累计统计（渠道）  item2:每天的累计统计
            var statTpList = Dal.Statistics.StatAutoStatistics.Instance.GetHeadArticleList(
                startDate, endDate, statArticleTypeEnum);

            var respDto = new RespDvHeadGrabDto<StatKeyValueBaseData, List<StatTypeData>>
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //头部顶上的汇总(累计统计：)
            //左边的圆形
            respDto.Info = AutoMapper.Mapper.Map<List<Entities.Statistics.StatAutoSummary>, List<StatInfo>>(statTpList.Item1);
            var leftPieList = statTpList.Item1.Where(s => s.ChannelId > 0).OrderBy(s => s.ChannelId);
            respDto.DataPie = new DataPieDto<StatKeyValueBaseData>
            {
                DataLegend = leftPieList.Select(s => s.ChannelName).Distinct().ToArray(),
                Data = leftPieList.Select(s => new StatKeyValueBaseData
                {
                    name = s.ChannelName,
                    value = s.StorageArticleCount
                }).ToList()
            };

            //todo:2.取出对应渠道的信息
            var rigthBarList = statTpList.Item2.OrderBy(s => s.ChannelId);
            respDto.DataBar = new DataBarDto<List<StatTypeData>>()
            {
                DataLegend = _latelyDaysList.ToArray(),
                DicInfo = new List<Dto.Response.Grab.DicInfo>(),
                Data = new List<StatTypeData>()
            };

            #region 日期补全

            var groupData = rigthBarList.GroupBy(s => s.ChannelId, (s, v) => new
            {
                ChannelId = s,
                list = v.OrderBy(t => t.Date)
            }).ToList();

            groupData.ForEach(s =>
            {
                var dicDto = new Dto.Response.Grab.DicInfo()
                {
                    TypeId = s.ChannelId,
                    Name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };
                respDto.DataBar.DicInfo.Add(dicDto);

                var statTypeData = new StatTypeData()
                {
                    TypeId = s.ChannelId,
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };

                var dataList = new List<int>();
                //7天或30天维度出发
                _latelyDaysList.ForEach(t =>
                {
                    var anyData = s.list.Where(a => a.Date.ToString("yyyy-MM-dd") == t && a.ChannelId == s.ChannelId).ToList();
                    dataList.Add(anyData.Any() ? anyData.First().StorageArticleCount : 0);
                });

                statTypeData.data = dataList.ToArray();//已匹配数据
                respDto.DataBar.Data.Add(statTypeData);
            });

            #endregion 日期补全

            return respDto;
        }

        public static List<string> GetLatelyDays(int latelyDays)
        {
            var dateList = new List<string>();
            var dt = DateTime.Now.AddDays(-latelyDays);
            for (var i = 0; i < latelyDays; i++)
            {
                dateList.Add(dt.AddDays(i).ToString("yyyy-MM-dd"));
            }
            return dateList;
        }

        #endregion 方法封装
    }
}