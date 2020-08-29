/********************************************************
*创建人：lixiong
*创建时间：2017/6/9 10:28:16
*说明：  --模板列表
        --运营角色
        --大部分返回字段都一样，可以共用
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.OpenXmlFormats.Dml;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1.AdTemplate
{
    public class AdTemplateYunYingQuery : PublishInfoQueryClient<RequestAdQueryDto, RespAdTemplateDto>
    {
        public AdTemplateYunYingQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespAdTemplateDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                        SELECT  ADT.RecID AS TemplateId,
                                ADT.BaseMediaID,
                                ADT.BaseAdID,
                                ADT.AdTemplateName,
                                ADT.AuditStatus,
                                ADT.CreateUserID,
                                ADT.CreateTime,
                                MBPP.Name AS BaseMediaName,
                                MBPP.HeadIconURL AS BaseMediaLogoUrl
                                --审核人 + 审核时间
                                ,
                                AuditUser = (SELECT TOP 1
                                                        '|' + ISNULL(CONVERT(VARCHAR(20), PAD.CreateTime, 120),'')
                                              FROM      dbo.PublishAuditInfo AS PAD WITH(NOLOCK)
                                              WHERE     PAD.MediaType = {0}
                                                        AND PAD.TemplateID = ADT.RecID
                                                        AND PAD.OptType = {1}--审核操作
                                              ORDER BY  PAD.RecID DESC
                                            )
                                ,UI.SysName AS SubmitUser
                        FROM dbo.App_AdTemplate AS ADT WITH (NOLOCK)
                             INNER JOIN dbo.Media_BasePCAPP AS MBPP WITH(NOLOCK) ON ADT.BaseMediaID = MBPP.RecID
                                                                                   AND MBPP.Status = 0
                             LEFT JOIN dbo.v_UserInfo AS UI WITH(NOLOCK) ON ADT.CreateUserID = UI.UserID
                            WHERE ADT.Status = 0 AND ADT.AuditStatus = {1}
                        ", (int)MediaType.APP, RequetQuery.TemplateAuditStatus);

            if (!string.IsNullOrWhiteSpace(RequetQuery.AdTemplateName))
            {
                sbSql.AppendFormat(" AND ADT.AdTemplateName = '{0}'", RequetQuery.AdTemplateName.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MBPP.Name = '{0}'", RequetQuery.MediaName.ToSqlFilter());
            }
            //查询提交人
            if (!string.IsNullOrWhiteSpace(RequetQuery.SubmitUser))
            {
                sbSql.AppendFormat(" AND (UI.UserName = '{0}' OR UI.Mobile = '{0}')",
                   RequetQuery.SubmitUser.ToSqlFilter());
            }
            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespAdTemplateDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " TemplateId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}