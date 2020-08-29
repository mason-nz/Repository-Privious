/********************************************************
*创建人：lixiong
*创建时间：2017/6/9 15:58:29
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1
{
    public class MediaAppNotPassQuery : PublishInfoQueryClient<RequestMediaAppQueryDto, RespMediaAppDto>
    {
        public MediaAppNotPassQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespMediaAppDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            #region sql主体

            sbSql.AppendFormat(@"
                        SELECT  MCP.MediaID ,
                                MCP.BaseMediaID ,
                                MCP.Name ,
                                MCP.HeadIconURL ,
                                MCP.DailyLive ,
		                        MCP.CreateTime,
                                MCP.AuditStatus,
		                        MQF.MediaRelations,
		                        MQF.OperatingType,
                                DI2.DictName AS OperatingTypeName,
		                        DI1.DictName AS MediaRelationsName,
                                CommonlyClassStr = ( SELECT    STUFF(( SELECT  '|'
                                                                            + ( ISNULL(( CAST(DIF.DictId AS VARCHAR(15))
                                                                                      + ','
                                                                                      + DIF.DictName ),
                                                                                      '') )
                                                                    FROM    dbo.Media_CommonlyClass AS MCC
                                                                            WITH ( NOLOCK )
                                                                            LEFT JOIN dbo.DictInfo AS DIF
                                                                            WITH ( NOLOCK ) ON MCC.CategoryID = DIF.DictId
                                                                    WHERE   MCC.MediaType = {0}
                                                                            AND MCC.MediaID = MCP.MediaID
                                                                    ORDER BY MCC.SortNumber DESC
                                                                  FOR
                                                                    XML PATH('')
                                                                  ), 1, 1, '')
                                                )
                                ,VUI.SysName AS TrueName
                        FROM    dbo.Media_PCAPP AS MCP WITH ( NOLOCK )
                        LEFT JOIN dbo.Media_Qualification AS MQF WITH ( NOLOCK ) ON MQF.MediaID = MCP.MediaID AND MQF.MediaType = {0}
                        LEFT JOIN DBO.DictInfo AS DI1 WITH(NOLOCK) ON MQF.MediaRelations = DI1.DictId
                        LEFT JOIN DBO.DictInfo AS DI2 WITH(NOLOCK) ON MQF.OperatingType = DI2.DictId
                        LEFT JOIN dbo.v_UserInfo AS VUI WITH ( NOLOCK ) ON VUI.UserID = MCP.LastUpdateUserID
                        WHERE   MCP.Status = 0 ", (int)MediaType.APP);

            if (!string.IsNullOrWhiteSpace(RequetQuery.AuditStatus))
            {
                sbSql.AppendFormat(" AND MCP.AuditStatus IN ({0})", RequetQuery.AuditStatus);
            }

            #endregion sql主体

            if (ConfigEntity.RoleTypeEnum == RoleEnum.YunYingOperate
                || ConfigEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                //AE角色 - 到角色,媒体主 - 个人
                if (RequetQuery.IsPassed)
                {
                    //运营审核页面，不展示AE的数据
                    sbSql.AppendFormat(" AND MQF.RecID > 0 ");
                }
            }
            else if (ConfigEntity.RoleTypeEnum == RoleEnum.AE)
            {
                sbSql.AppendFormat(@"AND MCP.CreateUserID IN (SELECT UserID
                                    FROM   dbo.UserRole
                                    WHERE  RoleID = '{0}' AND Status = 0)", RoleInfoMapping.AE);
            }
            else
            {
                sbSql.AppendFormat(" AND MCP.CreateUserID = {0}", RequetQuery.CreateUserId);
            }
            //媒体主名称
            if (!string.IsNullOrWhiteSpace(RequetQuery.SubmitUser))
            {
                sbSql.AppendFormat(" AND (VUI.TrueName = '{0}' OR VUI.Mobile = '{0}' OR VUI.UserName='{0}'  OR VUI.SysName='{0}') ", RequetQuery.SubmitUser.ToSqlFilter());
            }
            //媒体名称帐号
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MCP.Name = '{0}'", RequetQuery.MediaName.ToSqlFilter());
            }

            //媒体关系
            if (RequetQuery.MediaRelations != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MQF.MediaRelations = {0}", RequetQuery.MediaRelations);
            }
            if (RequetQuery.OperatingType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MQF.OperatingType = {0}", RequetQuery.OperatingType);
            }

            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespMediaAppDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MediaID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespMediaAppDto> GetResult(List<RespMediaAppDto> resultList, QueryPageBase<RespMediaAppDto> query)
        {
            if (resultList.Count == 0)
                return base.GetResult(resultList, query);

            resultList.ForEach(s =>
            {
                s.CommonlyClass = AppOperate.MapperToCommonlyClass(s.CommonlyClassStr);
            });
            return base.GetResult(resultList, query);
        }
    }
}