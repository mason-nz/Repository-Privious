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
    public class FpVideoQuery : PublishInfoQueryClient<RequestFrontPublishQueryDto, ResponseFrontVideo>
    {
        public FpVideoQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override PublishQuery<ResponseFrontVideo> GetQueryParams()
        {
            var sbSql = new StringBuilder("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
                        SELECT  MV.MediaID ,
                                MV.FansCount ,
                                MV.Number ,
                                MV.Name ,
                                MV.HeadIconURL ,
                                MV.Sex ,
                                MV.IsReserve ,
                                MV.ProvinceID ,
                                MV.CityID ,MV.AuthType,A.SaleDiscount,
		                        AI.AreaName AS ProvinceName,AI1.AreaName AS CityName,
                                DC.DictName AS CategoryName ,DC2.DictName AS [Platform],A.PubID,
                                PD1.Price AS FirstPrice ,PD2.Price AS SecondPrice ,
                               ( SELECT TOP 1
                                            PD.Price
                                  FROM      dbo.Publish_DetailInfo AS PD WITH ( NOLOCK )
                                  WHERE     PD.MediaType = {0}
                                            AND MV.MediaID = PD.MediaID
                                            AND PD.ADPosition1 = 9002--原创+发布
                                ) AS Price--如果有价格就代表可原创
                        FROM    dbo.Publish_BasicInfo AS A WITH ( NOLOCK )
                                INNER JOIN dbo.Media_Video AS MV WITH ( NOLOCK ) ON MV.MediaID = A.MediaID
                                LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = MV.CategoryID
                                LEFT JOIN dbo.DictInfo AS DC2 WITH(NOLOCK) ON DC2.DictId = MV.[Platform]
                                LEFT JOIN DBO.AreaInfo AS AI WITH(NOLOCK) ON AI.AreaID = MV.ProvinceID
                                LEFT JOIN DBO.AreaInfo AS AI1 WITH(NOLOCK) ON AI1.AreaID = MV.CityID
                                LEFT JOIN DBO.Publish_DetailInfo AS PD1 WITH(NOLOCK) ON PD1.PubID = A.PubID AND PD1.ADPosition1 = {1} --直发
                                LEFT JOIN DBO.Publish_DetailInfo AS PD2 WITH(NOLOCK) ON PD2.PubID = A.PubID AND PD2.ADPosition1 = {2} --原创发布
                        ", (int)MediaType.Video, (int)AdFormality4.直发, (int)AdFormality4.原创发布);
            sbSql.AppendLine("{#} ");
            sbSql.AppendLine(" WHERE 1=1 ");
            sbSql.AppendFormat(@" AND A.MediaType = {0} AND A.PublishStatus = {1}
                    AND GETDATE() BETWEEN A.BeginTime AND A.EndTime", (int)MediaType.Video, (int)EnumPublishStatus.OnSold);
            var sqlWhere = new StringBuilder();

            if (RequetQuery.CategoryID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MV.CategoryID = {0} ", RequetQuery.CategoryID);
            }
            if (RequetQuery.SearchMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND MV.MediaID = {0}", RequetQuery.SearchMediaId);
                RequetQuery.MediaName = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))//
            {
                sbSql.AppendFormat(" AND MV.Name LIKE '%{0}%' ", Utils.StringHelper.SqlFilter(RequetQuery.MediaName));
            }

            if (RequetQuery.IsAuth != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MV.AuthType = {0} ", RequetQuery.IsAuth);
            }
            if (RequetQuery.Sex != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MV.Sex = {0} ", RequetQuery.Sex);
            }
            if (RequetQuery.Profession != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND MV.Profession = {0} ", RequetQuery.Profession);
                sqlWhere.Append(" LEFT JOIN dbo.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = MV.Profession --查询条件职业：Profession");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.FansCount) && !string.IsNullOrWhiteSpace(RequetQuery.FansCount.Replace("-", "")))
            {
                var spFansCount = RequetQuery.FansCount.Split('-');
                if (spFansCount[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND MV.FansCount >= {0}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND MV.FansCount >= {0} AND MV.FansCount < {1}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit, spFansCount[1].ToInt() * RequetQuery.FansCountUnit);
                }
            }
            if (RequetQuery.Platform != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(" AND DC2.DictId = {0} ", RequetQuery.Platform);
                //sqlWhere.Append(" LEFT JOIN dbo.DictInfo AS DC2 WITH(NOLOCK) ON DC2.DictId = MV.[Platform] --查询条件职业：[Platform]");
            }
            if (RequetQuery.FansSex != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                if (RequetQuery.FansSex == 0)
                {
                    sbSql.AppendFormat(@" AND MV.FansMalePer > 50.00 ");
                }
                if (RequetQuery.FansSex == 1)
                    sbSql.AppendFormat(@" AND MV.FansFemalePer >50.00 ");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Price) && !string.IsNullOrWhiteSpace(RequetQuery.Price.Replace("-", "")))
            {
                var spPrice = RequetQuery.Price.Split('-');
                sbSql.Append(" AND MV.MediaID IN ( ");
                sbSql.Append(" SELECT PD.MediaID FROM dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) WHERE PD.MediaType = A.MediaType");
                if (spPrice[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND PD.Price * A.SaleDiscount >= {0}", spPrice[0].ToInt() * RequetQuery.PriceUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND PD.Price * A.SaleDiscount >= {0} AND PD.Price * A.SaleDiscount < {1}", spPrice[0].ToInt() * RequetQuery.PriceUnit, spPrice[1].ToInt() * RequetQuery.PriceUnit);
                }
                sbSql.Append(" ) ");
            }

            sbSql.AppendLine(@" ) T");
            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            var query = new PublishQuery<ResponseFrontVideo>()
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