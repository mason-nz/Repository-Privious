/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 17:18:45
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
    /// desc:机洗入库-明细数据
    /// </summary>
    public class StatDetailsJxQuery
        : PublishInfoQueryClient<ReqDetailsDto, RespStatDetailsJxDto>
    {
        public StatDetailsJxQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespStatDetailsJxDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT SAD.RecID ,
                                   SAD.ArticleID ,
                                   SAD.Title ,
                                   SAD.Url ,
                                   SAD.ArticleType ,
                                   DC1.DictName AS ArticleTypeName,
                                   SAD.ChannelID ,
                                   SAD.ChannelName ,
                                   SAD.ArticlePublishTime ,
                                   SAD.ArticleSpiderTime ,
                                   SAD.StorageTime ,
                                   SAD.SceneId ,
                                   SAD.SceneName ,
                                   SAD.AccountName ,
                                   SAD.AccountScore ,
                                   SAD.ArticleScore
	                               FROM dbo.Stat_AutoData AS SAD WITH(NOLOCK)
                                    LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = SAD.AAScoreTypeAccount
                                    LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = SAD.ArticleType
                            WHERE SAD.Status = 0
                                    AND SAD.StorageTime >= '{0}'
                                    AND SAD.StorageTime < '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(),
                        Convert.ToDateTime(RequetQuery.EndDate.ToSqlFilter()).AddDays(1).ToString("yyyy-MM-dd"));

            if (RequetQuery.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAD.ArticleType = {RequetQuery.ArticleType}");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAD.ChannelID = {RequetQuery.ChannelId}");
            }
            if (RequetQuery.SceneId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAD.SceneId = {RequetQuery.SceneId}");
            }
            if (RequetQuery.AAScoreTypeAccount != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND DC.DictId = {RequetQuery.AAScoreTypeAccount}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespStatDetailsJxDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " StorageTime DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}