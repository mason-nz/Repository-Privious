using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage
{
    public class FpAppQuery : PublishInfoQueryClient<RequestFrontPublishQueryDto, ResponseFrontApp>
    {
        public FpAppQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseFrontApp> GetQueryParams()
        {
            var sbSql = new StringBuilder("select T.* YanFaFROM ( ");

            sbSql.AppendLine(@"
                                 SELECT  A.PubID ,PD.RecID,
                                PCAPP.MediaID ,
                                DailyExposureCount ,
                                PCAPP.Name ,
                                EXT.AdPosition ,
                                EXT.AdForm ,
                                EXT.AdLegendURL ,
                                EXT.Style ,
                                EXT.CarouselCount ,
                                EXT.CanClick ,
                                EXT.PlayPosition ,
                                EXT.SysPlatform ,
                                dbo.fn_GetADPositionDicName(PD.ADPosition1) AS SaleMode ,--'售卖方式'
                                PD.IsCarousel ,
                                PD.Price ,
                                PD.RecID AS AdRecID ,
                                A.SaleDiscount
                        FROM    dbo.Publish_BasicInfo AS A WITH ( NOLOCK )
                                INNER JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = A.PubID
                                INNER JOIN dbo.Media_PCAPP AS PCAPP WITH ( NOLOCK ) ON PCAPP.MediaID = A.MediaID
                                INNER JOIN dbo.Publish_ExtendInfoPCAPP AS EXT WITH ( NOLOCK ) ON EXT.ADDetailID = PD.RecID
                                    {#}
                ");

            sbSql.AppendLine(" WHERE 1=1 ");
            sbSql.AppendFormat(@" AND A.MediaType = {0}  AND PD.PublishStatus = {1}
                    AND GETDATE() BETWEEN A.BeginTime AND A.EndTime", (int)MediaType.APP, (int)EnumPublishStatus.OnSold);

            var sqlWhere = new StringBuilder();
            if (RequetQuery.CategoryID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sqlWhere.Append(" INNER JOIN dbo.DictInfo AS DC WITH (NOLOCK) ON DC.DictId = PCAPP.CategoryID");
                sbSql.AppendFormat(" AND PCAPP.CategoryID = {0} ", RequetQuery.CategoryID);
            }
            if (RequetQuery.SaleMode != Entities.Constants.Constant.INT_INVALID_VALUE)//售卖方式
            {
                sbSql.AppendFormat(" AND PD.ADPosition1 = {0} ", RequetQuery.SaleMode);
            }
            if (RequetQuery.SearchMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PCAPP.MediaID = {0}", RequetQuery.SearchMediaId);
                RequetQuery.MediaName = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))//
            {
                sbSql.AppendFormat(" AND PCAPP.Name LIKE '%{0}%' ", Utils.StringHelper.SqlFilter(RequetQuery.MediaName));
                RequetQuery.MediaName = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.AdForm))//
            {
                sbSql.AppendFormat(" AND EXT.AdForm LIKE '%{0}%' ", Utils.StringHelper.SqlFilter(RequetQuery.AdForm));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.DailyExposureCount) && !string.IsNullOrWhiteSpace(RequetQuery.DailyExposureCount.Replace("-", "")))
            {
                var spDailyExposureCount = RequetQuery.DailyExposureCount.Split('-');
                if (spDailyExposureCount[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND EXT.DailyExposureCount >= {0}", spDailyExposureCount[0].ToInt() * RequetQuery.DailyExposureCountUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND EXT.DailyExposureCount >= {0} AND EXT.DailyExposureCount < {1}", spDailyExposureCount[0].ToInt() * RequetQuery.DailyExposureCountUnit
                        , spDailyExposureCount[1].ToInt() * RequetQuery.DailyExposureCountUnit);
                }
            }
            //APP的价格刊例是一维的，可以直接联合查询
            if (!string.IsNullOrWhiteSpace(RequetQuery.Price) && !string.IsNullOrWhiteSpace(RequetQuery.Price.Replace("-", "")))
            {
                var spPrice = RequetQuery.Price.Split('-');
                if (spPrice[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND PD.Price * A.SaleDiscount >= {0}", spPrice[0].ToInt() * RequetQuery.PriceUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND PD.Price * A.SaleDiscount >= {0} AND PD.Price * A.SaleDiscount < {1}", spPrice[0].ToInt() * RequetQuery.PriceUnit, spPrice[1].ToInt() * RequetQuery.PriceUnit);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.CoverageArea) && !string.IsNullOrWhiteSpace(RequetQuery.CoverageArea.Replace("-", "")))
            {
                sbSql.Append(@"
                        AND EXISTS
				        (
					        SELECT MAM.MediaID FROM  dbo.Media_Area_Mapping AS MAM WITH(NOLOCK)
					        WHERE MAM.MediaID = A.MediaID AND MAM.MediaType = A.MediaType
					    ");
                var spCoverageArea = RequetQuery.CoverageArea.Split('-');
                sbSql.AppendFormat(@" AND MAM.ProvinceID = {0} ", spCoverageArea[0]);
                if (!CityMapping.IsMunicipality(spCoverageArea[0].ToInt()))//直辖市不用查CityID
                {
                    if (!string.IsNullOrWhiteSpace(spCoverageArea[1]) && spCoverageArea[1].ToInt() > 0)
                        sbSql.AppendFormat(@" AND MAM.CityID = {0}", spCoverageArea[1]);
                }
                sbSql.Append(@" ) ");
            }
            sbSql.AppendLine(@" ) T");
            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            var query = new PublishQuery<ResponseFrontApp>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " RecID DESC ",//默认按日均曝光量倒序显示
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}