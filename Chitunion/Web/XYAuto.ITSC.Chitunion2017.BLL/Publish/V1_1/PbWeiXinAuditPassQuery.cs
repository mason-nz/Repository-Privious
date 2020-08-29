using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1
{
    public class PbWeiXinAuditPassQuery : PublishInfoQueryClient<RequestPublishQueryDto, RespPbWeiXinAuditPassDto>
    {
        public PbWeiXinAuditPassQuery(ConfigEntity configEntity) : base(configEntity)
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
                                WX.MediaID ,
                                WO.WxNumber AS Number ,
                                WO.NickName AS Name ,
                                WO.HeadImg AS HeadIconURL ,
                                UDI.UserID ,
                                UDI.TrueName,
                                DC1.DictName AS Wx_StatusName
                                {$}
                        FROM    dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                INNER JOIN dbo.Media_Weixin AS WX WITH ( NOLOCK ) ON WX.MediaID = PB.MediaID AND WX.Status = 0
                                INNER JOIN DBO.Weixin_OAuth AS WO WITH(NOLOCK) ON WO.RecID = WX.WxID AND WO.Status = 0
                                LEFT JOIN dbo.UserDetailInfo AS UDI WITH ( NOLOCK ) ON UDI.UserID = PB.CreateUserID
                                LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = PB.Wx_Status
                                {#}
                  ");
            sbSql.AppendLine("  WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.IsDel = 0 AND PB.MediaType = {0}", (int)MediaType.WeiXin);
            //sbSql.AppendFormat(@" AND PB.Wx_Status = {0}", (int)PublishBasicStatusEnum.已通过);
            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder();

            if (RequetQuery.SearchId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@"  AND WX.MediaID = {0}", RequetQuery.SearchId);
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Keyword))
            {
                sbSql.AppendFormat(@"  AND (WX.Number = '{0}' OR WX.Name = '{0}')", RequetQuery.Keyword.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.PubName))
            {
                sbSql.AppendFormat(@"  AND PB.PubName LIKE '%{0}%'", RequetQuery.PubName.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Wx_Status))
            {
                sbSql.AppendFormat(@"  AND PB.Wx_Status IN ({0})", RequetQuery.Wx_Status.Trim(',').ToSqlFilter());
            }
            //过期时间
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }

            if (RequetQuery.Source != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sqlWhere.AppendLine(" LEFT JOIN DBO.UserInfo AS UI WITH(NOLOCK) ON UDI.UserID = WX.CreateUserID ");
                sqlSelect.AppendLine(" ,UI.Source ");
                sbSql.AppendFormat(@"  AND WX.Source = {0}", RequetQuery.Source);
            }
            if (RequetQuery.CreateUserId > 0)
            {
                sbSql.AppendFormat(@"  AND PB.CreateUserID = {0}", RequetQuery.CreateUserId);
            }

            sbSql.AppendLine(@" ) T");
            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            sbSql.Replace("{$}", sqlSelect.ToString());//追加 查询字段
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