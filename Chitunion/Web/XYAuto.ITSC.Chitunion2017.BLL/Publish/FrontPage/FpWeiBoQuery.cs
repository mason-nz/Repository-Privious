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
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage
{
    public class FpWeiBoQuery : PublishInfoQueryClient<RequestFrontPublishQueryDto, ResponseFrontWeiBoDto>
    {
        public FpWeiBoQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseFrontWeiBoDto> GetQueryParams()
        {
            var sbSql = new StringBuilder("select T.* YanFaFROM ( ");
            sbSql.Append(@"SELECT  A.PubID,WB.MediaID ,
                            WB.Number ,
                            WB.Name ,
                            WB.HeadIconURL ,
                            --WB.TwoCodeURL ,
                            WB.FansCount ,
                            WB.FansCountURL ,WB.AuthType,WB.Sign,
                            WB.[Status] ,WB.CategoryID,DC.DictName AS CategoryName,
                            A.PublishStatus,A.MediaType,A.SaleDiscount,
							IW.AverageForwardCount,IW.AverageCommentCount,IW.AveragePointCount
                            {$}
                    FROM  dbo.Publish_BasicInfo AS A WITH (NOLOCK)
                            INNER JOIN dbo.Media_Weibo AS WB WITH (NOLOCK) ON WB.MediaID = A.MediaID AND WB.Status = 0
                    INNER JOIN DBO.Interaction_Weibo AS IW WITH(NOLOCK) ON IW.MediaID = WB.MediaID
					--AND IW.MeidaType = A.MediaType
					LEFT JOIN dbo.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = WB.CategoryID
                    {#}
					");

            sbSql.AppendLine(" WHERE 1=1 ");
            sbSql.AppendFormat(@" AND A.MediaType = {0} AND A.PublishStatus = {1}
                AND GETDATE() BETWEEN A.BeginTime AND A.EndTime", (int)MediaType.WeiBo, (int)EnumPublishStatus.OnSold);
            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder("");

            if (RequetQuery.CategoryID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.CategoryID = {0}", RequetQuery.CategoryID);
            }
            if (RequetQuery.SearchMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.MediaID = {0}", RequetQuery.SearchMediaId);
                RequetQuery.MediaName = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))//
            {
                sbSql.AppendFormat(" AND WB.Name LIKE '%{0}%' ", Utils.StringHelper.SqlFilter(RequetQuery.MediaName));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.FansCount) && !string.IsNullOrWhiteSpace(RequetQuery.FansCount.Replace("-", "")))
            {
                var spFansCount = RequetQuery.FansCount.Split('-');
                if (spFansCount[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND WB.FansCount >= {0}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND WB.FansCount >= {0} AND WB.FansCount < {1}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit, spFansCount[1].ToInt() * RequetQuery.FansCountUnit);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Price) && !string.IsNullOrWhiteSpace(RequetQuery.Price.Replace("-", "")))
            {
                var spPrice = RequetQuery.Price.Split('-');
                sbSql.Append(" AND WB.MediaID IN ( ");
                sbSql.Append(" SELECT MediaID FROM DBO.Publish_DetailInfo AS PD WITH(NOLOCK) WHERE PD.MediaType = A.MediaType");
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

            if (RequetQuery.AuthType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.AuthType = {0}", RequetQuery.AuthType);
            }
            if (RequetQuery.LevelType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.LevelType = {0}", RequetQuery.LevelType);
            }
            if (RequetQuery.Profession != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.Profession = {0}", RequetQuery.Profession);
            }
            if (RequetQuery.FansSex != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.FansSex = {0}", RequetQuery.FansSex);
            }
            if (RequetQuery.Sex != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WB.Sex = {0}", RequetQuery.Sex);
            }
            var orderByReference = string.Empty;//如果没有参考报价排序 优先级大于普通排序

            //参考报价排序
            if (RequetQuery.OrderByReference != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                var dic = new Dictionary<int, string>()
                {
                    { 8001,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},ASC",(int)AdTypeMapping.硬广,(int)AdFormality4.转发)},//硬广转发asc
                    { 8002,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},DESC",(int)AdTypeMapping.硬广,(int)AdFormality4.转发)},//硬广转发desc
                    { 8003,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},ASC",(int)AdTypeMapping.软广,(int)AdFormality4.转发)},//软广转发asc
                    { 8004,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},DESC",(int)AdTypeMapping.软广,(int)AdFormality4.转发)},//软广转发desc
                    { 8005,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},ASC",(int)AdTypeMapping.硬广,(int)AdFormality4.直发)},//硬广直发asc
                    { 8006,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},DESC",(int)AdTypeMapping.硬广,(int)AdFormality4.直发)},//硬广直发desc
                    { 8007,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},ASC",(int)AdTypeMapping.软广,(int)AdFormality4.直发)},//软广直发asc
                    { 8008,string.Format(" AND PD.ADPosition1={0} AND PD.ADPosition2={1},DESC",(int)AdTypeMapping.软广,(int)AdFormality4.直发)},//软广直发desc
                };
                var value = dic.FirstOrDefault(s => s.Key == RequetQuery.OrderByReference);

                if (!string.IsNullOrWhiteSpace(value.Value))
                {
                    var sp = value.Value.Split(',');
                    var ascDescOrderBy = sp[1];
                    sqlSelect.AppendFormat(@",(
			                    SELECT TOP 1 PD.Price FROM DBO.Publish_DetailInfo AS PD WITH(NOLOCK)
			                    WHERE PD.MediaID =WB.MediaID AND PD.MediaType = A.MediaType {0}
			                    ORDER BY PD.Price {1}
		                    ) AS Price", sp[0], ascDescOrderBy);
                    orderByReference = " Price " + ascDescOrderBy;
                }
            }

            sbSql.AppendLine(@" ) T");
            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            sbSql.Replace("{$}", sqlSelect.ToString());//追加sql
            var query = new PublishQuery<ResponseFrontWeiBoDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = string.IsNullOrWhiteSpace(orderByReference) ? GetOrderBy() : orderByReference,//默认按粉丝数倒序排列
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        private string GetOrderBy()
        {
            var orderByStr = " FansCount DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC"},
                {1002," FansCount ASC"},
                {2001," AverageForwardCount DESC"},//平均转发数
                {2002," AverageForwardCount ASC"},
                {3001," AverageCommentCount DESC"},//平均评论数
                {3002," AverageCommentCount ASC"},
                {4001," AveragePointCount DESC"},//平均点赞数
                {4002," AveragePointCount ASC"}
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == RequetQuery.OrderBy);
            return value.Value ?? orderByStr;
        }

        protected override BaseResponseEntity<ResponseFrontWeiBoDto> GetResult(List<ResponseFrontWeiBoDto> resultList, QueryPageBase<ResponseFrontWeiBoDto> query)
        {
            var mediaIdList = resultList.Select(s => s.MediaID).ToList();//分页中的媒体Id集合
            if (mediaIdList.Count == 0)
                return base.GetResult(resultList, query);
            //用媒体Id列表查询对应的详情
            var publishItemInfo = PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
            {
                MediaId = mediaIdList,
                MediaType = (int)MediaType.WeiBo
            });
            resultList.ForEach(item => { GetAdPositionList(publishItemInfo, item); });

            return base.GetResult(resultList, query);
        }

        private ResponseFrontWeiBoDto GetAdPositionList(List<PublishDetailInfo> publishDetailList,
            ResponseFrontWeiBoDto item)
        {
            var saleDiscount = item.SaleDiscount > 0 ? item.SaleDiscount : 1.0m;
            item.FirstName = new List<AdPositionEntity>();
            item.SecondName = new List<AdPositionEntity>();
            var groupItemInfo = publishDetailList.Where(s => s.MediaID == item.MediaID);
            var detailInfos = groupItemInfo as PublishDetailInfo[] ?? groupItemInfo.ToArray();
            //找到first 直发
            var firstList = detailInfos.Where(s => s.ADPosition2 == (int)AdFormality4.直发);
            var publishDetailInfos = firstList as PublishDetailInfo[] ?? firstList.ToArray();
            //找到first 直发-硬广
            var firstAdPosition27002 = publishDetailInfos.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.硬广);
            //找到first 直发-软广
            var firstAdPosition27003 = publishDetailInfos.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.软广);

            if (firstAdPosition27002 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27002.ADPosition1Name, Price = Math.Round(firstAdPosition27002.Price * saleDiscount, 2) });
            if (firstAdPosition27003 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27003.ADPosition1Name, Price = Math.Round(firstAdPosition27003.Price * saleDiscount, 2) });

            //找到second 转发
            var secondList = detailInfos.Where(s => s.ADPosition2 == (int)AdFormality4.转发);
            //找到second 转发-硬广
            var secondAdPosition27002 = secondList.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.硬广);
            //找到second 转发-软广
            var secondAdPosition27003 = secondList.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.软广);

            if (secondAdPosition27002 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27002.ADPosition1Name, Price = Math.Round(secondAdPosition27002.Price * saleDiscount, 2) });
            if (secondAdPosition27003 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27003.ADPosition1Name, Price = Math.Round(secondAdPosition27003.Price * saleDiscount, 2) });

            return item;
        }
    }
}