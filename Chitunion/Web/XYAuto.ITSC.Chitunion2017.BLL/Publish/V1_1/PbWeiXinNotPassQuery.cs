using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1
{
    public class PbWeiXinNotPassQuery : PublishInfoQueryClient<RequestPublishQueryDto, RespPbWeiXinAuditPassDto>
    {
        public PbWeiXinNotPassQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespPbWeiXinAuditPassDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"
                    SELECT  PB.PubID ,
                            PB.PubName ,
                            PB.BeginTime ,
                            PB.EndTime ,
                            PB.Wx_Status ,
                            PB.CreateTime ,
                            PB.IsAppointment ,
                            PB.MediaType ,
                            WX.MediaID ,
                            WX.Number ,
                            WX.Name ,
                            WX.HeadIconURL ,
                            UDI.UserID ,
                            UDI.TrueName ,
                            AuditDateTime = ( SELECT TOP 1
                                                        PAD.CreateTime
                                                FROM    dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
                                                WHERE   PAD.PublishID = PB.PubID
                                                        AND PAD.PubStatus = 15004 --驳回
                                                ORDER BY PAD.RecID DESC
                                              ),
                            WX.[Source] ,
                            AuditUser = ( SELECT TOP 1
                                                    PAD.CreateUserID
                                          FROM      dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
                                                    INNER JOIN dbo.UserInfo WITH ( NOLOCK ) ON UserInfo.UserID = PAD.CreateUserID
                                          WHERE     PAD.PublishID = PB.PubID
                                                    AND PAD.PubStatus = 15004 --驳回
                                          ORDER BY  PAD.RecID DESC
                                        )
                            ,	DC1.DictName AS Wx_StatusName
                    FROM    dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Weixin AS WX WITH ( NOLOCK ) ON WX.MediaID = PB.MediaID AND WX.Status = 0
		                    INNER JOIN DBO.Weixin_OAuth AS WO WITH(NOLOCK) ON WO.RecID = WX.WxID AND WO.Status = 0
                            LEFT JOIN dbo.UserDetailInfo AS UDI WITH ( NOLOCK ) ON UDI.UserID = PB.CreateUserID
                            LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = PB.Wx_Status
                  ");
            sbSql.AppendLine("  WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.IsDel = 0 AND PB.MediaType = {0}", (int)MediaType.WeiXin);
            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(RequetQuery.Wx_Status))
            {
                sbSql.AppendFormat(@"  AND PB.Wx_Status IN ({0})", RequetQuery.Wx_Status.Trim(',').ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.PubName))
            {
                sbSql.AppendFormat(@"  AND PB.PubName LIKE '%{0}%'", RequetQuery.PubName.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Keyword))
            {
                sbSql.AppendFormat(@"  AND (WX.Number = '{0}' OR WX.Name = '{0}')", RequetQuery.Keyword.ToSqlFilter());
            }
            //过期时间
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }
            if (RequetQuery.CreateUserId > 0)
            {
                sbSql.AppendFormat(@"  AND PB.CreateUserID = {0}", RequetQuery.CreateUserId);
            }
            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespPbWeiXinAuditPassDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " PubID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}