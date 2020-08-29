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
    public class PbWeiXinQueryByYunYing : PublishInfoQueryClient<RequestPublishQueryDto, RespPbWeiXinAuditPassDto>
    {
        public PbWeiXinQueryByYunYing(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespPbWeiXinAuditPassDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
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
                                                        AND PAD.PubStatus = {0} --驳回
                                                ORDER BY PAD.RecID DESC
                                                ),
                            WX.[Source] ,
                            AuditUser = ( SELECT TOP 1
                                                    PAD.CreateUserID
                                            FROM      dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
                                                    INNER JOIN dbo.UserInfo WITH ( NOLOCK ) ON UserInfo.UserID = PAD.CreateUserID
                                            WHERE     PAD.PublishID = PB.PubID
                                                    AND PAD.PubStatus = {0} --驳回
                                            ORDER BY  PAD.RecID DESC
                                        )
                            ,	DC1.DictName AS Wx_StatusName
                    FROM    dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Weixin AS WX WITH ( NOLOCK ) ON WX.MediaID = PB.MediaID AND WX.Status = 0
		                    INNER JOIN dbo.Weixin_OAuth AS WO WITH(NOLOCK) ON WO.RecID = WX.WxID AND WO.Status = 0
                            LEFT JOIN dbo.UserDetailInfo AS UDI WITH ( NOLOCK ) ON UDI.UserID = PB.CreateUserID
                            LEFT JOIN dbo.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = PB.Wx_Status
                  ", RequetQuery.Wx_Status.Replace(",", ""));
            sbSql.AppendLine("  WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.IsDel = 0 AND PB.MediaType = {0}", (int)MediaType.WeiXin);

            //当前运营角色需要看到 自己审核操作过的记录
            sbSql.AppendFormat(@" AND EXISTS(
	                        SELECT 1 FROM DBO.PublishAuditInfo AS PAI WITH(NOLOCK)
	                                WHERE PAI.PublishID = PB.PubID
                                     {0}
	                                --AND PAI.CreateUserID = --当前角色审核的刊例
	                                AND PAI.MediaType = PB.MediaType
	                                AND PAI.PubStatus = {1} --当前角色审核结果
                        )", string.Empty, RequetQuery.Wx_Status);
            //媒体帐号、名称
            if (!string.IsNullOrWhiteSpace(RequetQuery.Keyword))
            {
                sbSql.AppendFormat(@"  AND (WX.Number = '{0}' OR WX.Name = '{0}')", RequetQuery.Keyword.ToSqlFilter());
            }
            //过期时间
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }
            //查询刊例提交人
            if (!string.IsNullOrWhiteSpace(RequetQuery.SubmitUser))
            {
                sbSql.AppendFormat(" AND (UDI.TrueName = '{0}' OR (EXISTS(SELECT 1 FROM DBO.UserInfo AS UIDD WITH(NOLOCK) WHERE UIDD.UserName = '{0}')))",
                   RequetQuery.SubmitUser.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDateTime) || !string.IsNullOrWhiteSpace(RequetQuery.EndDateTime))
            {
                sbSql.AppendFormat(@" AND EXISTS(
                                        SELECT * FROM dbo.PublishAuditInfo AS PAI WITH(NOLOCK)
                                        WHERE PAI.PublishID = PB.PubID
                                        {0}
	                                    --AND PAI.CreateUserID = --当前角色审核的刊例
                                        AND PAI.PublishID = PB.PubID
                                        AND PAI.MediaType = PB.MediaType
                                        AND PAI.PubStatus = {1} --当前角色审核结果
                                    ", string.Empty, RequetQuery.Wx_Status);
                if (!string.IsNullOrWhiteSpace(RequetQuery.StartDateTime))
                {
                    sbSql.AppendFormat(@"  AND PAI.CreateTime >= '{0}' ", RequetQuery.StartDateTime);
                }
                if (!string.IsNullOrWhiteSpace(RequetQuery.EndDateTime))
                {
                    sbSql.AppendFormat(@"  AND PAI.CreateTime <= '{0}' ", RequetQuery.EndDateTime);
                }
                sbSql.AppendLine(@")");
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