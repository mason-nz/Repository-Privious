/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 14:27:43
*说明：趋势分析-tab页-抓取
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
using XYAuto.BUOC.ChiTuData2017.Entities.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider
{
    public class StatTabGrabProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载
        private static List<string> _latelyDaysList;

        public StatTabGrabProvider(int latelyDays, string contextType)
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
                { GetGrabType.grab_head.ToString(),s=> GetGrabHead()},
                { GetGrabType.grab_head_qudao.ToString(),s=> GetGrabHeadQuDao()},
                { GetGrabType.grab_head_wzcj.ToString(),s=> GetGrabHeadArticleAtCj()},
                { GetGrabType.grab_head_zhcj.ToString(),s=> GetGrabHeadAccountAtCj()},
                { GetGrabType.grab_body.ToString(), s=> GetGrabBody()},
                { GetGrabType.grab_body_qudao.ToString(),s=> GetGrabBodyQuDao()},
                { GetGrabType.grab_body_wzfl.ToString(),s=> GetGrabBodyArticleAtWzlb()}
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
                { GetGrabType.grab_head.ToString(),null},
                { GetGrabType.grab_head_qudao.ToString(),null},
                { GetGrabType.grab_head_wzcj.ToString(),null},
                { GetGrabType.grab_head_zhcj.ToString(),null},
                { GetGrabType.grab_body.ToString(),null},
                { GetGrabType.grab_body_qudao.ToString(),null},
                { GetGrabType.grab_body_wzfl.ToString(),null}
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
        /// 趋势分析-抓取-头部文章抓取
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadDto GetGrabHead()
        {
            return GetGrabHeadPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的头部文章在渠道上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadQuDao GetGrabHeadQuDao()
        {
            return GetGrabHeadQuDaoPackageMethod(StatArticleTypeEnum.Head);
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的头部文章在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetGrabHeadArticleAtCj()
        {
            return GetGrabHeadArticleOrAccountCj(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Articel);
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的头部账号在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetGrabHeadAccountAtCj()
        {
            return GetGrabHeadArticleOrAccountCj(StatArticleTypeEnum.Head, GetTotalCountTypeEnum.Account);
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的头部文章/账号在场景上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadCjDto GetGrabHeadArticleOrAccountCj(StatArticleTypeEnum articleTypeEnum,
            GetTotalCountTypeEnum getTotalCountTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatSpiderStatistics.Instance.GetStatSpiderSummaries(
               new StatSpiderQuery<StatSpiderSummary>()
               {
                   BeginTime = startDate,
                   EndTime = endDate,
                   ArticleType = articleTypeEnum,
                   FilterChannelId = OperatorEnum.等于,
                   FilterSceneId = OperatorEnum.大于
               });
            var respDto = new RespGrabHeadCjDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                Data = new List<StatKeyValueData>()
            };
            //todo:1.先按照场景排序，
            var spiderOrderList = spiderList.OrderBy(s => s.SceneId);
            respDto.DataLegend = spiderOrderList.Select(s => s.SceneName).Distinct().ToArray();
            //todo:2.取出对应场景的信息
            if (getTotalCountTypeEnum == GetTotalCountTypeEnum.Articel)
            {
                //文章的数量（因为图表需要切换2种类型的数据）
                respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
                {
                    name = s.SceneName,
                    value = s.ArticleCount
                }).OrderByDescending(s => s.value).ToList();
            }
            else
            {
                //账号的数量（因为图表需要切换2种类型的数据）
                respDto.Data = spiderOrderList.Select(s => new StatKeyValueData
                {
                    name = s.SceneName,
                    value = s.AccountCount
                }).OrderByDescending(s => s.value).ToList(); ;
            }

            return respDto;
        }

        #endregion 头部文章系列

        #region 腰部文章系列

        /// <summary>
        /// 趋势分析-抓取-腰部文章抓取
        /// </summary>
        /// <returns></returns>
        public RespGrabBaseDto GetGrabBody()
        {
            return GetGrabHeadPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的腰部文章在渠道上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabBaseDto GetGrabBodyQuDao()
        {
            return GetGrabHeadQuDaoPackageMethod(StatArticleTypeEnum.Body);
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的腰部文章在文章类别上的分布
        /// </summary>
        /// <returns></returns>
        public RespGrabBodyCjDto GetGrabBodyArticleAtWzlb()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatSpiderStatistics.Instance.GetStatSpiderSummaries(
               new StatSpiderQuery<StatSpiderSummary>()
               {
                   BeginTime = startDate,
                   EndTime = endDate,
                   ArticleType = StatArticleTypeEnum.Body,
                   FilterChannelId = OperatorEnum.等于,
                   FilterSceneId = OperatorEnum.等于,
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
                value = s.ArticleCount
            }).OrderByDescending(s => s.value).ToList();
            return respDto;
        }

        #endregion 腰部文章系列

        #region PackageMethod 方法封装

        /// <summary>
        /// 趋势分析-抓取-头部文章抓取（头部，腰部 获取的内容基本一致，只是文章类型不一致，可整合）
        /// </summary>
        /// <param name="articleTypeEnum"></param>
        /// <returns></returns>
        public RespGrabHeadDto GetGrabHeadPackageMethod(StatArticleTypeEnum articleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var tpGrabList = Dal.Statistics.StatSpiderStatistics.Instance.GetHeadArticleList(startDate, endDate, articleTypeEnum);

            var respDto = new RespGrabHeadDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
                Info = AutoMapper.Mapper.Map<List<Entities.Statistics.StatSpiderSummary>, List<StatInfo>>(tpGrabList.Item1)
            };
            //todo:1.先按照时间排序，
            respDto.DataLegend = _latelyDaysList.ToArray();
            //todo:2.GroupBy渠道，然后分组取出对应渠道的信息
            respDto.Data = new List<StatData>();

            #region 日期补全

            var groupData = tpGrabList.Item2.GroupBy(s => s.ChannelId, (s, v) => new
            {
                ChannelId = s,
                list = v.OrderBy(t => t.Date)
            }).ToList();

            groupData.ForEach(s =>
            {
                var statData = new StatData()
                {
                    name = s.list.Select(m => m.ChannelName).FirstOrDefault(),
                };

                var dataList = new List<int>();
                //7天或30天维度出发
                _latelyDaysList.ForEach(t =>
                {
                    var anyData = s.list.Where(a => a.Date.ToString("yyyy-MM-dd") == t && a.ChannelId == s.ChannelId).ToList();
                    dataList.Add(anyData.Any() ? anyData.First().ArticleCount : 0);
                });

                statData.data = dataList.ToArray();
                respDto.Data.Add(statData);
            });

            #endregion 日期补全

            return respDto;
        }

        /// <summary>
        /// 趋势分析-抓取-抓取的头部文章在渠道上的分布 （头部，腰部 获取的内容基本一致，只是文章类型不一致，可整合）
        /// </summary>
        /// <returns></returns>
        public RespGrabHeadQuDao GetGrabHeadQuDaoPackageMethod(StatArticleTypeEnum articleTypeEnum)
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var spiderList = Dal.Statistics.StatSpiderStatistics.Instance.GetStatSpiderSummaries(
                new StatSpiderQuery<StatSpiderSummary>()
                {
                    BeginTime = startDate,
                    EndTime = endDate,
                    ArticleType = articleTypeEnum,
                    FilterChannelId = OperatorEnum.大于,
                    FilterSceneId = OperatorEnum.等于
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
                value = s.ArticleCount
            }).ToList();
            return respDto;
        }

        #endregion PackageMethod 方法封装
    }
}