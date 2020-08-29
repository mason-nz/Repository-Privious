/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 16:37:16
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
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails
{
    public class StatDetailsGrabQuery
          : PublishInfoQueryClient<ReqDetailsDto, RespStatDetailsGrabDto>
    {
        public StatDetailsGrabQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespStatDetailsGrabDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT  SSD.RecID ,
                                    SSD.ArticleID ,
                                    SSD.Title ,
                                    SSD.Url ,
                                    SSD.ArticleType ,
                                    SSD.ChannelID ,
                                    SSD.ChannelName ,
                                    SSD.ArticlePublishTime ,
                                    SSD.ArticleSpiderTime ,
                                    SSD.SceneId ,
                                    SSD.SceneName ,
                                    SSD.AccountName ,
                                    SSD.Category ,
                                    SSD.AccountScore,
									SSD.AAScoreTypeAccount ,
                                    DC.DictName AS ArticleTypeName
		                            FROM    dbo.Stat_SpiderData AS SSD WITH ( NOLOCK )
                                    LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = SSD.ArticleType
                            WHERE   SSD.Status = 0
                                    AND SSD.ArticleSpiderTime >= '{0}'
                                    AND SSD.ArticleSpiderTime < '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(),
                        Convert.ToDateTime(RequetQuery.EndDate.ToSqlFilter()).AddDays(1).ToString("yyyy-MM-dd"));

            if (RequetQuery.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SSD.ArticleType = {RequetQuery.ArticleType}");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SSD.ChannelID = {RequetQuery.ChannelId}");
            }
            if (RequetQuery.SceneId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SSD.SceneId = {RequetQuery.SceneId}");
            }
            if (RequetQuery.AAScoreTypeAccount != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SSD.AAScoreTypeAccount = {RequetQuery.AAScoreTypeAccount}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespStatDetailsGrabDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " ArticleSpiderTime DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}