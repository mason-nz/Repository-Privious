using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    public class PbBroadcastQuery : PublishInfoQueryClient<RequestPublishQueryDto, ResponseBroadcastDto>
    {
        public PbBroadcastQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseBroadcastDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendLine(@"
                        SELECT  PB.PubID ,
                                MB.MediaID ,
		                        PD1.Price AS FirstPrice ,
                                PD2.Price AS SecondPrice ,
                                MB.Number ,
                                MB.Name ,
                                MB.HeadIconURL ,
                                MB.FansCount ,
                                PB.[Status] ,
                                PB.PublishStatus ,PB.SaleDiscount,
                                PB.BeginTime ,
                                PB.EndTime,
                                ( SELECT TOP 1
												PAD.RejectMsg
									  FROM      dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
									  WHERE     PAD.PublishID = PB.PubID
												AND PAD.PubStatus = 15004 --驳回
									  ORDER BY  PAD.RecID DESC
								) AS RejectMsg
                               {$}
                       ");

            sbSql.AppendFormat(@" FROM  dbo.Media_Broadcast AS MB WITH ( NOLOCK )
                                INNER JOIN dbo.Publish_BasicInfo AS PB WITH ( NOLOCK ) ON MB.MediaID = PB.MediaID AND MB.Status = 0
                                LEFT JOIN dbo.Publish_DetailInfo AS PD1 WITH ( NOLOCK ) ON PD1.PubID = PB.PubID AND PD1.ADPosition1 = {0}
                                LEFT JOIN dbo.Publish_DetailInfo AS PD2 WITH ( NOLOCK ) ON PD2.PubID = PB.PubID AND PD2.ADPosition1 = {1}
                                    ", (int)AdFormality5.活动现场直播, (int)AdFormality5.直播广告植入);
            sbSql.AppendLine(" {#} WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.MediaType = {0}", (int)MediaType.Broadcast);

            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder("");
            if (RequetQuery.CreateUserId > 0)//普通角色，只能看见自己的
            {
                sbSql.AppendFormat(@" AND MB.CreateUserID ={0}", RequetQuery.CreateUserId);
            }
            else
            {
                sqlSelect.AppendLine(@", DC1.DictName AS Source,DC2.DictName AS [Platform]");
                sqlWhere.AppendFormat(@"LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = MB.Source
                                         LEFT JOIN DBO.DictInfo AS DC2 WITH(NOLOCK) ON DC2.DictId = MB.[Platform]
                                        ");
                if (RequetQuery.Source != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    sbSql.AppendFormat(@" AND DC1.DictId = {0} ", RequetQuery.Source);
                }
                if (RequetQuery.Platform != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    sbSql.AppendFormat(@" AND DC2.DictId = {0} ", RequetQuery.Platform);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Number))
            {
                sbSql.AppendFormat(@" AND MB.Number = '{0}'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Number));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Name))
            {
                sbSql.AppendFormat(@" AND MB.Name LIKE '{0}%'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Name));
            }

            if (RequetQuery.PublishStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.PublishStatus ={0} ", RequetQuery.PublishStatus);
            }
            if (RequetQuery.Status != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.Status = {0} ", RequetQuery.Status);
            }
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }
            sbSql.AppendLine(@" ) T");

            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            sbSql.Replace("{$}", sqlSelect.ToString());//追加 查询字段
            var query = new PublishQuery<ResponseBroadcastDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MediaID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}