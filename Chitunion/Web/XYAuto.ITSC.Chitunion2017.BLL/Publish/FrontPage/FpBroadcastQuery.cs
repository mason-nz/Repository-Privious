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
    public class FpBroadcastQuery : PublishInfoQueryClient<RequestFrontPublishQueryDto, ResponseFrontBroadcast>
    {
        public FpBroadcastQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseFrontBroadcast> GetQueryParams()
        {
            var sbSql = new StringBuilder("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
              SELECT   A.PubID ,
                        MB.MediaID ,
                        MB.FansCount ,
                        MB.Number ,
                        MB.Name ,
                        MB.HeadIconURL ,
                        MB.Sex ,
                        MB.IsReserve ,MB.IsAuth AS AuthType,
                        MB.ProvinceID ,
                        MB.CityID ,
                        DC.DictName AS CategoryName ,
                        DC2.DictName AS [Platform] ,
                        PD1.Price AS FirstPrice ,
                        PD2.Price AS SecondPrice,A.SaleDiscount,
                        AI.AreaName AS ProvinceName,AI1.AreaName AS CityName
               FROM     dbo.Publish_BasicInfo AS A WITH ( NOLOCK )
                        INNER JOIN dbo.Media_Broadcast AS MB WITH ( NOLOCK ) ON MB.MediaID = A.MediaID
                        LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = MB.CategoryID
                        LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON DC2.DictId = MB.[Platform]
                        LEFT JOIN DBO.AreaInfo AS AI WITH(NOLOCK) ON AI.AreaID = MB.ProvinceID
                        LEFT JOIN DBO.AreaInfo AS AI1 WITH(NOLOCK) ON AI1.AreaID = MB.CityID
                        LEFT JOIN dbo.Publish_DetailInfo AS PD1 WITH ( NOLOCK ) ON PD1.PubID = A.PubID AND PD1.ADPosition1 = {0} --活动现场直播
                        LEFT JOIN dbo.Publish_DetailInfo AS PD2 WITH ( NOLOCK ) ON PD2.PubID = A.PubID AND PD2.ADPosition1 = {1} --直播广告植入
                    ", (int)AdFormality5.活动现场直播, (int)AdFormality5.直播广告植入);
            sbSql.AppendLine(" WHERE 1=1 ");
            sbSql.AppendFormat(@" AND A.MediaType = {0} AND A.PublishStatus = {1}
                    AND GETDATE() BETWEEN A.BeginTime AND A.EndTime", (int)MediaType.Broadcast, (int)EnumPublishStatus.OnSold);

            var sqlWhere = new StringBuilder();

            if (RequetQuery.CategoryID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MB.CategoryID = {0} ", RequetQuery.CategoryID);
            }
            if (RequetQuery.SearchMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND MB.MediaID = {0}", RequetQuery.SearchMediaId);
                RequetQuery.MediaName = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))//
            {
                sbSql.AppendFormat(" AND MB.Name LIKE '%{0}%' ", Utils.StringHelper.SqlFilter(RequetQuery.MediaName));
            }
            if (RequetQuery.IsAuth != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MB.IsAuth = {0} ", RequetQuery.IsAuth);
            }
            if (RequetQuery.Sex != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MB.Sex = {0} ", RequetQuery.Sex);
            }
            if (RequetQuery.Profession != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MB.Profession = {0} ", RequetQuery.Profession);
                sqlWhere.Append(" LEFT JOIN dbo.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = MB.Profession --查询条件职业：Profession");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.FansCount) && !string.IsNullOrWhiteSpace(RequetQuery.FansCount.Replace("-", "")))
            {
                var spFansCount = RequetQuery.FansCount.Split('-');
                if (spFansCount[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND MB.FansCount >= {0}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND MB.FansCount >= {0} AND MB.FansCount < {1}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit, spFansCount[1].ToInt() * RequetQuery.FansCountUnit);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Price) && !string.IsNullOrWhiteSpace(RequetQuery.Price.Replace("-", "")))
            {
                var spPrice = RequetQuery.Price.Split('-');
                if (spPrice[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND ( PD1.Price * A.SaleDiscount >= {0} OR PD2.Price * A.SaleDiscount >= {0} )", spPrice[0].ToInt() * RequetQuery.PriceUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND (( PD1.Price * A.SaleDiscount >= {0} AND PD1.Price * A.SaleDiscount < {1} )
                    OR ( PD2.Price * A.SaleDiscount >= {0} AND PD2.Price * A.SaleDiscount < {1} ))", spPrice[0].ToInt() * RequetQuery.PriceUnit, spPrice[1].ToInt() * RequetQuery.PriceUnit);
                }
            }
            if (RequetQuery.Platform != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND DC2.DictId = {0} ", RequetQuery.Platform);
                //sqlWhere.Append(" INNER JOIN dbo.DictInfo AS DC2 WITH(NOLOCK) ON DC2.DictId = MB.[Platform] --查询条件职业：[Platform]");
            }

            sbSql.AppendLine(@" ) T");

            var query = new PublishQuery<ResponseFrontBroadcast>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = GetOrderBy(),
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        private string GetOrderBy()
        {
            var orderByStr = " MediaID DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC"},
                {1002," FansCount ASC"},
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == RequetQuery.OrderBy);
            return value.Value ?? orderByStr;
        }
    }
}