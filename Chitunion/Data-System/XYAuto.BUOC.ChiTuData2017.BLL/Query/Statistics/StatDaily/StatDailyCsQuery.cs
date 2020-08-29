/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 13:51:15
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily
{
    /// <summary>
    /// auth:lixiong
    /// desc:初筛
    /// </summary>
    public class StatDailyCsQuery
          : PublishInfoQueryClient<ReqDailyDto, RespDailyCsDto>
    {
        public StatDailyCsQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDailyCsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT  SPS1.ChannelID ,
                                    SPS1.ChannelName ,
                                    SPS1.Date ,
                                    ( SELECT    STUFF(( SELECT  '|'
                                                                + CAST(SPS2.ConditionId AS VARCHAR(10))+ ',1,'+ ISNULL(CAST(SPS2.ArticleCount AS VARCHAR(10)),0)+','+CONVERT(VARCHAR(10),SPS2.Date)
                                                                + '#'
									                            + CAST(SPS2.ConditionId AS VARCHAR(10)) + ',2,' + ISNULL(CAST(SPS2.AccountCount AS VARCHAR(10)),0)+','+CONVERT(VARCHAR(10),SPS2.Date)
                                                        FROM    dbo.Stat_PrimaryStatistics AS SPS2 WITH ( NOLOCK )
                                                        WHERE   SPS2.ChannelID > 0
                                                                AND SPS2.ConditionId > 0
                                                                AND SPS2.Date BETWEEN '{0}' AND '{1}'
                                                                AND SPS2.ChannelID = SPS1.ChannelID
                                                                #StatInfoWhere#
                                                      FOR
                                                        XML PATH('')
                                                      ), 1, 1, '')
                                    ) AS StatInfo
                            FROM    dbo.Stat_PrimaryStatistics AS SPS1 WITH ( NOLOCK )
                            WHERE   SPS1.ChannelID > 0
		                            AND SPS1.ConditionId > 0
                                    AND SPS1.Date BETWEEN '{0}' AND '{1}'

                        ", RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());
            var statInfoWhere = new StringBuilder();
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SPS1.ChannelID = {RequetQuery.ChannelId}");
                statInfoWhere.AppendFormat($" AND SPS2.ChannelID = {RequetQuery.ChannelId}");
            }
            sbSql.AppendFormat(@" GROUP BY SPS1.ChannelID ,
                                          SPS1.ChannelName,SPS1.Date ");

            sbSql.Replace("#StatInfoWhere#", statInfoWhere.ToString());
            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDailyCsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " Date DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespDailyCsDto> GetResult(List<RespDailyCsDto> resultList, QueryPageBase<RespDailyCsDto> query)
        {
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            resultList.ForEach(SetStatInfo);

            return base.GetResult(resultList, query);
        }

        private void SetStatInfo(RespDailyCsDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.StatInfo))
            {
                return;
            }
            var list = GetArrayValue(dto.StatInfo);
            var usable = list.Where(s => s.ConditionId == (int)ConditionTypeEnum.可用 && s.Date.Equals(dto.Date.ToString("yyyy-MM-dd"))).ToList();
            var firstOrDefault = usable.FirstOrDefault(s => s.TypeId == 1);
            if (firstOrDefault != null)
                dto.ArticleCount = firstOrDefault.Count;
            var statKeyValueQuery = usable.FirstOrDefault(s => s.TypeId == 2);
            if (statKeyValueQuery != null)
                dto.AccountCount = statKeyValueQuery.Count;

            var toBody = list.Where(s => s.ConditionId == (int)ConditionTypeEnum.置为腰 && s.Date.Equals(dto.Date.ToString("yyyy-MM-dd"))).ToList();
            var tobodyArticle = toBody.FirstOrDefault(s => s.TypeId == 1);
            if (tobodyArticle != null)
                dto.ToBodyArticleCount = tobodyArticle.Count;
            var tobodyAccount = toBody.FirstOrDefault(s => s.TypeId == 2);
            if (tobodyAccount != null)
                dto.ToBodyAccountCount = tobodyAccount.Count;

            var unable = list.Where(s => s.ConditionId == (int)ConditionTypeEnum.作废 && s.Date.Equals(dto.Date.ToString("yyyy-MM-dd"))).ToList();
            var unableArticle = unable.FirstOrDefault(s => s.TypeId == 1);
            if (unableArticle != null)
                dto.NotUseArticleCount = unableArticle.Count;
            var unableyAccount = unable.FirstOrDefault(s => s.TypeId == 2);
            if (unableyAccount != null)
                dto.NotUseAccountCount = unableyAccount.Count;
        }

        public static List<StatKeyValueQuery> GetArrayValue(string statInfo)
        {
            /*
            示例：    1,1,100#1,2,1001|2,200#2,2001|3,300#3,3001
                    可用,文章数#可用,帐号数
            */
            if (string.IsNullOrWhiteSpace(statInfo))
            {
                return new List<StatKeyValueQuery>();
            }
            var spStatInfo = statInfo.Split('|');
            var list = new List<StatKeyValueQuery>();
            foreach (var sp in spStatInfo)
            {
                if (string.IsNullOrWhiteSpace(sp)) continue;
                var spArticleAccountInfo = sp.Split('#');

                foreach (var sp1 in spArticleAccountInfo)
                {
                    if (string.IsNullOrWhiteSpace(sp1)) continue;
                    var spl = sp1.Split(',');
                    list.Add(new StatKeyValueQuery
                    {
                        Count = VerifyOperateBase.GetArrayContent(spl, 2).ToInt(),
                        TypeId = VerifyOperateBase.GetArrayContent(spl, 1).ToInt(),
                        ConditionId = VerifyOperateBase.GetArrayContent(spl, 0).ToInt(),
                        Date = VerifyOperateBase.GetArrayContent(spl, 3),
                    });
                }
            }
            return list;
        }
    }

    public class StatKeyValueQuery
    {
        public int Count { get; set; }
        public int TypeId { get; set; }
        public int ConditionId { get; set; }
        public string Date { get; set; }
    }
}