/********************************************************
*创建人：lixiong
*创建时间：2017/11/24 13:25:03
*说明：机洗入库
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// desc:机洗入库
    /// </summary>
    public class StatTabMachineCleanProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载
        private static List<string> _latelyDaysList;

        public StatTabMachineCleanProvider(int latelyDays, string contextType)
        {
            _latelyDays = latelyDays;
            _contextType = contextType;
            _latelyDaysList = DataViewProvider.GetLatelyDays(_latelyDays);
            _queryDic = new Lazy<Dictionary<string, Func<int, dynamic>>>(Init);
        }

        private Dictionary<string, Func<int, dynamic>> Init()
        {
            var dic = new Dictionary<string, Func<int, dynamic>>()
            {
                { GetJxTypeEnum.jx_head.ToString(), s => GetJxHead()},
                { GetJxTypeEnum.jx_head_qudao.ToString(), s => GetJxHeadQuDao()},
                { GetJxTypeEnum.jx_head_wz_wzfz.ToString(), s => GetJxHeadArticleAtArticleFengZhi()},
                { GetJxTypeEnum.jx_head_zh_zhfz.ToString(), s => GetJxHeadAccountAtAccountFengZhi()},
                { GetJxTypeEnum.jx_head_wzcj.ToString(), s => GetJxHeadArticelCj()},
                { GetJxTypeEnum.jx_head_zhcj.ToString(), s => GetJxHeadAccountCj()},
                { GetJxTypeEnum.jx_head_zh.ToString(), s => GetJxHeadZhuanHua()},
                { GetJxTypeEnum.jx_body.ToString(), s => GetJxBody()},
                { GetJxTypeEnum.jx_body_qudao.ToString(), s => GetJxBodyQuDao()},
                { GetJxTypeEnum.jx_body_wzlb.ToString(), s => GetJxBodyCategory()},
                { GetJxTypeEnum.jx_body_zh.ToString(), s => GetJxBodyZhuanHua()}
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
                { GetJxTypeEnum.jx_head.ToString(), null},
                { GetJxTypeEnum.jx_head_qudao.ToString(), null},
                { GetJxTypeEnum.jx_head_wz_wzfz.ToString(), null},
                { GetJxTypeEnum.jx_head_zh_zhfz.ToString(),null},
                { GetJxTypeEnum.jx_head_wzcj.ToString(), null},
                { GetJxTypeEnum.jx_head_zhcj.ToString(), null},
                { GetJxTypeEnum.jx_head_zh.ToString(), null},
                { GetJxTypeEnum.jx_body.ToString(), null},
                { GetJxTypeEnum.jx_body_qudao.ToString(), null},
                { GetJxTypeEnum.jx_body_wzlb.ToString(), null},
                { GetJxTypeEnum.jx_body_zh.ToString(), null}
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

        #region 头部文章系列

        /// <summary>
        /// 趋势分析-机洗入库-头部文章机洗入库
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadDto GetJxHead()
        {
            return GetBarChartPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章在渠道上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadQuDao GetJxHeadQuDao()
        {
            return GetPieChartQuDaoPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 趋势分析-机洗入库-头部文章机洗入库的转化
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadDto<StatKeyValueBaseData> GetJxHeadZhuanHua()
        {
            return GetFunnelChartZhuanHua(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetJxHeadArticelCj()
        {
            return GetPieJxHeadCjPackageMethod(GetTotalCountTypeEnum.Articel);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部账号在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetJxHeadAccountCj()
        {
            return GetPieJxHeadCjPackageMethod(GetTotalCountTypeEnum.Account);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章在文章分值上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetJxHeadArticleAtArticleFengZhi()
        {
            return GetPieChartFenZhiPackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Articel, ScoreTypeEnum.文章分值类型);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部账号在账号分值上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetJxHeadAccountAtAccountFengZhi()
        {
            return GetPieChartFenZhiPackageMethod(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Account, ScoreTypeEnum.账号分值类型);
        }

        #endregion 头部文章系列

        #region 腰部文章系列

        /// <summary>
        /// 趋势分析-机洗入库-头部文章机洗入库
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadDto GetJxBody()
        {
            return GetBarChartPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章在渠道上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadQuDao GetJxBodyQuDao()
        {
            return GetPieChartQuDaoPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 趋势分析-机洗入库-头部文章机洗入库的转化
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadDto<StatKeyValueBaseData> GetJxBodyZhuanHua()
        {
            return GetFunnelChartZhuanHua(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的腰部文章在文章类别上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabBodyCjDto GetJxBodyCategory()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatAutoStatistics.Instance.GetStatAutoSummaries(
               new StatSpiderQuery<StatAutoSummary>()
               {
                   BeginTime = startDate,
                   EndTime = endDate,
                   ArticleType = StatArticleTypeEnum.Body,
                   FilterChannelId = OperatorEnum.等于,
                   FilterSceneId = OperatorEnum.等于,
                   FilterScoreType = OperatorEnum.等于,
                   FilterAutoCategory = OperatorEnum.不等于NULL
               });
            var respDto = new RespGrabBodyCjDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //todo:1.先按照场景排序，
            var spiderOrderList = spiderList.OrderBy(s => s.RecId);
            respDto.DataLegend = spiderOrderList.Select(s => s.Category).Distinct().ToArray();
            //todo:2.取出对应场景的信息
            //文章的数量（因为图表需要切换2种类型的数据）
            respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
            {
                name = s.Category,
                value = s.StorageArticleCount
            }).OrderByDescending(s => s.value).ToList();
            return respDto;
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章/账号在账号分值上的分布
        /// </summary>
        /// <returns></returns>
        //public RespGrabHeadCjDto GetJxBodyFengZhi()
        //{
        //    return GetPieChartFenZhiPackageMethod(StatArticleTypeEnum.Body);
        //}

        #endregion 腰部文章系列

        #region PackageMethod 方法封装

        /// <summary>
        /// 趋势分析-机洗入库-头部文章机洗入库-柱状图 （头部，腰部 获取的内容基本一致，只是文章类型不一致，可整合）
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <returns></returns>
        public RespGrabHeadDto GetBarChartPackageMethod(StatArticleTypeEnum articleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var tpGrabList = Dal.Statistics.StatAutoStatistics.Instance.GetHeadArticleList(startDate, endDate, articleTypeEnum);

            var respDto = new RespGrabHeadDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                Info = AutoMapper.Mapper.Map<List<Entities.Statistics.StatAutoSummary>, List<StatInfo>>(tpGrabList.Item1),
                Data = new List<StatData>()
            };
            //todo:1.先按照时间排序，
            respDto.DataLegend = _latelyDaysList.ToArray();
            //todo:2.GroupBy渠道，然后分组取出对应渠道的信息

            #region 日期补全

            var groupData = tpGrabList.Item2.Where(s => s.ChannelId > 0).GroupBy(s => s.ChannelId, (s, v) => new
            {
                ChannelId = s,
                list = v.OrderBy(t => t.Date)
            }).ToList();

            groupData.ForEach(s =>
            {
                var statData = new StatData()
                {
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                    data = new int[] { }
                };

                var dataList = new List<int>();
                //7天或30天维度出发
                _latelyDaysList.ForEach(t =>
                {
                    var anyData = s.list.Where(a => a.Date.ToString("yyyy-MM-dd") == t).ToList();
                    dataList.Add(anyData.Any() ? anyData.First().StorageArticleCount : 0);
                });

                statData.data = dataList.ToArray();
                respDto.Data.Add(statData);
            });
            respDto.Data = respDto.Data.Distinct().ToList();

            #endregion 日期补全

            return respDto;
        }

        /// <summary>
        /// 趋势分析-机洗入库-抓取的头部文章在渠道上的分布-环形图 （头部，腰部 获取的内容基本一致，只是文章类型不一致，可整合）
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadQuDao GetPieChartQuDaoPackageMethod(StatArticleTypeEnum articleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatAutoStatistics.Instance.GetStatAutoSummaries(
                new StatSpiderQuery<StatAutoSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    ArticleType = articleTypeEnum,
                    FilterChannelId = OperatorEnum.大于,
                    FilterSceneId = OperatorEnum.等于,
                    FilterScoreType = OperatorEnum.等于,
                    FilterAutoCategory = OperatorEnum.等于NULL
                });
            var respDto = new RespGrabHeadQuDao
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //todo:1.先按照时间排序，
            var spiderOrderList = spiderList.OrderBy(s => s.ChannelId);
            respDto.DataLegend = spiderOrderList.Select(s => s.ChannelName).Distinct().ToArray();
            //todo:2.取出对应渠道的信息
            respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
            {
                name = s.ChannelName,
                value = s.StorageArticleCount
            }).ToList();
            return respDto;
        }

        /// <summary>
        /// 趋势分析-机洗入库-头部文章机洗入库的转化-漏斗图   （头部，腰部 获取的内容基本一致，只是文章类型不一致，可整合）
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <returns></returns>
        public RespGrabHeadDto<StatKeyValueBaseData> GetFunnelChartZhuanHua(StatArticleTypeEnum articleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");
            //查出了 抓取数据统计（累计统计+渠道统计） and 机洗入库（累计统计+渠道统计）
            var autoList = Dal.Statistics.StatAutoStatistics.Instance.GetStatAutoSummariesZhuanHua(
                new StatSpiderQuery<StatAutoSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    ArticleType = articleTypeEnum
                });
            var respDto = new RespGrabHeadDto<StatKeyValueBaseData>()
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //todo:1.先按照时间排序，
            var autoOrderList = autoList.OrderBy(s => s.ChannelId);
            //todo:2.取出对应渠道的信息
            respDto.DataLegend = autoOrderList.Select(s => s.ChannelName).Distinct().ToArray();
            //todo:3.转化需要获取统计信息，把此信息填充到Info属性，typeId：1-抓取   2：机洗入库  区分 是抓取，还是机洗入库的

            respDto.Info = autoOrderList.Where(s => s.ChannelId > 0).Select(s => new StatInfo
            {
                ArticleCount = s.ArticleCount,
                AccountCount = s.AccountCount,
                Name = s.ChannelName,
                TypeId = s.TypeId//1.抓取   2.机洗入库
            }).ToList();

            respDto.Data = autoOrderList.Where(s => s.ChannelId == 0).Select(s => new StatKeyValueBaseData
            {
                name = s.TypeId == 1 ? "抓取文章" : "机洗入库",
                value = s.ArticleCount
            }).ToList();
            return respDto;
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章/账号在账号分值上的分布-漏斗图 （头部，腰部 获取的内容基本一致，只是文章类型不一致，可整合）
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <param name="getTotalCountTypeEnum"></param>
        /// <param name="scoreTypeEnum"></param>
        /// <returns></returns>
        public RespGrabHeadCjDto GetPieChartFenZhiPackageMethod(
            StatArticleTypeEnum articleTypeEnum,
            GetTotalCountTypeEnum getTotalCountTypeEnum,
            ScoreTypeEnum scoreTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatAutoStatistics.Instance.GetStatAutoSummaries(
               new StatSpiderQuery<Entities.Statistics.StatAutoSummary>()
               {
                   BeginTime = startDate,
                   EndTime = endDate,
                   ArticleType = articleTypeEnum,
                   FilterChannelId = OperatorEnum.等于,
                   FilterSceneId = OperatorEnum.等于,
                   ScoreType = scoreTypeEnum,
                   FilterAutoCategory = OperatorEnum.等于NULL
               });
            var respDto = new RespGrabHeadCjDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //todo:1.先按照场景排序，
            var spiderOrderList = spiderList.OrderBy(s => s.AaScoreType);
            respDto.DataLegend = spiderOrderList.Select(s => s.AAScoreTypeName).Distinct().ToArray();
            //todo:2.取出对应场景的信息(因为这里是两列的数据)

            //文章的数量（因为图表需要切换2种类型的数据）
            if (getTotalCountTypeEnum == GetTotalCountTypeEnum.Articel)
            {
                respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
                {
                    name = s.AAScoreTypeName,
                    value = s.StorageArticleCount,
                    TypeId = 1
                }).OrderByDescending(s => s.value).ToList();
            }
            else
            {
                //账号的数量（因为图表需要切换2种类型的数据）
                respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
                {
                    name = s.AAScoreTypeName,
                    value = s.StorageAccountCount,
                    TypeId = 2
                }).OrderByDescending(s => s.value).ToList();
            }

            return respDto;
        }

        /// <summary>
        /// 趋势分析-机洗入库-机洗入库的头部文章/账号在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetPieJxHeadCjPackageMethod(GetTotalCountTypeEnum getTotalCountTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatAutoStatistics.Instance.GetStatAutoSummaries(
               new StatSpiderQuery<Entities.Statistics.StatAutoSummary>()
               {
                   BeginTime = startDate,
                   EndTime = endDate,
                   ArticleType = StatArticleTypeEnum.Head,
                   FilterChannelId = OperatorEnum.等于,
                   FilterSceneId = OperatorEnum.大于,
                   FilterAutoCategory = OperatorEnum.等于NULL
               });
            var respDto = new RespGrabHeadCjDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            //todo:1.先按照场景排序，
            var spiderOrderList = spiderList.OrderBy(s => s.SceneId);
            respDto.DataLegend = spiderOrderList.Select(s => s.SceneName).Distinct().ToArray();
            //todo:2.取出对应场景的信息
            //文章的数量（因为图表需要切换2种类型的数据）
            if (getTotalCountTypeEnum == GetTotalCountTypeEnum.Articel)
            {
                respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
                {
                    name = s.SceneName,
                    value = s.StorageArticleCount,
                    TypeId = 1
                }).OrderByDescending(s => s.value).ToList();
            }
            else
            {
                //账号的数量（因为图表需要切换2种类型的数据）
                respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
                {
                    name = s.SceneName,
                    value = s.StorageAccountCount,
                    TypeId = 2
                }).OrderByDescending(s => s.value).ToList();
            }

            return respDto;
        }

        #endregion PackageMethod 方法封装
    }
}