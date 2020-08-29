/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 19:09:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails
{
    /// <summary>
    /// auth:lixiong
    /// desc:初筛
    /// </summary>
    public class StatDetailsCsQuery
           : PublishInfoQueryClient<ReqDetailsDto, RespStatDetailsCsDto>
    {
        public StatDetailsCsQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespStatDetailsCsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                            SELECT  SPD.RecID ,
                                    SPD.ArticleId ,
                                    SPD.Title ,
                                    SPD.Url ,
                                    SPD.ArticleType ,
                                    SPD.IsOriginal ,
                                    SPD.ChannelID ,
                                    SPD.ChannelName ,
                                    SPD.ArticlePublishTime ,
                                    SPD.ArticleSpiderTime ,
                                    SPD.PrimaryTime ,
                                    SPD.SceneId ,
                                    SPD.SceneName ,
                                    SPD.Account as AccountName,
                                    SPD.AccountSorce as AccountScore ,
                                    SPD.ArticleScore ,
                                    SPD.ConditionId ,
                                    SPD.ConditionName ,
                                    SPD.Reason
                            FROM    dbo.Stat_PrimaryData AS SPD WITH ( NOLOCK )
                                    LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = SPD.AAScoreTypeAccount
                            WHERE   SPD.Status = 0
                                    AND SPD.PrimaryTime >= '{0}'
                                    AND SPD.PrimaryTime < '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(),
                        Convert.ToDateTime(RequetQuery.EndDate.ToSqlFilter()).AddDays(1).ToString("yyyy-MM-dd"));

            if (RequetQuery.AAScoreTypeAccount != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SPD.AAScoreTypeAccount = {RequetQuery.AAScoreTypeAccount}");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SPD.ChannelID = {RequetQuery.ChannelId}");
            }
            if (RequetQuery.SceneId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SPD.SceneId = {RequetQuery.SceneId}");
            }
            if (RequetQuery.ConditionId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SPD.ConditionId = {RequetQuery.ConditionId}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespStatDetailsCsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " PrimaryTime DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}