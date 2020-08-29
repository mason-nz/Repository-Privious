﻿/********************************************************
*创建人：lixiong
*创建时间：2017/6/16 10:02:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
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
    public class MediaAppQueryByYunYingAudit : PublishInfoQueryClient<RequestMediaAppQueryDto, RespMediaAppDto>
    {
        public MediaAppQueryByYunYingAudit(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespMediaAppDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            #region 主体sql

            sbSql.AppendFormat(@"

                    SELECT  MBPC.RecID AS BaseMediaID ,
                            MBPC.Name ,
                            MBPC.HeadIconURL ,
                            MBPC.DailyLive,
                            MCP.MediaID ,
                            MCP.AuditStatus,
		                    MQF.MediaRelations,
		                    DI1.DictName AS MediaRelationsName,
		                    MCP.CreateTime
                            ,AdTemplateId = (
			                    --判断当前媒体下是否有已审核通过的模板或自己添加的未通过的模板
                                    SELECT  TemplateId = ISNULL(( CASE WHEN PassTemplateId > 0 THEN PassTemplateId
																ELSE NotPassTemplateId
														   END ),0)
									FROM    ( SELECT    MAX(PassTemplateId) AS PassTemplateId ,
														MAX(NotPassTemplateId) AS NotPassTemplateId
											  FROM      ( SELECT    *
														  FROM      ( SELECT TOP 1
																				ADDT.RecID AS PassTemplateId ,
																				0 AS NotPassTemplateId
																	  FROM      dbo.App_AdTemplate AS ADDT WITH ( NOLOCK )
																	  WHERE     ADDT.BaseMediaID = MBPC.RecID
																				AND ADDT.AuditStatus = {1} --已通过
																				AND ADDT.Status = 0
																	  ORDER BY  ADDT.RecID ASC
																	) AS Template1
														) AS D
											) AS C
		                    )
		                    ,AdCount = (
                                --AdUpShelfCount:上架广告数据，其他：AdTotleCount-AdUpShelfCount
			                    SELECT  CAST(D.AdTotleCount AS VARCHAR(15)) + ','
					                    + CAST(D.AdUpShelfCount AS VARCHAR(15))
			                    FROM    (
                                        SELECT      MAX(AdUpShelfCount) AS AdUpShelfCount ,
                                                    MAX(AdTotleCount) AS AdTotleCount
                                        FROM    (
                                                        SELECT    COUNT(1) AS AdUpShelfCount ,
								                                0 AS AdTotleCount
					                                    FROM      dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
					                                    WHERE     PB.MediaType = {0}
								                                    AND PB.MediaID = MCP.MediaID
								                                    AND PB.Wx_Status = {5} --已上架
								                                    AND PB.IsDel = 0
					                                    UNION
					                                    SELECT    0 AS AdUpShelfCount ,
								                                    COUNT(1) AS AdTotleCount
					                                    FROM      dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
					                                    WHERE     PB.MediaType = {0}
								                                AND PB.MediaID = MCP.MediaID
								                                AND PB.IsDel = 0
					                            ) AS D
                                        ) AS D
		                    )
                            ,HasOnPub=(
								 SELECT  CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END
								FROM DBO.Publish_BasicInfo AS PB WITH(NOLOCK)
								WHERE PB.MediaType = {0}
								AND PB.MediaID = MBPC.RecID
								AND PB.Wx_Status = {5}
							)
                    FROM    dbo.Media_PCAPP AS MCP WITH ( NOLOCK )
                            INNER JOIN dbo.Media_BasePCAPP AS MBPC WITH ( NOLOCK ) ON MBPC.RecID = MCP.BaseMediaID
                            LEFT JOIN dbo.Media_Qualification AS MQF WITH ( NOLOCK ) ON MQF.MediaID = MCP.MediaID AND MQF.MediaType = {0}
		                    LEFT JOIN DBO.DictInfo AS DI1 WITH(NOLOCK) ON MQF.MediaRelations = DI1.DictId
                    WHERE   MCP.Status = 0
                ", (int)MediaType.APP, (int)AppTemplateEnum.已通过, (int)AppTemplateEnum.待审核, (int)AppTemplateEnum.已驳回
                , RequetQuery.CreateUserId, (int)AppPublishStatus.已上架);

            if (!string.IsNullOrWhiteSpace(RequetQuery.AuditStatus))
            {
                sbSql.AppendFormat(" AND MCP.AuditStatus = {0}", RequetQuery.AuditStatus);
            }

            #endregion 主体sql

            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MBPC.Name = '{0}'", RequetQuery.MediaName.ToSqlFilter());
            }

            //媒体关系
            if (RequetQuery.MediaRelations != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MQF.MediaRelations = {0}", RequetQuery.MediaRelations);
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
            //AdUpShelfCount:上架广告数据，其他：AdTotleCount-AdUpShelfCount
            resultList.ForEach(s =>
            {
                var spAdCount = s.AdCount.Split(new string[] { "," }, StringSplitOptions.None);
                var totleAdCount = CurrentOperateBase.GetAppContent(spAdCount, 0).ToInt();
                s.UpShelfAdCount = CurrentOperateBase.GetAppContent(spAdCount, 1).ToInt();
                s.OtherAdCount = totleAdCount - s.UpShelfAdCount;
                //s.CommonlyClass = AppOperate.MapperToCommonlyClass(s.CommonlyClassStr);
            });
            return base.GetResult(resultList, query);
        }
    }
}