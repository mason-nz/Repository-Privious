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
    public class PbAppQuery : PublishInfoQueryClient<RequestPublishQueryDto, ResponseAppDto>
    {
        public PbAppQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseAppDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"SELECT  MPAPP.Name ,
                                    PB.PubID ,
                                    PB.MediaType ,
                                    PB.MediaID ,
                                    PB.BeginTime ,
                                    PB.EndTime ,
                                    PB.CreateTime ,
                                    PB.CreateUserID ,
                                    PB.[Status] ,
		                            ( SELECT TOP 1
												PAD.RejectMsg
									  FROM      dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
									  WHERE     PAD.PublishID = PB.PubID
												AND PAD.PubStatus = 15004 --驳回
									  ORDER BY  PAD.RecID DESC
									) AS RejectMsg ,
                                    ( SELECT    COUNT(*)
                                      FROM      dbo.Publish_DetailInfo AS PD WITH (NOLOCK)
                                      WHERE     PB.PubID = PD.PubID
                                    ) AS AdCount,
	                                DC2.DictName AS Category
                            FROM dbo.Publish_BasicInfo AS PB WITH (NOLOCK)
                        INNER JOIN dbo.Media_PCAPP AS MPAPP WITH (NOLOCK) ON MPAPP.MediaID = PB.MediaID AND MPAPP.Status = 0
                        LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON DC2.DictId = MPAPP.CategoryID
						");
            sbSql.AppendLine("  WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.MediaType = {0}", (int)MediaType.APP);
            if (RequetQuery.CreateUserId > 0)
            {
                sbSql.AppendFormat(@" AND MPAPP.CreateUserID ={0}", RequetQuery.CreateUserId);
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Name))
            {
                sbSql.AppendFormat(@" AND MPAPP.Name LIKE '{0}%'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Name));
            }
            if (RequetQuery.CategoryId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND DC2.DictId = {0} ", RequetQuery.CategoryId);
            }
            if (RequetQuery.Status != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.Status = {0} ", RequetQuery.Status);
            }

            sbSql.AppendLine(@" ) T");

            var query = new PublishQuery<ResponseAppDto>()
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