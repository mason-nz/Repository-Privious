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
    public class PbVideoQuery : PublishInfoQueryClient<RequestPublishQueryDto,
        ResponseVideoDto>
    {
        public PbVideoQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseVideoDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendLine(@"
                            SELECT  PB.PubID ,
                                    MV.MediaID ,
                                    PD1.Price AS FirstPrice ,
                                    PD2.Price AS SecondPrice ,
                                    PD3.Price AS ThirdPrice ,
                                    MV.Number ,
                                    MV.Name ,
                                    MV.HeadIconURL ,
                                    MV.FansCount ,
                                    MV.FansCountURL ,
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
            sbSql.AppendFormat(@"FROM  dbo.Media_Video AS MV WITH ( NOLOCK )
                                    INNER JOIN dbo.Publish_BasicInfo AS PB WITH ( NOLOCK ) ON MV.MediaID = PB.MediaID AND MV.Status = 0
                                    LEFT JOIN dbo.Publish_DetailInfo AS PD1 WITH ( NOLOCK ) ON PD1.PubID = PB.PubID AND PD1.ADPosition1 = {0}
                                    LEFT JOIN dbo.Publish_DetailInfo AS PD2 WITH ( NOLOCK ) ON PD2.PubID = PB.PubID AND PD2.ADPosition1 = {1}
                                    LEFT JOIN dbo.Publish_DetailInfo AS PD3 WITH ( NOLOCK ) ON PD3.PubID = PB.PubID AND PD3.ADPosition1 = {2}
                                ", (int)AdFormality4.直发, (int)AdFormality4.原创发布, (int)AdFormality4.转发);
            sbSql.AppendLine(" {#} WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.MediaType = {0}", (int)MediaType.Video);
            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder("");
            if (RequetQuery.CreateUserId > 0)//普通角色，只能看见自己的
            {
                sbSql.AppendFormat(@" AND MV.CreateUserID ={0}", RequetQuery.CreateUserId);
            }
            else
            {
                sqlSelect.AppendLine(@" ,DC1.DictName AS Source,DC2.DictName AS [Platform]");
                sqlWhere.AppendFormat(@" LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = MV.Source
                                        LEFT JOIN DBO.DictInfo AS DC2 WITH(NOLOCK) ON DC2.DictId = MV.[Platform]
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
                sbSql.AppendFormat(@" AND MV.Number = '{0}'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Number));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Name))
            {
                sbSql.AppendFormat(@" AND MV.Name LIKE '{0}%'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Name));
            }
            if (RequetQuery.Status != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.Status = {0} ", RequetQuery.Status);
            }
            if (RequetQuery.PublishStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.PublishStatus ={0} ", RequetQuery.PublishStatus);
            }
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }

            sbSql.AppendLine(@" ) T");

            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            sbSql.Replace("{$}", sqlSelect.ToString());//追加 查询字段
            var query = new PublishQuery<ResponseVideoDto>()
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