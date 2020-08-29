using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    public class CollectionListQuery : PublishInfoQueryClient<ReqCollectPullBackQueryDto, ResponseCollectionDto>
    {
        public CollectionListQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseCollectionDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
                            SELECT  WO.WxNumber AS Number ,
                                    WO.NickName AS Name ,
                                    WO.HeadImg AS HeadIconURL,
                                    WO.FansCount ,
                                    MW.Status,
		                            MW.MediaID,
                                    MW.PublishStatus ,--上下架状态
                                    IW.MaxinumReading ,--最高阅读数
                                    IW.ReferReadCount --参考阅读数
                                    ,
                                    CommonlyClass = ( SELECT DC1.DictName + ','
                                                     FROM   dbo.MediaCategory AS MC WITH ( NOLOCK )
                                                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MC.CategoryID
                                                     WHERE  MC.WxID = WO.RecID
                                                            AND MC.MediaType = MCB.MediaType
                                                            ORDER BY SortNumber DESC
                                                            FOR XML PATH('')
                                                   )
		                            --
                                    ,
                                    Price = ( SELECT TOP 1
                                                        MIN(PD.SalePrice)
                                              FROM      dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                        LEFT JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = PB.PubID
                                              WHERE     PB.MediaID = MW.MediaID
                                                        AND PB.MediaType = MCB.MediaType
                                                        AND PB.Wx_Status IN ({0})
                                                        --AND GETDATE() BETWEEN PB.BeginTime AND PB.EndTime
                                              GROUP BY  PD.SalePrice ,
                                                        PB.BeginTime
                                              ORDER BY  PD.SalePrice ASC ,
                                                        PB.BeginTime ASC
                                            )
                            FROM    dbo.Media_CollectionBlacklist AS MCB WITH ( NOLOCK )
                                    INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON MW.MediaID = MCB.MediaID --AND MW.Status = 0
                                    INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID AND WO.Status = 0
                                    LEFT JOIN dbo.Interaction_Weixin AS IW WITH ( NOLOCK ) ON WO.RecID = IW.WxID
						WHERE MCB.MediaType = {1} AND MCB.RelationType = {2} AND MCB.Status = {3}", ((int)PublishBasicStatusEnum.上架 + "," + (int)PublishBasicStatusEnum.已通过),
                        (int)MediaType.WeiXin, (int)CollectPullBackTypeEnum.Collection, (int)DataStatusEnum.Active);

            if (RequetQuery.CreateUserId > 0)
            {
                sbSql.AppendFormat(" AND MCB.CreateUserID = {0}", RequetQuery.CreateUserId);
            }

            sbSql.AppendLine(@" ) T");

            var query = new PublishQuery<ResponseCollectionDto>()
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