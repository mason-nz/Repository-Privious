/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 16:53:48
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider
{
    /// <summary>
    /// 漏斗-文章
    /// </summary>
    public class FunnelStatProvider
    {
        private readonly int _latelyDays;
        private readonly string _contextType;
        private readonly ReqFunnelDto _requestDateRangeDto;
        private static Lazy<Dictionary<string, Func<int, dynamic>>> _queryDic;//lazy加载

        public FunnelStatProvider(int latelyDays, string contextType, ReqFunnelDto requestDateRangeDto)
        {
            _latelyDays = latelyDays;
            _contextType = contextType;
            _requestDateRangeDto = requestDateRangeDto;
            _queryDic = new Lazy<Dictionary<string, Func<int, dynamic>>>(Init);
        }

        #region 初始化

        private Dictionary<string, Func<int, dynamic>> Init()
        {
            var dic = new Dictionary<string, Func<int, dynamic>>()
            {
                { GetFunnelTypeEnum.head.ToString(), s => GetFunnelHead()},
                { GetFunnelTypeEnum.body.ToString(), s => GetFunnelBody()},
            };
            return dic;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public dynamic GetFunnelData()
        {
            if (string.IsNullOrWhiteSpace(_contextType))
            {
                throw new ExportBusinessTypeException("请输入合法的TabType");
            }
            return _queryDic.Value[_contextType].Invoke(0);
        }

        #endregion 初始化

        /// <summary>
        /// 漏斗分析-头部文章
        /// </summary>
        /// <returns></returns>
        public RespGrabBaseDto GetFunnelHead()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statFunnelList = Dal.Statistics.StatFunnelHead.Instance.GetStatFunnelHead(startDate, endDate);

            var respDto = new RespGrabBaseDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            var statInfo = statFunnelList.FirstOrDefault();
            if (statInfo != null)
            {
                respDto.Info = new List<StatInfo>()
                {
                    new StatInfo() { Name = "抓取",AccountCount =statInfo.SpiderAccountCount,ArticleCount =statInfo.SpiderArticleCount },
                    new StatInfo() { Name = "机洗入库",AccountCount =statInfo.AutoAccountCount,ArticleCount =statInfo.AutoArticleCount },
                    new StatInfo() { Name = "初筛保留",AccountCount =statInfo.PrimaryAccountCount,ArticleCount =statInfo.PrimaryArticleCount },
                    new StatInfo() { Name = "人工清洗保留",AccountCount =statInfo.ArtificialAccountCount,ArticleCount =statInfo.ArtificialArticleCount },
                    new StatInfo() { Name = "封装使用文章",AccountCount =statInfo.EncapsulateAccountCount,ArticleCount =statInfo.EncapsulateArticleCount }
                };
            }
            return respDto;
        }

        /// <summary>
        /// 漏斗分析-腰部文章
        /// </summary>
        /// <returns></returns>
        public RespGrabBaseDto GetFunnelBody()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statFunnelList = Dal.Statistics.StatFunnelHead.Instance.GetStatFunnelWaists(startDate, endDate);

            var respDto = new RespGrabBaseDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            var statInfo = statFunnelList.FirstOrDefault();
            if (statInfo != null)
            {
                respDto.Info = new List<StatInfo>()
                {
                    new StatInfo() { Name = "抓取文章",ArticleCount =statInfo.SpiderCount },
                    new StatInfo() { Name = "机洗保留文章",ArticleCount =statInfo.AutoCleanCount },
                    new StatInfo() { Name = "匹配车型文章",ArticleCount =statInfo.MatchedCount },
                    new StatInfo() { Name = "人工清洗保留",ArticleCount =statInfo.ArtificialCount },
                    new StatInfo() { Name = "封装使用文章",ArticleCount =statInfo.EncapsulateCount }
                };
            }
            return respDto;
        }

        /// <summary>
        /// 漏斗分析-物料
        /// </summary>
        /// <returns></returns>
        public RespGrabBaseDto GetMaterielChart()
        {
            var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var startDate = DateTime.Now.AddDays(-_latelyDays).ToString("yyyy-MM-dd");

            var statFunnelList = Dal.Statistics.StatFunnelHead.Instance.GetStatFunnelMaterials(startDate, endDate);

            var respDto = new RespGrabBaseDto
            {
                Date = new StatDate { BeginTime = startDate, EndTime = endDate },
            };
            var statInfo = statFunnelList.FirstOrDefault();
            if (statInfo != null)
            {
                respDto.Info = new List<StatInfo>()
                {
                    new StatInfo() { Name = "封装",ArticleCount =statInfo.Encapsulate,TypeId = 1},
                    new StatInfo() { Name = "分发",ArticleCount =statInfo.Distribute ,TypeId = 1},
                    new StatInfo() { Name = "转发",ArticleCount =statInfo.Forward ,TypeId = 2},
                    new StatInfo() { Name = "线索",ArticleCount =statInfo.Clue ,TypeId = 2}
                };
            }
            return respDto;
        }
    }

    public enum GetFunnelTypeEnum
    {
        [Description("头部文章")]
        head,

        [Description("腰部文章")]
        body
    }
}